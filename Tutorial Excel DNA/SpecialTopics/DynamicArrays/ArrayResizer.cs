using System;
using ExcelDna.Integration;
using static ExcelDna.Integration.XlCall;

// This class defines a few test functions that can be used to explore the automatic array resizing under pre-dynamic arrays Excel
public static class ResizeTestFunctions
{
    // Makes an array, but automatically resizes the result
    public static object dnaMakeArrayAndResize(int rows, int columns, string unused, string unusedtoo)
    {
        object[,] result = ExampleFunctions.dnaMakeArray(rows, columns);
        return ArrayResizer.dnaResize(result);

        // Can also call Resize via Excel - so if the Resize add-in is not part of this code, it should still work
        // (though calling direct is better for large arrays - it prevents extra marshaling).
        // return XlCall.Excel(XlCall.xlUDF, "Resize", result);
    }
}

public class ArrayResizer
{
    // This function will run in the UDF context
    public static object dnaResize(object[,] array)
    {
        var caller = Excel(xlfCaller) as ExcelReference;
        if (caller == null)
            return array;

        int rows = array.GetLength(0);
        int columns = array.GetLength(1);

        if (rows == 0 || columns == 0)
            return array;

        // For dynamic-array aware Excel we don't do anything if the caller is a single cell
        // Excel will expand in this case
        if (UtilityFunctions.dnaSupportsDynamicArrays() && 
            caller.RowFirst == caller.RowLast &&
            caller.ColumnFirst == caller.ColumnLast)
        return array;

        if ((caller.RowLast - caller.RowFirst + 1 == rows) &&
            (caller.ColumnLast - caller.ColumnFirst + 1 == columns))
        {
            // Size is already OK - just return result
            return array;
        }

        var rowLast = caller.RowFirst + rows - 1;
        var columnLast = caller.ColumnFirst + columns - 1;

        // Check for the sheet limits
        if (rowLast > ExcelDnaUtil.ExcelLimits.MaxRows - 1 ||
            columnLast > ExcelDnaUtil.ExcelLimits.MaxColumns - 1)
        {
            // Can't resize - goes beyond the end of the sheet - just return #VALUE
            // (Can't give message here, or change cells)
            return ExcelError.ExcelErrorValue;
        }

        // TODO: Add some kind of guard for ever-changing result?
        ExcelAsyncUtil.QueueAsMacro(() =>
        {
            // Create a reference of the right size
            var target = new ExcelReference(caller.RowFirst, rowLast, caller.ColumnFirst, columnLast, caller.SheetId);
            DoResize(target); // Will trigger a recalc by writing formula
        });
        // Return the whole array even if we plan to resize - to prevent flashing #N/A
        return array;
    }

    public static double[,] dnaResizeDoubles(double[,] array)
    {
        var caller = Excel(xlfCaller) as ExcelReference;
        if (caller == null)
            return array;

        int rows = array.GetLength(0);
        int columns = array.GetLength(1);

        if (rows == 0 || columns == 0)
            return array;

    // For dynamic-array aware Excel we don't do anything if the caller is a single cell
    // Excel will expand in this case
    if (UtilityFunctions.dnaSupportsDynamicArrays() &&
        caller.RowFirst == caller.RowLast &&
        caller.ColumnFirst == caller.ColumnLast)
        return array;

        if ((caller.RowLast - caller.RowFirst + 1 == rows) &&
            (caller.ColumnLast - caller.ColumnFirst + 1 == columns))
        {
            // Size is already OK - just return result
            return array;
        }

        var rowLast = caller.RowFirst + rows - 1;
        var columnLast = caller.ColumnFirst + columns - 1;

        if (rowLast > ExcelDnaUtil.ExcelLimits.MaxRows - 1 ||
            columnLast > ExcelDnaUtil.ExcelLimits.MaxColumns - 1)
        {
            // Can't resize - goes beyond the end of the sheet - just return null (for #NUM!)
            // (Can't give message here, or change cells)
            return null;
        }

        // TODO: Add guard for ever-changing result?
        ExcelAsyncUtil.QueueAsMacro(() =>
        {
            // Create a reference of the right size
            var target = new ExcelReference(caller.RowFirst, rowLast, caller.ColumnFirst, columnLast, caller.SheetId);
            DoResize(target); // Will trigger a recalc by writing formula
        });
        // Return what we have - to prevent flashing #N/A
        return array;
    }

