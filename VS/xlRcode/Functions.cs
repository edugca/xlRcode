using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using ExcelDna.Integration;
using ExcelDna.Logging;
using ExcelDna.Registration;
using ExcelDna.Integration.CustomUI;

using RDotNet;
using RDotNet.Utilities;
using RDotNet.Devices;
using RDotNet.Internals;

using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Data;
using System.Reflection.Emit;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using static xlRcode.Global;
using Image = System.Drawing.Image;

namespace xlRcode
{

    public static class MyFunctions
    {

        public static object TestRDotNet()
        {
            // .NET Framework array to R vector.
            NumericVector group1 = _engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
            _engine.SetSymbol("group1", group1);
            // Direct parsing from R script.
            NumericVector group2 = _engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();

            // Test difference of mean and get the P-value.
            GenericVector testResult = _engine.Evaluate("t.test(group1, group2)").AsList();
            double p = testResult["p.value"].AsNumeric().First();

            return string.Format("Group1: [{0}], Group2: [{1}], P-value = {2:0.000}", string.Join(", ", group1), string.Join(", ", group2), p);
        }


        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object XLRFUNCTION(string functionName, string functionComplement, params object[] paramsList)
        {
            int paramsCount = paramsList.Length;
            string[] paramsListTypeNames = new string[paramsList.Length];
            string[] paramsString = new string[paramsList.Length];

            REnvironment env = _engine.GlobalEnvironment;

            for (int i = 0; i < paramsList.Length; i++)
            {
                paramsListTypeNames[i] = paramsList[i].GetType().ToString();

                xlR_ArgumentParser("xlR_param_" + i, paramsListTypeNames[i], paramsList[i], env);

                paramsString[i] = "xlR_param_" + i;
                
            }


            string expression = functionName + "(" + string.Join(",", paramsString) + ")" + functionComplement;
            RichTextBox tbConsoleExcel = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleExcel"].Controls["tbConsoleExcel"];
            WinFormsExtensions.AppendLine(tbConsoleExcel, System.Environment.NewLine + expression, Color.Black, SetUp.rConsoleLineLimit);
            SymbolicExpression result = _engine.Evaluate(expression);

            return xlR_ConvertOutput(result);
            
        }


