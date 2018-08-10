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
using FISCA.DSAUtil;
using Framework.Feature;
using System.Xml;
using FISCA.UDT;

namespace K12.Behavior.TheCadre.CadreMeritManage
{
    public partial class CadreMeritManage : BaseForm
    {
        private Dictionary<string, ClassCadreNameObj> _dicCadreNameObByKey = new Dictionary<string, ClassCadreNameObj>();
        private int _schoolYear = int.Parse(School.DefaultSchoolYear);
        private int _semester = int.Parse(School.DefaultSemester);
        private bool initFinish = false;
        private AccessHelper _access = new AccessHelper();

        public CadreMeritManage()
        {
            InitializeComponent();
        }

        private void CadreMeritManage_Load(object sender, EventArgs e)
        {
            #region Init _dicCadreNameOb
            {
                // 取得幹部敘獎物件資料
                List<ClassCadreNameObj> listCNO = this._access.Select<ClassCadreNameObj>();

                // 資料整理
                foreach (ClassCadreNameObj obj in listCNO)
                {
                    string key = string.Format("{0}_{1}", obj.NameType, obj.CadreName);

                    if (!this._dicCadreNameObByKey.ContainsKey(key))
                    {
                        this._dicCadreNameObByKey.Add(key, obj);
                    }
                }
            }
            #endregion

            #region Init schoolYear
            {
                schoolYearCbx.Items.Add(_schoolYear + 1);
                schoolYearCbx.Items.Add(_schoolYear);
                schoolYearCbx.Items.Add(_schoolYear - 1);
                schoolYearCbx.SelectedIndex = 1;
            }
            #endregion

            #region Init semester
            {
                semesterCbx.Items.Add(1);
                semesterCbx.Items.Add(2);
                semesterCbx.SelectedIndex = _semester - 1;
            }
            #endregion

            #region Init cadreType
            {
                cadreTypeCbx.Items.Add("--全部--");
                cadreTypeCbx.Items.Add("班級幹部");
                cadreTypeCbx.Items.Add("社團幹部");
                cadreTypeCbx.Items.Add("學校幹部");
                cadreTypeCbx.SelectedIndex = 0;
            }
            #endregion

            #region Init cadreName
            {
                string sql = string.Format(@"
SELECT DISTINCT 
    cadrename
FROM
    $behavior.thecadre
WHERE
    schoolyear = '{0}'
    AND semester = '{1}'
    AND cadreName IS NOT NULL
                ", _schoolYear, _semester);
                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(sql);
                cadreNameCbx.Items.Add("--全部--");
                foreach (DataRow row in dt.Rows)
                {
                    cadreNameCbx.Items.Add("" + row["cadrename"]);
                }
                cadreNameCbx.SelectedIndex = 0;
            }
            #endregion
            
            dateTimeInput1.Value = DateTime.Now;

            ReloadDataGridView();

            initFinish = true;
        }

        public void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            List<string> studentIDList = new List<string>();
            #region Init DataGridView

            string condition = string.Format(@"
                cadre.schoolyear = '{0}'
                AND cadre.semester = '{1}'
                AND student.status IN(1,2)
                AND student.id IS NOT NULL
            ", schoolYearCbx.Text, semesterCbx.Text);
            if (cadreTypeCbx.Text != "--全部--" && cadreTypeCbx.Text != "")
            {
                condition += string.Format("AND cadre.referencetype = '{0}'", cadreTypeCbx.Text);
            }
            if (cadreNameCbx.Text != "--全部--" && cadreNameCbx.Text != "")
            {
                condition += string.Format("AND cadre.cadrename = '{0}'", cadreNameCbx.Text);
            }
            string sql = string.Format(@"
SELECT
    student.id
    , class.class_name
    , student.seat_no
    , student.name
    , cadre.cadreName
    , cadre.referencetype
FROM
    $behavior.thecadre AS cadre
    LEFT OUTER JOIN student
        ON student.id = cadre.studentid::BIGINT
    LEFT OUTER JOIN class
        ON class.id = student.ref_class_id
WHERE
    {0}
                ", condition);
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            foreach (DataRow row in dt.Rows)
            {
                DataGridViewRow dgvrow = new DataGridViewRow();
                dgvrow.CreateCells(dataGridViewX1);

                int index = 0;
                dgvrow.Cells[index++].Value = "" + row["class_name"];
                dgvrow.Cells[index++].Value = "" + row["seat_no"];
                dgvrow.Cells[index++].Value = "" + row["name"];
                dgvrow.Cells[index++].Value = "" + row["referencetype"];
                dgvrow.Cells[index++].Value = "" + row["cadrename"];
                dgvrow.Cells[8].Value = string.Format("[幹部][{0}][{1}]{2}", row["referencetype"], row["cadrename"], reasonTbx.Text);

                dgvrow.Tag = "" + row["id"]; // StudentID

                if (!studentIDList.Contains("" + row["id"]))
                {
                    studentIDList.Add("" + row["id"]);
                }
                studentIDList.Add("" + row["id"]);

                dataGridViewX1.Rows.Add(dgvrow);
            }

            #endregion

            #region 已有敘獎紀錄判斷
            // 取得學生獎勵紀錄
            List<DisciplineRecord> meritList = Discipline.SelectByStudentIDs(studentIDList);

            // 學生獎勵紀錄整理: Key 1.學生編號 2.獎勵是由 
            Dictionary<string, Dictionary<string, DisciplineRecord>> dicMeritRecordByStudentIDByReason = new Dictionary<string, Dictionary<string, DisciplineRecord>>();

            foreach (DisciplineRecord merit in meritList)
            {
                if ("" + merit.SchoolYear == schoolYearCbx.Text && "" + merit.Semester == semesterCbx.Text && merit.MeritFlag == "1")
                {
                    if (!dicMeritRecordByStudentIDByReason.ContainsKey(merit.RefStudentID))
                    {
                        dicMeritRecordByStudentIDByReason.Add(merit.RefStudentID, new Dictionary<string, DisciplineRecord>());
                    }
                    if (!dicMeritRecordByStudentIDByReason[merit.RefStudentID].ContainsKey(merit.Reason))
                    {
                        dicMeritRecordByStudentIDByReason[merit.RefStudentID].Add(merit.Reason, merit);
                    }
                }
            }

            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                bool hadMeritRecord = false;
                string cadreType = "" + dgvrow.Cells[3].Value;
                string cadreName = "" + dgvrow.Cells[4].Value;
                string studentID = "" + dgvrow.Tag;

                if (dicMeritRecordByStudentIDByReason.ContainsKey(studentID))
                {
                    string reasonKey = string.Format("[幹部][{0}][{1}]", cadreType, cadreName);
                    List<string> listReason = dicMeritRecordByStudentIDByReason[studentID].Keys.ToList();

                    foreach (string reason in listReason)
                    {
                        if (reason.Contains(reasonKey)) // 有獎勵紀錄
                        {
                            DisciplineRecord merit = dicMeritRecordByStudentIDByReason[studentID][reason];

                            dgvrow.Cells[5].Value = merit.MeritA;
                            dgvrow.Cells[6].Value = merit.MeritB;
                            dgvrow.Cells[7].Value = merit.MeritC;
                            dgvrow.Cells[8].Value = merit.Reason;
                            dgvrow.ReadOnly = true;
                            dgvrow.DefaultCellStyle.BackColor = Color.LightGray;

                            hadMeritRecord = true;
                        }
                    }
                }
                if (!hadMeritRecord) // 沒有獎勵紀錄，但幹部紀錄有對應到幹部敘獎物件的話: 初始敘獎資料
                {
                    string key = string.Format("{0}_{1}", cadreType, cadreName);

                    if (this._dicCadreNameObByKey.ContainsKey(key))
                    {
                        ClassCadreNameObj obj = this._dicCadreNameObByKey[key];
                        dgvrow.Cells[5].Value = obj.MeritA;
                        dgvrow.Cells[6].Value = obj.MeritB;
                        dgvrow.Cells[7].Value = obj.MeritC;
                        dgvrow.Cells[8].Value = string.Format("[幹部][{0}][{1}]{2}", cadreType, cadreName, obj.Reason);
                    }
                }
            }

            //foreach (DisciplineRecord merit in meritList)
            //{
            //    if ("" + merit.SchoolYear == schoolYearCbx.Text && "" + merit.Semester == semesterCbx.Text && merit.MeritFlag == "1")
            //    {
            //        foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            //        {
            //            bool hadMeritRecord = false;
            //            string cadreType = "" + dgvrow.Cells[3].Value;
            //            string cadreName = "" + dgvrow.Cells[4].Value;

            //            if (merit.RefStudentID == "" + dgvrow.Tag)
            //            {
            //                string reason = merit.Reason;
            //                hadMeritRecord = reason.Contains(string.Format("[幹部][{0}][{1}]", cadreType, cadreName));
            //            }
            //            if (hadMeritRecord) // 已有獎勵記錄
            //            {
            //                dgvrow.Cells[5].Value = merit.MeritA;
            //                dgvrow.Cells[6].Value = merit.MeritB;
            //                dgvrow.Cells[7].Value = merit.MeritC;
            //                dgvrow.Cells[8].Value = merit.Reason;
            //                dgvrow.ReadOnly = true;
            //                dgvrow.DefaultCellStyle.BackColor = Color.LightGray;
            //            }
            //            if (!hadMeritRecord) // 沒有獎勵紀錄，但幹部紀錄有對應到幹部敘獎物件的話: 初始敘獎資料
            //            {
            //                string key = string.Format("{0}_{1}", cadreType, cadreName);

            //                if (this._dicCadreNameObByKey.ContainsKey(key))
            //                {
            //                    ClassCadreNameObj obj = this._dicCadreNameObByKey[key];
            //                    dgvrow.Cells[5].Value = obj.MeritA;
            //                    dgvrow.Cells[6].Value = obj.MeritB;
            //                    dgvrow.Cells[7].Value = obj.MeritC;
            //                    dgvrow.Cells[8].Value = string.Format("[幹部][{0}][{1}]{2}", cadreType, cadreName, obj.Reason);
            //                }
            //            }
            //        }
            //    }
            //}

            #endregion
        }

        public void ReloadCadreNameCbx()
        {
            cadreNameCbx.Items.Clear();
            string condition = string.Format(@"
                schoolyear = '{0}'
                AND semester = '{1}'
                ", schoolYearCbx.Text, semesterCbx.Text);
            if (cadreNameCbx.Text != "--全部--")
            {
                condition += string.Format(" AND referencetype = '{0}'", cadreTypeCbx.Text);
            }
            string sql = string.Format(@"
SELECT DISTINCT 
    cadrename
FROM
    $behavior.thecadre
WHERE
    {0}
              ", condition);

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            cadreNameCbx.Items.Add("--全部--");
            foreach (DataRow row in dt.Rows)
            {
                cadreNameCbx.Items.Add("" + row["cadrename"]);
            }
            cadreNameCbx.SelectedIndex = 0;
        }

        private void schoolYearCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void semesterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void cadreTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initFinish)
            {
                ReloadDataGridView();
                ReloadCadreNameCbx();
            }
        }

        private void cadreNameCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initFinish)
            {
                ReloadDataGridView();
            }
        }

