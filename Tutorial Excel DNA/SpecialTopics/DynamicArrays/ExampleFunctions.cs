using System;
using System.Threading;
using ExcelDna.Integration;

public static class ExampleFunctions
{
    // Returns a rows x cols sized array with some values
    public static object[,] dnaMakeArray(int rows, int cols)
    {
        object[,] result = new object[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = $"{i},{j}";
            }
        }
        return result;
    }

    // Describes the size of the input array
    public static object dnaDescribeArray(object[,] input)
    {
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);

        return $"Input: {rows} rows, {cols} columns";
    }

    public static object dnaAddPrefix(string prefix, object[,] input)
    {
        var rows = input.GetLength(0);
        var cols = input.GetLength(1);

        var result = new object[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i,j] = $"{prefix}{input[i, j]}";
            }
        }
        return result;
    }

    public static double dnaAddThem(double val1, double val2)
    {
        return val1 + val2;
    }

    // To implement an array-aware function, we sometimes need to decide how to process various size combinations
    public static object[,] dnaConcatenate(string separator, object[,] val1, object[,] val2)
    {
        int rows1 = val1.GetLength(0);
        int cols1 = val1.GetLength(1);
        int rows2 = val2.GetLength(0);
        int cols2 = val2.GetLength(1);

        if (rows1 == rows2 && cols1 == cols2)
        {
            // Same shapes, operate elementwise
            object[,] result = new object[rows1, cols1];
            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols1; j++)
                {
                    result[i, j] = $"{val1[i, j]}{separator}{val2[i, j]}";
                }
            }
            return result;
        }

        if (rows1 > 1)
        {
            // Lots of rows in input1, we'll take its first column only, and take the columns of input2
            var rows = rows1;
            var cols = cols2;

            var output = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    output[i, j] = $"{val1[i, 0]}{separator}{val2[0, j]}";

            return output;
        }
        else
        {

            // Single row in input1, we'll take its columns, and take the rows from input2
            var rows = rows2;
            var cols = cols1;

            var output = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    output[i, j] = $"{val1[0, j]}{separator}{val2[i, 0]}";

            return output;
        }
    }

    // Functions can take ExcelReference references, as well as return them
    // On the sheet the ExcelReference result will be converted to the values,
    // but when input to another function, the ExcelReference is passed directly
    public static object dnaArrayGetHead([ExcelArgument(AllowReference = true)] object input, int numRows)
    {
        if (input is ExcelReference inputRef)
        {
            var rowFirst = inputRef.RowFirst;
            var rowLast = Math.Min(inputRef.RowFirst + numRows, inputRef.RowLast);
            return new ExcelReference(rowFirst, rowLast, inputRef.ColumnFirst, inputRef.ColumnLast, inputRef.SheetId);
        }
        else if (input is object[,] inputArray)
        {
            var rows = inputArray.GetLength(0);
            var cols = inputArray.GetLength(1);

            var resultRows = Math.Min(rows, numRows);
            var result = new object[resultRows, cols];
            for (int i = 0; i < resultRows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = inputArray[i, j];
                }
            }
            return result;
        }
        else
        {
            // Just a scalar value
            if (numRows >= 1)
            {
                return input;
            }
        }
        // Otherwise we have an error - return #VALUE!
        return ExcelError.ExcelErrorValue;
    }

    // RTD-based async and streaming functions work correctly with dyncami arrays
    public static object dnaMakeArrayAsync(int delayMs, int rows, int cols)
    {
        var funcName = nameof(dnaMakeArrayAsync);
        var args = new object[] { delayMs, rows, cols };

        return ExcelAsyncUtil.Run(funcName, args, () =>
        {
            Thread.Sleep(delayMs);
            object[,] result = new object[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = $"{i}|{j}";
                }
            }
            return result;
        });
    }
}
