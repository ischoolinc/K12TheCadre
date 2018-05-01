using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.Data;
using Aspose.Cells;
using System.IO;
using System.Diagnostics;
using FISCA.Presentation;
using FISCA.LogAgent;
using FISCA.Authentication;

namespace K12.Behavior.TheCadre.CadreEdit
{
    public partial class CadreEditForm : BaseForm
    {
        private Dictionary<string, string> classDic = new Dictionary<string, string>();

        private string clientInfo = ClientInfo.GetCurrentClientInfo().OutputResult().OuterXml.Replace("'", "''");

        private string actor = DSAServices.UserAccount.Replace("'", "''");

        public CadreEditForm()
        {
            InitializeComponent();

            conditionCbx.SelectedIndex = 0;

            // Init SchoolYearCbx、semesterCbx
            int schoolYear = int.Parse("" + School.DefaultSchoolYear);
            int semester = int.Parse("" + School.DefaultSemester);
            schoolYearCbx.Items.Add(schoolYear + 1);
            schoolYearCbx.Items.Add(schoolYear);
            schoolYearCbx.Items.Add(schoolYear - 1);
            schoolYearCbx.SelectedIndex = 1;

            semesterCbx.Items.Add(1);
            semesterCbx.Items.Add(2);
            semesterCbx.SelectedIndex = semester - 1;

            // Init ClassCbx
            string sql2 = "SELECT * FROM class ORDER BY grade_year, display_order";
            QueryHelper qh2 = new QueryHelper();
            DataTable dt2 = qh2.Select(sql2);
            foreach (DataRow row in dt2.Rows)
            {
                classCbx.Items.Add("" + row["class_name"]);
                if (!classDic.ContainsKey("" + row["class_name"]))
                {
                    classDic.Add("" + row["class_name"], "" + row["id"]);
                }
            }
            // Init GradeYearCbx
            string sql = "SELECT DISTINCT grade_year FROM class WHERE grade_year IS NOT NULL ORDER BY grade_year";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);
            foreach (DataRow row in dt.Rows)
            {
                gradeYearCbx.Items.Add("" + row[0]);
            }
            gradeYearCbx.SelectedIndex = 0;

            // Init CadreNameCbx
            string sql3 = "SELECT DISTINCT cadreName FROM $behavior.thecadre";
            QueryHelper qh3 = new QueryHelper();
            DataTable dt3 = qh2.Select(sql3);
            foreach (DataRow row in dt3.Rows)
            {
                cadreNameCbx.Items.Add("" + row[0]);
            }
            cadreNameCbx.SelectedIndex = 0;
        }

        private void conditionCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = conditionCbx.SelectedIndex;

            lb1.Visible = false;
            lb2.Visible = false;

            classCbx.Visible = false;
            gradeYearCbx.Visible = false;
            studentNumberTbx.Visible = false;
            seatNoTbx.Visible = false;
            cadreNameCbx.Visible = false;

