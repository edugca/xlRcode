namespace xlRcode
{
    partial class fConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fConsole));
            this.tbConsoleCode = new System.Windows.Forms.RichTextBox();
            this.tabControlConsole = new System.Windows.Forms.TabControl();
            this.tabPageConsoleCode = new System.Windows.Forms.TabPage();
            this.tabPageConsoleExcel = new System.Windows.Forms.TabPage();
            this.tbConsoleExcel = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlCode = new System.Windows.Forms.TabControl();
            this.tabPageCode1 = new System.Windows.Forms.TabPage();
            this.tbCode = new System.Windows.Forms.RichTextBox();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlConsole.SuspendLayout();
            this.tabPageConsoleCode.SuspendLayout();
            this.tabPageConsoleExcel.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.tabControlCode.SuspendLayout();
            this.tabPageCode1.SuspendLayout();
            this.msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbConsoleCode
            // 
            this.tbConsoleCode.AcceptsTab = true;
            this.tbConsoleCode.DetectUrls = false;
            this.tbConsoleCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConsoleCode.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConsoleCode.ForeColor = System.Drawing.Color.Blue;
            this.tbConsoleCode.Location = new System.Drawing.Point(3, 3);
            this.tbConsoleCode.Name = "tbConsoleCode";
            this.tbConsoleCode.Size = new System.Drawing.Size(599, 484);
            this.tbConsoleCode.TabIndex = 2;
            this.tbConsoleCode.Text = "";
            this.tbConsoleCode.Click += new System.EventHandler(this.tbConsoleCode_Click);
            this.tbConsoleCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbConsoleCode_KeyDown);
            // 
            // tabControlConsole
            // 
            this.tabControlConsole.Controls.Add(this.tabPageConsoleCode);
            this.tabControlConsole.Controls.Add(this.tabPageConsoleExcel);
            this.tabControlConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlConsole.Location = new System.Drawing.Point(622, 28);
            this.tabControlConsole.Name = "tabControlConsole";
            this.tabControlConsole.SelectedIndex = 0;
            this.tabControlConsole.Size = new System.Drawing.Size(613, 516);
            this.tabControlConsole.TabIndex = 2;
            this.tabControlConsole.TabStop = false;
            // 
            // tabPageConsoleCode
            // 
            this.tabPageConsoleCode.Controls.Add(this.tbConsoleCode);
            this.tabPageConsoleCode.Location = new System.Drawing.Point(4, 22);
            this.tabPageConsoleCode.Name = "tabPageConsoleCode";
            this.tabPageConsoleCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConsoleCode.Size = new System.Drawing.Size(605, 490);
            this.tabPageConsoleCode.TabIndex = 1;
            this.tabPageConsoleCode.Text = "Code Console";
            this.tabPageConsoleCode.UseVisualStyleBackColor = true;
            // 
            // tabPageConsoleExcel
            // 
            this.tabPageConsoleExcel.Controls.Add(this.tbConsoleExcel);
            this.tabPageConsoleExcel.Location = new System.Drawing.Point(4, 22);
            this.tabPageConsoleExcel.Name = "tabPageConsoleExcel";
            this.tabPageConsoleExcel.Size = new System.Drawing.Size(605, 490);
            this.tabPageConsoleExcel.TabIndex = 2;
            this.tabPageConsoleExcel.Text = "Excel Console";
            this.tabPageConsoleExcel.UseVisualStyleBackColor = true;
            // 
            // tbConsoleExcel
            // 
            this.tbConsoleExcel.AcceptsTab = true;
            this.tbConsoleExcel.DetectUrls = false;
            this.tbConsoleExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConsoleExcel.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConsoleExcel.ForeColor = System.Drawing.Color.Blue;
            this.tbConsoleExcel.Location = new System.Drawing.Point(0, 0);
            this.tbConsoleExcel.Name = "tbConsoleExcel";
            this.tbConsoleExcel.ReadOnly = true;
            this.tbConsoleExcel.Size = new System.Drawing.Size(605, 490);
            this.tbConsoleExcel.TabIndex = 3;
            this.tbConsoleExcel.Text = "";
            this.tbConsoleExcel.Click += new System.EventHandler(this.tbConsoleExcel_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.tabControlCode, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tabControlConsole, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.msMenu, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1238, 547);
            this.tableLayoutPanel.TabIndex = 12;
            // 
            // tabControlCode
            // 
            this.tabControlCode.Controls.Add(this.tabPageCode1);
            this.tabControlCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCode.Location = new System.Drawing.Point(3, 28);
            this.tabControlCode.Name = "tabControlCode";
            this.tabControlCode.SelectedIndex = 0;
            this.tabControlCode.Size = new System.Drawing.Size(613, 516);
            this.tabControlCode.TabIndex = 5;
            this.tabControlCode.TabStop = false;
            // 
            // tabPageCode1
            // 
            this.tabPageCode1.Controls.Add(this.tbCode);
            this.tabPageCode1.Location = new System.Drawing.Point(4, 22);
            this.tabPageCode1.Name = "tabPageCode1";
            this.tabPageCode1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCode1.Size = new System.Drawing.Size(605, 490);
            this.tabPageCode1.TabIndex = 0;
            this.tabPageCode1.Text = "Untitled1*";
            this.tabPageCode1.UseVisualStyleBackColor = true;
            // 
            // tbCode
            // 
            this.tbCode.AcceptsTab = true;
            this.tbCode.DetectUrls = false;
            this.tbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCode.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCode.Location = new System.Drawing.Point(3, 3);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(599, 484);
            this.tbCode.TabIndex = 1;
            this.tbCode.Text = "";
            this.tbCode.Click += new System.EventHandler(this.tbCode_Click);
            this.tbCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCode_KeyDown);
            // 
            // msMenu
            // 
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tabToolStripMenuItem});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(619, 24);
            this.msMenu.TabIndex = 4;
            this.msMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // tabToolStripMenuItem
            // 
            this.tabToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.tabToolStripMenuItem.Name = "tabToolStripMenuItem";
            this.tabToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.tabToolStripMenuItem.Text = "Tab";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.tabsCloseToolStripMenuItem_Click);
            // 
            // fConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 547);
            this.Controls.Add(this.tableLayoutPanel);
            this.ForeColor = System.Drawing.Color.Blue;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMenu;
            this.Name = "fConsole";
            this.Text = "xlRcode Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fConsole_FormClosing);
            this.tabControlConsole.ResumeLayout(false);
            this.tabPageConsoleCode.ResumeLayout(false);
            this.tabPageConsoleExcel.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tabControlCode.ResumeLayout(false);
            this.tabPageCode1.ResumeLayout(false);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.RichTextBox tbConsoleCode;
        private System.Windows.Forms.TabControl tabControlConsole;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TabPage tabPageConsoleCode;
        private System.Windows.Forms.TabPage tabPageConsoleExcel;
        public System.Windows.Forms.RichTextBox tbConsoleExcel;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlCode;
        private System.Windows.Forms.TabPage tabPageCode1;
        private System.Windows.Forms.RichTextBox tbCode;
    }
}