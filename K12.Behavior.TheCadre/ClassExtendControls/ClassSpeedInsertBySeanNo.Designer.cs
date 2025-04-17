namespace K12.Behavior.TheCadre
{
    partial class ClassSpeedInsertBySeanNo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colCadreName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeatNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.lbSchoolYear = new DevComponents.DotNetBar.LabelX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lbSemester = new DevComponents.DotNetBar.LabelX();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSelect = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.intSemester = new System.Windows.Forms.NumericUpDown();
            this.intSchoolYear = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.AllowUserToResizeRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCadreName,
            this.colStudentName,
            this.colSeatNo});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(10, 41);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(486, 492);
            this.dataGridViewX1.TabIndex = 6;
            this.dataGridViewX1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellEndEdit);
            // 
            // colCadreName
            // 
            this.colCadreName.DataPropertyName = "_CadreName";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCyan;
            this.colCadreName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCadreName.HeaderText = "幹部名稱";
            this.colCadreName.Name = "colCadreName";
            this.colCadreName.ReadOnly = true;
            this.colCadreName.Width = 200;
            // 
            // colStudentName
            // 
            this.colStudentName.DataPropertyName = "_StudentName";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightCyan;
            this.colStudentName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colStudentName.HeaderText = "姓名";
            this.colStudentName.Name = "colStudentName";
            this.colStudentName.ReadOnly = true;
            this.colStudentName.Width = 80;
            // 
            // colSeatNo
            // 
            this.colSeatNo.DataPropertyName = "_StudentSeatNo";
            this.colSeatNo.HeaderText = "座號";
            this.colSeatNo.Name = "colSeatNo";
            this.colSeatNo.Width = 80;
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(340, 553);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "儲存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(421, 553);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
            this.lbSchoolYear.Location = new System.Drawing.Point(10, 12);
            this.lbSchoolYear.Name = "lbSchoolYear";
            this.lbSchoolYear.Size = new System.Drawing.Size(47, 21);
            this.lbSchoolYear.TabIndex = 0;
            this.lbSchoolYear.Text = "學年度";
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxX1.AutoSize = true;
            this.checkBoxX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.Class = "";
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Location = new System.Drawing.Point(104, 555);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(174, 21);
            this.checkBoxX1.TabIndex = 8;
            this.checkBoxX1.Text = "儲存完成後進行敘獎作業";
            this.checkBoxX1.TextColor = System.Drawing.Color.Red;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(12, 557);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(86, 17);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "管理幹部名稱";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
            this.lbSemester.Location = new System.Drawing.Point(136, 12);
            this.lbSemester.Name = "lbSemester";
            this.lbSemester.Size = new System.Drawing.Size(34, 21);
            this.lbSemester.TabIndex = 2;
            this.lbSemester.Text = "學期";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "_CadreName";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightCyan;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn1.HeaderText = "幹部名稱";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "_StudentName";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightCyan;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn2.HeaderText = "姓名";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 80;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "_StudentSeatNo";
            this.dataGridViewTextBoxColumn3.HeaderText = "座號";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // tbSelect
            // 
            this.tbSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.tbSelect.Border.Class = "TextBoxBorder";
            this.tbSelect.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbSelect.Location = new System.Drawing.Point(319, 10);
            this.tbSelect.Name = "tbSelect";
            this.tbSelect.Size = new System.Drawing.Size(177, 25);
            this.tbSelect.TabIndex = 5;
            this.tbSelect.WatermarkText = "幹部名稱 / 姓名";
            this.tbSelect.TextChanged += new System.EventHandler(this.tbSelect_TextChanged);
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
            this.labelX1.Location = new System.Drawing.Point(279, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(34, 21);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "篩選";
            // 
            // intSemester
            // 
            this.intSemester.Location = new System.Drawing.Point(176, 10);
            this.intSemester.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.intSemester.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intSemester.Name = "intSemester";
            this.intSemester.Size = new System.Drawing.Size(54, 25);
            this.intSemester.TabIndex = 3;
            this.intSemester.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intSemester.ValueChanged += new System.EventHandler(this.intSemester_ValueChanged);
            // 
            // intSchoolYear
            // 
            this.intSchoolYear.Location = new System.Drawing.Point(63, 10);
            this.intSchoolYear.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.intSchoolYear.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.intSchoolYear.Name = "intSchoolYear";
            this.intSchoolYear.Size = new System.Drawing.Size(55, 25);
            this.intSchoolYear.TabIndex = 1;
            this.intSchoolYear.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.intSchoolYear.ValueChanged += new System.EventHandler(this.intSchoolYear_ValueChanged);
            // 
            // ClassSpeedInsertBySeanNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 590);
            this.Controls.Add(this.intSchoolYear);
            this.Controls.Add(this.intSemester);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.tbSelect);
            this.Controls.Add(this.lbSemester);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lbSchoolYear);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.checkBoxX1);
            this.DoubleBuffered = true;
            this.Name = "ClassSpeedInsertBySeanNo";
            this.Text = "班級幹部登錄";
            this.Load += new System.EventHandler(this.CadreByStudentSean_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSemester)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.LabelX lbSchoolYear;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevComponents.DotNetBar.LabelX lbSemester;
        private DevComponents.DotNetBar.Controls.TextBoxX tbSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCadreName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeatNo;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.NumericUpDown intSemester;
        private System.Windows.Forms.NumericUpDown intSchoolYear;
    }
}