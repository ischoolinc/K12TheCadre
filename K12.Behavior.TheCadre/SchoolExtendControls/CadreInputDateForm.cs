using DevComponents.Editors.DateTimeAdv;
using FISCA.Presentation.Controls;
using K12.Data.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K12.Behavior.TheCadre
{
    public partial class CadreInputDateForm : BaseForm
    {
        string ConfigName = "設定教師幹部輸入時間";
        ConfigData cd { get; set; }

        public CadreInputDateForm()
        {
            InitializeComponent();

            LoadPreference();
        }

        private void LoadPreference()
        {
            //載入日期時間
            cd = K12.Data.School.Configuration[ConfigName];
            string StartDate = cd["開始時間"];
            string EndDate = cd["結束時間"];
            DateTime dt1;
            DateTime dt2;
            if (DateTime.TryParse(StartDate, out dt1))
                dateTimeInput1.Value = dt1;
            else
            {
                this.Text = "";
                dateTimeInput1.Value = DateTime.Today;
            }

            if (DateTime.TryParse(EndDate, out dt2))
                dateTimeInput2.Value = dt2;
            else
            {
                this.Text = "";
                dateTimeInput2.Value = DateTime.Today;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SavePreference();

                string logtring = string.Format("儲存幹部登錄時間：\n開始時間「{0}」結束時間「{1}」",
    dateTimeInput1.Value.ToString("yyyy/MM/dd"), dateTimeInput2.Value.ToString("yyyy/MM/dd"));

                FISCA.LogAgent.ApplicationLog.Log("幹部登錄時間設定", "修改", logtring);
            }
            catch (Exception ex)
            {
                MsgBox.Show("發生錯誤:\n" + ex.Message);
                return;
            }

            MsgBox.Show("儲存成功!");

            this.Close();
        }

        private void SavePreference()
        {
            cd = K12.Data.School.Configuration[ConfigName];

            DateTime dt1 = dateTimeInput1.Value;
            DateTime dt2 = dateTimeInput2.Value;

            cd["開始時間"] = dt1.ToString("yyyy/MM/dd");
            cd["結束時間"] = dt2.ToString("yyyy/MM/dd");
            cd.Save();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
