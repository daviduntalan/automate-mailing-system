namespace AutomateMailingOfBirForm {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.btnImportPdfFile = new System.Windows.Forms.Button();
            this.txtPdfSourceFile = new System.Windows.Forms.TextBox();
            this.lstFoundNames = new System.Windows.Forms.ListBox();
            this.btnSendToAll = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnImportPdfFile
            // 
            this.btnImportPdfFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportPdfFile.Location = new System.Drawing.Point(672, 30);
            this.btnImportPdfFile.Name = "btnImportPdfFile";
            this.btnImportPdfFile.Size = new System.Drawing.Size(100, 40);
            this.btnImportPdfFile.TabIndex = 1;
            this.btnImportPdfFile.Text = "Import File...";
            this.btnImportPdfFile.UseVisualStyleBackColor = true;
            this.btnImportPdfFile.Click += new System.EventHandler(this.btnImportPdfFile_Click);
            // 
            // txtPdfSourceFile
            // 
            this.txtPdfSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPdfSourceFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPdfSourceFile.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPdfSourceFile.Location = new System.Drawing.Point(12, 30);
            this.txtPdfSourceFile.Multiline = true;
            this.txtPdfSourceFile.Name = "txtPdfSourceFile";
            this.txtPdfSourceFile.ReadOnly = true;
            this.txtPdfSourceFile.Size = new System.Drawing.Size(654, 40);
            this.txtPdfSourceFile.TabIndex = 0;
            // 
            // lstFoundNames
            // 
            this.lstFoundNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFoundNames.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFoundNames.FormattingEnabled = true;
            this.lstFoundNames.ItemHeight = 16;
            this.lstFoundNames.Location = new System.Drawing.Point(12, 76);
            this.lstFoundNames.Name = "lstFoundNames";
            this.lstFoundNames.ScrollAlwaysVisible = true;
            this.lstFoundNames.Size = new System.Drawing.Size(760, 260);
            this.lstFoundNames.TabIndex = 2;
            // 
            // btnSendToAll
            // 
            this.btnSendToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendToAll.Location = new System.Drawing.Point(566, 344);
            this.btnSendToAll.Name = "btnSendToAll";
            this.btnSendToAll.Size = new System.Drawing.Size(100, 40);
            this.btnSendToAll.TabIndex = 3;
            this.btnSendToAll.Text = "Send To All";
            this.btnSendToAll.UseVisualStyleBackColor = true;
            this.btnSendToAll.Click += new System.EventHandler(this.btnSendToAll_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(672, 344);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close/Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnImportPdfFile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.lstFoundNames);
            this.Controls.Add(this.txtPdfSourceFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSendToAll);
            this.Controls.Add(this.btnImportPdfFile);
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "MainForm";
            this.Text = "Automate Mailing of BIR Form";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportPdfFile;
        private System.Windows.Forms.TextBox txtPdfSourceFile;
        private System.Windows.Forms.ListBox lstFoundNames;
        private System.Windows.Forms.Button btnSendToAll;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnClose;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

