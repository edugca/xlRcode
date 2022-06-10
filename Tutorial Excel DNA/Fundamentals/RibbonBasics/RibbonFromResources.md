# Project layout with ribbon and images in a resources file


An alternative approach is to add the .xml file in a separate file and embed as a resource in the add-in.
For complicated projects, this might be a better structure.

I'll put the ribbon xml file and related resources (like images we show in the ribbon) into a project folder called `RibbonResources`.

Steps to add the `.xml` file as a resource:
1. Add a new folder under our project to hold our ribbon resources - called e.g. `RibbonResources`
2. Add a new xml file called `Ribbon.xml` with the following content inside the `RibbonResources` folder:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui' loadImage='LoadImage'>
	<ribbon>
		<tabs>
			<tab id='tab1' label='My Tab'>
				<group id='group1' label='My Group'>
					<button id='button1' label='My Button' onAction='OnButtonPressed' image='MagicWand'/>
				</group>
			</tab>
		</tabs>
	</ribbon>
</customUI>
```

3. Add a new resources file to the project called `RibbonResources.resx`.
4. Into the resource file, `Add Existing File` and select the `RibbonResouces\Ribbon.xml` file. Check that it is called `Ribbon` - we'll use this name a bit later.

5. Take similar steps for an image, where the name of the image in the resources file matches the name in the `image` attribute in the ribbon markup.

6. The class will now explicitly get the ribbon markup and images from the resources class. It does this my overriding the `GetCustomUI` and `LoadImage` methods of the `ExcelRibbon` base class.

```vb
Imports System.Runtime.InteropServices
Imports ExcelDna.Integration.CustomUI

<ComVisible(True)>
Public Class Ribbon
    Inherits ExcelRibbon

    Public Overrides Function GetCustomUI(RibbonID As String) As String
        Return RibbonResources.Ribbon ' The name here is the resource name that the ribbon xml has in the RibbonResources resource file
    End Function

    Public Overrides Function LoadImage(imageId As String) As Object
        ' This will return the image resource with the name specified in the image='xxxx' tag
        Return RibbonResources.ResourceManager.GetObject(imageId)
    End Function

    Public Sub OnButtonPressed(control As IRibbonControl)
        MsgBox("Hello")
    End Sub

End Class
```
