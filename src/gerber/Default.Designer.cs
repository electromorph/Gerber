namespace UI
{
    partial class Default
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
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ofDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnLEDOff = new System.Windows.Forms.Button();
            this.btnLEDOn = new System.Windows.Forms.Button();
            this.btnIndexRight = new System.Windows.Forms.Button();
            this.btnIndexLeft = new System.Windows.Forms.Button();
            this.lblLineCount = new System.Windows.Forms.Label();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(352, 10);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(352, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ofDialog
            // 
            this.ofDialog.FileName = "Open File";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(3, 33);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(343, 20);
            this.txtFileName.TabIndex = 2;
            this.txtFileName.Text = "C:\\_MAX\\Gerber\\TestFiles\\EntireGerberFile1 - Copy - Copy.txt";
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Location = new System.Drawing.Point(3, 10);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(56, 13);
            this.lblSelectFile.TabIndex = 3;
            this.lblSelectFile.Text = "Select File";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(65, 5);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(32, 22);
            this.btnSelectFile.TabIndex = 4;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnLEDOff);
            this.pnlControls.Controls.Add(this.btnLEDOn);
            this.pnlControls.Controls.Add(this.btnIndexRight);
            this.pnlControls.Controls.Add(this.btnIndexLeft);
            this.pnlControls.Controls.Add(this.lblLineCount);
            this.pnlControls.Controls.Add(this.btnClose);
            this.pnlControls.Controls.Add(this.btnProcess);
            this.pnlControls.Controls.Add(this.btnSelectFile);
            this.pnlControls.Controls.Add(this.txtFileName);
            this.pnlControls.Controls.Add(this.lblSelectFile);
            this.pnlControls.Location = new System.Drawing.Point(12, 12);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(444, 104);
            this.pnlControls.TabIndex = 6;
            // 
            // btnLEDOff
            // 
            this.btnLEDOff.Location = new System.Drawing.Point(264, 60);
            this.btnLEDOff.Name = "btnLEDOff";
            this.btnLEDOff.Size = new System.Drawing.Size(75, 23);
            this.btnLEDOff.TabIndex = 10;
            this.btnLEDOff.Text = "LED Off";
            this.btnLEDOff.UseVisualStyleBackColor = true;
            this.btnLEDOff.Click += new System.EventHandler(this.btnLEDOff_Click);
            // 
            // btnLEDOn
            // 
            this.btnLEDOn.Location = new System.Drawing.Point(182, 60);
            this.btnLEDOn.Name = "btnLEDOn";
            this.btnLEDOn.Size = new System.Drawing.Size(75, 23);
            this.btnLEDOn.TabIndex = 9;
            this.btnLEDOn.Text = "LED On";
            this.btnLEDOn.UseVisualStyleBackColor = true;
            this.btnLEDOn.Click += new System.EventHandler(this.btnLEDOn_Click);
            // 
            // btnIndexRight
            // 
            this.btnIndexRight.Location = new System.Drawing.Point(100, 60);
            this.btnIndexRight.Name = "btnIndexRight";
            this.btnIndexRight.Size = new System.Drawing.Size(75, 23);
            this.btnIndexRight.TabIndex = 8;
            this.btnIndexRight.Text = "Right";
            this.btnIndexRight.UseVisualStyleBackColor = true;
            this.btnIndexRight.Click += new System.EventHandler(this.btnIndexRight_Click);
            // 
            // btnIndexLeft
            // 
            this.btnIndexLeft.Location = new System.Drawing.Point(18, 60);
            this.btnIndexLeft.Name = "btnIndexLeft";
            this.btnIndexLeft.Size = new System.Drawing.Size(75, 23);
            this.btnIndexLeft.TabIndex = 7;
            this.btnIndexLeft.Text = "Left";
            this.btnIndexLeft.UseVisualStyleBackColor = true;
            this.btnIndexLeft.Click += new System.EventHandler(this.btnIndexLeft_Click);
            // 
            // lblLineCount
            // 
            this.lblLineCount.AutoSize = true;
            this.lblLineCount.Location = new System.Drawing.Point(15, 75);
            this.lblLineCount.Name = "lblLineCount";
            this.lblLineCount.Size = new System.Drawing.Size(0, 13);
            this.lblLineCount.TabIndex = 6;
            // 
            // Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(480, 136);
            this.Controls.Add(this.pnlControls);
            this.Name = "Default";
            this.Text = "Process Gerber File";
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.OpenFileDialog ofDialog;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Label lblLineCount;
        private System.Windows.Forms.Button btnLEDOff;
        private System.Windows.Forms.Button btnLEDOn;
        private System.Windows.Forms.Button btnIndexRight;
        private System.Windows.Forms.Button btnIndexLeft;
    }
}

