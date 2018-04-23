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

namespace K12.Behavior.TheCadre.CadreMeritManage
{
    public partial class CadreMeritManage : BaseForm
    {

        Dictionary<string, string> ReasonDic = new Dictionary<string, string>();
        private int _schoolYear = int.Parse(School.DefaultSchoolYear);
        private int _semester = int.Parse(School.DefaultSemester);
        private bool initFinish = false;

        public CadreMeritManage()
        {
            InitializeComponent();

            schoolYearCbx.Items.Add(_schoolYear + 1);
            schoolYearCbx.Items.Add(_schoolYear);
            schoolYearCbx.Items.Add(_schoolYear - 1);
            schoolYearCbx.SelectedIndex = 1;

            semesterCbx.Items.Add(1);
            semesterCbx.Items.Add(2);
            semesterCbx.SelectedIndex = _semester - 1;

            cadreTypeCbx.Items.Add("--全部--");
            cadreTypeCbx.Items.Add("班級幹部");
            cadreTypeCbx.Items.Add("社團幹部");
            cadreTypeCbx.Items.Add("學校幹部");
            cadreTypeCbx.SelectedIndex = 0;

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

            foreach (DisciplineRecord merit in meritList)
            {
                if ("" + merit.SchoolYear == schoolYearCbx.Text && "" + merit.Semester == semesterCbx.Text && merit.MeritFlag == "1")
                {
                    foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                    {
                        bool hadMeritRecord = false;
                        if (merit.RefStudentID == "" + dgvrow.Tag)
                        {
                            string reason = merit.Reason;
                            hadMeritRecord = reason.Contains(string.Format("[幹部][{0}][{1}]", dgvrow.Cells[3].Value, dgvrow.Cells[4].Value));
                        }
                        if (hadMeritRecord) // 已有敘獎記錄
                        {
                            dgvrow.Cells[5].Value = merit.MeritA;
                            dgvrow.Cells[6].Value = merit.MeritB;
                            dgvrow.Cells[7].Value = merit.MeritC;
                            dgvrow.Cells[8].Value = merit.Reason;
                            dgvrow.ReadOnly = true;
                            dgvrow.DefaultCellStyle.BackColor = Color.LightGray;
                        }
                    }
                }
            }

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

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string schoolYear = schoolYearCbx.Text;
            string semester = semesterCbx.Text;
            string occurDate = DateTime.Now.ToString("yyyy/MM/dd");
            string registerDate = dateTimeInput1.Value.ToString("yyyy/MM/dd");
            string merit_flag = "1";
            List<string> dataRow = new List<string>();

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

    }
}
