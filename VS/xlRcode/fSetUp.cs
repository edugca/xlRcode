using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using SMcMaster;

namespace xlRcode
{
    public partial class fSetUp : Form
    {
        public fSetUp()
        {
            InitializeComponent();
            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);

            tbRHome.Text = Properties.Settings.Default.RHome;
            tbRPath.Text = Properties.Settings.Default.RPath;
            tbFunctions.Text = Properties.Settings.Default.FunctionsFolder;
            tbCRAN.Text = Properties.Settings.Default.CRANMirror;
            tbConsoleLineLimit.Text = Properties.Settings.Default.ConsoleLineLimit.ToString();
            tbInitializationCode.Text = File.ReadAllText(Properties.Settings.Default.InitializationCodeFile );
        }

        private void btSave_Click(object sender, EventArgs e)
        {

            if (tbRHome.Text != Properties.Settings.Default.RHome
                || tbRPath.Text != Properties.Settings.Default.RPath
                || tbFunctions.Text != Properties.Settings.Default.FunctionsFolder
                || tbCRAN.Text != Properties.Settings.Default.CRANMirror
                || tbInitializationCode.Text.Replace("\r", "") != SetUp.initialInstructions.Replace("\r", "") ) 
            {
                DialogResult d;
                d = MessageBox.Show("You will have to manually restart Excel for changes to become effective. Do you want to continue?", "xlRcode", MessageBoxButtons.YesNo);

                if (d == DialogResult.Yes)
                {

                    // Update config
                    Properties.Settings.Default.RHome = tbRHome.Text;
                    Properties.Settings.Default.RPath = tbRPath.Text;
                    Properties.Settings.Default.FunctionsFolder = tbFunctions.Text;
                    Properties.Settings.Default.CRANMirror = tbCRAN.Text;
                    Properties.Settings.Default.ConsoleLineLimit = Int32.Parse(tbConsoleLineLimit.Text);
                    File.WriteAllText(Properties.Settings.Default.InitializationCodeFile, tbInitializationCode.Text);
                    Properties.Settings.Default.Save();

                    // Restart R
                    //myRDotNet._engine.Dispose();
                    //myRDotNet.InitializeRDotNet();

                    // Close form
                    Close();
                }
                
            }
            else
            {
                // Update config
                Properties.Settings.Default.ConsoleLineLimit = Int32.Parse(tbConsoleLineLimit.Text);
                Properties.Settings.Default.Save();

                // Update SetUp
                SetUp.rConsoleLineLimit = Properties.Settings.Default.ConsoleLineLimit;

                // Close form
                Close();
            }
        }

        private void btRHome_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbRHome.Text))
            {
                btRHome.BackColor = Color.LightGreen;
                btRHome.Text = "OK";
            }
            else 
            {
                btRHome.BackColor = Color.Red;
                btRHome.Text = "FAIL";

                DialogResult d;
                d = MessageBox.Show("The path specified does not exist.", "xlRcode");
            }

        }

        private void btRPath_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbRPath.Text))
            {
                if (File.Exists(tbRPath.Text + Path.DirectorySeparatorChar + "R.exe"))
                {
                    btRPath.BackColor = Color.LightGreen;
                    btRPath.Text = "OK";
                }
                else 
                {
                    btRPath.BackColor = Color.Red;
                    btRPath.Text = "FAIL";

                    DialogResult d;
                    d = MessageBox.Show("There is no R.exe file in the path specified.", "xlRcode");
                }
                
            }
            else
            {
                btRPath.BackColor = Color.Red;
                btRPath.Text = "FAIL";

                DialogResult d;
                d = MessageBox.Show("The path specified does not exist.", "xlRcode");
            }
        }

        private void btFunctions_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbFunctions.Text))
            {
                btFunctions.BackColor = Color.LightGreen;
                btFunctions.Text = "OK";
            }
            else
            {
                btFunctions.BackColor = Color.Red;
                btFunctions.Text = "FAIL";

                DialogResult d;
                d = MessageBox.Show("The path specified does not exist.", "xlRcode");
            }
        }

        private void btCRAN_Click(object sender, EventArgs e)
        {
            string code = @"chooseCRANmirror()
                            as.character(options('repos')$repos[1])";

            object[,] CRANRepository = xlRcode.MyFunctions.XLRCODE(code) as object[,];

            tbCRAN.Text = CRANRepository[0,0].ToString();
        }

        private void tbInitializationCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                RichTextBox tb = (RichTextBox)sender;

                tb.Paste(DataFormats.GetFormat(DataFormats.Text));

                e.Handled = true;
            }
        }
    }
}
