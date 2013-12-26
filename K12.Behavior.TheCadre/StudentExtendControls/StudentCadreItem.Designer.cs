namespace K12.Behavior.TheCadre
{
    partial class StudentCadreItem
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentCadreItem));
            this.dataGridViewX1 = new K12.Behavior.TheCadre.CustomDataGridView();
            this.ColCadreID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CadreType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColSchoolYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSemster = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCadreName = new K12.Behavior.TheCadre.DataGridViewComboBoxExColumn();
            this.ColCadreRefName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCadreID,
            this.CadreType,
            this.ColSchoolYear,
            this.ColSemster,
            this.ColCadreName,
            this.ColCadreRefName});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(13, 11);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.Size = new System.Drawing.Size(524, 153);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewX1_CellBeginEdit);
            this.dataGridViewX1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellEndEdit);
            this.dataGridViewX1.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewX1_UserAddedRow);
            // 
            // ColCadreID
            // 
            this.ColCadreID.HeaderText = "幹部參考ID";
            this.ColCadreID.Name = "ColCadreID";
            this.ColCadreID.Visible = false;
            this.ColCadreID.Width = 5;
            // 
            // CadreType
            // 
            this.CadreType.HeaderText = "幹部類別";
            this.CadreType.Items.AddRange(new object[] {
            "班級幹部",
            "社團幹部",
            "學校幹部"});
            this.CadreType.Name = "CadreType";
            this.CadreType.Width = 90;
            // 
            // ColSchoolYear
            // 
            this.ColSchoolYear.HeaderText = "學年度";
            this.ColSchoolYear.Name = "ColSchoolYear";
            this.ColSchoolYear.Width = 75;
            // 
            // ColSemster
            // 
            this.ColSemster.HeaderText = "學期";
            this.ColSemster.Name = "ColSemster";
            this.ColSemster.Width = 60;
            // 
            // ColCadreName
            // 
            this.ColCadreName.HeaderText = "幹部名稱";
            this.ColCadreName.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("ColCadreName.Items")));
            this.ColCadreName.Name = "ColCadreName";
            this.ColCadreName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColCadreName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColCadreName.Width = 110;
            // 
            // ColCadreRefName
            // 
            this.ColCadreRefName.HeaderText = "說明";
            this.ColCadreRefName.Name = "ColCadreRefName";
            this.ColCadreRefName.Width = 200;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(16, 171);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(86, 17);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "管理幹部名稱";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.AutoSize = true;
            this.checkBoxX1.Location = new System.Drawing.Point(317, 171);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(191, 21);
            this.checkBoxX1.TabIndex = 2;
            this.checkBoxX1.Text = "新增資料時,帶入學年度學期";
            this.checkBoxX1.CheckedChanged += new System.EventHandler(this.checkBoxX1_CheckedChanged);
            // 
            // StudentCadreItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxX1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.dataGridViewX1);
            this.Name = "StudentCadreItem";
            this.Size = new System.Drawing.Size(550, 200);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomDataGridView dataGridViewX1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCadreID;
        private System.Windows.Forms.DataGridViewComboBoxColumn CadreType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSchoolYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSemster;
        private DataGridViewComboBoxExColumn ColCadreName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCadreRefName;
    }
}
