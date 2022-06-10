using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Excel = Microsoft.Office.Interop.Excel;

namespace Ribbon
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {
        public override string GetCustomUI(string RibbonID)
        {
            //<button id='button_registerFunctions' label='Register functions' onAction='OnButtonPressed_RegisterFunctions' imageMso='EmptyTrash'/>

            // </group >
            //< group id = 'group_excelConsole' label = 'Main' >  
            //       < button id = 'button_excelConsole' label = 'Console' onAction = 'OnButtonPressed_excelConsole' />

            return @"
              <customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
              <ribbon>
                <tabs>
                  <tab id='tab_xlR' label='xlRcode'>
                    <group id='group_setUp' label='SetUp'>
                        <button id='button_setUp' label='SetUp' onAction='OnButtonPressed_SetUp' getImage='GetCustomImage_setUp'/>
                        <button id='button_packages' label='Packages' onAction='OnButtonPressed_Packages' imageMso='FunctionsLookupReferenceInsertGallery'/>
                    </group>
                    <group id='group_main' label='Main'>
                        <button id='button_console' label='Console' onAction='OnButtonPressed_Console' imageMso='CodeEdit'/>
                        <button id='button_graphic' label='Graphic' onAction='OnButtonPressed_Graphic' imageMso='ChartInsert'/>
                        <button id='button_clearObjects' label='Clear objects' onAction='OnButtonPressed_ClearObjects' getImage='GetCustomImage_clearObjects'/>
                        <button id='button_garbageCollector' label='Garbage collector' onAction='OnButtonPressed_GarbageCollector' imageMso='EmptyTrash'/>
                    </group>
                    <group id='group_Excel' label='Excel'>
                        <button id='button_calculateRange' label='Calculate range' size='large' onAction='OnButtonPressed_CalculateRange' imageMso='GroupPivotChartTools'/>
                    </group>
                  </tab>
                </tabs>
              </ribbon>
            </customUI>";
        }

        public System.Drawing.Image GetCustomImage_setUp(IRibbonControl control)
        {

            return xlRcode.Properties.Resources.xlRcode.ToBitmap(); // resource Bitmap
            
        }

        public System.Drawing.Image GetCustomImage_clearObjects(IRibbonControl control)
        {

            return xlRcode.Properties.Resources.ClearObjects.ToBitmap(); // resource Bitmap

        }

        public void OnButtonPressed_SetUp(IRibbonControl control)
        {
            Form f = new xlRcode.fSetUp(); // Instantiate a Form object.
            f.ShowDialog();
        }

        public void OnButtonPressed_Packages(IRibbonControl control)
        {
            Form f = new xlRcode.fPackages(); // Instantiate a Form object.
            f.ShowDialog();
        }

        public void OnButtonPressed_Console(IRibbonControl control)
        {
            //Form myfConsole = new xlR.fConsole(); // Instantiate a Form object.
            xlRcode.Global.myfConsole.Show();
            xlRcode.Global.myfConsole.BringToFront();
        }

        public void OnButtonPressed_Graphic(IRibbonControl control)
        {
            xlRcode.MyFunctions.XLRCODE("x11()");
        }

        public void OnButtonPressed_GarbageCollector(IRibbonControl control)
        {
            //Call garbage collector
            xlRcode.MyFunctions.XLRCODE("gc()");

            
            DialogResult d;
            d = MessageBox.Show("Garbage collected!", "xlRcode");
        }

        public void OnButtonPressed_ClearObjects(IRibbonControl control)
        {
            //Clear objects from the workspace
            xlRcode.MyFunctions.XLRCODE("rm(list = ls())");
            xlRcode.MyFunctions.XLRCODE( File.ReadAllText(xlRcode.Properties.Settings.Default.InitializationCodeFile) );

            DialogResult d;
            d = MessageBox.Show("Objects cleared!", "xlRcode");
        }

        public void OnButtonPressed_RegisterFunctions(IRibbonControl control)
        {
            xlRcode.AddIn.RunFunctionScripts();
            xlRcode.AddIn.CreateFunctions();
        }

        public void OnButtonPressed_excelConsole(IRibbonControl control)
        {
            //Form myfConsole = new xlR.fConsole(); // Instantiate a Form object.
            xlRcode.CTPManager.ShowCTP();
        }

        public void OnButtonPressed_CalculateRange(IRibbonControl control)
        {

            Microsoft.Office.Interop.Excel.Application xlApp = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;

            try
            {
                Excel.Range selectedRange = (Excel.Range)(xlApp.Selection);
                selectedRange.Dirty();
                selectedRange.Calculate();
            }
            catch
            {
                DialogResult d;
                d = MessageBox.Show("A range of cells must be previously selected!", "xlRcode");
            }
        }
    }
}