        // 大功
        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX1.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        row.Cells[5].Value = textBoxX1.Text;
                    }
                }
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(textBoxX1, "輸入內容非數字!!");
            }
        }
        // 小功
        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX2.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        row.Cells[6].Value = textBoxX2.Text;
                    }
                }
                errorProvider2.Clear();
            }
            else
            {
                errorProvider2.SetError(textBoxX2, "輸入內容非數字!!");
            }
        }
        // 嘉獎
        private void textBoxX3_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX3.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        row.Cells[7].Value = textBoxX3.Text;
                    }
                }
                errorProvider3.Clear();
            }
            else
            {
                errorProvider3.SetError(textBoxX3, "輸入內容非數字!!");
            }
        }
        // 事由
        private void reasonTbx_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (!row.ReadOnly)
                {
                    string reason = "" + row.Cells[8].Value;

                    int index = reason.LastIndexOf("]") + 1;
                    int length = reason.Length;
                    int removeCcount = length - index;
                    //reason += reasonTbx.Text;

                    row.Cells[8].Value = reason.Remove(index, removeCcount);
                    row.Cells[8].Value += reasonTbx.Text;
                }
            }
        }

        private bool intParse(string CellValue)
        {
            int TextIndex;
            if (int.TryParse(CellValue, out TextIndex) || string.IsNullOrEmpty(CellValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void leaveBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool checkColValue()
        {
            bool correctValue = true;
            foreach (DataGridViewRow dgv in dataGridViewX1.Rows)
            {
                int index = 0;
                for (int i = 5; i < 8; i++)
                {
                    if (!int.TryParse("" + dgv.Cells[i].Value, out index) && "" + dgv.Cells[i].Value != "") // 空值預設為0，這邊不處理
                    {
                        correctValue = false;
                    }
                }
            }
            return correctValue;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string schoolYear = schoolYearCbx.Text;
            string semester = semesterCbx.Text;
            string occurDate = dateTimeInput1.Value.ToString("yyyy/MM/dd"); //  發生日期
            string registerDate = DateTime.Now.ToString("yyyy/MM/dd"); // 獎勵登錄日期
            string merit_flag = "1";
            List<string> dataRow = new List<string>();

            if (!checkColValue())
            {
                MessageBox.Show("資料格式錯誤!");
                return;
            }

            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (!dgvrow.ReadOnly)
                {
                    int a = "" + dgvrow.Cells[5].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[5].Value);
                    int b = "" + dgvrow.Cells[6].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[6].Value);
                    int c = "" + dgvrow.Cells[7].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[7].Value);

                    if ((a + b + c) > 0)
                    {
                        string studentID = "" + dgvrow.Tag;
                        string reason = "" + dgvrow.Cells[8].Value;
                        string detail = string.Format("<Discipline><Merit A = \"{0}\" B = \"{1}\" C = \"{2}\"/></Discipline>", a, b, c);

                        string data = string.Format(@"
SELECT
{0}::INT AS school_year
, {1}::INT AS semester
, '{2}'::timestamp AS occur_date
, '{3}'::TEXT AS reason
, '{4}'::TEXT AS detail
, {5}::INT AS ref_student_id
, {6}::INT AS merit_flag
, '{7}'::timestamp AS register_date
                    ", schoolYear, semester, occurDate, reason, detail, studentID, merit_flag, registerDate);

                        dataRow.Add(data);
                    }
                }
            }

            bool toSave = true;
            if (dataRow.Count == 0)
            {
                MessageBox.Show("沒有可登錄資料!");
                toSave = false;
            }
            if (toSave)
            {
                string sql = string.Format(@"
WITH data_row AS(
    {0}
)
INSERT INTO discipline(
    school_year
    , semester
    , occur_date
    , reason
    , detail
    , ref_student_id
    , merit_flag
    , register_date
)
SELECT
    school_year
    , semester
    , occur_date
    , reason
    , detail
    , ref_student_id
    , merit_flag
    , register_date
FROM
    data_row
                ", string.Join(" UNION ALL", dataRow));
                UpdateHelper up = new UpdateHelper();
                up.Execute(sql);

                MessageBox.Show("獎勵登錄完成");
                ReloadDataGridView();
            }

        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                int i = 0;
                string colValue = "" + dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!int.TryParse(colValue, out i))
                {
                    dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "只允許填入數字!";
                }
                if (int.TryParse(colValue, out i))
                {
                    dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
                }
            }
        }

    }
}
