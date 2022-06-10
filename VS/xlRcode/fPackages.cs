using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using static xlRcode.Global;
using SMcMaster;

namespace xlRcode
{
    public partial class fPackages : Form
    {
        public fPackages()
        {

            InitializeComponent();
            (new TabOrderManager(this)).SetTabOrder(TabOrderManager.TabScheme.AcrossFirst);

            loadPackages();
        }

        private void loadPackages()
        {
            // LOADING
            SetLoading(loadingImage_Installed, true);

            // Load package list
            string code = @"packages <- library()$results[,1]
                            versions <- sapply(lapply(packages, packageVersion), FUN = toString)
                            packsLoaded <- packages %in% (.packages())
                            unname(as.matrix(data.frame(packages, versions, packsLoaded)))";

            object[,] listPackages = xlRcode.MyFunctions.XLRCODE(code) as object[,];

            // Fill in table
            DataTable dtPackages = new DataTable();
            dtPackages = Helpers.ArraytoDatatable(listPackages);
            dtPackages.Columns[0].ColumnName = "Package";
            dtPackages.Columns[1].ColumnName = "Version";
            dtPackages.Columns[2].ColumnName = "Loaded";

            dgvInstalled.DataSource = dtPackages;

            dgvInstalled.Sort(dgvInstalled.Columns[0], ListSortDirection.Ascending);
            
            // LOADING
            SetLoading(loadingImage_Installed, false);
        }


