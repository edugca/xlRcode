namespace xlRcode
{
    partial class fPackages
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageInstalled = new System.Windows.Forms.TabPage();
            this.loadingImage_Installed = new System.Windows.Forms.PictureBox();
            this.dgvInstalled = new System.Windows.Forms.DataGridView();
            this.tabPageCRAN = new System.Windows.Forms.TabPage();
            this.loadingImage_CRAN = new System.Windows.Forms.PictureBox();
            this.dgvCRAN = new System.Windows.Forms.DataGridView();
            this.btLoadSelected = new System.Windows.Forms.Button();
            this.btInstallSelected = new System.Windows.Forms.Button();
            this.btUninstallSelected = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.lbSearch = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageInstalled.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage_Installed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).BeginInit();
            this.tabPageCRAN.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage_CRAN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCRAN)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageInstalled);
            this.tabControl1.Controls.Add(this.tabPageCRAN);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 106);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(689, 546);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageInstalled
            // 
            this.tabPageInstalled.Controls.Add(this.loadingImage_Installed);
            this.tabPageInstalled.Controls.Add(this.dgvInstalled);
            this.tabPageInstalled.Location = new System.Drawing.Point(4, 29);
            this.tabPageInstalled.Name = "tabPageInstalled";
            this.tabPageInstalled.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInstalled.Size = new System.Drawing.Size(681, 513);
            this.tabPageInstalled.TabIndex = 0;
            this.tabPageInstalled.Text = "Installed Packages";
            this.tabPageInstalled.UseVisualStyleBackColor = true;
            // 
            // loadingImage_Installed
            // 
            this.loadingImage_Installed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingImage_Installed.Image = global::xlRcode.Properties.Resources.Spinner;
            this.loadingImage_Installed.InitialImage = global::xlRcode.Properties.Resources.Spinner;
            this.loadingImage_Installed.Location = new System.Drawing.Point(266, 185);
            this.loadingImage_Installed.Name = "loadingImage_Installed";
            this.loadingImage_Installed.Size = new System.Drawing.Size(149, 143);
            this.loadingImage_Installed.TabIndex = 4;
            this.loadingImage_Installed.TabStop = false;
            this.loadingImage_Installed.Visible = false;
            // 
            // dgvInstalled
            // 
            this.dgvInstalled.AllowUserToAddRows = false;
            this.dgvInstalled.AllowUserToDeleteRows = false;
            this.dgvInstalled.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInstalled.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstalled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInstalled.Location = new System.Drawing.Point(3, 3);
            this.dgvInstalled.Name = "dgvInstalled";
            this.dgvInstalled.ReadOnly = true;
            this.dgvInstalled.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInstalled.Size = new System.Drawing.Size(675, 507);
            this.dgvInstalled.TabIndex = 2;
            // 
            // tabPageCRAN
            // 
            this.tabPageCRAN.Controls.Add(this.loadingImage_CRAN);
            this.tabPageCRAN.Controls.Add(this.dgvCRAN);
            this.tabPageCRAN.Location = new System.Drawing.Point(4, 29);
            this.tabPageCRAN.Name = "tabPageCRAN";
            this.tabPageCRAN.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCRAN.Size = new System.Drawing.Size(681, 513);
            this.tabPageCRAN.TabIndex = 1;
            this.tabPageCRAN.Text = "CRAN Packages";
            this.tabPageCRAN.UseVisualStyleBackColor = true;
            // 
            // loadingImage_CRAN
            // 
            this.loadingImage_CRAN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingImage_CRAN.Image = global::xlRcode.Properties.Resources.Spinner;
            this.loadingImage_CRAN.InitialImage = global::xlRcode.Properties.Resources.Spinner;
            this.loadingImage_CRAN.Location = new System.Drawing.Point(266, 185);
            this.loadingImage_CRAN.Name = "loadingImage_CRAN";
            this.loadingImage_CRAN.Size = new System.Drawing.Size(149, 143);
            this.loadingImage_CRAN.TabIndex = 4;
            this.loadingImage_CRAN.TabStop = false;
            this.loadingImage_CRAN.Visible = false;
            // 
            // dgvCRAN
            // 
            this.dgvCRAN.AllowUserToAddRows = false;
            this.dgvCRAN.AllowUserToDeleteRows = false;
            this.dgvCRAN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCRAN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCRAN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCRAN.Location = new System.Drawing.Point(3, 3);
            this.dgvCRAN.Name = "dgvCRAN";
            this.dgvCRAN.ReadOnly = true;
            this.dgvCRAN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCRAN.Size = new System.Drawing.Size(675, 507);
            this.dgvCRAN.TabIndex = 3;
            // 
            // btLoadSelected
            // 
            this.btLoadSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLoadSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLoadSelected.Location = new System.Drawing.Point(508, 12);
            this.btLoadSelected.Name = "btLoadSelected";
            this.btLoadSelected.Size = new System.Drawing.Size(186, 31);
            this.btLoadSelected.TabIndex = 4;
            this.btLoadSelected.Text = "Load/Unload Selected";
            this.btLoadSelected.UseVisualStyleBackColor = true;
            this.btLoadSelected.Click += new System.EventHandler(this.btLoadSelected_Click);
            // 
            // btInstallSelected
            // 
            this.btInstallSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btInstallSelected.Enabled = false;
            this.btInstallSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInstallSelected.Location = new System.Drawing.Point(508, 49);
            this.btInstallSelected.Name = "btInstallSelected";
            this.btInstallSelected.Size = new System.Drawing.Size(186, 31);
            this.btInstallSelected.TabIndex = 5;
            this.btInstallSelected.Text = "Install Selected";
            this.btInstallSelected.UseVisualStyleBackColor = true;
            this.btInstallSelected.Click += new System.EventHandler(this.btInstallSelected_Click);
            // 
            // btUninstallSelected
            // 
            this.btUninstallSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btUninstallSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btUninstallSelected.Location = new System.Drawing.Point(508, 86);
            this.btUninstallSelected.Name = "btUninstallSelected";
            this.btUninstallSelected.Size = new System.Drawing.Size(186, 31);
            this.btUninstallSelected.TabIndex = 6;
            this.btUninstallSelected.Text = "Uninstall Selected";
            this.btUninstallSelected.UseVisualStyleBackColor = true;
            this.btUninstallSelected.Click += new System.EventHandler(this.btUninstallSelected_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearch.Location = new System.Drawing.Point(74, 17);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(335, 24);
            this.tbSearch.TabIndex = 7;
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // lbSearch
            // 
            this.lbSearch.AutoSize = true;
            this.lbSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSearch.Location = new System.Drawing.Point(13, 16);
            this.lbSearch.Name = "lbSearch";
            this.lbSearch.Size = new System.Drawing.Size(55, 18);
            this.lbSearch.TabIndex = 8;
            this.lbSearch.Text = "Search";
            // 
            // btSearch
            // 
            this.btSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSearch.Location = new System.Drawing.Point(415, 16);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(49, 27);
            this.btSearch.TabIndex = 9;
            this.btSearch.Text = "Go";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // fPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 664);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.lbSearch);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.btUninstallSelected);
            this.Controls.Add(this.btInstallSelected);
            this.Controls.Add(this.btLoadSelected);
            this.Controls.Add(this.tabControl1);
            this.Name = "fPackages";
            this.Text = "xlRcode Packages";
            this.tabControl1.ResumeLayout(false);
            this.tabPageInstalled.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage_Installed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstalled)).EndInit();
            this.tabPageCRAN.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage_CRAN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCRAN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageInstalled;
        private System.Windows.Forms.DataGridView dgvInstalled;
        private System.Windows.Forms.TabPage tabPageCRAN;
        private System.Windows.Forms.DataGridView dgvCRAN;
        private System.Windows.Forms.Button btLoadSelected;
        private System.Windows.Forms.Button btInstallSelected;
        private System.Windows.Forms.Button btUninstallSelected;
        private System.Windows.Forms.PictureBox loadingImage_Installed;
        private System.Windows.Forms.PictureBox loadingImage_CRAN;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label lbSearch;
        private System.Windows.Forms.Button btSearch;
    }
}