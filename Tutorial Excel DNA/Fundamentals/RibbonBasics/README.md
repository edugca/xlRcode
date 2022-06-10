# Ribbon Basics

In this tutorial we will add a ribbon extension to an Excel-DNA add-in.
The Office ribbon extensions are defined in an .xml markup format which we will add to the project.
Then we define a class to process the ribbon callback methods, to react to commands like a button press.

For this tutorial I will use a Visual Basic add-in; everything works similar in a C# project.

There are some more advanced topics not covered in this tutorial:
* Show how to change or enable elements of the interface based on some internal state.
* Request a ribbon interface update from an event handler or external trigger.
* Compare the native ribbon interface we use here, with the high-level wrapper provided by VSTO.
* Explain internals of how the ribbon implementation works in Excel-DNA.

## Prepare

Our starting point is a simple Excel-DNA add-in that declares a single UDF as a test.
To prepare the environment for working on the ribbon extensions, we add two steps.

1. Install XML schemas (optional)

To get IntelliSense help for the ribbon extension, we can either 
* install the `Excel-DNA XML Schemas` extension to Visual Studio, or 
* install the `ExcelDna.XmlSchemas` package in our add-in.

The first approach requires admin permissions on the machine, but has the advantage of not adding any extra files to the project and only being done once.

2. Configure Excel to show interface errors (optional)

Excel has a setting to display any errors in interface extensions like the ribbon. Enabling this settings is a great help while developing a ribbon extension.
It can be found under **Tools -> Options -> Advanced: Show add-in user interface errors**.

![Excel-show-interface-errors](https://user-images.githubusercontent.com/414659/99848436-7397c080-2b82-11eb-8543-c4b20e94ede6.jpg)

## Add the ribbon xml markup

We now add a new `.xml` file to the project, which we will add to the .dna configuration file. This approach allows us to easily edit the .xml file in Visual Studio and is good for a simple add-in. Two other options for locating the same xml information are:
* Put the xml text inline in the ribbon handler class.
* Put the xml content in a separate file embedded in a resource.

For more complicated add-ins, I prefer putting keeping the xml content in a separate file because it is easier to edit, and does not clutter the .dna file.
But for this example I will take the simple approach with the ribbon markup inside the .dna file.

Edit the `<Project>-AddIn.dna` file, and add the following markup under the `<DnaLibrary>` tag:

```xml
<CustomUI>
  <customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>
    <ribbon>
      <tabs>
        <tab id='tab1' label='My Tab'>
          <group id='group1' label='My Group'>
            <button id='button1' label='Say Hello' onAction='OnSayHelloPressed'/>
          </group>
        </tab>
      </tabs>
    </ribbon>
  </customUI>
</CustomUI>
```

Note that there are two nested tags called `<CustomUI>` and `<customUI>` respectively.

## Add the Ribbon handler code

Now we add code to handle the ribbon callback - in our example the button has an `onAction='OnSayHelloPressed'` attribute.
So we need:
* a class that derives from the `ExcelRibbon` base class, marked as `<ComVisible(True)>` and 
* a method called `OnSayHelloPressed` with the right signature to handle the `onAction` callback for a `button`.

To implement this, add a new class to the project, with this content:

```vb
Imports System.Runtime.InteropServices
Imports ExcelDna.Integration.CustomUI

<ComVisible(True)>
Public Class Ribbon
    Inherits ExcelRibbon

    Public Sub OnSayHelloPressed(control As IRibbonControl)
        MsgBox("Hello from .NET!")
    End Sub

End Class
```

We can now build and run the add-in, to test the new ribbon.

## Interact with the Excel COM object model

Typically you would run some macro command form the ribbon handler, which would interact with the Excel COM object model similar to a VBA macro.

### Reference the Excel COM interop assemblies

First, we add a reference to the COM interop assemblies to our add-in. This will allow us to easily use the Excel COM object model from our add-in.
To do this, add the NuGet package 'ExcelDna.Interop' to the project. It would also be possible to reference the COM libraries directly.

The `ExcelDna.Interop` package includes the Excel 2010 version of the COM object model. This means an add-in that uses these features should work under any Excel 2010 and later versions. However, newer features are not available.

### Add macro code to the ribbon callback

In the Excel-DNA case, this means we need to get hold of the root Application object with a call to `ExcelDnaUtil.Application`.
From there we can use the object model in a similar way to VBA.

Modify the example like this:

```vb
' Add these namespace imports at the top of the file
Imports Microsoft.Office.Interop.Excel
Imports ExcelDna.Integration

' And then change the method like this

    Public Sub OnSayHelloPressed(control As IRibbonControl)
        Dim app As Application
        Dim rng As Range
        
        app = ExcelDnaUtil.Application      ' This gets the root Excel Application object from the Excel-DNA environment
        rng = app.Range("A1")               ' Get a Range object for cell A1 on the ActiveSheet
        
        rng.Value = "Hello from .NET!"      ' Set the value in that cell
    End Sub

```

## Add an image

Next we add an image to display on the ribbon button.

For this we adjust the markup
1. Add an image file to our project, (I put it an a project folder as `Images\MagicWand.png`) and set its `Copy to Output Directory` property to `Copy if Newer`. That ensures the image file will be available when debugging.
2. Add a `loadImage` callback to the `customUI` tag. Excel-DNA internally implements the `LoadImage` method on the `ExcelRibbon` base class.
3. Add an `image` attribute to the `button` to select the image.
4. Add an `Image` tag in the .dna file to identify and pack the image file, with the `Path` pointing to the location - in this case I've put it under an Images folder in the project, so I'll use `Path='Images\MagicWand.png`. I also add `Pack='true'` so that the image file will be included in the packed .xll file.

The .dna file gets these changes:

```xml

<CustomUI>
  <customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui' loadImage='LoadImage'>
    <ribbon>
      <tabs>
        <tab id='tab1' label='My Tab'>
          <group id='group1' label='My Group'>
            <button id='button1' label='Say Hello' onAction='OnSayHelloPressed' image='MagicWand'/>
          </group>
        </tab>
      </tabs>
    </ribbon>
  </customUI>
</CustomUI>

<Image Name='MagicWand' Path='Images\MagicWand.png' Pack='true' />

```

Now build and run to see the image added to the button in Excel.

## Ribbon xml and callback documentation

Excel-DNA is responsible for loading the ribbon helper add-in, but is not otherwise involved in the ribbon extension. This means that the custom UI xml schema, and the signatures for the callback methods are exactly as documented by Microsoft. The best documentation for these aspects can be found in the three-part series on 'Customizing the 2007 Office Fluent Ribbon for Developers':

* [Part 1 - Overview](https://msdn.microsoft.com/en-us/library/aa338202.aspx)
* [Part 2 - Controls and callback reference](https://msdn.microsoft.com/en-us/library/aa338199.aspx)
* [Part 3 - Frequently asked questions, including C# and VB.NET callback signatures](https://msdn.microsoft.com/en-us/library/aa722523.aspx)

Information related to the Excel 2010 extensions to the ribbon can be found here:

* [Customizing Context Menus in Office 2010](https://msdn.microsoft.com/en-us/library/office/ee691832.aspx)
* [Customizing the Office 2010 Backstage View](https://msdn.microsoft.com/en-us/library/office/ee815851.aspx)
* [Ribbon Extensibility in Office 2010: Tab Activation and Auto-Scaling](https://msdn.microsoft.com/en-us/library/office/ee691834.aspx)
