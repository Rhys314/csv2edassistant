namespace csv2edassistant
{
    partial class mainWindow
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
                this.fileWasCreated = false;
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
            this.browseCSV = new System.Windows.Forms.Button();
            this.browseEDA = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.csvFilename = new System.Windows.Forms.TextBox();
            this.edaFilename = new System.Windows.Forms.TextBox();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // browseCSV
            // 
            this.browseCSV.Location = new System.Drawing.Point(423, 10);
            this.browseCSV.Name = "browseCSV";
            this.browseCSV.Size = new System.Drawing.Size(75, 23);
            this.browseCSV.TabIndex = 0;
            this.browseCSV.Text = "Browse CSV";
            this.browseCSV.UseVisualStyleBackColor = true;
            this.browseCSV.Click += new System.EventHandler(this.browseCSV_Click);
            // 
            // browseEDA
            // 
            this.browseEDA.Location = new System.Drawing.Point(423, 39);
            this.browseEDA.Name = "browseEDA";
            this.browseEDA.Size = new System.Drawing.Size(75, 23);
            this.browseEDA.TabIndex = 1;
            this.browseEDA.Text = "Browse EDA";
            this.browseEDA.UseVisualStyleBackColor = true;
            this.browseEDA.Click += new System.EventHandler(this.browseEDA_Click);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(423, 64);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // csvFilename
            // 
            this.csvFilename.Location = new System.Drawing.Point(12, 12);
            this.csvFilename.Name = "csvFilename";
            this.csvFilename.Size = new System.Drawing.Size(405, 20);
            this.csvFilename.TabIndex = 5;
            this.csvFilename.TextChanged += new System.EventHandler(this.csvFilename_TextChanged);
            // 
            // edaFilename
            // 
            this.edaFilename.Location = new System.Drawing.Point(12, 38);
            this.edaFilename.Name = "edaFilename";
            this.edaFilename.Size = new System.Drawing.Size(405, 20);
            this.edaFilename.TabIndex = 5;
            this.edaFilename.TextChanged += new System.EventHandler(this.edaFilename_TextChanged);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(341, 64);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(510, 97);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.edaFilename);
            this.Controls.Add(this.csvFilename);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.browseEDA);
            this.Controls.Add(this.browseCSV);
            this.Name = "mainWindow";
            this.Text = "CSV (firehose) To EDASSISTANT";
            this.Load += new System.EventHandler(this.mainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseCSV;
        private System.Windows.Forms.Button browseEDA;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.TextBox csvFilename;
        private System.Windows.Forms.TextBox edaFilename;
        private System.Windows.Forms.Button cancel;
        private csvData data;
        private bool fileWasCreated;
        private GameData outputData;
        //private stationSystemMap mapData;
    }
}

