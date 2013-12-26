namespace K12.Behavior.TheCadre
{
    partial class CadreProveReport
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
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxX3 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnPrint.Location = new System.Drawing.Point(187, 103);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.labelX1.Location = new System.Drawing.Point(11, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(114, 21);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "請選擇列印內容：";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.btnExit.Location = new System.Drawing.Point(271, 103);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.AutoSize = true;
            this.checkBoxX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.Class = "";
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Checked = true;
            this.checkBoxX1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxX1.CheckValue = "Y";
            this.checkBoxX1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.checkBoxX1.Location = new System.Drawing.Point(51, 57);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(80, 21);
            this.checkBoxX1.TabIndex = 3;
            this.checkBoxX1.Text = "社團幹部";
            // 
            // checkBoxX2
            // 
            this.checkBoxX2.AutoSize = true;
            this.checkBoxX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxX2.BackgroundStyle.Class = "";
            this.checkBoxX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX2.Checked = true;
            this.checkBoxX2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxX2.CheckValue = "Y";
            this.checkBoxX2.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.checkBoxX2.Location = new System.Drawing.Point(140, 57);
            this.checkBoxX2.Name = "checkBoxX2";
            this.checkBoxX2.Size = new System.Drawing.Size(80, 21);
            this.checkBoxX2.TabIndex = 4;
            this.checkBoxX2.Text = "班級幹部";
            // 
            // checkBoxX3
            // 
            this.checkBoxX3.AutoSize = true;
            this.checkBoxX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxX3.BackgroundStyle.Class = "";
            this.checkBoxX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX3.Checked = true;
            this.checkBoxX3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxX3.CheckValue = "Y";
            this.checkBoxX3.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.checkBoxX3.Location = new System.Drawing.Point(229, 57);
            this.checkBoxX3.Name = "checkBoxX3";
            this.checkBoxX3.Size = new System.Drawing.Size(80, 21);
            this.checkBoxX3.TabIndex = 5;
            this.checkBoxX3.Text = "學校幹部";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.linkLabel1.Location = new System.Drawing.Point(10, 109);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(60, 17);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "設定樣板";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Location = new System.Drawing.Point(79, 109);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(99, 17);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "下載欄位總位表";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // CadreProveReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 134);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.checkBoxX3);
            this.Controls.Add(this.checkBoxX2);
            this.Controls.Add(this.checkBoxX1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnPrint);
            this.Name = "CadreProveReport";
            this.Text = "學生幹部證明單";
            this.Load += new System.EventHandler(this.StudentCadreProve_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX2;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}