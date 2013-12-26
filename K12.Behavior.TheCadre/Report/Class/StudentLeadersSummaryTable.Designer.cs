namespace K12.Behavior.TheCadre
{
    partial class StudentLeadersSummaryTable
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
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.cbClassCadre = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cbSchoolCadre = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cbAndCadre = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.intSchoolYear = new DevComponents.Editors.IntegerInput();
            this.intSemester = new DevComponents.Editors.IntegerInput();
            this.lbSchoolYear = new DevComponents.DotNetBar.LabelX();
            this.lbSemester = new DevComponents.DotNetBar.LabelX();
            this.gbSelectPrintFom = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.gbSelectPrintType = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cbPrintAllSchoolYear = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).BeginInit();
            this.gbSelectPrintFom.SuspendLayout();
            this.gbSelectPrintType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.AutoSize = true;
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Location = new System.Drawing.Point(157, 225);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 25);
            this.btnPrint.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(238, 225);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbClassCadre
            // 
            this.cbClassCadre.AutoSize = true;
            this.cbClassCadre.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbClassCadre.BackgroundStyle.Class = "";
            this.cbClassCadre.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbClassCadre.Location = new System.Drawing.Point(12, 25);
            this.cbClassCadre.Name = "cbClassCadre";
            this.cbClassCadre.Size = new System.Drawing.Size(80, 21);
            this.cbClassCadre.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbClassCadre.TabIndex = 4;
            this.cbClassCadre.Text = "班級幹部";
            // 
            // cbSchoolCadre
            // 
            this.cbSchoolCadre.AutoSize = true;
            this.cbSchoolCadre.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbSchoolCadre.BackgroundStyle.Class = "";
            this.cbSchoolCadre.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbSchoolCadre.Location = new System.Drawing.Point(108, 25);
            this.cbSchoolCadre.Name = "cbSchoolCadre";
            this.cbSchoolCadre.Size = new System.Drawing.Size(80, 21);
            this.cbSchoolCadre.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbSchoolCadre.TabIndex = 5;
            this.cbSchoolCadre.Text = "學校幹部";
            // 
            // cbAndCadre
            // 
            this.cbAndCadre.AutoSize = true;
            this.cbAndCadre.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbAndCadre.BackgroundStyle.Class = "";
            this.cbAndCadre.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbAndCadre.Location = new System.Drawing.Point(204, 25);
            this.cbAndCadre.Name = "cbAndCadre";
            this.cbAndCadre.Size = new System.Drawing.Size(80, 21);
            this.cbAndCadre.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbAndCadre.TabIndex = 6;
            this.cbAndCadre.Text = "社團幹部";
            // 
            // intSchoolYear
            // 
            this.intSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSchoolYear.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSchoolYear.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSchoolYear.Location = new System.Drawing.Point(90, 23);
            this.intSchoolYear.MaxValue = 999;
            this.intSchoolYear.MinValue = 90;
            this.intSchoolYear.Name = "intSchoolYear";
            this.intSchoolYear.ShowUpDown = true;
            this.intSchoolYear.Size = new System.Drawing.Size(53, 25);
            this.intSchoolYear.TabIndex = 7;
            this.intSchoolYear.Value = 90;
            // 
            // intSemester
            // 
            this.intSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSemester.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intSemester.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSemester.Location = new System.Drawing.Point(211, 23);
            this.intSemester.MaxValue = 2;
            this.intSemester.MinValue = 1;
            this.intSemester.Name = "intSemester";
            this.intSemester.ShowUpDown = true;
            this.intSemester.Size = new System.Drawing.Size(53, 25);
            this.intSemester.TabIndex = 8;
            this.intSemester.Value = 1;
            // 
            // lbSchoolYear
            // 
            this.lbSchoolYear.AutoSize = true;
            this.lbSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbSchoolYear.BackgroundStyle.Class = "";
            this.lbSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbSchoolYear.Location = new System.Drawing.Point(26, 25);
            this.lbSchoolYear.Name = "lbSchoolYear";
            this.lbSchoolYear.Size = new System.Drawing.Size(47, 21);
            this.lbSchoolYear.TabIndex = 9;
            this.lbSchoolYear.Text = "學年度";
            // 
            // lbSemester
            // 
            this.lbSemester.AutoSize = true;
            this.lbSemester.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbSemester.BackgroundStyle.Class = "";
            this.lbSemester.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbSemester.Location = new System.Drawing.Point(160, 25);
            this.lbSemester.Name = "lbSemester";
            this.lbSemester.Size = new System.Drawing.Size(34, 21);
            this.lbSemester.TabIndex = 10;
            this.lbSemester.Text = "學期";
            // 
            // gbSelectPrintFom
            // 
            this.gbSelectPrintFom.BackColor = System.Drawing.Color.Transparent;
            this.gbSelectPrintFom.CanvasColor = System.Drawing.SystemColors.Control;
            this.gbSelectPrintFom.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gbSelectPrintFom.Controls.Add(this.lbSchoolYear);
            this.gbSelectPrintFom.Controls.Add(this.intSchoolYear);
            this.gbSelectPrintFom.Controls.Add(this.lbSemester);
            this.gbSelectPrintFom.Controls.Add(this.intSemester);
            this.gbSelectPrintFom.Location = new System.Drawing.Point(11, 11);
            this.gbSelectPrintFom.Name = "gbSelectPrintFom";
            this.gbSelectPrintFom.Size = new System.Drawing.Size(302, 99);
            // 
            // 
            // 
            this.gbSelectPrintFom.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gbSelectPrintFom.Style.BackColorGradientAngle = 90;
            this.gbSelectPrintFom.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gbSelectPrintFom.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintFom.Style.BorderBottomWidth = 1;
            this.gbSelectPrintFom.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gbSelectPrintFom.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintFom.Style.BorderLeftWidth = 1;
            this.gbSelectPrintFom.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintFom.Style.BorderRightWidth = 1;
            this.gbSelectPrintFom.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintFom.Style.BorderTopWidth = 1;
            this.gbSelectPrintFom.Style.Class = "";
            this.gbSelectPrintFom.Style.CornerDiameter = 4;
            this.gbSelectPrintFom.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gbSelectPrintFom.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gbSelectPrintFom.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gbSelectPrintFom.StyleMouseDown.Class = "";
            this.gbSelectPrintFom.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gbSelectPrintFom.StyleMouseOver.Class = "";
            this.gbSelectPrintFom.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gbSelectPrintFom.TabIndex = 12;
            this.gbSelectPrintFom.Text = "選擇列印範圍";
            // 
            // gbSelectPrintType
            // 
            this.gbSelectPrintType.BackColor = System.Drawing.Color.Transparent;
            this.gbSelectPrintType.CanvasColor = System.Drawing.SystemColors.Control;
            this.gbSelectPrintType.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gbSelectPrintType.Controls.Add(this.cbClassCadre);
            this.gbSelectPrintType.Controls.Add(this.cbSchoolCadre);
            this.gbSelectPrintType.Controls.Add(this.cbAndCadre);
            this.gbSelectPrintType.Location = new System.Drawing.Point(11, 116);
            this.gbSelectPrintType.Name = "gbSelectPrintType";
            this.gbSelectPrintType.Size = new System.Drawing.Size(302, 98);
            // 
            // 
            // 
            this.gbSelectPrintType.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gbSelectPrintType.Style.BackColorGradientAngle = 90;
            this.gbSelectPrintType.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gbSelectPrintType.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintType.Style.BorderBottomWidth = 1;
            this.gbSelectPrintType.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gbSelectPrintType.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintType.Style.BorderLeftWidth = 1;
            this.gbSelectPrintType.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintType.Style.BorderRightWidth = 1;
            this.gbSelectPrintType.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gbSelectPrintType.Style.BorderTopWidth = 1;
            this.gbSelectPrintType.Style.Class = "";
            this.gbSelectPrintType.Style.CornerDiameter = 4;
            this.gbSelectPrintType.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gbSelectPrintType.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gbSelectPrintType.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gbSelectPrintType.StyleMouseDown.Class = "";
            this.gbSelectPrintType.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gbSelectPrintType.StyleMouseOver.Class = "";
            this.gbSelectPrintType.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gbSelectPrintType.TabIndex = 13;
            this.gbSelectPrintType.Text = "選擇列印類型";
            // 
            // cbPrintAllSchoolYear
            // 
            this.cbPrintAllSchoolYear.AutoSize = true;
            this.cbPrintAllSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.cbPrintAllSchoolYear.BackgroundStyle.Class = "";
            this.cbPrintAllSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.cbPrintAllSchoolYear.Location = new System.Drawing.Point(173, 13);
            this.cbPrintAllSchoolYear.Name = "cbPrintAllSchoolYear";
            this.cbPrintAllSchoolYear.Size = new System.Drawing.Size(121, 21);
            this.cbPrintAllSchoolYear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbPrintAllSchoolYear.TabIndex = 11;
            this.cbPrintAllSchoolYear.Text = "列印所有學年期";
            this.cbPrintAllSchoolYear.CheckedChanged += new System.EventHandler(this.cbPrintAllSchoolYear_CheckedChanged);
            // 
            // StudentLeadersSummaryTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 261);
            this.Controls.Add(this.cbPrintAllSchoolYear);
            this.Controls.Add(this.gbSelectPrintType);
            this.Controls.Add(this.gbSelectPrintFom);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPrint);
            this.Name = "StudentLeadersSummaryTable";
            this.Text = "班級幹部總表";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StudentLeadersSummaryTable_FormClosing);
            this.Load += new System.EventHandler(this.StudentLeadersSummaryTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).EndInit();
            this.gbSelectPrintFom.ResumeLayout(false);
            this.gbSelectPrintFom.PerformLayout();
            this.gbSelectPrintType.ResumeLayout(false);
            this.gbSelectPrintType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbClassCadre;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbSchoolCadre;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbAndCadre;
        private DevComponents.Editors.IntegerInput intSchoolYear;
        private DevComponents.Editors.IntegerInput intSemester;
        private DevComponents.DotNetBar.LabelX lbSchoolYear;
        private DevComponents.DotNetBar.LabelX lbSemester;
        private DevComponents.DotNetBar.Controls.GroupPanel gbSelectPrintFom;
        private DevComponents.DotNetBar.Controls.GroupPanel gbSelectPrintType;
        private DevComponents.DotNetBar.Controls.CheckBoxX cbPrintAllSchoolYear;
    }
}