        private static void xlR_ArgumentParser(string paramName, string  paramsListTypeNames, object paramsList, REnvironment env)
        {

            switch (paramsListTypeNames)
            {
                case "System.Int":
                    long dummyLong = Convert.ToInt64(paramsList);
                    _engine.SetSymbol(paramName, _engine.CreateNumeric(dummyLong), env);
                    break;
                case "System.Double":
                    double dummyDouble = Convert.ToDouble(paramsList);
                    _engine.SetSymbol(paramName, _engine.CreateNumeric(dummyDouble), env);
                    break;
                case "System.String":
                    string dummyString = Convert.ToString(paramsList);
                    _engine.SetSymbol(paramName, _engine.CreateCharacter(dummyString), env);
                    break;
                case "System.Object[,]":

                    // CHECK DATA TYPES!!!
                    string subTypeName;
                    if (paramName.StartsWith("n_"))
                    {
                        subTypeName = "System.Double";
                    }
                    else if (paramName.StartsWith("s_"))
                    {
                        subTypeName = "System.String";
                    }
                    else if (paramName.StartsWith("df_"))
                    {
                        subTypeName = "DataFrame";
                    }
                    else
                    {
                        subTypeName = ((object[,])paramsList)[0, 0].GetType().ToString();
                    }


                    switch (subTypeName)
                    {
                        case "System.Double":
                            
                            double[,] dummyDoubleMatrix = new double[((object[,])paramsList).GetUpperBound(0) + 1,
                                                  ((object[,])paramsList).GetUpperBound(1) + 1];

                            //Cast and fill each item
                            for (int x = 0; x <= ((object[,])paramsList).GetUpperBound(0); x++)
                            {
                                for (int y = 0; y <= ((object[,])paramsList).GetUpperBound(1); y++)
                                {
                                    dummyDoubleMatrix[x, y] = (double)((object[,])paramsList)[x, y];
                                }
                            }

                            _engine.SetSymbol(paramName, _engine.CreateNumericMatrix(dummyDoubleMatrix), env);
                            break;
                        case "System.String":
                            string[,] dummyStringMatrix = new string[((object[,])paramsList).GetUpperBound(0) + 1,
                                                 ((object[,])paramsList).GetUpperBound(1) + 1];

                            //Cast and fill each item
                            for (int x = 0; x <= ((object[,])paramsList).GetUpperBound(0); x++)
                            {
                                for (int y = 0; y <= ((object[,])paramsList).GetUpperBound(1); y++)
                                {
                                    dummyStringMatrix[x, y] = (string)((object[,])paramsList)[x, y];
                                }
                            }

                            _engine.SetSymbol(paramName, _engine.CreateCharacterMatrix(dummyStringMatrix), env);
                            break;

                        case "DataFrame":
                            string[,] dummyDFMatrix = new string[((object[,])paramsList).GetUpperBound(0) + 1,
                                                 ((object[,])paramsList).GetUpperBound(1) + 1];

                            //Cast and fill each item
                            for (int x = 0; x <= ((object[,])paramsList).GetUpperBound(0); x++)
                            {
                                for (int y = 0; y <= ((object[,])paramsList).GetUpperBound(1); y++)
                                {
                                    dummyDFMatrix[x, y] = ((object[,])paramsList)[x, y].ToString();
                                }
                            }

                            _engine.SetSymbol(paramName, _engine.CreateCharacterMatrix(dummyDFMatrix), env);
                            _engine.Evaluate(paramName + "<- as.data.frame(" + paramName + ", stringsAsFactors = FALSE)", env);
                            break;

                    }
                    break;
                case "ExcelDna.Integration.ExcelEmpty":
                    _engine.Evaluate(paramName + " = NULL;", env);
                    break;
            }

        }

        
        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object XLRGRAPH(string commands, string graphName, string graphPath, params object[] paramsList)
        {

            string environment = String.Empty;

            commands = "local({" + commands + "})";

            object result = XLRGRAPH_ENV(commands, environment, graphName, graphPath, paramsList);

            return result;

        }

        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object XLRGRAPH_ENV(string commands, string environment, string graphName, string graphPath, object graphCell, params object[] paramsList)
        {

            object result = XLRCODE_ENV(commands, environment, paramsList);

            // Check graph file path
            if (!File.Exists(graphPath))
            {
                return "# Graph file path does not exist";
            }

            Microsoft.Office.Interop.Excel.Application xlApp = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;
            Microsoft.Office.Interop.Excel.Worksheet ws = xlApp.ActiveSheet;

            //Create image 
            float leftPosition;
            float topPosition;
            float height;
            float width;
            try
            {
                Microsoft.Office.Interop.Excel.Shape oldImage = ws.Shapes.Item(graphName);

                //Gathering information of an old image
                leftPosition = oldImage.Left;
                topPosition = oldImage.Top;
                height = oldImage.Height;
                width = oldImage.Width;

                //Removing the old image
                oldImage.Delete();
            }
            catch 
            {
                leftPosition = 0;
                topPosition = 0;
                height = 100;
                width = 100;
            }
            
            
            //Replace the image with new one and assigning its bounds
            Image newImageFile = Image.FromFile(graphPath);
            Microsoft.Office.Interop.Excel.Shape newImage = ws.Shapes.AddPicture(
                graphPath,
                Microsoft.Office.Core.MsoTriState.msoTrue,
                Microsoft.Office.Core.MsoTriState.msoTrue,
                leftPosition,
                topPosition,
                newImageFile.Width, //width,
                newImageFile.Height //height
                );
            newImage.Name = graphName;


            return true;

        }


        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object XLRCODE(string commands, params object[] paramsList)
        {

            string environment = String.Empty;

            commands = "local({" + commands + "})";

            object result = XLRCODE_ENV(commands, environment, paramsList);

            return result;

        }


        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object XLRCODE_ENV(string commands, string environment, params object[] paramsList)
        {

            // Define environment
            REnvironment env = _engine.GlobalEnvironment;
            if (environment.Length != 0)
            {
                if (_engine.Evaluate("exists('" + environment + "', mode='environment')", env).AsLogical()[0] == false)
                {
                    _engine.Evaluate(environment + " <- new.env()", env); 
                }
                env = _engine.Evaluate(environment, env).AsEnvironment();
            }
            

            int paramsCount = paramsList.Length;
            string[] paramsListTypeNames = new string[paramsList.Length];
            string[] paramsString = new string[paramsList.Length];

            for (int i = 0; i < paramsList.Length; i += 2)
            {
                paramsListTypeNames[i+1] = paramsList[i+1].GetType().ToString();

                paramsString[i] = (string)paramsList[i];
                xlR_ArgumentParser(paramsString[i], paramsListTypeNames[i+1], paramsList[i+1], env);

            }

            SymbolicExpression result;
            RichTextBox tbConsoleExcel = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleExcel"].Controls["tbConsoleExcel"];
            //RichTextBox tbConsoleExcel = (RichTextBox)((consoleControl)xlRcode.CTPManager.ctp).Controls["tbConsoleExcel"];
            try
            {
                WinFormsExtensions.AppendLine(tbConsoleExcel, "> " + commands + System.Environment.NewLine, Color.Blue, SetUp.rConsoleLineLimit);
                result = _engine.Evaluate(commands, env);
            }
            catch {
                result = _engine.Evaluate("geterrmessage()");
                WinFormsExtensions.AppendLine(tbConsoleExcel, result + System.Environment.NewLine, Color.Red, SetUp.rConsoleLineLimit);
            }

            //Block selection
            tbConsoleExcel.SelectAll();
            tbConsoleExcel.SelectionProtected = true;
            tbConsoleExcel.Select(tbConsoleExcel.Text.Length, 0);
            tbConsoleExcel.Tag = tbConsoleExcel.Text.Length; //keep the position of the last protected character

            return xlR_ConvertOutput(result);

        }


