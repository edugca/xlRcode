# IntelliSense for VBA functions

This tutorial shows you how to enable in-sheet IntelliSense help for VBA Functions.

```vb
Function TempFahrenheit(tempInCelsius)
...
```

![IntelliSense in action](https://user-images.githubusercontent.com/414659/100517124-37bbb700-3191-11eb-8c3b-8125322a6f63.gif)


[![Excel-DNA IntelliSense for VBA Functions](http://img.youtube.com/vi/-BIOzl1igXU/0.jpg)](http://www.youtube.com/watch?v=-BIOzl1igXU "Excel-DNA IntelliSense for VBA Functions")


## Background

When you edit a formula on a worksheet, Excel has two mechanisms to assist you in entering the right function call information.

### The 'Function Arguments' dialog

This helps you to to select a function and enter or select the right arguments for the function.

![FunctionArguments](https://user-images.githubusercontent.com/414659/100516737-8ca9fe00-318e-11eb-9bc8-299855869a59.png)

### The in-sheet 'Formula AutoComplete' features

While entering the formula in the cell, the function list and function details help is displayed as an IntelliSense-style popup.

![FunctionSelection](https://user-images.githubusercontent.com/414659/100516786-ef02fe80-318e-11eb-8c85-606902c43e1d.png)
![FunctionEntry](https://user-images.githubusercontent.com/414659/100516789-f924fd00-318e-11eb-97f3-31421c5a9e18.png)


### Descriptions and help information for user-defined functions (UDFs)

UDFs that are declared in .xll add-ins (like those made in .NET with Excel-DNA) can be registered with function and argument descriptions, and these will display in the 'Insert Function' dialog. For Functions in VBA code (either in an .xlam add-in or in the workbook itself) you can is possible to use the `Application.MacroOptions` method to add function and argument descriptions.

However, information added about UDFs with these mechanism only display in the 'Insert Function' dialog. Excel exposes no built-in mechanism that lets UDFs defined in .xll add-ins or VBA to also participate in the in-sheet 'Formula AutoComplete' feature.

## Excel-DNA IntelliSense extension

To enable in-sheet help for UDFs, I developed the Excel-DNA IntelliSense extension. This extension allows functions defined in .xll add-ins as well as VBA functions to register and show IntelliSense information.

You can find more details about the extension at the [Excel-DNA IntelliSense GitHub site](https://github.com/Excel-DNA/IntelliSense), but here I will focus on the use for VBA functions.

### Download the ExcelDna.IntelliSense(64).xll add-in

The newest release of the IntelliSense add-in can be found here:
https://github.com/Excel-DNA/IntelliSense/releases

You need to check whether your version of Excel is a 32-bit or 64-bit version, then download the matching .xll file.
To test you can just follow File -> Open and select the .xll file. Installing the add-in so that it opens automatically can be done in the `Alt+t, i` Add-Ins dialog - also at File -> Options -> Add-Ins, Manage: Excel add-ins.

### Example functions

I will use these VBA functions as an example:

```vb
Function TempCelsius(tempInFahrenheit As Double) As Double
    TempCelsius = (tempInFahrenheit - 32#) * 5# / 9#
End Function

Function TempFahrenheit(tempInCelsius As Double) As Double
    TempFahrenheit = (tempInCelsius / 5# * 9#) + 32#
End Function

Function TempDeltaFromHeat(heatDelta As Double, mass As Double, specificHeatCapacity As Double)
    TempDeltaFromHeat = heatDelta / mass / specificHeatCapacity
End Function
```

To enable the Excel-DNA IntelliSense you need to load the ExcelDna.IntelliSense.xll (or ExcelDna.IntelliSense64.xll) add-in.
You then provide function descriptions in:
* a special (possibly hidden) worksheet,
* an extra file next to the workbook or add-in with the descriptions in xml format, or
* the same xml format information saved in the 'CustomXML' properties of the workbook.

#### Create function descriptions worksheet

| | A                 | B              | C               | D              | E                | F    | G              | H                    | I                 |
|-|-------------------|----------------|-----------------|----------------|------------------|------|----------------|----------------------|-------------------|
|1| FunctionInfo      | 1              | Temperature     |                |                  |      |                |                      |                   |
|2| TempCelsius       | converts ...   | https://en.w... | tempFahrenheit | is the temper... |      |                |                      |                   |
|3| TempFahrenheit    | converts ...   | https://en.w... | tempCelsius    | is the temper... |      |                |                      |                   |
|4| TempDeltaFromHeat | calculates ... | https://www...  | heatDelta      | is the amount... | mass | is the mass... | specificHeatCapacity | is the specific.. |

Details of the sheet format are:
* The name of the sheet must be '\_IntelliSense\_'; it may be a hidden sheet
* The first cell (A1) must contain the string 'FunctionInfo'
* The next cell across (B1) must contain the value 1
* The next call across (C1) may contain a category for the functions (not read by the IntelliSense add-in, used only in the `MacroOptions` code shown below)
* From the second row down, each row contains the information for a single function
  * Function name
  * Function description
  * Function help link
  * Argument1 name
  * Argument1 description
  * Argument2 name
  * Argument2 description
  * etc.
 
#### Function descriptions xml file
Another way you can provide the function information to the IntelliSense add-in is with an xml file next to the workbook or add-in file.
For a workbook with the name 'MyWorkbook.xlsm' the IntelliSense file must be named 'MyWorkbook.intellisense.xml'.

The contents of the xml file, matching the above example worksheet, would be

```xml
<IntelliSense xmlns="http://schemas.excel-dna.net/intellisense/1.0">
  <FunctionInfo>
    <Function Name="TempCelsius" 
              Description="Converts the temperature from degrees Fahrenheit to degrees Celsius" >
      <Argument Name="tempInFahrenheit" 
                Description="is the temperature in degrees Fahrenheit" />
    </Function>
    <Function Name="TempFahrenheit" 
              Description="Converts the temperature from degrees Celsius to degrees Fahrenheit " >
      <Argument Name="tempInCelsius" 
                Description="is the temperature in degrees Celsius" />
    </Function>
    <Function Name="TempDeltaFromHeat"
              Description="Calculates the temperature change for a body, given the amount of heat absorbed or released, in K (or equivalantly degrees C)" 
              HelpTopic="https://www.softschools.com/formulas/physics/temperature_formula/640/">
      <Argument Name="heatDelta"
                Description="is the amount of heat absorbed or released (in J)" />
      <Argument Name="mass"
                Description="is the mass of the body (in kg)" />
      <Argument Name="specificHeatCapacity"
                Description="is the specific heat capacity of the substance (in J/Kg/°C)" />
    </Function>
  </FunctionInfo>
</IntelliSense>
```

#### Function descriptions xml in CustomXML part

A third options for providing the function description details is to add the xml content as a `CustomXMLPart` of the workbook.

This snippet can be used to embed the xml file in a workbook:

```vb
Sub EmbedIntelliSense()

    Dim strFilename As String
    Dim strFileContent As String
    Dim iFile As Integer
    
    strFilename = "C:\Path\To\VBAFunctions.IntelliSense.xml"
    iFile = FreeFile
    Open strFilename For Input As #iFile
    strFileContent = Input(LOF(iFile), iFile)
    Close #iFile
    
    Debug.Print strFileContent
    
    ThisWorkbook.CustomXMLParts.Add strFileContent

End Sub
```


### Application.MacroOptions

Function and argument descriptions for VBA functions can be registered for display in the 'Function Arguments' dialog with the `Application.MacroOptions` method.
If we have already defined this information on a worksheet as described above, it is convenient to add `MacroOptions` registration from the same sheet.
A macro that would do this might look like this

```vb

Sub RegisterMacroOptions()
    Dim ws As Worksheet
    Dim row As Range
    Dim rowi As Integer
    Dim coli As Integer
    Dim args As Integer

    Dim category As String
    Dim functionName As String
    Dim functionDescription As String
    Dim helpTopic As String
    Dim ArgDescriptions() As String
    
    Set ws = ThisWorkbook.Worksheets("_IntelliSense_")
    category = ws.Cells(1, 3)
    If category = "" Then
        category = ThisWorkbook.Name
    End If
    
    rowi = 2
    
    Do While True
        Set row = ws.Rows(rowi)
        functionName = row.Cells(1, 1).Value
        If functionName = "" Then
            Exit Do
        End If
        
        functionDescription = row.Cells(1, 2)
        helpTopic = row.Cells(1, 3)
        
        args = 0
        For coli = 5 To 45 Step 2
            If row.Cells(1, coli) = "" Then
                Exit For
            End If
            
            args = args + 1
            ReDim Preserve ArgDescriptions(args - 1)
            ArgDescriptions(args - 1) = row.Cells(1, coli)
        Next
        
        Application.MacroOptions functionName, functionDescription, False, "", False, "", "", "", "", helpTopic, ArgDescriptions
        rowi = rowi + 1
    Loop
End Sub
```

After running this routine, the descriptions will be saved together with the workbook. So you only have to run the routine again to update the descriptions.

The end result is a workbook or add-in that has in-sheet IntelliSense when the `ExcelDna.IntelliSense(64).xll` add-in is loaded, and also shows the function descriptions in the Excel `Function Arguments` dialog.

## Conclusion

We've shown how to add in-sheet IntelliSense for VBA functions using the Excel-DNA IntelliSense add-in.

More information about the Excel-DNA IntelliSense add-in, including details on how to use it with .NET-based Excel-DNA add-ins see the [IntelliSense Github Repository](https://github.com/Excel-DNA/IntelliSense).