    static void DoResize(ExcelReference target)
    {
        // Get the current state for reset later
        using (new ExcelEchoOffHelper())
        using (new ExcelCalculationManualHelper())
        {
            ExcelReference firstCell = new ExcelReference(target.RowFirst, target.RowFirst, target.ColumnFirst, target.ColumnFirst, target.SheetId);

            // Get the formula in the first cell of the target
            string formula = (string)Excel(xlfGetCell, 41, firstCell);
            bool isFormulaArray = (bool)Excel(xlfGetCell, 49, firstCell);
            if (isFormulaArray)
            {
                // Select the sheet and firstCell - needed because we want to use SelectSpecial.
                using (new ExcelSelectionHelper(firstCell))
                {
                    // Extend the selection to the whole array and clear
                    Excel(xlcSelectSpecial, 6);
                    ExcelReference oldArray = (ExcelReference)Excel(xlfSelection);

                    oldArray.SetValue(ExcelEmpty.Value);
                }
            }
            // Get the formula and convert to R1C1 mode
            bool isR1C1Mode = (bool)Excel(xlfGetWorkspace, 4);
            string formulaR1C1 = formula;
            if (!isR1C1Mode)
            {
                object formulaR1C1Obj;
                XlReturn formulaR1C1Return = TryExcel(xlfFormulaConvert, out formulaR1C1Obj, formula, true, false, ExcelMissing.Value, firstCell);
                if (formulaR1C1Return != XlReturn.XlReturnSuccess || formulaR1C1Obj is ExcelError)
                {
                    string firstCellAddress = (string)Excel(xlfReftext, firstCell, true);
                    Excel(xlcAlert, "Cannot resize array formula at " + firstCellAddress + " - formula might be too long when converted to R1C1 format.");
                    firstCell.SetValue("'" + formula);
                    return;
                }
                formulaR1C1 = (string)formulaR1C1Obj;
            }
            // Must be R1C1-style references
            object ignoredResult;
            //Debug.Print("Resizing START: " + target.RowLast);
            XlReturn formulaArrayReturn = TryExcel(xlcFormulaArray, out ignoredResult, formulaR1C1, target);
            //Debug.Print("Resizing FINISH");

            // TODO: Find some dummy macro to clear the undo stack

            if (formulaArrayReturn != XlReturn.XlReturnSuccess)
            {
                string firstCellAddress = (string)Excel(xlfReftext, firstCell, true);
                Excel(xlcAlert, "Cannot resize array formula at " + firstCellAddress + " - result might overlap another array.");
                // Might have failed due to array in the way.
                firstCell.SetValue("'" + formula);
            }
        }
    }
}

public class ExcelEchoOffHelper : XlCall, IDisposable
{
    object oldEcho;

    public ExcelEchoOffHelper()
    {
        oldEcho = Excel(xlfGetWorkspace, 40);
        Excel(xlcEcho, false);
    }

    public void Dispose()
    {
        Excel(xlcEcho, oldEcho);
    }
}

public class ExcelCalculationManualHelper : XlCall, IDisposable
{
    object oldCalculationMode;

    public ExcelCalculationManualHelper()
    {
        oldCalculationMode = Excel(xlfGetDocument, 14);
        Excel(xlcOptionsCalculation, 3);
    }

    public void Dispose()
    {
        Excel(xlcOptionsCalculation, oldCalculationMode);
    }
}

// Select an ExcelReference (perhaps on another sheet) allowing changes to be made there.
// On clean-up, resets all the selections and the active sheet.
// Should not be used if the work you are going to do will switch sheets, amke new sheets etc.
public class ExcelSelectionHelper : XlCall, IDisposable
{
    object oldSelectionOnActiveSheet;
    object oldActiveCellOnActiveSheet;

    object oldSelectionOnRefSheet;
    object oldActiveCellOnRefSheet;

    public ExcelSelectionHelper(ExcelReference refToSelect)
    {
        // Remember old selection state on the active sheet
        oldSelectionOnActiveSheet = Excel(xlfSelection);
        oldActiveCellOnActiveSheet = Excel(xlfActiveCell);

        // Switch to the sheet we want to select
        string refSheet = (string)Excel(xlSheetNm, refToSelect);
        Excel(xlcWorkbookSelect, new object[] { refSheet });

        // record selection and active cell on the sheet we want to select
        oldSelectionOnRefSheet = Excel(xlfSelection);
        oldActiveCellOnRefSheet = Excel(xlfActiveCell);

        // make the selection
        Excel(xlcFormulaGoto, refToSelect);
    }

    public void Dispose()
    {
        // Reset the selection on the target sheet
        Excel(xlcSelect, oldSelectionOnRefSheet, oldActiveCellOnRefSheet);

        // Reset the sheet originally selected
        string oldActiveSheet = (string)Excel(xlSheetNm, oldSelectionOnActiveSheet);
        Excel(xlcWorkbookSelect, new object[] { oldActiveSheet });

        // Reset the selection in the active sheet (some bugs make this change sometimes too)
        Excel(xlcSelect, oldSelectionOnActiveSheet, oldActiveCellOnActiveSheet);
    }
}

public static class UtilityFunctions
{
    static bool? _supportsDynamicArrays;
    [ExcelFunction(IsHidden = false)]
    public static bool dnaSupportsDynamicArrays()
    {
        if (!_supportsDynamicArrays.HasValue)
        {
            try
            {
                var result = XlCall.Excel(614, new object[] { 1 }, new object[] { true });
                _supportsDynamicArrays = true;
            }
            catch
            {
                _supportsDynamicArrays = false;
            }
        }
        return _supportsDynamicArrays.Value;
    }
}