        public static string XLRCODE_Routine(string commands, bool silentCommands = false)
        {

            REnvironment env = _engine.GlobalEnvironment;

            SymbolicExpression result;

            // Define result string
            string dummyResultString = String.Empty;
            string msgWarnings = String.Empty;
            bool isError = false;
            bool isVisible = true;

            // Adapt commands to show visibility status
            //string adaptedCommands = "withVisible(" + commands + ");";

            // Adapt commands to capture warnings
            //string preCommands = "xlRcode_warningsAfter <- NULL; xlRcode_warningsBefore <- warnings(); assign('last.warning', NULL, envir = baseenv());";
            //string postCommands = "xlRcode_warningsAfter <- warnings(); if( length(last.warning)==0 ){ assign('last.warning', xlRcode_warningsBefore, envir = baseenv()) };";

            RichTextBox tbConsoleCode = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleCode"].Controls["tbConsoleCode"];
            try
            {
                if (!silentCommands)
                {
                    WinFormsExtensions.AppendLine(tbConsoleCode, commands + System.Environment.NewLine, Color.Blue, SetUp.rConsoleLineLimit);
                }
                else { WinFormsExtensions.AppendLine(tbConsoleCode, System.Environment.NewLine, Color.Blue, SetUp.rConsoleLineLimit);  }

                // Check if code is syntatically perfect
                SymbolicExpression hasParsed = _engine.Evaluate("str2expression('" + commands + "')", env);

                //_engine.Evaluate(preCommands, env);
                result = _engine.Evaluate(commands, env);
                //SymbolicExpression msgWarningsObj = _engine.Evaluate("last.warning", env);
                //_engine.Evaluate(postCommands, env);

                // Check visibility of result
                //isVisible = result.AsList()[1].AsLogical()[0];
                //result = result.AsList()[0];

                // Get warnings
                //if (msgWarningsObj.Type.ToString() != "Null")
                //{
                //    msgWarnings = msgWarningsObj.AsCharacter().ToString();
                //}
            }
            catch
            {
                isError = true;
                result = _engine.Evaluate("geterrmessage()", env);
            }

            
            //Block selection
            tbConsoleCode.SelectAll();
            tbConsoleCode.SelectionProtected = true;
            tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
            tbConsoleCode.Tag = tbConsoleCode.Text.Length; //keep the position of the last protected character

            // Convert result into matrix
            if (!isVisible)
            {
                //return dummyResultString;
            }
            else
            {
                // Check for null result
                if (result == null || result.Type.ToString() == "Null")
                {
                    //return dummyResultString;
                }
                else
                {
                    try
                    {
                        CharacterMatrix resultObj = result.AsCharacterMatrix();
                        int nRows = resultObj.RowCount;
                        int nCols = resultObj.ColumnCount;
                        bool resultIsList = result.IsList();

                        CharacterMatrix listNames = null;
                        if (resultIsList) 
                        {
                            listNames = result.GetAttribute("names").AsCharacterMatrix();
                        }

                        if (nRows == 1 && nCols == 1)
                        {
                            if (!isError)
                            {
                                if (!resultIsList)
                                {
                                    dummyResultString += "[1] " + resultObj.AsCharacter()[0];
                                }
                                else
                                {
                                    dummyResultString += "$" + listNames[0,0] + Environment.NewLine + resultObj.AsCharacter()[0];
                                }
                                
                            }
                            else 
                            {
                                dummyResultString += resultObj.AsCharacter()[0];
                            }
                        }
                        else if (nCols == 1)
                        {
                            CharacterVector resultObjVec = result.AsCharacter();

                            if (!resultIsList)
                            {
                                dummyResultString += "\t[,1]" + Environment.NewLine;

                                for (int x = 0; x <= nRows - 1; x++)
                                {
                                    dummyResultString += "[" + (x + 1).ToString() + ",]" + "\t";
                                    dummyResultString += resultObjVec[x];
                                    if (x < nRows - 1) { dummyResultString += Environment.NewLine; }
                                }
                            }
                            else 
                            {
                                for (int x = 0; x <= nRows - 1; x++)
                                {
                                    dummyResultString += "$" + listNames[x, 0] + Environment.NewLine + resultObjVec[x];
                                    if (x < nRows - 1) { dummyResultString += Environment.NewLine; }
                                }
                            }    

                        }
                        else
                        {
                            string[] matrixColHeaderArray = new string[nCols];

                            // Matrix column header
                            for (int y = 0; y <= nCols - 1; y++)
                            {
                                matrixColHeaderArray[y] =  "[," + (y+1).ToString() + "]";
                            }
                            dummyResultString += "\t" + String.Join("\t", matrixColHeaderArray) + Environment.NewLine;

                            // Matrix items
                            for (int x = 0; x <= nRows - 1; x++)
                            {
                                dummyResultString += "[" + (x + 1).ToString() + ",]" + "\t";

                                for (int y = 0; y <= nCols - 1; y++)
                                {
                                    dummyResultString += resultObj[x, y] + "\t";
                                }

                                if (x < nRows - 1) { dummyResultString += Environment.NewLine; }
                            }
                        }
                    }
                    catch (Exception ex) { dummyResultString = ex.ToString(); }
                }

            }

            // Append warnings
            if (msgWarnings.Length != 0) {
                if (dummyResultString.Length != 0)
                {
                    dummyResultString += Environment.NewLine + msgWarnings;
                }
                else 
                {
                    dummyResultString += msgWarnings;
                }
            }

            Color msgColor = Color.Blue;
            if (isError)
            {
                msgColor = Color.Red; 
            }
            else
            {
                msgColor = Color.Black;
                if (dummyResultString.Length != 0) { dummyResultString += Environment.NewLine; }
            }
            
            WinFormsExtensions.AppendLine(tbConsoleCode, dummyResultString, msgColor, SetUp.rConsoleLineLimit);

            return dummyResultString;

        }

