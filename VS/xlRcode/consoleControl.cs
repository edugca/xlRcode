using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using RDotNet;
using RDotNet.Utilities;
using System.Windows.Forms.Integration;

using static xlRcode.Global;

namespace xlRcode
{
    [ComVisible(true)]
    public partial class consoleControl : UserControl
    {

        public consoleControl()
        {
            InitializeComponent();

            //Instantiate tbConsoleExcel
            tbConsoleExcel = ((RichTextBox)Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleCode"].Controls["tbConsoleCode"]).Clone() ;
            tbConsoleExcel.Text = "> ";
            tbConsoleExcel.SelectAll();
            tbConsoleExcel.SelectionProtected = true;
            tbConsoleExcel.Select(tbConsoleExcel.Text.Length, 0);
            tbConsoleExcel.Tag = 2; //keep the position of the last protected character
        }

        

        private void tbConsole_MouseClick(object sender, MouseEventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Clear all", new EventHandler(clearAllCode));

            RichTextBox tbConsoleCode = (RichTextBox)sender;
            tbConsoleCode.ContextMenu = cm;
        }
        private void tbConsoleCode_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                RichTextBox tbConsoleCode = (RichTextBox)sender;
                int lastProtected = (int)tbConsoleCode.Tag;
                string code = tbConsoleCode.Text.Substring(lastProtected);
                string result = xlRcode.MyFunctions.XLRCODE_Routine(code, true);
                WinFormsExtensions.AppendLine(tbConsoleCode, result + System.Environment.NewLine + "> ", Color.Black, SetUp.rConsoleLineLimit);
                tbConsoleCode.SelectAll();
                tbConsoleCode.SelectionProtected = true;
                tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
                tbConsoleCode.Tag = tbConsoleCode.Text.Length; //keep the position of the last protected character
                tbConsoleCode.ScrollToCaret();
            }
        }

        private void clearAllCode(object sender, EventArgs e)
        {
            RichTextBox tbConsoleCode = (RichTextBox)sender;

            tbConsoleCode.Clear();
            tbConsoleCode.Text = "> ";
            tbConsoleCode.SelectAll();
            tbConsoleCode.SelectionProtected = true;
            tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
            tbConsoleCode.Tag = tbConsoleCode.Text.Length; //keep the position of the last protected character

        }

    }
}
