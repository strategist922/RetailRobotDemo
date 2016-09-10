namespace RetailClientWinForm
{
    partial class RobotMain
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
            this.components = new System.ComponentModel.Container();
            this.log = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Right;
            this.log.Location = new System.Drawing.Point(177, 0);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(804, 466);
            this.log.TabIndex = 0;
            // 
            // start
            // 
            this.start.Dock = System.Windows.Forms.DockStyle.Top;
            this.start.Location = new System.Drawing.Point(0, 0);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(177, 23);
            this.start.TabIndex = 1;
            this.start.Text = "&Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // RobotMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 466);
            this.Controls.Add(this.start);
            this.Controls.Add(this.log);
            this.Name = "RobotMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button start;
    }
}

