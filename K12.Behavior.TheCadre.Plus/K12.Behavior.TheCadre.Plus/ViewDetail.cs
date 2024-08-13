using Campus.ePaper;
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

namespace K12.Behavior.TheCadre.Plus
{
    public partial class ViewDetail : BaseForm
    {
        string _title;
        string _message;
        FISCA.Data.QueryHelper _q = new FISCA.Data.QueryHelper();
        List<string> _IDList = new List<string>();
        BackgroundWorker bgw = new BackgroundWorker();

        /// <summary>
        /// mode = True懲戒
        /// mode = False缺曠
        /// </summary>
        public ViewDetail()
        {
            InitializeComponent();
            ConfigData cd = K12.Data.School.Configuration["設定教師幹部輸入時間"];

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(@"親愛的老師您好
班級幹部登錄期間為
{0} ~ {1}
請依據期間內登入1Campus WEB
使用班級幹部登錄功能
指定各級幹部", cd["開始時間"], cd["結束時間"]));


            _message = sb.ToString();
            tbTitle.Text = "班級幹部 登錄期間通知";
            textBoxX1.Text = _message.Replace("<br>", "\r\n");

            //取得目前導師資料
            DataTable dt = _q.Select(@"SELECT class.id as ref_class_id,class_name,grade_year,
teacher.id as ref_teacher_id,teacher.teacher_name,teacher.st_login_name
FROM class join teacher on class.ref_teacher_id=teacher.id
where grade_year is not null
order by grade_year,class_name");


            foreach (DataRow each in dt.Rows)
            {
                DataGridViewRow gRow = new DataGridViewRow();
                gRow.CreateCells(dataGridViewX1);

                if ("" + each["st_login_name"] != "")
                {
                    gRow.Cells[0].Value = "" + each["ref_teacher_id"];
                    gRow.Cells[1].Value = "" + each["grade_year"];
                    gRow.Cells[2].Value = "" + each["class_name"];
                    gRow.Cells[3].Value = "" + each["teacher_name"];
                    gRow.Cells[4].Value = "" + each["st_login_name"];
                }
                else
                {
                    gRow.Cells[0].Value = "" + each["ref_teacher_id"];
                    gRow.Cells[0].Style.BackColor = Color.Silver;
                    gRow.Cells[1].Value = "" + each["grade_year"];
                    gRow.Cells[1].Style.BackColor = Color.Silver;
                    gRow.Cells[2].Value = "" + each["class_name"];
                    gRow.Cells[2].Style.BackColor = Color.Silver;
                    gRow.Cells[3].Value = "" + each["teacher_name"];
                    gRow.Cells[3].Style.BackColor = Color.Silver;
                    gRow.Cells[4].Value = "(無法發送)";
                    gRow.Cells[4].Style.BackColor = Color.Silver;
                }

                dataGridViewX1.Rows.Add(gRow);
            }

            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.Rows.Count > 0)
            {
                if (textBoxX1.Text != "")
                {
                    btnSendMessage.Enabled = false;
                    if (!bgw.IsBusy)
                    {
                        _title = tbTitle.Text;
                        _message = textBoxX1.Text.Replace("\r\n", "</br>");

                        foreach (DataGridViewRow row in dataGridViewX1.Rows)
                        {
                            //有帳號的教師
                            if ("" + row.Cells[4].Value != "")
                            {
                                string id = "" + row.Cells[0].Value;
                                _IDList.Add(id);
                            }
                        }

                        bgw.RunWorkerAsync();
                    }
                    else
                    {
                        MsgBox.Show("系統忙錄中\n請稍後試再試");
                    }
                }
                else
                {
                    MsgBox.Show("請輸入推播內容!");
                }
            }
            else
            {
                MsgBox.Show("清單中無可以發送的學生");
            }


        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            SendMessage send = new SendMessage(_IDList, _title, _message);
            send.Run();

        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSendMessage.Enabled = true;

            if (!e.Cancelled)
            {
                if (e.Error != null)
                {
                    MsgBox.Show("推播發生錯誤:\n" + e.Error.Message);
                }
                else
                {
                    FISCA.Presentation.MotherForm.SetStatusBarMessage("推播完成");
                    MsgBox.Show("推播完成!");

                    this.DialogResult = DialogResult.Yes;
                }
            }
            else
            {
                MsgBox.Show("作業已取消");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
