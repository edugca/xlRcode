Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop.Excel
Imports ExcelDna.Integration.CustomUI
Imports ExcelDna.Integration

<ComVisible(True)>
Public Class Ribbon
    Inherits ExcelRibbon

    Public Sub OnSayHelloPressed(control As IRibbonControl)
        Dim app As Application
        Dim rng As Range

        app = ExcelDnaUtil.Application
        rng = app.Range("A1")

        rng.Value = "Hello from .NET!"

    End Sub

End Class
