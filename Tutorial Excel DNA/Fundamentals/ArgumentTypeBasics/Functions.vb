Imports ExcelDna.Integration

Public Module Functions

    ' Provides information about the data type and value that is passed in as argument
    Public Function dnaArgumentInfo(arg As Object) As String

        If TypeOf arg Is ExcelMissing Then
            Return "<<Missing>>"
        ElseIf TypeOf arg Is ExcelEmpty Then
            Return "<<Empty>>"
        ElseIf TypeOf arg Is Double Then
            Return "Double: " & CDbl(arg)
        ElseIf TypeOf arg Is String Then
            Return "String: " & CStr(arg)
        ElseIf TypeOf arg Is Boolean Then
            Return "Boolean: " & CBool(arg)
        ElseIf TypeOf arg Is ExcelError Then
            Return "ExcelError: " & arg.ToString()
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

    ' The parameter type is declared as a 2D array.
    ' The function can take a single value, or any rectangular range as input.
    ' Union references with multiple areas will only pass in the first area
    Public Function dnaSumEvenNumbers2D(arg(,) As Object) As Double

        Dim sum As Double = 0
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

    ' The parameter type is declared as a 1D array.
    ' The function can take a single value, a single row or a single column
    ' If a 2D range with more than one row and column is passed in, only the first row will be passed in here
    Public Function dnaSumEvenNumbers1D(arg() As Object) As Double

        Dim sum As Double = 0
        Dim val As Object

        For Each val In arg
            If val Mod 2 = 0 Then
                sum += val
            End If
        Next

        Return sum

    End Function

    ' The parameter type is declared as a 1D double array.
    ' If the values passed in cannot be converted to numbers, Excel will return a #VALUE! error to the sheet
    Public Function dnaSumEvenNumbersDouble1D(arg() As Double) As Double

        Dim sum As Double = 0

        For i As Integer = 0 To arg.Length - 1   ' Alternative to `For Each ...` for going through the array

            Dim val As Object = arg(i)
            If val Mod 2 = 0 Then
                sum += val
            End If

        Next

        Return sum

    End Function

    ' The parameter type is declared as a 2D double array.
    ' If the values passed in cannot be converted to numbers, Excel will return a #VALUE! error to the sheet
    Public Function dnaSumEvenNumbersDouble2D(arg(,) As Double) As Double

        Dim sum As Double = 0
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

End Module
