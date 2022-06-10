# Argument Type Basics

This discussion is about classic data types like Double, String and Boolean that are used in Excel worksheets, and are passed into and back from VBA or .NET user-defined functions.  I want to explain how different data types are used by Excel for the worksheet data, in VBA for UDF arguments and variable values and correspondingly for UDFs created in the .NET type system. I'll discuss the  basic data types as they appear in these different settings, with a specific focus on user-defined functions (UDFs) created in VBA vs. those created in Excel-DNA with the .NET languages.

[![Excel-DNA - UDF Argument Types from VBA to .NET](http://img.youtube.com/vi/6x0rrFY8egA/0.jpg)](http://www.youtube.com/watch?v=6x0rrFY8egA "Excel-DNA - UDF Argument Types from VBA to .NET")

The new [Linked Data Types](https://support.microsoft.com/en-us/office/excel-data-types-stocks-and-geography-61a33056-9935-484f-8ac8-f1a89e210877) feature of Excel allow extended types like 'Geography' and properties like 'Location' for data exploration in Excel. These 'Linked Data Types' won't be covered in this 'Basics' topic.

Other data type related issues that I don't cover in detail in this 'Basics' topic include:
* Dynamic arrays and how they work with UDFs.
* Data types when dealing with the COM object model from .NET.
* Data transformations in the Excel-DNA UDF implementation - e.g. `Task<>` types for async functions, and 'object handles' that represent references to internal data structures.

> **Argument vs Parameter**
>
> I tend to use these two terms inconsistently, and when I first created Excel-DNA I followed the Excel C API documentation usage, which led me somewhat astray.
> On [StackOverflow](https://stackoverflow.com/questions/156767/whats-the-difference-between-an-argument-and-a-parameter#:~:text=A%20parameter%20is%20a%20variable,pass%20into%20the%20method's%20parameters.&text=Parameter%20is%20variable%20in%20the,that%20gets%20passed%20to%20function.) there is a clear definition of these terms:
> "A parameter is a variable in a method definition. When a method is called, the arguments are the data you pass into the method's parameters. Parameter is variable in the declaration of function. Argument is the actual value of this variable that gets passed to function."

## Worksheet value types

A cell in Excel is empty or it has a value and possibly also a formula associated with it. If it has a formula, the cell has a value that is computed from the formula.
The cell values will have one of the following value types:
* **Number (Double)** - always represented by a 64-bit floating point number
* **Text (String)** - represented as a Unicode string with at most 32767 characters. An empty (0-length) string is not the same as an empty cell.
* **Logical (Boolean)** - either TRUE or FALSE
* **Error** - the various possible Excel error values like #VALUE!
* **Empty** - a cell is never empty if the cell has a formula - the formula or UDF can't result in an 'empty' cell value - though possibly in an empty (0-length) string

For a UDF called from an Excel sheet, there are a few other types involved.
A parameter of the UDF might have additional value types:
* **Missing** - if no value is specified for the parameter in the formula
* **Array** of basic values - 1D or 2D array of scalar values, from an array literal, a sheet array reference or from a Dynamic Arrays spill range reference.
* **Sheet Reference** (Range in VBA) - one area where VBA and Excel-DNA will differ a bit and is discussed in detail below.

> **Notes**
> * There is no date / time value type in the list. For Excel, Date/Time is a formatting options for internal double values that represent the date and time. So date/time display formatting is similar to font selection or colours applied to a cell - it is a display option and not an internal value. But as well soon see, cell values that are formatted as dates are sometimes interpreted as DateTime values in VBA. (Specifically, the `Range.Value` property sometimes returns a COM DateTime or Currency value, in constrast with the `Range.Value2` property which returns a `double` in both these cases.)
> * Similar to date / time, there is no special currency type, with the above caveat about conversion in the `Range.Value` property.
> * A cell itself cannot contain an array of values. With the 'Dynamic Arrays' feature, a cell can become the anchor cell for an spill region.
> * Excel does not store integer (whole number) values in a cell - it uses floating point values to represent all numbers.
> * Values from Defined Names will have the same set of value types as for cells, but can also evaluate to array values.

## Worksheet with various types
In the worksheet 'ArgumentTypes.xlsm' that accompanies this discussion I show all the different data types that cells can have, and how they are described by the built-in Excel `=TYPE(...)` function.

## UDF argument types in VBA

In this section I look at how the worksheet data types relate to VBA types for user-defined functions.
The exact formulas and VBA used for this workbook can be check in the accompaying [ArgumentTypeBasics.xlsm Workbook](ArgumentTypeBasics.xlsm).

Consider a UDF that has one parameter. In VBA we'd write that most simply as 
```vb
Function MyFunc(input)
End Function
```

If we're using data type declarations, as is required with 'Option Explicit', the equivalent declaration with explicit type annotations would be:
```vb
Function MyFunc(input as Variant) As Variant
End Function
```

The [VBA Variant data type](https://docs.microsoft.com/en-us/office/vba/language/reference/user-interface-help/variant-data-type) is a special type that can contain any type of value. We'll see later that the equivalent type in .NET is called `Object`.

We can examine the actual type of a Variant value using the [`VarType`](https://docs.microsoft.com/en-us/office/vba/language/reference/user-interface-help/vartype-function) function, and some other helpers. One catch with using `VarType` is that if you pass it a `Variant` that contains an object (like a `Range`), then `VarType` will return the type of the default property of the object. So we can't use `VarType` to distinguish between a `Double` and a `Range` object where the default property `Range.Value` returns a `Double` value. Therefore we check for the `Range` case separately. The full code is in the workbook that accompanies this tutorial, but the main function looks like this:

```vb
' Describes the type and details of the argument passed in when the UDF is called.
Function vbaArgumentInfo(Optional arg As Variant) As String
    Dim value
    Dim value2

    If IsMissing(arg) Then
        vbaArgumentInfo = "<Missing>"
    ElseIf IsObject(arg) And TypeOf arg Is Range Then
        value = arg.Value
        value2 = arg.Value2
        If varType(value) = varType(value2) Then
            vbaArgumentInfo = "Range(" & arg.Address & "): " & vbaArgumentInfo(value)
        Else
            vbaArgumentInfo = "Range(" & arg.Address & "): " & vbaArgumentInfo(value) & " (" & vbaArgumentInfo(value2) & ")"
        End If
    ElseIf IsArray(arg) Then
        vbaArgumentInfo = "Array" + ArraySize(arg)
    Else
        vbaArgumentInfo = VarTypeName(varType(arg)) & ": " & CStr(arg)
    End If
        
End Function

' Return a string with the description of the variant type passed in
Function VarTypeName(varType As Integer)
    Select Case varType
        Case vbEmpty: ' 0
            VarTypeName = "vbEmpty" ' Empty (uninitialized)
            
        ' Many more cases like this are not shown ....
        
    End Select
End Function
```

On the 'ArgumentValue' sheet we can now see how different datatype look on the sheet, when a VBA function is called with a reference, and then with a literal value in the formula:

| Value Type                   | Example Value                     |       | Excel TYPE | Name            | ArgumentInfoVBA - Reference Argument              | ArgumentInfoVBA - Literal Argument |
|------------------------------|-----------------------------------|-------|------------|-----------------|---------------------------------------------------|------------------------------------|
| Empty Cell                   |                                   |       | 1          | Number          | Range($B$2): vbEmpty:                             | <Missing>                          |
| Number                       | 1.234                             |       | 1          | Number          | Range($B$3): vbDouble: 1.234                      | vbDouble: 1.234                    |
|                              | 42                                |       | 1          | Number          | Range($B$4): vbDouble: 42                         | vbDouble: 42                       |
|                              | 9.87E+201                         |       | 1          | Number          | Range($B$5): vbDouble: 9.87E+201                  | vbDouble: 9.87E+201                |
|     Formatted as Date        | 06-Nov-20                         |       | 1          | Number          | Range($B$6): vbDate: 2020/11/06 (vbDouble: 44141) | vbDouble: 44141                    |
|     Formatted as Currency    | R99.99                            |       | 1          | Number          | Range($B$7): vbCurrency: 99.99 (vbDouble: 99.99)  | vbDouble: 99.99                    |
| String                       | Hello, World!                     |       | 2          | Text            | Range($B$8): vbString: Hello, World!              | vbString: Hello, World!            |
|     Empty (0-length)         |                                   |       | 2          | Text            | Range($B$9): vbString:                            | vbString:                          |
| Boolean                      | TRUE                              |       | 4          | Logical Value   | Range($B$10): vbBoolean: True                     | vbBoolean: True                    |
|                              | FALSE                             |       | 4          | Logical Value   | Range($B$11): vbBoolean: False                    | vbBoolean: False                   |
| Error                        | #DIV/0!                           |       | 16         | Error Value     | Range($B$12): vbError: Error 2007                 | vbError: Error 2007                |
|                              | #VALUE!                           |       | 16         | Error Value     | Range($B$13): vbError: Error 2015                 | vbError: Error 2015                |
|                              | #REF!                             |       | 16         | Error Value     | Range($B$14): vbError: Error 2023                 | vbError: Error 2023                |
|                              | #NAME?                            |       | 16         | Error Value     | Range($B$15): vbError: Error 2029                 | vbError: Error 2029                |
|                              | #NUM!                             |       | 16         | Error Value     | Range($B$16): vbError: Error 2036                 | vbError: Error 2036                |
|                              | #N/A                              |       | 16         | Error Value     | Range($B$17): vbError: Error 2042                 | vbError: Error 2042                |
|                              | #SPILL!                           |       | 16         | Error Value     | Range($B$18): vbError: Error 2045                 | NOTE: =ERROR.TYPE(????)            |
| Ctrl+Shift+Enter 2D Array    | 1                                 | A     | 64         | Array           | Range($B$19:$C$20): Array(1:2,1:2)                | Array(1:2,1:2)                     |
|                              | 0.1                               | FALSE |            |                 |                                                   |                                    |
| Dynamic 2D Array             | 99                                | B     | 64         | Array           | Range($B$21:$C$22): Array(1:2,1:2)                | Array(1:2,1:2)                     |
|                              |                                   | TRUE  |            |                 |                                                   |                                    |
| Single Row Array             | 1                                 | A     | 64         | Array           | Range($B$23:$C$23): Array(1:1,1:2)                | Array(1:2)                         |
| Single Column Array          | 1                                 |       | 64         | Array           | Range($B$24:$B$25): Array(1:2,1:1)                | Array(1:2,1:1)                     |
|                              | A                                 |       |            |                 |                                                   |                                    |
| Array Literal in Single Cell | 1                                 |       | 1          | Number          | Range($B$26): vbDouble: 1                         | Array(1:2)                         |
| Linked Data                  | MICROSOFT CORPORATION (XNAS:MSFT) |       | 128        | Linked Data (?) | Range($B$28): vbError: Error 2015                 |                                    |
|                              | 1993                              |       | 1          | Number          | Range($B$29): vbDouble: 1993                      |                                    |
  
  
#### Some observations
* VBA defines a variety of other [data types](https://docs.microsoft.com/en-us/office/vba/language/reference/user-interface-help/data-type-summary) in the language, but these do not appear in function calls from an Excel formula.
* We see the different types returned by `Range.Value` and `Range.Value2` for numbers formatted as date/time or currency on the sheet.
* Arrays read from `Range.Value` are always 2D arrays, but literal arrays passed from the formula are sometimes received as 1D arrays.
* A cell with a Linked Data Type like the Stock data type in the example return a `#VALUE!` error from `Range.Value`. I don't think it is possible to refer to these in a formula directly.

### Implicit conversions

When the parameter type of the VBA function is not `Variant` but something like `String` or `Double`, Excel will attempt to do a data type conversion when calling the function with a different type. Simple function can be used to experiment with these conversion:

That conversion might fail (e.g. if a string if passed to `ArgumentIntegerVBA`) or might round or otherwise change the input to convert it to the parameter type (e.g. when passing a `Double` to the `-Integer`, the number will be rounded).

When using these implicit conversions for argument values, you should check carefully how different input types are interpreted, and that this is consistent with the intention of the function.

```vb
Function vbaArgumentDouble(arg As Double)
    vbaArgumentDouble = arg
End Function

Function vbaArgumentBoolean(arg As Boolean)
    vbaArgumentBoolean = arg
End Function

Function vbaArgumentInteger(arg As Integer)
    vbaArgumentInteger = arg
End Function
```


### Arrays and Range parameters

In VBA there are two options for declaring a function that will process an array input. We can decalre the parameter type to be
* `Variant` - and check for the different variant types passed, or
* `Range` - and then either process each cell in the range or call `Range.Value` (`.Value2) on the range to get a variant with the contents, which we check and process as above.

Consider a function like `=SumEvenValues` where we want to take an array (or range reference of values and then add the even numbers.
A simple implementation (borrowed from the lovely [Excel-Easy](https://www.excel-easy.com/vba/examples/user-defined-function.html) website) might look like this:
```vb
' This is the -R version, which has the input parameter seclared as Range
Function vbaSumEvenNumbersR(rng As Range)
    Dim cell As Range
    Dim sum As Double
    
    For Each cell In rng
        If cell.value Mod 2 = 0 Then
            sum = sum + cell.value
        End If
    Next
    
    vbaSumEvenNumbersR = sum
End Function
```

While simple, this approach to accepting array inputs in function raises a problem, and it will be instructive to try to address it.

Because we are declaring the parameter as 'Range', the function can't be called with the input from a literal array or other function. So these calls will fail: `=vbaSumEvenNumbersR({1,2,3,4})` and `=vbaSumEvenNumbersR(SEQUENCE(100))`.
(Here the SEQUENCE function is only available under Dynamic Array versions of Excel, but `=SEQUENCE(n)` returns an array of numbers `1, 2, 3, ... n`.)

Note that in this function, we are calling `cell.Value` on each cell, which returns a `Variant` containing one of the Excel types as discussed above. If the call value is a string or an error, we should really decide how that is processed in our function. For adding evens it's probably OK to ignore non-nuymeric values, but in other cases the handling of an empty cell might be important (e.g. not counted as a '0' in an `Average` function).

Another problem with the function is that the performance when called with a large input is likely to be bad, because we are eumerating through the cells in the input, and not reading all the values at once. That's a bit of an implementation detail in the algorithm, so I'll focus on the data type issues for now.

An alternative that takes 'Variant' arguments would have to analyze the input more carefully. It is longer, but more explicit about how the argument types are handled.

```vb
' This is the -V version, which has the input parameter seclared as Variant
Function vbaSumEvenNumbersV(arg As Variant)
    Dim i As Long
    Dim j As Long
    Dim rng As Range
    Dim area As Range
    Dim sum As Double
    
    If IsObject(arg) And TypeOf arg Is Range Then
        ' Get the dereferenced values from each Area of the Range and call ourselves with the actual values - then we'll process as below
        Set rng = arg
        For Each area In rng.Areas
            sum = sum + vbaSumEvenNumbersV(area.value)
        Next
    ElseIf IsArray(arg) Then
        If ArrayDim(arg) = 1 Then
            ' 1D array
            For i = LBound(arg, 1) To UBound(arg, 1)
                If arg(i) Mod 2 = 0 Then
                    sum = sum + arg(i)
                End If
            Next
            
        Else
            ' 2D array
            For i = LBound(arg, 1) To UBound(arg, 1)
                For j = LBound(arg, 2) To UBound(arg, 2)
                    If arg(i, j) Mod 2 = 0 Then
                        sum = sum + arg(i, j)
                    End If
                Next
            Next
        End If
    Else
        ' We have a single value - maybe string, bool etc.
        ' We'll process it using whatever type conversion VBA does when evaluating the 'Mod' operator
        ' Alternatively we could examine further
        If arg Mod 2 = 0 Then
            sum = arg
        Else
            sum = 0
        End If
    End If
    
    vbaSumEvenNumbersV = sum
End Function
```

The advantage is that we can now deal with 1D and 2D literal inputs, in addition to the `Range` reference as input.

> Note that there is a little quirk with `Range`s with multiple areas, where calling `Range.Value` will return the values from the first area only. So to really replicate the first function which does `For Each cell in theRange` we need to iterate through all the areas.

On the sheet the different options look like this

| `A` | `B`                                  |
|-----|--------------------------------------|
| `1` | `=vbaSumEvenNumbersR(A1:A5)`          |
| `2` | `=vbaSumEvenNumbersR({1,2,3,4,5})`    |
| `3` | `=vbaSumEvenNumbersV(A1:A5)`         |
| `4` | `=vbaSumEvenNumbersV({1,2,3,4,5})`   |
| `5` | `=vbaSumEvenNumbersV(SEQUENCE(100))` |

With calculated results as discussed


| `A` | `B`       |
|-----|-----------|
| `1` | `6`       |
| `2` | `#VALUE!` |
| `3` | `6`       |
| `4` | `6`       |
| `5` | `2550`    |


## UDF Arguments in .NET

We now consider the data type situation for UDFs defined with Excel-DNA in .NET.
For this discussion we are using a new 'Class Library (.NET Framework)' project created with Visual Basic, and that has the `ExcelDna.AddIn` NuGet package installed.

Our argument description function in VB.NET would look like this:

```vb
Imports ExcelDna.Integration

Public Module Functions

    ' Provides information about the data type and value that is passed in as argument
    Public Function dnaArgumentInfo(arg As Object) As String
    
        If TypeOf arg Is ExcelMissing Then
            Return "<<Missing>>"
        ElseIf TypeOf arg Is ExcelEmpty Then
            Return "<<Empty>>"
        ElseIf TypeOf arg Is Double Then
            Return "Double: " + CDbl(arg)
        ElseIf TypeOf arg Is String Then
            Return "String: " + CStr(arg)
        ElseIf TypeOf arg Is Boolean Then
            Return "Boolean: " + CBool(arg)
        ElseIf TypeOf arg Is ExcelError Then
            Return "ExcelError: " + arg.ToString()
        ElseIf TypeOf arg Is Object(,) Then
            ' The object array returned here may contain a mixture of different types,
            ' corresponding to the different cell contents.
            ' Arrays received will always be 0-based 2D arrays
            Dim argArray(,) As Object = arg
            Return String.Format("Array({0},{1})", argArray.GetLength(0), argArray.GetLength(1))
        Else
            Return "!? Unheard Of ?!"
        End If
        
    End Function
    
End Module
```


### Array argument types

Excel-DNA supports additional array parameter types that VBA does not support for UDFs
* `Object(,)` - if a single scalar value is passed as the argument (either a literal in the formula or a single-cell reference), the function will receive a 1x1 array with the value. This makes the code a bit simpler for functions that  expect to usually get array inputs.
* `Object()` - if a 1D array is declared for the parameter then a single row or column can be passed in, or the first row is taken from a larger array
* `Double(,)` - only called if all values in the input can be converted to doubles by Excel, otherwise Excel will return `#VALUE!` to the sheet.
* `Double()`

Using these, we can re-write the `SumEvenNumbers` like this
```vb
    ' The parameter type is declared as a 2D array.
    ' The function can take a single value, or any rectangular range as input.
    ' Union references with multiple areas will only pass in the first area (?)
    Public Function dnaSumEvenNumbers2D(arg(,) As Object) As Double

        Dim sum As Double
        Dim rows As Integer
        Dim cols As Integer

        rows = arg.GetLength(0)
        cols = arg.GetLength(1)

        For i As Integer = 0 To rows - 1
            For j As Integer = 0 To cols - 1

                Dim val As Object = arg(i, j)
                If val Mod 2 = 0 Then
                    sum += val
                End If

            Next
        Next

        Return sum

    End Function
```

The accompanying add-in project has a few additional array function to experiment with.

### `ExcelReference` and the `AllowReference:=True` option

One difference we see between the Excel-DNA and VBA versions of the argument description function is when a sheet reference is put in the formula.
The Excel-DNA function (with an `Object` parameter) receives the dereferenced value (an array of values if appropriate) directly, while the VBA function (with the `Variant` parameter) will receive a COM `Range` object, from where the value (single or array) can be retrieved or other properties of the `Range` can be read.

In most cases the UDF requires only the values passed in, so the default Excel-DNA behaviour is appropriate. However, sometimes it is useful or required to get information about the reference passed to the function, and not only the value(s) at that reference. For this case, Excel-DNA support an option that is enabled by adding an `<ExcelArgument(AllowReference:=True)>` attribute to the paramter definition. This attribute indicates that a reference in the formula should be passed through to the function.

A version of the above `dnaArgumentInfo` function that can accept references could look like this

```vb
    ' Adding the AllowReference:=True flag means we can also accept sheet references
    Public Function dnaArgumentInfoAllowRef(<ExcelArgument(AllowReference:=True)> arg As Object) As String

        If TypeOf arg Is ExcelReference Then
            Dim argRef As ExcelReference = arg

            ' We could construct a COM Range object from the ExcelReference if we needed to access some of the Range information
            ' For now we just read the values, and call the normal description function to describe the values at the sheet reference
            Return "Reference " & argRef.ToString() & ": " & dnaArgumentInfo(argRef.GetValue())
        Else
            Return dnaArgumentInfo(arg)
        End If

    End Function
```

In this case we're just processing the reference by calling `ExcelReference.GetValue()` to read the value, and then describing this value further.

One advantage of receiving the reference is that we can now support union references correctly, by accessing the `ExcelReference.InnerReferences` property to read each area.
Our final version of `SumEvenNumbers` then looks like this:

```vb
    ' The parameter type is declared as Object, and marked with the AllowReference:=True flag
    ' The function can take a single value, or any sheet reference as input
    ' Sheet references might be to union ranges with multiple areas, which we process individually
    Public Function dnaSumEvenNumbersR(<ExcelArgument(AllowReference:=True)> arg As Object)
        Dim sum As Double

        If TypeOf arg Is ExcelReference Then
            ' To be really careful, we get the 2D array of values from each of the areas in the reference
            ' This lets the function suppport union ranges as input correctly
            Dim argRef As ExcelReference = arg
            For Each rect In argRef.InnerReferences
                sum += dnaSumEvenNumbers2D(rect.GetValue())
            Next
        ElseIf TypeOf arg Is Object(,) Then
            ' We can call the array implementation directly
            sum = dnaSumEvenNumbers2D(arg)
        Else
            ' Just check and return the value
            If arg Mod 2 = 0 Then
                sum = arg
            End If
        End If
        
        Return sum
    End Function
```

### The `ExcelReference` type and converting to a COM `Range` object

The `ExcelReference` represents a sheet reference, but is a much simpler structure than the COM `Range` object. It only encodes the sheet reference information required to make further calls to the Excel C API - a topic that needs to be explored separately. I'll just note here that it is possible to create and use a COM `Range` object using the `ExcelReference` information, and then use the `Range` further from there.

## Reference - argument types supported by Excel-DNA

This section provides a reference to all the argument types supported for UDFs by Excel-DNA.
I use the C# notation for arrays, where `Double[,]` denotes a 2D array of doubles.

For Excel-DNA UDFs, the supported function parameter and return types are:
* `Double`
* `String`
* `DateTime`    -- returns a double to Excel that can be formatted to display as a date/time
* `Double[]`    -- if only one column is passed in, takes that column, else first row is taken
* `Double[,]`
* `Object`
* `Object[]`    -- if only one column is passed in, takes that column, else first row is taken
* `Object[,]`
* `Boolean (bool)`
* `Int32 (int)`
* `Int16 (short)`
* `UInt16 (ushort)`
* `Decimal`
* `Int64 (long)`

Incoming function parameters of type `Object` will only arrive as one of the following:
* `Double`
* `String`
* `Boolean`
* `ExcelDna.Integration.ExcelError`
* `ExcelDna.Integration.ExcelMissing`
* `ExcelDna.Integration.ExcelEmpty`
* `Object[,]` containing an array with a mixture of the above types
* `ExcelReference` -- (Only if AllowReference=true in ExcelArgumentAttribute)

Function parameters of type `Object[]` or `Object[,]` will receive an array containing a mixture of the above types (excluding `Object[,]` and `ExcelReference`)
For `Object[]` parameters, if the area or array passed in has more then one row and more than one column, onlyk the first row will be received.

Return values of type Object are allowed to be:
* `Double`
* `String`
* `DateTime`
* `Boolean`
* `Double[]`
* `Double[,]`
* `Object[]`
* `Object[,]`
* `ExcelDna.Integration.ExcelError`
* `ExcelDna.Integration.ExcelMissing.Value` // Converted by Excel to be 0.0 in a cell
* `ExcelDna.Integration.ExcelEmpty.Value`   // Converted by Excel to be 0.0 in a cell
* `ExcelDna.Integration.ExcelReference`
* `Int32 (int)`
* `Int16 (short)`
* `UInt16 (ushort)`
* `Decimal`
* `Int64 (long)`
* otherwise the function return a `#VALUE!` error to the caller

Return values of type Object[] and Object[,] are processed as arrays of the type Object, containing a mixture of the above, excluding the array types.