            if (index == 1)
            {
                lb1.Text = "年級";
                lb1.Visible = true;
                gradeYearCbx.Visible = true;
            }
            if (index == 2)
            {
                lb1.Text = "班級";
                lb1.Visible = true;
                classCbx.Visible = true;
            }
            if (index == 3)
            {
                lb1.Text = "學號";
                lb1.Visible = true;
                studentNumberTbx.Visible = true;
            }
            if (index == 4)
            {
                lb1.Text = "班級";
                lb2.Text = "座號";
                lb1.Visible = true;
                lb2.Visible = true;
                classCbx.Visible = true;
                seatNoTbx.Visible = true;
            }
            if (index == 5)
            {
                lb1.Text = "幹部名稱";
                lb1.Visible = true;

                cadreNameCbx.Visible = true;
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            IDList.Clear();
            #region 整理查詢條件

            string condition = conditionCbx.SelectedItem.ToString();
            List<string> whereList = new List<string>();

            if (!allCkbx.Checked)
            {
                string schoolYear = schoolYearCbx.Text;
                string semester = semesterCbx.Text;
                whereList.Add("schoolyear = '" + schoolYear + "'");
                whereList.Add("semester = '" + semester + "'");
            }
            if (condition == "班級")
            {
                if (classCbx.Text == "")
                {
                    MessageBox.Show("請選擇查詢班級!");
                    return;
                }
                string classID = classDic[classCbx.Text];
                whereList.Add("class.id = " + classID);
            }
            if (condition == "年級")
            {
                string gradeYear = gradeYearCbx.Text;
                whereList.Add("class.grade_year = " + gradeYear);
            }
            if (condition == "學號")
            {
                if (studentNumberTbx.Text == "")
                {
                    MessageBox.Show("請輸入查詢學生之學號!");
                    return;
                }
                string studentNumber = studentNumberTbx.Text;
                whereList.Add("student.student_number = " + "'" + studentNumber + "'");
            }
            if (condition == "班級座號")
            {
                int i = 0;
                if (!int.TryParse(seatNoTbx.Text,out i))
                {
                    MessageBox.Show("座號格式錯誤!");
                    return;
                }
                if (classCbx.Text == "" || seatNoTbx.Text == "")
                {
                    MessageBox.Show("請輸入查詢學生之班級、座號!");
                    return;
                }
                string classID = classDic[classCbx.Text];
                string seatNo = seatNoTbx.Text;
                whereList.Add("class.id = " + classID);
                whereList.Add("student.seat_no = " + seatNo);
            }
            if (condition == "幹部名稱")
            {
                string cadreName = cadreNameCbx.SelectedItem.ToString();
                whereList.Add("cadrename = " + "'" + cadreName + "'");
            }
            string where = "";
            if (whereList.Count > 0)
            {
                where = " AND ";
            }
            where += string.Join(" AND ", whereList);

            List<string> cardTypeList = new List<string>();
            if (classCardCbx.Checked)
            {
                cardTypeList.Add("'班級幹部'");
            }
            if (clubCardCbx.Checked)
            {
                cardTypeList.Add("'社團幹部'");
            }
            if (schoolCardCbx.Checked)
            {
                cardTypeList.Add("'學校幹部'");
            }
            string cardType = string.Join(",", cardTypeList);
            #endregion

            string sql = string.Format(@"
                SELECT
                    card.*
                    , class.class_name
                    , student.student_number
                    , student.seat_no
                    , student.name
                FROM
                    $behavior.thecadre AS card
                    LEFT OUTER JOIN student
                        ON student.id = card.studentid::BIGINT
                    LEFT OUTER JOIN class
                        ON class.id = student.ref_class_id
                WHERE
                    card.referencetype IN ( {0} )
                    {1}
                ", cardType, where);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                int index = 0;
                DataGridViewRow datarow = new DataGridViewRow();
                datarow.CreateCells(dataGridViewX1);
                datarow.Tag = "" + row["uid"];
                datarow.Cells[index++].Value = "" + row["class_name"];
                datarow.Cells[index++].Value = "" + row["seat_no"];
                datarow.Cells[index++].Value = "" + row["student_number"];
                datarow.Cells[index++].Value = "" + row["name"];
                datarow.Cells[index++].Value = "" + row["schoolyear"];
                datarow.Cells[index++].Value = "" + row["semester"];
                datarow.Cells[index++].Value = "" + row["referencetype"];
                datarow.Cells[index++].Value = "" + row["cadrename"];
                datarow.Cells[index++].Value = "" + row["text"];

                dataGridViewX1.Rows.Add(datarow);
            }
        }

