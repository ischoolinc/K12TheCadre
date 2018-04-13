namespace K12.Behavior.TheCadre.CadreEdit
{
    partial class CadreNameEditForm
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
            this.cancelBtn = new DevComponents.DotNetBar.ButtonX();
            this.confirmBtn = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cadreNameCbx = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
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
            this.cancelBtn.TabIndex = 0;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
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
            this.confirmBtn.TabIndex = 1;
            this.confirmBtn.Text = "確定";
            this.confirmBtn.Click += new System.EventHandler(this.confirmBtn_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 38);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(107, 23);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "修改幹部名稱為:";
            // 
            // cadreNameCbx
            // 
            this.cadreNameCbx.DisplayMember = "Text";
            this.cadreNameCbx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cadreNameCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cadreNameCbx.FormattingEnabled = true;
            this.cadreNameCbx.ItemHeight = 19;
            this.cadreNameCbx.Location = new System.Drawing.Point(125, 38);
            this.cadreNameCbx.Name = "cadreNameCbx";
            this.cadreNameCbx.Size = new System.Drawing.Size(196, 25);
            this.cadreNameCbx.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cadreNameCbx.TabIndex = 4;
            // 
            // CadreNameEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 121);
            this.Controls.Add(this.cadreNameCbx);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.confirmBtn);
            this.Controls.Add(this.cancelBtn);
            this.DoubleBuffered = true;
            this.Name = "CadreNameEditForm";
            this.Text = "修改幹部名稱";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX cancelBtn;
        private DevComponents.DotNetBar.ButtonX confirmBtn;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cadreNameCbx;
    }
}