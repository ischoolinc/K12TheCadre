namespace K12.Behavior.TheCadre.CadreEdit
{
    partial class SchoolYearSemesterEditForm
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
            this.confirmBtn = new DevComponents.DotNetBar.ButtonX();
            this.cancelBtn = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.schoolYearIP = new DevComponents.Editors.IntegerInput();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.semesterIP = new DevComponents.Editors.IntegerInput();
            ((System.ComponentModel.ISupportInitialize)(this.schoolYearIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.semesterIP)).BeginInit();
            this.SuspendLayout();
            // 
            // confirmBtn
            // 
            this.confirmBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.confirmBtn.BackColor = System.Drawing.Color.Transparent;
            this.confirmBtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.confirmBtn.Location = new System.Drawing.Point(165, 86);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmBtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.confirmBtn.TabIndex = 0;
            this.confirmBtn.Text = "確認";
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.cancelBtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelBtn.Location = new System.Drawing.Point(246, 86);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 37);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(50, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "學年度";
            // 
            // schoolYearIP
            // 
            this.schoolYearIP.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.schoolYearIP.BackgroundStyle.Class = "DateTimeInputBackground";
            this.schoolYearIP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.schoolYearIP.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.schoolYearIP.Location = new System.Drawing.Point(68, 35);
            this.schoolYearIP.Name = "schoolYearIP";
            this.schoolYearIP.ShowUpDown = true;
            this.schoolYearIP.Size = new System.Drawing.Size(80, 25);
            this.schoolYearIP.TabIndex = 3;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(184, 37);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(50, 23);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "學期";
            // 
            // semesterIP
            // 
            this.semesterIP.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.semesterIP.BackgroundStyle.Class = "DateTimeInputBackground";
            this.semesterIP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.semesterIP.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.semesterIP.Location = new System.Drawing.Point(240, 35);
            this.semesterIP.Name = "semesterIP";
            this.semesterIP.ShowUpDown = true;
            this.semesterIP.Size = new System.Drawing.Size(80, 25);
            this.semesterIP.TabIndex = 5;
            // 
            // SchoolYearSemeaterEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 121);
            this.Controls.Add(this.semesterIP);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.schoolYearIP);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.confirmBtn);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(349, 160);
            this.MinimumSize = new System.Drawing.Size(349, 160);
            this.Name = "SchoolYearSemeaterEditForm";
            this.Text = "修改學年度學期";
            ((System.ComponentModel.ISupportInitialize)(this.schoolYearIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.semesterIP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX confirmBtn;
        private DevComponents.DotNetBar.ButtonX cancelBtn;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.Editors.IntegerInput schoolYearIP;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.Editors.IntegerInput semesterIP;
    }
}