        private void dataGridViewX1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenu1.Show(dataGridViewX1, new Point(e.X, e.Y));
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addTempBtn_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                list.Add("" + row.Tag);
            }
            K12.Presentation.NLDPanels.Student.AddToTemp(list);

            MessageBox.Show(string.Format("新增{0}名學生於待處理", dataGridViewX1.SelectedRows.Count));

            if (K12.Presentation.NLDPanels.Student.TempSource.Count > 0)
            {
                detailLb.Visible = true;
                detailLb.Text = string.Format("待處理學生共{0}名學生", K12.Presentation.NLDPanels.Student.TempSource.Count);

                clearTempBtn.Visible = true;
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            Workbook sample = new Workbook(new MemoryStream(Properties.Resources.匯出幹部批次修改樣版));

            Workbook wb = new Workbook();
            wb.Copy(sample);
            Worksheet sheet = wb.Worksheets[0];

            sheet.Name = "幹部資料";

            int row = 1;
            Style style = sheet.Cells.GetCellStyle(0, 0);
            int count = dataGridViewX1.Columns.Count;
            foreach (DataGridViewRow datarow in dataGridViewX1.Rows)
            {
                for (int col = 0; col < count; col++)
                {
                    sheet.Cells[row, col].PutValue(datarow.Cells[col].Value);
                    sheet.Cells[row, col].SetStyle(style);
                }
                row++;
            }

            // 存檔
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";
            SaveFileDialog.FileName = "匯出幹部批次修改";
            try
            {
                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    wb.Save(SaveFileDialog.FileName);
                    Process.Start(SaveFileDialog.FileName);
                    MotherForm.SetStatusBarMessage("課堂點名明細,列印完成!!");
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("檔案未儲存");
                    return;
                }
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                MotherForm.SetStatusBarMessage("檔案儲存錯誤,請檢查檔案是否開啟中!!");
            }
        }

        private void clearTemptBtn_Click(object sender, EventArgs e)
        {
            List<string> removeDataList = K12.Presentation.NLDPanels.Student.TempSource;
            K12.Presentation.NLDPanels.Student.RemoveFromTemp(removeDataList);

            detailLb.Visible = false;
            clearTempBtn.Visible = false;

            MessageBox.Show("已清空待處理所有學生!");
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += delegate (object ob, DoWorkEventArgs ev)
            {
                bgw.ReportProgress(10);
                #region 刪除幹部紀錄
                if (IDList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("儲存資料中有" + IDList.Count + "筆幹部紀錄將被刪除", "警告", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        UpdateHelper up2 = new UpdateHelper();
                        string IDs = string.Join(",", IDList);
                        //string sql2 = string.Format(@"DELETE FROM $behavior.thecadre WHERE uid IN ( {0} )", IDs);

                        #region LOG

                        string sql4 = string.Format(@"
                            WITH data_row AS(
                                SELECT
                                    student.name AS student_name
                                    , cadre.studentid
                                    , cadre.referencetype
                                    , cadre.cadrename
                                    , cadre.schoolyear
                                    , cadre.semester
                                FROM
                                    $behavior.thecadre AS cadre
                                    LEFT OUTER JOIN student
                                        ON student.id = cadre.studentid::BIGINT
                                WHERE
                                    cadre.uid IN ( {0} )
                            ) , delete_cadre_data AS(
                                DELETE FROM $behavior.thecadre WHERE uid IN ( {0} )
                                RETURNING *
                            )
                            INSERT INTO log(
		                        actor
		                        , action_type
		                        , action
		                        , target_category
		                        , target_id
		                        , server_time
		                        , client_info
		                        , action_by
		                        , description
	                        )
                            SELECT
                                '{1}' AS actor
		                        , 'Record' AS action_type
		                        , '刪除幹部紀錄' AS action
		                        , 'student' AS target_category
		                        , data_row.studentid AS target_id
		                        , now() AS server_time
		                        , '{2}' AS client_info
		                        , '批次修改幹部紀錄' AS action_by
		                        , '刪除幹部紀錄: 學生「'|| data_row.student_name || '」 學年度「' || data_row.schoolyear || '」 學期「' || data_row.semester || '」 幹部類別「' || data_row.referencetype || '」幹部名稱「' || data_row.cadrename || '」' AS description
                            FROM
                                data_row
                        ", IDs, actor, clientInfo, clientInfo);

                        up2.Execute(sql4);

                        #endregion

                    }
                }
                bgw.ReportProgress(40);
                #endregion

                #region 更新幹部紀錄
                List<string> dataList = new List<string>();
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (IDList.Contains("" + row.Tag))
                    {
                        continue;
                    }
                    if ("" + row.HeaderCell.Value == "修改")
                    {
                        string id = "" + row.Tag;
                        string schoolYear = "" + row.Cells[4].Value;
                        string semester = "" + row.Cells[5].Value;
                        string cadre_name = "" + row.Cells[7].Value;
                        string text = ("" + row.Cells[8].Value) == "" ? "NULL" : ("'" + row.Cells[8].Value + "'");

                        string data = string.Format(@"
                            SELECT
                                {0} AS id
                                , '{1}'::TEXT AS school_year
                                , '{2}'::TEXT AS semester
                                , '{3}'::TEXT AS cadre_name
                                , {4}::TEXT AS text
                             ", id, schoolYear, semester, cadre_name, text);

                        dataList.Add(data);
                    }
                }

                bgw.ReportProgress(60);

                if (dataList.Count > 0)
                {
                    string dataRow = string.Join(" UNION ALL ", dataList);
                    string updateSql = string.Format(@"
                        WITH data_row AS(
                            {0}
                        ) , update_data AS(
                            UPDATE $behavior.thecadre SET
                                schoolyear = data_row.school_year
                                , semester = data_row.semester
                                , cadrename = data_row.cadre_name
                                , text = data_row.text
                            FROM
                                data_row
                            WHERE
                                data_row.id = $behavior.thecadre.uid
                            RETURNING *
                        ) , old_data AS(
                            SELECT 
                                *
                            FROM
                                $behavior.thecadre AS cadre
                            WHERE
                                cadre.uid IN ( SELECT id FROM data_row )
                        )
                        INSERT INTO log(
		                    actor
		                    , action_type
		                    , action
		                    , target_category
		                    , target_id
		                    , server_time
		                    , client_info
		                    , action_by
		                    , description
	                    )
	                    SELECT
		                    '{1}' AS actor
		                    , 'Record' AS action_type
		                    , '修改幹部紀錄' AS action
		                    , 'student' AS target_category
		                    , old_data.studentid AS target_id
		                    , now() AS server_time
		                    , '{2}' AS client_info
		                    , '批次修改幹部紀錄' AS action_by
		                    , '修改幹部紀錄: 學生「'|| student.name || '」學年度「' || old_data.schoolyear || '」 學期「' || old_data.semester || '」 幹部名稱「' || old_data.cadrename || '」修改為「' || data_row.cadre_name || '」幹部說明「' || old_data.text || '」修改為「' || data_row.text || '」' AS description
	                    FROM
		                    data_row
		                    LEFT OUTER JOIN old_data	
			                    ON old_data.uid = data_row.id           
                            LEFT OUTER JOIN student
                                ON student.id = old_data.studentid::BIGINT
                ", dataRow, actor, clientInfo);

                    bgw.ReportProgress(80);

                    UpdateHelper up = new UpdateHelper();
                    up.Execute(updateSql);
                }

                bgw.ReportProgress(100);
                #endregion
            };
            bgw.RunWorkerCompleted += delegate (object ob, RunWorkerCompletedEventArgs ev)
            {
                ReloadDataGridView();

                MessageBox.Show("儲存成功");

                MotherForm.SetStatusBarMessage("幹部資料儲存完成!");
            };
            bgw.ProgressChanged += delegate (object ob, ProgressChangedEventArgs ev)
            {
                MotherForm.SetStatusBarMessage("幹部資料儲存中...", ev.ProgressPercentage);
            };
            bgw.RunWorkerAsync();

        }

        #region 右鍵事件
        private void schoolYearSemesterItem_Click(object sender, EventArgs e)
        {
            SchoolYearSemesterEditForm ssf = new SchoolYearSemesterEditForm();
            ssf.ShowDialog();

            string schoolYear = "" + ssf._schoolYear;
            string semester = "" + ssf._semester;

            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                // 4、5
                if ("" + row.Cells[4].Value != schoolYear)
                {
                    row.Cells[4].Value = schoolYear;
                    row.Cells[4].Style.BackColor = Color.PowderBlue;
                    row.HeaderCell.Value = "修改";
                }
                if ("" + row.Cells[5].Value != semester)
                {
                    row.Cells[5].Value = semester;
                    row.Cells[5].Style.BackColor = Color.PowderBlue;
                    row.HeaderCell.Value = "修改";
                }
            }
        }

        private void cardItem_Click(object sender, EventArgs e)
        {
            // 先檢查幹部類別 -- 相同的幹部類別才可以做批次修改幹部名稱
            Dictionary<string, string> cardTypeDic = new Dictionary<string, string>();
            string cardType = "";
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                cardType = "" + row.Cells[6].Value;
                if (!cardTypeDic.ContainsKey(cardType))
                {
                    cardTypeDic.Add(cardType, "");
                }
            }
            if (cardTypeDic.Keys.Count > 1)
            {
                MessageBox.Show("無法批次修改不同幹部類別的幹部名稱!");
                return;
            }

            CadreNameEditForm cnef = new CadreNameEditForm(cardType);
            cnef.ShowDialog();
            string cardName = cnef._cadreName;

            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                row.Cells[7].Value = cardName;
                row.Cells[7].Style.BackColor = Color.PowderBlue;
                row.HeaderCell.Value = "修改";
            }
        }

        private void detailItem_Click(object sender, EventArgs e)
        {
            DetailEditForm def = new DetailEditForm();
            def.ShowDialog();

            string detail = def._detail;

            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                row.Cells[8].Value = detail;
                row.Cells[8].Style.BackColor = Color.PowderBlue;
                row.HeaderCell.Value = "修改";
            }
        }

        private List<string> IDList = new List<string>(); // 幹部紀錄UID

        private void deleteItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Font f = dataGridViewX1.DefaultCellStyle.Font;
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                if (!IDList.Contains("" + row.Tag))
                {
                    IDList.Add("" + row.Tag);
                    row.DefaultCellStyle.Font = new System.Drawing.Font(f, FontStyle.Strikeout);
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.HeaderCell.Value = "刪除";
                }
            }
        }

        private void allCbx_CheckedChanged(object sender, EventArgs e)
        {
            if (allCkbx.Checked)
            {
                schoolYearCbx.Enabled = false;
                semesterCbx.Enabled = false;
            }
            if (!allCkbx.Checked)
            {
                schoolYearCbx.Enabled = true;
                semesterCbx.Enabled = true;
            }
        }

        private void cancelDeleteItem_Click(object sender, EventArgs e)
        {
            System.Drawing.Font f = dataGridViewX1.DefaultCellStyle.Font;
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                int index = IDList.IndexOf("" + row.Tag);
                IDList.RemoveAt(index);
                row.DefaultCellStyle.Font = new System.Drawing.Font(f, FontStyle.Regular);
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.HeaderCell.Value = "";

            }
        }

        private void addTempItem_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                list.Add("" + row.Tag);
            }
            K12.Presentation.NLDPanels.Student.AddToTemp(list);

            MessageBox.Show(string.Format("新增{0}名學生於待處理", dataGridViewX1.SelectedRows.Count));

            if (K12.Presentation.NLDPanels.Student.TempSource.Count > 0)
            {
                detailLb.Visible = true;
                detailLb.Text = string.Format("待處理學生共{0}名學生", K12.Presentation.NLDPanels.Student.TempSource.Count);

                clearTempBtn.Visible = true;
            }
        }
        #endregion


    }
}
