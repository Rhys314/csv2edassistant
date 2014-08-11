namespace csv2edassistant
{
    partial class starSystemNames_Input
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
            this.starSystemName = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.stationName = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // starSystemName
            // 
            this.starSystemName.Location = new System.Drawing.Point(12, 30);
            this.starSystemName.Name = "starSystemName";
            this.starSystemName.Size = new System.Drawing.Size(167, 20);
            this.starSystemName.TabIndex = 0;
            this.starSystemName.TextChanged += new System.EventHandler(this.starSystemName_TextChanged);
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(197, 56);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // stationName
            // 
            this.stationName.AutoSize = true;
            this.stationName.Location = new System.Drawing.Point(185, 33);
            this.stationName.Name = "stationName";
            this.stationName.Size = new System.Drawing.Size(35, 13);
            this.stationName.TabIndex = 2;
            this.stationName.Text = "label1";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(116, 56);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // starSystemNames_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 87);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.stationName);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.starSystemName);
            this.Name = "starSystemNames_Input";
            this.Text = "StarSystemNames";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox starSystemName;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label stationName;
        private System.Windows.Forms.Button cancel;
        private string startSystemNameValue;
    }
}