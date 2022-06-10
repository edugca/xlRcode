using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RDotNet;
using RDotNet.Utilities;
using SMcMaster;
using static xlRcode.Global;

namespace xlRcode
{
    public partial class fConsole : Form
    {

        List<String> previousCommands = new List<String>();
        int idxPreviousCommands = -1;

        public fConsole()
        {
            InitializeComponent();
            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);

            tbConsoleCode.Text = "> ";
            tbConsoleCode.SelectAll();
            tbConsoleCode.SelectionProtected = true;
            tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
            tbConsoleCode.Tag = 2; //keep the position of the last protected character

            tbConsoleExcel.Text = "> ";
            tbConsoleExcel.SelectAll();
            tbConsoleExcel.SelectionProtected = true;
            tbConsoleExcel.Select(tbConsoleExcel.Text.Length, 0);
            tbConsoleExcel.Tag = 2; //keep the position of the last protected character

        }

        private void fConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (new StackTrace().GetFrames().Any(x => x.GetMethod().Name == "Close"))
            {
                //Closed by calling Close()
            }
            else
            {
                //Closed by X or Alt+F4
                e.Cancel = true;
                this.Hide();
            }

        }

        private void tbCode_Click(object sender, EventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Execute selection", new EventHandler(executeSelectedCode));
            cm.MenuItems.Add("Execute all", new EventHandler(executeAllCode));
            cm.MenuItems.Add("-");
            cm.MenuItems.Add("Cut", new EventHandler(cutSelectedCode));
            cm.MenuItems.Add("Copy", new EventHandler(copySelectedCode));
            cm.MenuItems.Add("Paste", new EventHandler(pasteSelectedCode));

            RichTextBox tb = (RichTextBox)sender;
            tb.ContextMenu = cm;
        }

        private void pasteSelectedCode(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((ContextMenu)((MenuItem)sender).Parent).SourceControl;

            tb.Paste(DataFormats.GetFormat(DataFormats.Text));

        }

        private void copySelectedCode(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((ContextMenu)((MenuItem)sender).Parent).SourceControl;

            if (tb.SelectedText.Length > 0)
            {
                Clipboard.SetText(tb.SelectedText);
            }
            else { Clipboard.Clear(); }
            
        }

        private void cutSelectedCode(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((ContextMenu)((MenuItem)sender).Parent).SourceControl;

            Clipboard.SetText(tb.SelectedText);
            tb.SelectedText = string.Empty;
        }

        private void executeAllCode(object sender, EventArgs e)
        {
            RichTextBox tbCodeSelected = (RichTextBox)this.tabControlCode.SelectedTab.Controls[0];
            string code = tbCodeSelected.Text;
            string result = xlRcode.MyFunctions.XLRCODE_Routine(code);
            WinFormsExtensions.AppendLine(tbConsoleCode, result + System.Environment.NewLine + "> ", Color.Black, SetUp.rConsoleLineLimit);
            tbConsoleCode.SelectAll();
            tbConsoleCode.SelectionProtected = true;
            tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
            tbConsoleCode.Tag = tbConsoleCode.Text.Length; //keep the position of the last protected character
        }

        private void executeSelectedCode(object sender, EventArgs e)
        {
            RichTextBox tbCodeSelected = (RichTextBox)this.tabControlCode.SelectedTab.Controls[0];
            string code = tbCodeSelected.SelectedText;
            string result = xlRcode.MyFunctions.XLRCODE_Routine(code);
            WinFormsExtensions.AppendLine(tbConsoleCode, result + System.Environment.NewLine + "> ", Color.Black, SetUp.rConsoleLineLimit);
            tbConsoleCode.SelectAll();
            tbConsoleCode.SelectionProtected = true;
            tbConsoleCode.Select(tbConsoleCode.Text.Length, 0);
            tbConsoleCode.Tag = tbConsoleCode.Text.Length; //keep the position of the last protected character
        }


        private void clearAllCode(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((ContextMenu)((MenuItem)sender).Parent).SourceControl;

            tb.Clear();
            tb.Text = "> ";
            tb.SelectAll();
            tb.SelectionProtected = true;
            tb.Select(tb.Text.Length, 0);
            tb.Tag = tb.Text.Length; //keep the position of the last protected character

        }

        private void tbConsoleCode_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                RichTextBox tb = (RichTextBox)sender;
                int lastProtected = (int)tb.Tag;

                string code = tb.Text.Substring(lastProtected);

                previousCommands.Add(code);
                idxPreviousCommands = previousCommands.Count;

                string result = xlRcode.MyFunctions.XLRCODE_Routine(code, true);

                WinFormsExtensions.AppendLine(tb, "> ", Color.Black, SetUp.rConsoleLineLimit);
                tb.SelectAll();
                tb.SelectionProtected = true;
                tb.Select(tb.Text.Length, 0);
                tb.Tag = tb.Text.Length; //keep the position of the last protected character
                tb.ScrollToCaret();

                e.Handled = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                
                if (idxPreviousCommands > -1)
                {
                    idxPreviousCommands = (idxPreviousCommands > 0) ? idxPreviousCommands - 1 : 0;

                    RichTextBox tb = (RichTextBox)sender;
                    int lastProtected = (int)tb.Tag;

                    if (tb.Text.Length > lastProtected) { tb.Select(lastProtected, tb.Text.Length - lastProtected); tb.SelectedText = String.Empty; }
                    tb.AppendText(previousCommands[idxPreviousCommands]);

                    tb.SelectionStart = tb.TextLength;
                    tb.SelectionLength = 0;
                    tb.SelectionColor = tb.ForeColor;
                    tb.ScrollToCaret(); //Scroll down to the cursor

                }

                e.Handled = true;

            }

            if (e.KeyCode == Keys.Down)
            {
                if (idxPreviousCommands > -1 & idxPreviousCommands < previousCommands.Count)
                {
                    idxPreviousCommands = (idxPreviousCommands < (previousCommands.Count - 1)) ? idxPreviousCommands + 1 : previousCommands.Count - 1;

                    RichTextBox tb = (RichTextBox)sender;
                    int lastProtected = (int)tb.Tag;

                    if (tb.Text.Length > lastProtected) { tb.Select(lastProtected, tb.Text.Length - lastProtected); tb.SelectedText = String.Empty; }
                    tb.AppendText(previousCommands[idxPreviousCommands]);

                    tb.SelectionStart = tb.TextLength;
                    tb.SelectionLength = 0;
                    tb.SelectionColor = tb.ForeColor;
                    tb.ScrollToCaret(); //Scroll down to the cursor

                }

                e.Handled = true;
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                RichTextBox tb = (RichTextBox)sender;

                tb.Paste(DataFormats.GetFormat(DataFormats.Text));

                e.Handled = true;
            }
        }

        private void tbCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                RichTextBox tb = (RichTextBox)sender;

                tb.Paste(DataFormats.GetFormat(DataFormats.Text));

                e.Handled = true;
            }
        }

        private void tbConsoleCode_Click(object sender, EventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Clear all", new EventHandler(clearAllCode));

            cm.MenuItems.Add("-");
            cm.MenuItems.Add("Cut", new EventHandler(cutSelectedCode));
            cm.MenuItems.Add("Copy", new EventHandler(copySelectedCode));
            cm.MenuItems.Add("Paste", new EventHandler(pasteSelectedCode));

            RichTextBox tb = (RichTextBox)sender;
            tb.ContextMenu = cm;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse R Files",
                DefaultExt = "r",
                Filter = "R files (*.r)|*.r|All files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Read the file as one string.
                string fileText = System.IO.File.ReadAllText(openFileDialog1.FileName);

                TabPage tabPage1 = new TabPage
                {
                    Text = Path.GetFileName(openFileDialog1.FileName),
                    Tag = openFileDialog1.FileName
                    //BackColor = Color.Green,
                    //ForeColor = Color.White,
                    //Font = new Font("Verdana", 12),
                    //Width = 100,
                    //Height = 100
                };

                tabControlCode.TabPages.Add(tabPage1);

                // Create a code RichTextBox and add it to TabPage1  
                RichTextBox tbCodeTabPage1 = tbCode.Clone();

                // Add control to TabPage  
                tabPage1.Controls.Add(tbCodeTabPage1);

                // Fill in content
                tbCodeTabPage1.Text = fileText;

                // Select added tab
                tabControlCode.SelectTab(tabPage1);
                tbCodeTabPage1.Select();
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tabPage1 = new TabPage
            {
                Text = "Untitled",
                //BackColor = Color.Green,
                //ForeColor = Color.White,
                //Font = new Font("Verdana", 12),
                //Width = 100,
                //Height = 100
            };

            tabControlCode.TabPages.Add(tabPage1);

            // Create a code RichTextBox and add it to TabPage1  
            RichTextBox tbCodeTabPage1 = tbCode.Clone();

            // Clear text
            tbCodeTabPage1.Clear();

            // Add control to TabPage  
            tabPage1.Controls.Add(tbCodeTabPage1);

            // Select added tab
            tabControlCode.SelectTab(tabPage1);
            tbCodeTabPage1.Select();

        }

        private void tabsCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlCode.TabPages.Remove(tabControlCode.SelectedTab);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TabPage selTab = tabControlCode.SelectedTab;
            string filePath = selTab.Tag.ToString();
            string newScriptCode = ((RichTextBox)selTab.Controls[0]).Text;

            System.IO.File.WriteAllText(filePath, newScriptCode);

            if (File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, newScriptCode);

                DialogResult d;
                d = MessageBox.Show("File was saved!", "xlRcode");
            }
            else
            {
               
                DialogResult d;
                d = MessageBox.Show("File does not exist anymore! Try to save it as a new file.", "xlRcode");
            }

            

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Title = "Save R script as",
                DefaultExt = "r",
                Filter = "R files (*.r)|*.r|All files (*.*)|*.*",
                CheckPathExists = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Save the file as one string.
                string newScriptCode = ((RichTextBox)tabControlCode.SelectedTab.Controls[0]).Text;
                System.IO.File.WriteAllText(saveFileDialog1.FileName, newScriptCode);

                DialogResult d;
                d = MessageBox.Show("File was saved!", "xlRcode");

            }
        }

        private void tbConsoleExcel_Click(object sender, EventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Clear all", new EventHandler(clearAllCode));

            cm.MenuItems.Add("-");
            //cm.MenuItems.Add("Cut", new EventHandler(cutSelectedCode));
            cm.MenuItems.Add("Copy", new EventHandler(copySelectedCode));
            //cm.MenuItems.Add("Paste", new EventHandler(pasteSelectedCode));

            RichTextBox tb = (RichTextBox)sender;
            tb.ContextMenu = cm;
        }
    }
}