        private static object xlR_ConvertOutput(SymbolicExpression result)
        {
            string[] attributesList;

            if (result is null){
                return null;
            }

            string resultType = result.Type.ToString();

            string[] numericTypes = { "NumericVector", "NumericMatrix", "IntegerVector", "IntegerMatrix" };
            string[] complexTypes = { "ComplexVector", "ComplexMatrix" };
            string[] stringTypes = { "CharacterVector", "CharacterMatrix" };
            string[] logicalTypes = { "LogicalVector", "LogicalMatrix" };

            switch (resultType)
            {
                case "NumericVector":
                case "NumericMatrix":
                case "IntegerVector":
                case "IntegerMatrix":
                case "ComplexVector":
                case "ComplexMatrix":
                case "CharacterVector":
                case "CharacterMatrix":
                case "LogicalVector":
                case "LogicalMatrix":

                    var dummyListAux = (dynamic)null;

                    int nRows;
                    int nCols;

                    if (numericTypes.Contains(resultType))
                    {
                        dummyListAux = result.AsNumericMatrix();
                        nRows = dummyListAux.RowCount;
                        nCols = dummyListAux.ColumnCount;
                    }
                    else if ( stringTypes.Contains(resultType) ) {
                        dummyListAux = result.AsCharacterMatrix();
                        nRows = dummyListAux.RowCount;
                        nCols = dummyListAux.ColumnCount;
                    }
                    else if (logicalTypes.Contains(resultType))
                    {
                        dummyListAux = result.AsLogicalMatrix();
                        nRows = dummyListAux.RowCount;
                        nCols = dummyListAux.ColumnCount;
                    }
                    else
                    {
                        dummyListAux = result.AsComplexMatrix();
                        nRows = dummyListAux.RowCount;
                        nCols = dummyListAux.ColumnCount;

                        dummyListAux = ((ComplexMatrix)dummyListAux).AsCharacterMatrix();
                    }


                    attributesList = result.GetAttributeNames();
                    if (
                        result.GetAttributeNames().Contains("names") || result.GetAttributeNames().Contains("dimnames")
                        )
                    {

                        var dummyList = dummyListAux;

                        bool hasRowNames = false;
                        bool hasColNames = false;
                        string rowNamesField;
                        string colNamesField;
                        string[] rowNames = Array.Empty<string>();
                        string[] colNames = Array.Empty<string>();
                        
                        if (result.GetAttributeNames().Contains("names")) {
                            hasRowNames = true;
                            rowNamesField = "names";
                            rowNames = result.GetAttribute(rowNamesField).AsCharacter().ToArray();
                        }
                        else
                        {
                            hasRowNames = true;
                            hasColNames = true;
                            rowNamesField = "dimnames";
                            colNamesField = "dimnames";
                            rowNames = _engine.Evaluate(result.GetAttribute(rowNamesField).AsCharacter().ToArray()[0]).AsCharacter().ToArray();
                            colNames = _engine.Evaluate(result.GetAttribute(colNamesField).AsCharacter().ToArray()[1]).AsCharacter().ToArray();
                        }


                        int rowShift = hasColNames ? 1 : 0;
                        int colShift = hasRowNames ? 1 : 0;

                        object[,] dummyListObject = new object[dummyList.RowCount + rowShift, dummyList.ColumnCount + colShift];


                        //Cast and fill column and row names
                        if (hasRowNames & hasColNames) { dummyListObject[0, 0] = ""; }
                        if (hasRowNames){
                            for (int x = rowShift; x <= nRows - 1 + rowShift; x++)
                            {
                                dummyListObject[x, 0] = rowNames[x - rowShift];
                            }
                        }
                        if (hasColNames) {
                            for (int y = colShift; y <= nCols - 1 + colShift; y++)
                            {
                                dummyListObject[0, y] = colNames[y - colShift];
                            }
                        }

                        //Cast and fill each item
                        for (int x = 0; x <= nRows - 1; x++)
                        {
                            for (int y = 0; y <= nCols - 1; y++)
                            {
                                dummyListObject[x + rowShift, y + colShift] = (dummyList)[x, y];
                            }
                        }

                        //Convert NaN to Excel Error
                        dummyListObject = checkNaN(dummyListObject, ExcelDna.Integration.ExcelError.ExcelErrorNum, ExcelDna.Integration.ExcelError.ExcelErrorNull);

                        return dummyListObject;
                    }
                    else {

                        object[,] dummyListObject = new object[nRows, nCols];

                        //Cast and fill each item
                        for (int x = 0; x <= nRows - 1; x++)
                        {
                            for (int y = 0; y <= nCols - 1; y++)
                            {
                                dummyListObject[x, y] = (dummyListAux)[x, y];
                            }
                        }

                        //Convert NaN to Excel Error
                        dummyListAux = checkNaN(dummyListObject, ExcelDna.Integration.ExcelError.ExcelErrorNum, ExcelDna.Integration.ExcelError.ExcelErrorNull);

                        // Check whether it is an empty array
                        if (dummyListAux.Length != 0)
                        {
                            return dummyListAux;
                        }
                        else { return String.Empty; }


                    }
                        
                case "List":

                    // Check whether it is a data frame
                    attributesList = result.GetAttributeNames();
                    if (
                        result.GetAttributeNames().Contains("class") &&
                        result.GetAttributeNames().Contains("names") &&
                        result.GetAttributeNames().Contains("row.names")
                        )
                    {
                        var dummyDFList = result.AsDataFrame();
                        object[,] dummyListObject = new object[dummyDFList.RowCount + 1, dummyDFList.ColumnCount + 1];

                        var rowNames = dummyDFList.RowNames;
                        var colNames = dummyDFList.ColumnNames;

                        //Cast and fill column and row names
                        dummyListObject[0, 0] = "";
                        for (int x = 1; x <= (dummyDFList).RowCount; x++)
                        {
                            dummyListObject[x, 0] = rowNames[x-1];    
                        }
                        for (int y = 1; y <= (dummyDFList).ColumnCount; y++)
                        {
                            dummyListObject[0, y] = colNames[y-1];
                        }

                        //Cast and fill each item
                        for (int x = 0; x <= (dummyDFList).RowCount - 1; x++)
                        {
                            for (int y = 0; y <= (dummyDFList).ColumnCount - 1; y++)
                            {
                                dummyListObject[x+1, y+1] = (dummyDFList)[x, y];
                            }
                        }

                        //Convert NaN to Excel Error
                        dummyListObject = checkNaN(dummyListObject, ExcelDna.Integration.ExcelError.ExcelErrorNum, ExcelDna.Integration.ExcelError.ExcelErrorNull);

                        return dummyListObject;
                    }
                    else
                    {
                        // UNIDIMENSIONAL LIST
                        var dummyList = result.AsList();
                        object[] dummyListObject = new object[dummyList.Length];

                        //Cast and fill each item
                        for (int x = 0; x <= (dummyList).Length - 1; x++)
                        {
                            if (numericTypes.Contains( (dummyList)[x].Type.ToString() ) )
                            {
                                dummyListObject[x] = (dummyList)[x].AsNumericMatrix().ToArray()[0,0];
                            }
                        }

                        //Convert NaN to Excel Error
                        dummyListObject = checkNaN(dummyListObject, ExcelDna.Integration.ExcelError.ExcelErrorNum, ExcelDna.Integration.ExcelError.ExcelErrorNull);

                        return dummyListObject;    
                    }

                case "Null":
                    return null;

                case "S4":
                default:
                    return "#Result is of type " + resultType + " (convert to other)";
            }
        }

