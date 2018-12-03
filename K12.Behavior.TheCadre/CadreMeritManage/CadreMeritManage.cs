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
        private Dictionary<string, string> _dicReasonByKey = new Dictionary<string, string>();
        private int _schoolYear = int.Parse(School.DefaultSchoolYear);
        private int _semester = int.Parse(School.DefaultSemester);
        private bool _initFinish = false;
        private AccessHelper _access = new AccessHelper();
        private QueryHelper _qh = new QueryHelper();
        private UpdateHelper _up = new UpdateHelper();
        private CadreType _cadreType;

        public CadreMeritManage()
        {
            InitializeComponent();
        }

        public CadreMeritManage(int schoolYear,int semester,CadreType type)
        {
            InitializeComponent();
            this._cadreType = type;
            this._schoolYear = schoolYear;
            this._semester = semester;
        }

        private void CadreMeritManage_Load(object sender, EventArgs e)
        {
            #region Init _dicCadreNameOb
            {
                this.ReloadDicCadreNameOb();
            }
            #endregion

            #region Init schoolYear
            {
                cbxSchoolYear.Items.Add(_schoolYear + 1);
                cbxSchoolYear.Items.Add(_schoolYear);
                cbxSchoolYear.Items.Add(_schoolYear - 1);
                cbxSchoolYear.SelectedIndex = 1;
            }
            #endregion

            #region Init semester
            {
                cbxSemester.Items.Add(1);
                cbxSemester.Items.Add(2);
                cbxSemester.SelectedIndex = _semester - 1;
            }
            #endregion

            #region Init cadreType
            {
                cbxCadreType.Items.Add("--全部--");
                cbxCadreType.Items.Add("班級幹部");
                cbxCadreType.Items.Add("社團幹部");
                cbxCadreType.Items.Add("學校幹部");

                switch (this._cadreType)
                {
                    case CadreType.ClassCadre:
                        cbxCadreType.SelectedIndex = 1;
                        break;
                    case CadreType.ClubCadre:
                        cbxCadreType.SelectedIndex = 2;
                        break;
                    case CadreType.SchoolCadre:
                        cbxCadreType.SelectedIndex = 3;
                        break;
                    case CadreType.SelectAll:
                        cbxCadreType.SelectedIndex = 0;
                        break;
                    default:
                        cbxCadreType.SelectedIndex = 0;
                        break;
                }
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
                cbxCadreName.Items.Add("--全部--");
                foreach (DataRow row in dt.Rows)
                {
                    cbxCadreName.Items.Add("" + row["cadrename"]);
                }
                cbxCadreName.SelectedIndex = 0;
            }
            #endregion

            #region Init cbxMerit
            {
                cbxMerit.Items.Add("--全部--");
                cbxMerit.Items.Add("已敘獎");
                cbxMerit.Items.Add("未敘獎");
                cbxMerit.SelectedIndex = 2;
            }
            #endregion

            #region Init Reason
            {
                KeyValuePair<string, string> fkvp = new KeyValuePair<string, string>("", "");
                this.cbxReason.Items.Add(fkvp);

                DSResponse dsrsp = Config.GetDisciplineReasonList();
                foreach (XmlElement element in dsrsp.GetContent().GetElements("Reason"))
                {
                    if (element.GetAttribute("Type") == "獎勵")
                    {
                        string k = element.GetAttribute("Code") + "-" + element.GetAttribute("Description");
                        string v = element.GetAttribute("Description");
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(k, v);
                        if (!_dicReasonByKey.ContainsKey(element.GetAttribute("Code")))
                        {
                            _dicReasonByKey.Add(element.GetAttribute("Code"), v);
                        }
                        
                        this.cbxReason.Items.Add(kvp);
                    }
                }
                this.cbxReason.DisplayMember = "Key";
                this.cbxReason.ValueMember = "Value";
                this.cbxReason.SelectedIndex = 0;
            }
            #endregion

            dateTimeInput1.Value = DateTime.Now;

            ReloadDataGridView();

            _initFinish = true;
        }

        private void ReloadDicCadreNameOb()
        {
            this._dicCadreNameObByKey.Clear();
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

        private void ReloadDataGridView()
        {
            dataGridViewX1.Rows.Clear();
            List<string> listStudentID = new List<string>();

            #region Init DataGridView

            #region SQL
            string condition = string.Format(@"
                cadre.schoolyear = '{0}'
                AND cadre.semester = '{1}'
                AND student.status IN(1,2)
                AND student.id IS NOT NULL
            ", cbxSchoolYear.Text, cbxSemester.Text);
            if (cbxCadreType.Text != "--全部--")
            {
                condition += string.Format("AND cadre.referencetype = '{0}'", cbxCadreType.Text);
            }
            if (cbxCadreName.Text != "--全部--")
            {
                condition += string.Format("AND cadre.cadrename = '{0}'", cbxCadreName.Text);
            }
            string sql = string.Format(@"
SELECT 
	class.class_name
    , student.id
	, student.seat_no
	, student.name
	, cadre.referencetype
	, cadre.cadrename
	--, cadretype.merita
	--, cadretype.meritb
	--, cadretype.meritc
	--, cadretype.reason
FROM 
	$behavior.thecadre  AS cadre
	LEFT OUTER JOIN student
		ON student.id = cadre.studentid::BIGINT
	LEFT OUTER JOIN class
		ON class.id = student.ref_class_id
	LEFT OUTER JOIN $behavior.thecadre.cadretype AS cadretype
		ON cadretype.nametype = cadre.referencetype
			AND cadretype.cadrename = cadre.cadrename
WHERE
    {0}
                ", condition); 
            #endregion

            DataTable dt = this._qh.Select(sql);

            if (dt.Rows.Count > 0)
            {
                #region 填資料
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
                    //dgvrow.Cells[index++].Value = "" + row["merita"];
                    //dgvrow.Cells[index++].Value = "" + row["meritb"];
                    //dgvrow.Cells[index++].Value = "" + row["meritc"];
                    //dgvrow.Cells[8].Value = "" + row["reason"];
                    dgvrow.Cells[8].Value = string.Format("[{0}][{1}]", row["referencetype"], row["cadrename"]);

                    dgvrow.Tag = row; // row

                    if (!listStudentID.Contains("" + row["id"]))
                    {
                        listStudentID.Add("" + row["id"]);
                    }
                    listStudentID.Add("" + row["id"]);

                    dataGridViewX1.Rows.Add(dgvrow);
                } 
                #endregion
            }

            #endregion

            #region 已有敘獎紀錄判斷
            // 取得學生獎勵紀錄
            List<DisciplineRecord> listMerit = Discipline.SelectByStudentIDs(listStudentID);

            #region 學生獎勵紀錄整理: Key 1.學生編號 2.獎勵是由 
            Dictionary<string, Dictionary<string, DisciplineRecord>> dicMeritRecordByStudentIDByReason = new Dictionary<string, Dictionary<string, DisciplineRecord>>();

            foreach (DisciplineRecord merit in listMerit)
            {
                if ("" + merit.SchoolYear == cbxSchoolYear.Text && "" + merit.Semester == cbxSemester.Text && merit.MeritFlag == "1")
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
            #endregion

            #region 填敘獎資料
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                bool hadMeritRecord = false;
                string cadreType = "" + dgvrow.Cells[3].Value;
                string cadreName = "" + dgvrow.Cells[4].Value;
                string studentID = "" + ((DataRow)dgvrow.Tag)["id"];

                if (dicMeritRecordByStudentIDByReason.ContainsKey(studentID))
                {
                    string reasonKey = string.Format("[{0}][{1}]", cadreType, cadreName);
                    List<string> listReason = dicMeritRecordByStudentIDByReason[studentID].Keys.ToList();

                    foreach (string reason in listReason)
                    {
                        #region 有獎勵紀錄
                        if (reason.Contains(reasonKey))
                        {
                            DisciplineRecord merit = dicMeritRecordByStudentIDByReason[studentID][reason];

                            dgvrow.Cells[5].Value = merit.MeritA;
                            dgvrow.Cells[6].Value = merit.MeritB;
                            dgvrow.Cells[7].Value = merit.MeritC;
                            dgvrow.Cells[8].Value = merit.Reason;
                            dgvrow.ReadOnly = true;
                            dgvrow.DefaultCellStyle.BackColor = Color.LightGray;

                            #region 根據敘獎條件設定 visible
                            if (cbxMerit.SelectedItem.ToString() == "--全部--")
                            {
                                dgvrow.Visible = true;
                            }
                            else
                            {
                                dgvrow.Visible = cbxMerit.SelectedItem.ToString() == "已敘獎" ? true : false;
                            } 
                            #endregion

                            hadMeritRecord = true;
                        } 
                        #endregion
                    }
                }
                #region 沒有獎勵紀錄
                if (!hadMeritRecord)
                {
                    string key = string.Format("{0}_{1}", cadreType, cadreName);

                    if (this._dicCadreNameObByKey.ContainsKey(key)) // 幹部紀錄有對應到幹部敘獎物件 : 初始敘獎設定
                    {
                        ClassCadreNameObj obj = this._dicCadreNameObByKey[key];
                        dgvrow.Cells[5].Value = obj.MeritA;
                        dgvrow.Cells[6].Value = obj.MeritB;
                        dgvrow.Cells[7].Value = obj.MeritC;
                        // 如果是由當中有關鍵字"[幹部]"的話，移到是由最前面
                        if (obj.Reason.IndexOf("[幹部]") > -1)
                        {
                            string reason = obj.Reason.Remove(obj.Reason.IndexOf("[幹部]"), 4);
                            dgvrow.Cells[8].Value = string.Format("[幹部][{0}][{1}]{2}", cadreType, cadreName, reason);
                        }
                        else
                        {
                            dgvrow.Cells[8].Value = string.Format("[{0}][{1}]{2}", cadreType, cadreName, obj.Reason);
                        }

                        //dgvrow.Cells[8].ReadOnly = true;
                        dgvrow.DefaultCellStyle.BackColor = Color.LightGreen;

                        #region 根據敘獎條件設定 visible
                        if (cbxMerit.SelectedItem.ToString() == "--全部--")
                        {
                            dgvrow.Visible = true;
                        }
                        else
                        {
                            dgvrow.Visible = cbxMerit.SelectedItem.ToString() == "未敘獎" ? true : false;
                        } 
                        #endregion

                    }
                    else // 幹部紀錄沒有對應到幹部敘獎物件 : 預設獎勵支數 0
                    {
                        dgvrow.Cells[5].Value = 0;
                        dgvrow.Cells[6].Value = 0;
                        dgvrow.Cells[7].Value = 0;

                        #region 根據敘獎條件設定 visible
                        if (cbxMerit.SelectedItem.ToString() == "--全部--")
                        {
                            dgvrow.Visible = true;
                        }
                        else
                        {
                            dgvrow.Visible = cbxMerit.SelectedItem.ToString() == "未敘獎" ? true : false;
                        } 
                        #endregion

                    }
                } 
                #endregion

            } 
            #endregion

            #endregion
        }

        private void ReloadCadreNameCbx()
        {
            cbxCadreName.Items.Clear();
            string condition = string.Format(@"
                schoolyear = '{0}'
                AND semester = '{1}'
                ", cbxSchoolYear.Text, cbxSemester.Text);
            if (cbxCadreName.Text != "--全部--")
            {
                condition += string.Format(" AND referencetype = '{0}'", cbxCadreType.Text);
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

            cbxCadreName.Items.Add("--全部--");
            foreach (DataRow row in dt.Rows)
            {
                cbxCadreName.Items.Add("" + row["cadrename"]);
            }
            cbxCadreName.SelectedIndex = 0;
        }

        private void schoolYearCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void semesterCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void cadreTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
                ReloadCadreNameCbx();
            }
        }

        private void cadreNameCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
            {
                ReloadDataGridView();
            }
        }

        private void cbxMerit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initFinish)
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
                        row.Cells[5].Value = textBoxX1.Text == "" ? "0" : textBoxX1.Text;
                        row.Cells[5].ErrorText = null;
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
                        row.Cells[6].Value = textBoxX2.Text == "" ? "0" : textBoxX2.Text;
                        row.Cells[6].ErrorText = null;
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
                        row.Cells[7].Value = textBoxX3.Text == "" ? "0" : textBoxX3.Text;
                        row.Cells[7].ErrorText = null;
                    }
                }
                errorProvider3.Clear();
            }
            else
            {
                errorProvider3.SetError(textBoxX3, "輸入內容非數字!!");
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

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                string colValue = "" + dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                int n;
                bool isNumber = int.TryParse(colValue, out n);

                if (isNumber)
                {
                    dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = null;
                }
                else
                {
                    dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "只允許填入數字!";
                }
            }
        }

        private void cbxReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                int col = 8;
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)this.cbxReason.SelectedItem;
                foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
                {
                    if (!dgvrow.ReadOnly)
                    {
                        //row.Cells[8].Value = kvp.Value;
                        DataRow row = (DataRow)dgvrow.Tag;
                        dgvrow.Cells[col].Value = string.Format("[{0}][{1}]{2}", row["referencetype"], row["cadrename"], kvp.Value);
                    }
                }
            }
        }

        private void cbxReason_TextChanged(object sender, EventArgs e)
        {
            if (this._initFinish)
            {
                int col = 8;
                string comText = this.cbxReason.Text;
                comText = comText.Remove(0, comText.IndexOf('-') + 1);

                string reasonValue = GetReason(comText);

                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        string reason = "" + row.Cells[col].Value;
                        int index = reason.LastIndexOf("]") + 1;
                        int length = reason.Length;
                        int removeCcount = length - index;

                        row.Cells[col].Value = reason.Remove(index, removeCcount);
                        row.Cells[col].Value += reasonValue;
                    }
                }
            }
        }

        private string GetReason(string comText)
        {
            string reasonValue = "";
            List<string> list = new List<string>();
            string[] reasonList = comText.Split(',');
            foreach (string each in reasonList)
            {
                string each1 = each.Replace("\r\n", "");
                if (this._dicReasonByKey.ContainsKey(each1))
                {
                    list.Add(this._dicReasonByKey[each1]);
                }
                else
                {
                    list.Add(each1);
                }
            }

            reasonValue = string.Join(",", list);
            return reasonValue;
        }

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._initFinish)
            {
                // 針對事由欄位特別處理 col index = 8
                int col = 8;
                if (e.RowIndex > -1 && e.ColumnIndex == col)
                {
                    string changeValue = "" + dataGridViewX1.Rows[e.RowIndex].Cells[col].Value;

                    string cadreType = "" + dataGridViewX1.Rows[e.RowIndex].Cells[3].Value;
                    string cadreName = "" + dataGridViewX1.Rows[e.RowIndex].Cells[4].Value;
                    string key = string.Format("{0}_{1}", cadreType, cadreName);
                    string defaultValue = "";

                    #region default value
                    if (this._dicCadreNameObByKey.ContainsKey(key))
                    {
                        ClassCadreNameObj obj = this._dicCadreNameObByKey[key];
                        if (obj.Reason.IndexOf("[幹部]") > -1)
                        {
                            defaultValue = string.Format("[幹部][{0}][{1}]", cadreType, cadreName);
                        }
                        else
                        {
                            defaultValue = string.Format("[{0}][{1}]", cadreType, cadreName);
                        }
                    }
                    else
                    {
                        defaultValue = string.Format("[{0}][{1}]", cadreType, cadreName);
                    }
                    #endregion

                    if (changeValue.IndexOf(defaultValue) > -1)
                    {
                        return;
                    }
                    else
                    {
                        dataGridViewX1.Rows[e.RowIndex].Cells[col].Value = defaultValue;
                    }

                    //if (changeValue.Length < defaultValue.Length)
                    //{
                    //    dataGridViewX1.Rows[e.RowIndex].Cells[col].Value = defaultValue;
                    //}
                    //else if (changeValue.Length == defaultValue.Length)
                    //{
                    //    return;
                    //}
                    //else
                    //{
                    //    char lastChar = (changeValue.ToCharArray()).Last();
                    //    dataGridViewX1.Rows[e.RowIndex].Cells[col].Value = defaultValue + lastChar;
                    //}
                }
            }
        }

        private void linkDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MsgBox.Show(@"
1.灰色資料行代表幹部紀錄已登錄敘獎。

2.綠色資料行代表幹部紀錄尚未登錄敘獎並且受幹部名稱管理，
　可透過下方「管理幹部名稱」連結進行修改。

3.白色資料行代表幹部紀錄尚未登錄敘獎並未受幹部名稱管理。

4.事由欄位【幹部類別】【幹部名稱】為識別幹部紀錄與獎勵
　紀錄關聯的條件。 ", "說明",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void linkManageCadreName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewCadreSetup form = new NewCadreSetup();
            form.FormClosed += delegate
            {
                if (form.DataChange)
                {
                    //this.Refresh();
                    this.ReloadDicCadreNameOb();
                    this.ReloadDataGridView();
                }
            };
            form.ShowDialog();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string schoolYear = cbxSchoolYear.Text;
            string semester = cbxSemester.Text;
            string occurDate = dateTimeInput1.Value.ToString("yyyy/MM/dd"); //  發生日期
            string registerDate = DateTime.Now.ToString("yyyy/MM/dd"); // 獎勵登錄日期
            string merit_flag = "1";
            List<string> dataRow = new List<string>();

            #region 資料驗證
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                for (int i = 5; i < 8; i++)
                {
                    if (!string.IsNullOrEmpty(dgvrow.Cells[i].ErrorText))
                    {
                        MsgBox.Show(string.Format("資料驗證錯誤:{0}",dgvrow.Cells[i].ErrorText));
                        return;
                    }
                }
            }

            #endregion

            #region 資料整理
            foreach (DataGridViewRow dgvrow in dataGridViewX1.Rows)
            {
                if (dgvrow.DefaultCellStyle.BackColor != Color.LightGray)
                {
                    int a = "" + dgvrow.Cells[5].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[5].Value);
                    int b = "" + dgvrow.Cells[6].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[6].Value);
                    int c = "" + dgvrow.Cells[7].Value == "" ? 0 : int.Parse("" + dgvrow.Cells[7].Value);

                    if ((a + b + c) > 0)
                    {
                        string studentID = "" + ((DataRow)dgvrow.Tag)["id"];
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
            #endregion


            if (dataRow.Count > 0)
            {
                #region SQL
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
                #endregion

                try
                {
                    this._up.Execute(sql);
                    MessageBox.Show("獎勵登錄完成");
                    ReloadDataGridView();
                }
                catch(Exception ex)
                {
                    MsgBox.Show(string.Format("獎勵登入失敗:{0}",ex.Message));
                }
                
            }
            else
            {
                MsgBox.Show("沒有可登錄資料!");
            }

        }

        private void leaveBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
