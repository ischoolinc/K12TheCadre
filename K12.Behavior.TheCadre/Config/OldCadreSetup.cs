using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace K12.Behavior.TheCadre
{
    public partial class OldCadreSetup : BaseForm
    {
        K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];

        public OldCadreSetup()
        {
            InitializeComponent();

            if (DateConfig.Count != 0)
            {
                foreach (string each in DateConfig["幹部名稱"].Split(','))
                {
                    if (!string.IsNullOrEmpty(each))
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridViewX1);
                        row.Cells[Column1.Index].Value = each;
                        dataGridViewX1.Rows.Add(row);
                    }
                }
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;
            Column5.Visible = true;
            Column6.Visible = true;
            Column7.Visible = true;
            DataGridViewExport export = new DataGridViewExport(dataGridViewX1);
            export.Save(saveFileDialog1.FileName);
            Column2.Visible = false;
            Column3.Visible = false;
            Column4.Visible = false;
            Column5.Visible = false;
            Column6.Visible = false;
            Column7.Visible = false;
            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            foreach (DataGridViewRow eachRow in dataGridViewX1.Rows)
            {
                if (!eachRow.IsNewRow)
                {
                    list.Add("" + eachRow.Cells[Column1.Index].Value);
                }
            }

            DateConfig["幹部名稱"] = string.Join(",", list.ToArray());

            DateConfig.Save();
            this.Close();
        }
    }
}
