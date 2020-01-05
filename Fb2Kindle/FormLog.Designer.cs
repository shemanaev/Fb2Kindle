namespace Fb2Kindle
{
    partial class FormLog
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
            this.textLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textLog
            // 
            this.textLog.AcceptsReturn = true;
            this.textLog.AcceptsTab = true;
            this.textLog.BackColor = System.Drawing.SystemColors.Window;
            this.textLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textLog.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLog.Location = new System.Drawing.Point(0, 0);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textLog.Size = new System.Drawing.Size(784, 561);
            this.textLog.TabIndex = 0;
            this.textLog.WordWrap = false;
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.textLog);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textLog;
    }
}