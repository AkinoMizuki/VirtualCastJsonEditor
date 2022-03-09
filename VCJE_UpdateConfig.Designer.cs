namespace VirtualCastJsonEditor
{
    partial class VCJE_UpdateConfig
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
            this.UpdateConfigBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // UpdateConfigBox
            // 
            this.UpdateConfigBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.UpdateConfigBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpdateConfigBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.UpdateConfigBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.UpdateConfigBox.Location = new System.Drawing.Point(0, 0);
            this.UpdateConfigBox.Multiline = true;
            this.UpdateConfigBox.Name = "UpdateConfigBox";
            this.UpdateConfigBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UpdateConfigBox.Size = new System.Drawing.Size(484, 491);
            this.UpdateConfigBox.TabIndex = 0;
            // 
            // VCJE_UpdateConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(484, 491);
            this.Controls.Add(this.UpdateConfigBox);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCJE_UpdateConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "更新確認";
            this.Load += new System.EventHandler(this.VCJE_UpdateConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UpdateConfigBox;
    }
}