        private void btLoadSelected_Click(object sender, EventArgs e)
        {
            DataGridView dvg = dgvInstalled;

            int nRows = dvg.SelectedRows.Count;
            int nRowsNotLoaded = 0;
            foreach (DataGridViewRow r in dvg.SelectedRows)
            {
                if (r.Cells[2].Value.ToString().ToLower() == false.ToString().ToLower())
                {
                    nRowsNotLoaded += 1;
                }

            }

            string code = String.Empty;
            if (nRows == nRowsNotLoaded)
            {
                // LOAD ALL SELECTED PACKAGES
                foreach (DataGridViewRow r in dvg.SelectedRows)
                {
                    string pck = r.Cells[0].Value.ToString();
                    code += "library(" + pck + ")" + Environment.NewLine;
                }
                this.Close();
                writeToCodeConsole(code);

            }
            else if (nRowsNotLoaded == 0)
            {
                // UNLOAD ALL SELECTED PACKAGES
                foreach (DataGridViewRow r in dvg.SelectedRows)
                {
                    string pck = r.Cells[0].Value.ToString();
                    code += "detach(package:" + pck + ", unload=TRUE)" + Environment.NewLine;
                }
                this.Close();
                writeToCodeConsole(code);
            }
            else
            {
                DialogResult d;
                d = MessageBox.Show("You must select only loaded or only not loaded packages.", "xlRcode");
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = (TabControl)sender;

            if (tc.SelectedIndex == 0)
            {
                btLoadSelected.Enabled = true;
                btInstallSelected.Enabled = false;
                btUninstallSelected.Enabled = true;
            }
            else 
            {
                btLoadSelected.Enabled = false;
                btInstallSelected.Enabled = true;
                btUninstallSelected.Enabled = false;

                if (dgvCRAN.RowCount == 0)
                {

                    loadCRAN();
                    
                }
            }
        }

        private void SetLoading(PictureBox loadingControl, bool displayLoader)
        {
            if (displayLoader)
            {

                loadingControl.Visible = true;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            }
            else
            {
                loadingControl.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void loadCRAN()
        {
            // LOADING
            SetLoading(loadingImage_CRAN, true);

            string code = @"if(as.character(options('repos')[1]) == 'NULL')
                            {
                                chooseCRANmirror()
                            }
                            available.packages()";

            object[,] listCRANPackages = xlRcode.MyFunctions.XLRCODE(code) as object[,];

            // Fill in table
            DataTable dtPackages = new DataTable();
            string colName = String.Empty;
            dtPackages = Helpers.ArraytoDatatable(listCRANPackages);

            // Delete redundant column
            dtPackages.Columns.RemoveAt(0);

            for (int iCol = 0; iCol < dtPackages.Columns.Count; iCol++)
            {
                colName = dtPackages.Rows[0].ItemArray[iCol].ToString();
                dtPackages.Columns[iCol].ColumnName = colName != String.Empty ? colName : " ";
            }
            dtPackages.Rows[0].Delete();

            dgvCRAN.DataSource = dtPackages;

            string rowName = String.Empty;
            for (int iRow = 0; iRow < dgvCRAN.Rows.Count; iRow++)
            {
                rowName = dgvCRAN.Rows[iRow].Cells[0].Value.ToString();
                dgvCRAN.Rows[iRow].HeaderCell.Value = rowName != String.Empty ? rowName : " ";
            }

            dgvCRAN.Sort(dgvCRAN.Columns[0], ListSortDirection.Ascending);

            // LOADING
            SetLoading(loadingImage_CRAN, false);
        }

        private void btInstallSelected_Click(object sender, EventArgs e)
        {
            DataGridView dvg = dgvCRAN;

            int nRows = dvg.SelectedRows.Count;

            if (nRows > 0)
            {
                string code = String.Empty;

                // INSTALL ALL SELECTED PACKAGES
                foreach (DataGridViewRow r in dvg.SelectedRows)
                {
                    string pck = r.Cells[0].Value.ToString();
                    code += "install.packages('" + pck + "')" + Environment.NewLine;
                }
                this.Close();
                writeToCodeConsole(code);
            }
            else if (nRows == 0)
            {
                DialogResult d;
                d = MessageBox.Show("You must select at least one package to install.", "xlRcode");
            }
        }

        private void btUninstallSelected_Click(object sender, EventArgs e)
        {
            DataGridView dvg = dgvInstalled;

            int nRows = dvg.SelectedRows.Count;

            if (nRows > 0)
            {
                string code = String.Empty;

                // UNINSTALL ALL SELECTED PACKAGES
                foreach (DataGridViewRow r in dvg.SelectedRows)
                {
                    string pck = r.Cells[0].Value.ToString();
                    code += "remove.packages('" + pck + "')" + Environment.NewLine;
                }
                this.Close();
                writeToCodeConsole(code);
            }
            else if (nRows == 0)
            {
                DialogResult d;
                d = MessageBox.Show("You must select at least one package to install.", "xlRcode");
            }
        }

        private void writeToCodeConsole(string code)
        {
            myfConsole.Show();
            RichTextBox tb = (RichTextBox)xlRcode.Global.myfConsole.Controls["tableLayoutPanel"].Controls["tabControlConsole"].Controls["tabPageConsoleCode"].Controls["tbConsoleCode"];
            object result = xlRcode.MyFunctions.XLRCODE_Routine(code, false);
            WinFormsExtensions.AppendLine(tb, result + System.Environment.NewLine + "> ", Color.Black, SetUp.rConsoleLineLimit);
            tb.SelectAll();
            tb.SelectionProtected = true;
            tb.Select(tb.Text.Length, 0);
            tb.Tag = tb.Text.Length; //keep the position of the last protected character
            tb.ScrollToCaret();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            DataGridView dvg;
            if (tabControl1.SelectedTab.Text == "Installed Packages")
            {
                dvg = dgvInstalled;
            }
            else
            {
                dvg = dgvCRAN;
            }
            

            String searchValue = tbSearch.Text;
            int rowIndex = -1;
            int currentRowIndex = -1;
            if (dvg.SelectedRows.Count > 0) 
            { currentRowIndex = dvg.SelectedRows[0].Index; }
            
            foreach (DataGridViewRow row in dvg.Rows)
            {
                if (row.Cells[0].Value.ToString().Contains(searchValue))
                {
                    if (rowIndex == -1)
                    {
                        rowIndex = row.Index;
                        if (currentRowIndex < rowIndex)
                        {
                            break;
                        }
                    }
                    else if (row.Index > currentRowIndex)
                    {
                        rowIndex = row.Index;
                        break;
                    }
                }
                
            }

            dvg.ClearSelection();
            if (rowIndex > -1)
            {
                dvg.Rows[rowIndex].Selected = true;
                dvg.FirstDisplayedScrollingRowIndex = dvg.SelectedRows[0].Index;
            }

        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSearch_Click(this, new EventArgs());
                e.Handled = e.SuppressKeyPress = true;
            }
        }
    }
}
