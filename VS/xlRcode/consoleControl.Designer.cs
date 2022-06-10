namespace xlRcode
{
    partial class consoleControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbConsoleExcel = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tbConsoleExcel
            // 
            this.tbConsoleExcel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbConsoleExcel.Location = new System.Drawing.Point(0, 0);
            this.tbConsoleExcel.Name = "tbConsoleExcel";
            this.tbConsoleExcel.Size = new System.Drawing.Size(414, 303);
            this.tbConsoleExcel.TabIndex = 0;
            this.tbConsoleExcel.Text = "";
            // 
            // consoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbConsoleExcel);
            this.Name = "consoleControl";
            this.Size = new System.Drawing.Size(414, 303);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox tbConsoleExcel;
    }
}