        private static object[,] checkNaN(object[,] myArray, ExcelError excelErrorNum, ExcelError excelErrorNull)
        {

            for (int x = 0; x <= myArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= myArray.GetUpperBound(1); y++)
                {
                    if (myArray[x,y] == null)
                    {
                        myArray[x, y] = excelErrorNull;
                    }
                    else if (myArray[x, y].GetType().ToString() == "System.Double") 
                    {
                        if (double.IsNaN((double)myArray[x, y]))
                        {
                            myArray[x, y] = excelErrorNum;
                        }
                    }
                }
            }

            return myArray;

        }

        private static object[] checkNaN(object[] myArray, ExcelError excelErrorNum, ExcelError excelErrorNull)
        {

            for (int x = 0; x <= myArray.GetUpperBound(0); x++)
            {
                if (myArray[x] == null)
                {
                    myArray[x] = excelErrorNull;
                }
                else if(myArray[x].GetType().ToString() == "System.Double")
                {
                    if (double.IsNaN((double)myArray[x]))
                    {
                        myArray[x] = excelErrorNum;
                    }
                }
            }

            return myArray;

        }

        

        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object TimerNow()
        {
            return XlCall.RTD("xlRcode.RtdServer", null, "NOW");
        }
        
        [ExcelFunction(HelpTopic = "http://www.google.com")]
        public static object TimerWave(double amplitude, double frequency)
        {
            return XlCall.RTD("xlRcode.RtdServer", null, "WAVE", amplitude.ToString(), frequency.ToString());
        }

    }

    public static class Helpers
    {
        public static DataTable ArraytoDatatable(Object[,] numbers)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < numbers.GetLength(1); i++)
            {
                dt.Columns.Add("Column" + (i + 1));
            }

            for (var i = 0; i < numbers.GetLength(0); ++i)
            {
                DataRow row = dt.NewRow();
                for (var j = 0; j < numbers.GetLength(1); ++j)
                {
                    row[j] = numbers[i, j];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    public static class WinFormsExtensions
    {
        public static void AppendLine(this RichTextBox source, string value, Color textColor, int? maxLine = null)
        {

            /*max line check*/
            if (maxLine != null && maxLine > 0)
            {
                if (source.Lines.Count() >= maxLine)
                {
                    List<string> lines = source.Lines.ToList();
                    lines.RemoveRange(0, value.Split(System.Environment.NewLine.ToCharArray()).Length);
                    source.Lines = lines.ToArray();
                }
            }


            source.SelectionStart = source.TextLength;
            source.SelectionLength = 0;
            source.SelectionColor = textColor;

            if (source.Text == "> ")
                if (value.StartsWith("> "))
                { source.Text = value; }
                else { source.Text = "> " + value;  }
            else
                source.AppendText(value);

            source.SelectionStart = source.TextLength;
            source.SelectionLength = 0;
            source.SelectionColor = source.ForeColor;
            source.ScrollToCaret(); //Scroll down to the cursor
        }

        public static string ReadLine(Control tbConsole)
        {
            DisableControls(tbConsole);

            myfConsole.ShowDialog();
            tbConsole.Select();

            /////////////////////////////
            RichTextBox tb = (RichTextBox)tbConsole;
            int lastProtected = (int)tb.Tag;
            string userInput = tb.Text.Substring(lastProtected);
            ///////////////////////////

            EnableControls(tbConsole);

            return userInput;
        }

        public static Task<String> ConsoleReadLine(Control tb)
        {
            TaskCompletionSource<String> tcs = new TaskCompletionSource<String>();

            myfConsole.VisibleChanged += (o, e) =>
            {
                tcs.SetResult(tb.Text);
            };
            myfConsole.ShowDialog();

            return tcs.Task;
        }

        private static void DisableControls(Control con)
        {
            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            con.Enabled = false;
        }

        private static void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }

    }

    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }
    }

}
