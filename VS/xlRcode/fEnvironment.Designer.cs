namespace xlRcode
{
    partial class fEnvironment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fEnvironment));
            this.dgvEnvironment = new System.Windows.Forms.DataGridView();
            this.btRefresh = new System.Windows.Forms.Button();
            this.btCopy = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.cbRowHeaders = new System.Windows.Forms.CheckBox();
            this.cbColHeaders = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironment)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvEnvironment
            // 
            this.dgvEnvironment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvEnvironment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironment.Location = new System.Drawing.Point(12, 91);
            this.dgvEnvironment.Name = "dgvEnvironment";
            this.dgvEnvironment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnvironment.Size = new System.Drawing.Size(613, 437);
            this.dgvEnvironment.TabIndex = 0;
            // 
            // btRefresh
            // 
            this.btRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRefresh.Location = new System.Drawing.Point(12, 12);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(81, 39);
            this.btRefresh.TabIndex = 1;
            this.btRefresh.Text = "Refresh";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btCopy
            // 
            this.btCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCopy.Location = new System.Drawing.Point(284, 12);
            this.btCopy.Name = "btCopy";
            this.btCopy.Size = new System.Drawing.Size(197, 39);
            this.btCopy.TabIndex = 2;
            this.btCopy.Text = "Copy to clipboard (R table)";
            this.btCopy.UseVisualStyleBackColor = true;
            this.btCopy.Click += new System.EventHandler(this.btCopy_Click);
            // 
            // btExport
            // 
            this.btExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btExport.Location = new System.Drawing.Point(504, 12);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(120, 39);
            this.btExport.TabIndex = 3;
            this.btExport.Text = "Export as formula";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // cbRowHeaders
            // 
            this.cbRowHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRowHeaders.AutoSize = true;
            this.cbRowHeaders.Location = new System.Drawing.Point(284, 57);
            this.cbRowHeaders.Name = "cbRowHeaders";
            this.cbRowHeaders.Size = new System.Drawing.Size(89, 17);
            this.cbRowHeaders.TabIndex = 4;
            this.cbRowHeaders.Text = "Row headers";
            this.cbRowHeaders.UseVisualStyleBackColor = true;
            // 
            // cbColHeaders
            // 
            this.cbColHeaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbColHeaders.AutoSize = true;
            this.cbColHeaders.Location = new System.Drawing.Point(379, 57);
            this.cbColHeaders.Name = "cbColHeaders";
            this.cbColHeaders.Size = new System.Drawing.Size(102, 17);
            this.cbColHeaders.TabIndex = 5;
            this.cbColHeaders.Text = "Column headers";
            this.cbColHeaders.UseVisualStyleBackColor = true;
            // 
            // fEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 540);
            this.Controls.Add(this.cbColHeaders);
            this.Controls.Add(this.cbRowHeaders);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btCopy);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.dgvEnvironment);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fEnvironment";
            this.Text = "xlRcode Environment";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvEnvironment;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btCopy;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.CheckBox cbRowHeaders;
        private System.Windows.Forms.CheckBox cbColHeaders;
    }
}