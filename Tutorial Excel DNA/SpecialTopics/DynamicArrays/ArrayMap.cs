using System;
using ExcelDna.Integration;
using static ExcelDna.Integration.XlCall;

public static class ArrayMapFunctions
{
    // This helper function is converted from https://github.com/Excel-DNA/Samples/blob/master/ArrayMap/Functions.vb
    [ExcelFunction(Description = "Evaluates the two-argument function for every value in the first and second inputs. " + "Takes a single value and any rectangle, or one row and one column, or one column and one row.")]
    public static object dnaArrayMap2([ExcelArgument(Description = "The function to evaluate - either enter the name without any quotes or brackets (for .xll functions), or as a string (for VBA functions)")] object function, [ExcelArgument(Description = "The input value(s) for the first argument (row, column or rectangular range) ")] object input1, [ExcelArgument(Description = "The input value(s) for the second argument (row, column or rectangular range) ")] object input2)
    {
        {
            Func<object, object, object> evaluate;
            if (function is double)
            {
                evaluate = (x, y) => Excel(xlUDF, function, x, y);
            }
            else if (function is string)
            {
                // First try to get the RegisterId, if it's an .xll UDF
                object registerId;
                registerId = Excel(xlfEvaluate, function);
                if (registerId is double)
                {
                    evaluate = (x, y) => Excel(xlUDF, registerId, x, y);
                }
                else
                {
                    // Just call as string, hoping it's a valid VBA function
                    evaluate = (x, y) => Excel(xlUDF, function, x, y);
                }
            }
            else
            {
                return ExcelError.ExcelErrorValue;
            }

            // Check for the case where one of the arguments is not an array, so we evaluate as a 1D function
            if (!(input1 is object[,] inputArr1))
            {
                object evaluate1(object x) => evaluate(input1, x);
                return ArrayEvaluate(evaluate1, input2);
            }
            if (!(input2 is object[,] inputArr2))
            {
                object evaluate1(object x) => evaluate(x, input2);
                return ArrayEvaluate(evaluate1, input1);
            }

            // Otherwise we now have the function to evaluate, and two arrays
            return ArrayEvaluate2(evaluate, inputArr1, inputArr2);
        }
    }

    private static object[,] ArrayEvaluate2(Func<object, object, object> evaluate, object[,] inputArr1, object[,] inputArr2)
    {

        // Now we know both input1 and input2 are arrays
        // We assume they are 1D, else we'll do our best to combine - the exact rules might be decided more carefully
        if (inputArr1.GetLength(0) > 1)
        {
            // Lots of rows in input1, we'll take its first column only, and take the columns of input2
            var rows = inputArr1.GetLength(0);
            var cols = inputArr2.GetLength(1);

            var output = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    output[i, j] = evaluate(inputArr1[i, 0], inputArr2[0, j]);

            return output;
        }
        else
        {

            // Single row in input1, we'll take its columns, and take the rows from input2
            var rows = inputArr2.GetLength(0);
            var cols = inputArr1.GetLength(1);

            var output = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    output[i, j] = evaluate(inputArr1[0, j], inputArr2[i, 0]);

            return output;
        }
    }

    private static object ArrayEvaluate(Func<object, object> evaluate, object input)
    {
        if (input is object[,] inputArr)
        {
            var rows = inputArr.GetLength(0);
            var cols = inputArr.GetLength(1);
            var output = new object[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    output[i, j] = evaluate(inputArr[i, j]);

            return output;
        }
        else
        {
            return evaluate(input);
        }
    }
}
