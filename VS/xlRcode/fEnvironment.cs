using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using RDotNet;

using ExcelDna.Integration;
using ExcelDna.Logging;
using ExcelDna.Registration;
using ExcelDna.Integration.CustomUI;

using Excel = Microsoft.Office.Interop.Excel;

using static xlRcode.Global;

namespace xlRcode
{
    public partial class fEnvironment : Form
    {
        public fEnvironment()
        {
            InitializeComponent();

            RefreshEnvironment();

        }

        private void RefreshEnvironment()
        {
            // Check whether R engine has been correctly initialized
            if (Global.isEngineWorking == false){ return; }

            string[] varNames = _engine.Evaluate("ls.str()").AsCharacter().ToArray();
            string[] varTypes = new string[varNames.Length];
            string[] varMemUsages = new string[varNames.Length];
            for (int i = 0; i <= varNames.Length - 1; i++)
            {
                varTypes[i] = _engine.Evaluate("typeof(" + varNames[i] + ")").AsCharacter()[0].ToString();
                varMemUsages[i] = _engine.Evaluate("object.size(" + varNames[i] + ")").AsCharacter()[0].ToString();
            }

            // Fill in table
            DataTable dtEnvironment = new DataTable();
            dtEnvironment.Columns.Add("Name");
            dtEnvironment.Columns.Add("Type");
            dtEnvironment.Columns.Add("Memory usage");

            // Create three new DataRow objects and add
            // them to the DataTable
            DataRow row;
            for (int i = 0; i <= varNames.Length - 1; i++)
            {
                row = dtEnvironment.NewRow();
                row["Name"] = varNames[i];
                row["Type"] = varTypes[i];
                row["Memory usage"] = varMemUsages[i];
                dtEnvironment.Rows.Add(row);
            }

            dgvEnvironment.DataSource = dtEnvironment;

            dgvEnvironment.Font = new Font("Lucida console", 10, FontStyle.Regular);
            dgvEnvironment.Columns["Memory usage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvEnvironment.Sort(dgvEnvironment.Columns[0], ListSortDirection.Ascending);
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshEnvironment();
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            
            if (dgvEnvironment.SelectedRows.Count > 0) 
            {
                string varName = dgvEnvironment.SelectedRows[0].Cells[0].Value.ToString();

                Microsoft.Office.Interop.Excel.Application xlApp = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;

                string rowHeaders = cbRowHeaders.Checked ? "TRUE" : "FALSE";
                string colHeaders = cbColHeaders.Checked ? "TRUE" : "FALSE";
                string decSeparator = xlApp.DecimalSeparator;

                try
                {
                    _engine.Evaluate("write.table(" + varName + 
                        ", 'clipboard', sep = '\t', row.names = " + rowHeaders + 
                        ", col.names = " + colHeaders +
                        ", dec = \"" + decSeparator + "\"" +
                        ")");
                }
                catch 
                {
                    DialogResult d;
                    d = MessageBox.Show("The selected variable does not have a supported format.", "xlRcode");
                }
            }
            
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if (dgvEnvironment.SelectedRows.Count > 0)
            {
                string varName = dgvEnvironment.SelectedRows[0].Cells[0].Value.ToString();
                object result = xlRcode.MyFunctions.XLRCODE_ENV(varName, "");

                Microsoft.Office.Interop.Excel.Application xlApp = (Microsoft.Office.Interop.Excel.Application)ExcelDnaUtil.Application;

                try
                {
                    Excel.Range selectedRange = (Excel.Range)(xlApp.Selection);

                    // Dynamic array is enabled?
                    try
                    {
                        selectedRange.Cells[1].Formula2 = "=XLRCODE_ENV(\"" + varName + "\")";
                    }
                    catch 
                    {
                        selectedRange.Cells[1].Formula = "=XLRCODE_ENV(\"" + varName + "\")";
                    }
                    
                }
                catch
                {
                    DialogResult d;
                    d = MessageBox.Show("A range of cells must be previously selected!", "xlRcode");
                }
            }
        }

    }
}
