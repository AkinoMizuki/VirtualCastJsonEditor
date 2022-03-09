namespace VirtualCastJsonEditor
{
    partial class VCJE_Config
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
            this.ProjectFileBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.XML_RadioButton = new System.Windows.Forms.RadioButton();
            this.Json_RadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.NoneFile_RadioButton = new System.Windows.Forms.RadioButton();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.SteamRadioButton = new System.Windows.Forms.RadioButton();
            this.DLRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectFileBox
            // 
            this.ProjectFileBox.Location = new System.Drawing.Point(15, 32);
            this.ProjectFileBox.Name = "ProjectFileBox";
            this.ProjectFileBox.Size = new System.Drawing.Size(239, 19);
            this.ProjectFileBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "保存場所";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(260, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SelectProjectFile_MouseUp);
            // 
            // XML_RadioButton
            // 
            this.XML_RadioButton.AutoSize = true;
            this.XML_RadioButton.Location = new System.Drawing.Point(16, 18);
            this.XML_RadioButton.Name = "XML_RadioButton";
            this.XML_RadioButton.Size = new System.Drawing.Size(45, 16);
            this.XML_RadioButton.TabIndex = 5;
            this.XML_RadioButton.TabStop = true;
            this.XML_RadioButton.Text = "XML";
            this.XML_RadioButton.UseVisualStyleBackColor = true;
            // 
            // Json_RadioButton
            // 
            this.Json_RadioButton.AutoSize = true;
            this.Json_RadioButton.Location = new System.Drawing.Point(67, 18);
            this.Json_RadioButton.Name = "Json_RadioButton";
            this.Json_RadioButton.Size = new System.Drawing.Size(48, 16);
            this.Json_RadioButton.TabIndex = 6;
            this.Json_RadioButton.TabStop = true;
            this.Json_RadioButton.Text = "Json";
            this.Json_RadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ProjectFileBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 64);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DL版 Virtual Castの保存場所";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.NoneFile_RadioButton);
            this.groupBox2.Controls.Add(this.XML_RadioButton);
            this.groupBox2.Controls.Add(this.Json_RadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 43);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "起動時の読み込みファイル指定";
            // 
            // NoneFile_RadioButton
            // 
            this.NoneFile_RadioButton.AutoSize = true;
            this.NoneFile_RadioButton.Location = new System.Drawing.Point(121, 18);
            this.NoneFile_RadioButton.Name = "NoneFile_RadioButton";
            this.NoneFile_RadioButton.Size = new System.Drawing.Size(49, 16);
            this.NoneFile_RadioButton.TabIndex = 9;
            this.NoneFile_RadioButton.TabStop = true;
            this.NoneFile_RadioButton.Text = "None";
            this.NoneFile_RadioButton.UseVisualStyleBackColor = true;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(161, 176);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 9;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(80, 176);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // InitializeButton
            // 
            this.InitializeButton.Location = new System.Drawing.Point(242, 176);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Size = new System.Drawing.Size(75, 23);
            this.InitializeButton.TabIndex = 11;
            this.InitializeButton.Text = "Initialize";
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.InitializeButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SteamRadioButton);
            this.groupBox3.Controls.Add(this.DLRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(305, 43);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Virtual Cast選択";
            // 
            // SteamRadioButton
            // 
            this.SteamRadioButton.AutoSize = true;
            this.SteamRadioButton.Location = new System.Drawing.Point(16, 18);
            this.SteamRadioButton.Name = "SteamRadioButton";
            this.SteamRadioButton.Size = new System.Drawing.Size(67, 16);
            this.SteamRadioButton.TabIndex = 5;
            this.SteamRadioButton.TabStop = true;
            this.SteamRadioButton.Text = "Steam版";
            this.SteamRadioButton.UseVisualStyleBackColor = true;
            this.SteamRadioButton.CheckedChanged += new System.EventHandler(this.SteamRadioButton_CheckedChanged);
            // 
            // DLRadioButton
            // 
            this.DLRadioButton.AutoSize = true;
            this.DLRadioButton.Location = new System.Drawing.Point(89, 18);
            this.DLRadioButton.Name = "DLRadioButton";
            this.DLRadioButton.Size = new System.Drawing.Size(49, 16);
            this.DLRadioButton.TabIndex = 6;
            this.DLRadioButton.TabStop = true;
            this.DLRadioButton.Text = "DL版";
            this.DLRadioButton.UseVisualStyleBackColor = true;
            // 
            // VCJE_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(334, 211);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.InitializeButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCJE_Config";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Config";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ProjectFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton XML_RadioButton;
        private System.Windows.Forms.RadioButton Json_RadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton NoneFile_RadioButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton SteamRadioButton;
        private System.Windows.Forms.RadioButton DLRadioButton;
    }
}