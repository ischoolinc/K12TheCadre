using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.DSAUtil;
using System.Xml;
using Framework.Feature;
using FISCA.LogAgent;
using K12.Data;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    public partial class MutiMeritDemerit : BaseForm
    {
        string _DemeritOrMerit;

        Dictionary<string, string> ReasonDic = new Dictionary<string, string>();

        StringBuilder sb = new StringBuilder();

        List<SchoolObject> _CadreList = new List<SchoolObject>();

        string _CadreType;

        Dictionary<string, bool> CadreDic { get; set; }

        private int _SchoolYear { get; set; }
        private int _Semester { get; set; }

        /// <summary>
        /// 傳入獎勵或懲戒字串,以決定模式
        /// </summary>
        /// <param name="DemeritOrMerit"></param>
        public MutiMeritDemerit(string DemeritOrMerit, List<SchoolObject> CadreList, string CadreType, int SchoolYear, int Semester)
        {
            InitializeComponent();
            _SchoolYear = SchoolYear;
            _Semester = Semester;

            _CadreList = CadreList;
            _DemeritOrMerit = DemeritOrMerit;
            _CadreType = CadreType;
        }

        //Load
        private void MutiMeritDemerit_Load(object sender, EventArgs e)
        {
            //comboBoxEx1.DisplayMember = "Key";
            //comboBoxEx1.ValueMember = "Value";
            integerInput1.Value = _SchoolYear;
            integerInput2.Value = _Semester;
            dateTimeInput1.Text = DateTime.Now.ToShortDateString();
            dateTimeInput2.Text = DateTime.Now.ToShortDateString();

            KeyValuePair<string, string> fkvp = new KeyValuePair<string, string>("", "");
            comboBoxEx1.Items.Add(fkvp);

            if (_DemeritOrMerit == "獎勵")
            {
                #region 獎勵
                DSResponse dsrsp = Config.GetDisciplineReasonList();
                foreach (XmlElement element in dsrsp.GetContent().GetElements("Reason"))
                {
                    if (element.GetAttribute("Type") == "獎勵")
                    {
                        string k = element.GetAttribute("Code") + "-" + element.GetAttribute("Description");
                        string v = element.GetAttribute("Description");
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(k, v);
                        if (!ReasonDic.ContainsKey(element.GetAttribute("Code")))
                        {
                            ReasonDic.Add(element.GetAttribute("Code"), v);
                        }
                        //KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(k, v);
                        comboBoxEx1.Items.Add(kvp);
                    }
                }
                #endregion
            }

            comboBoxEx1.DisplayMember = "Key";
            comboBoxEx1.ValueMember = "Value";
            comboBoxEx1.SelectedIndex = 0;

            #region 取得幹部設定檔
            AccessHelper accessHelper = new AccessHelper();

            //取得幹部設定檔
            List<ClassCadreNameObj> CadreList = accessHelper.Select<ClassCadreNameObj>(string.Format("NameType = '{0}'", _CadreType));
            CadreList.Sort(SortIndex);
            foreach (ClassCadreNameObj each in CadreList)
            {
                for (int x = 1; x <= each.Number; x++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewX1);
                    row.Cells[0].Value = each.CadreName;
                    row.Cells[1].Value = "";
                    row.Cells[2].Value = "";
                    row.Cells[3].Value = "";
                    row.Cells[4].Value = each.MeritA;
                    row.Cells[5].Value = each.MeritB;
                    row.Cells[6].Value = each.MeritC;
                    row.Cells[7].Value = each.Reason;
                    dataGridViewX1.Rows.Add(row);
                }
            }
            #endregion

            SetObj();

            #region 填入學生

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                foreach (SchoolObject cadre in _CadreList)
                {
                    if (!CadreDic[cadre.UID] && "" + row.Cells[0].Value == cadre.CadreName)
                    {
                        StudentRecord student = Student.SelectByID(cadre.StudentID);
                        row.Tag = student;
                        row.Cells[1].Value = student.Class != null ? student.Class.Name : "";
                        row.Cells[2].Value = student.SeatNo.HasValue ? student.SeatNo.Value.ToString() : "";
                        row.Cells[3].Value = student.Name;

                        CadreDic[cadre.UID] = true;
                        break;

                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 依幹部類別清單,建立判斷依據
        /// </summary>
        private void SetObj()
        {
            CadreDic = new Dictionary<string, bool>();
            foreach (SchoolObject each in _CadreList)
            {
                if (!CadreDic.ContainsKey(each.UID))
                {
                    CadreDic.Add(each.UID, false);
                }
            }
        }

        private int SortIndex(ClassCadreNameObj x, ClassCadreNameObj y)
        {
            return x.Index.CompareTo(y.Index);
        }

        //儲存
        //未完成(檢查儲存資料是否A+B+C=0)
        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (CheckNotInt())
            {
                MsgBox.Show("您的資料並非數字!!");
                return;
            }

            if (CheckEmpty())
            {
                MsgBox.Show("目前大功/小功/嘉獎相加為零\n需要輸入大於1之數字!!");
                return;
            }

            //檢查事由
            CheckReasonEmpty();


            if (_DemeritOrMerit == "獎勵")
            {
                _SchoolYear = integerInput1.Value;
                _Semester = integerInput2.Value;

                List<K12.Data.MeritRecord> MeritList = GetMeritList();
                try
                {
                    K12.Data.Merit.Insert(MeritList);
                }
                catch (Exception ex)
                {
                    MsgBox.Show("新增獎勵失敗\n" + ex.Message);
                    return;
                }
                ApplicationLog.Log("幹部外掛模組", "幹部記錄敘獎作業", sb.ToString());
                MsgBox.Show("新增幹部敘獎成功!");
                this.Close();
            }
        }

        //取得獎勵資料
        private List<K12.Data.MeritRecord> GetMeritList()
        {
            sb.AppendLine("幹部記錄敘獎作業");
            sb.AppendLine("學年度「" + _SchoolYear + "」");
            sb.AppendLine("學期「" + _Semester + "」");
            sb.AppendLine("獎勵日期：" + dateTimeInput1.Value.ToShortDateString());
            sb.AppendLine("詳細資料：");

            List<K12.Data.MeritRecord> MeritList = new List<K12.Data.MeritRecord>();
            //每一位學生的獎懲資料
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                K12.Data.StudentRecord student = (K12.Data.StudentRecord)row.Tag;

                if (student == null) //未有學生之資料,則不儲存
                    continue;

                K12.Data.MeritRecord mr = new K12.Data.MeritRecord();

                mr.RefStudentID = student.ID; //學生ID
                mr.SchoolYear = _SchoolYear; //學年度
                mr.Semester = _Semester; //學期

                mr.MeritA = int.Parse(string.IsNullOrEmpty("" + row.Cells[4].Value) ? "0" : "" + row.Cells[4].Value); //大功
                mr.MeritB = int.Parse(string.IsNullOrEmpty("" + row.Cells[5].Value) ? "0" : "" + row.Cells[5].Value);  //小功
                mr.MeritC = int.Parse(string.IsNullOrEmpty("" + row.Cells[6].Value) ? "0" : "" + row.Cells[6].Value);  //嘉獎
                if (mr.MeritA + mr.MeritB + mr.MeritC <= 0)
                {
                    continue;
                }

                mr.Reason = "" + row.Cells[7].Value;

                mr.OccurDate = dateTimeInput1.Value; //獎勵日期
                mr.RegisterDate = dateTimeInput2.Value; //登錄日期

                MeritList.Add(mr);

                sb.AppendLine("學生「" + student.Name + "」"
                    + "大功「" + row.Cells[4].Value + "」"
                    + "小功「" + row.Cells[5].Value + "」"
                    + "嘉獎「" + row.Cells[6].Value + "」"
                    + "事由「" + row.Cells[7].Value + "」");
            }

            return MeritList;
        }

        /// <summary>
        /// 檢查是否為數字
        /// </summary>
        /// <returns></returns>
        private bool CheckNotInt()
        {
            bool returnTrue = false;

            ////大功/小功/嘉獎 - 不是數字為錯誤
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7)
                    {

                        if (!intParse("" + cell.Value))
                        {
                            returnTrue = true;
                        }
                    }
                }
            }
            return returnTrue;
        }

        /// <summary>
        /// 檢查是否相加為零
        /// </summary>
        /// <returns></returns>
        private bool CheckEmpty()
        {
            bool returnTrue = false;

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                int a = 0;
                int.TryParse("" + row.Cells[4].Value, out a);
                int b = 0;
                int.TryParse("" + row.Cells[5].Value, out b);
                int c = 0;
                int.TryParse("" + row.Cells[6].Value, out c);

                if (a + b + c <= 0)
                {
                    returnTrue = true;
                }
            }
            return returnTrue;
        }

        /// <summary>
        /// 檢查事由是否輸入內容
        /// </summary>
        /// <returns></returns>
        private bool CheckReasonEmpty()
        {
            bool returnTrue = false;

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                string Cell6 = "" + row.Cells[7].Value;
                if (string.IsNullOrEmpty(Cell6.Trim()))
                {
                    row.Cells[7].Style.BackColor = Color.Yellow;
                    returnTrue = true;
                }
                else
                {
                    row.Cells[7].Style.BackColor = Color.White;
                    returnTrue = false;
                }
            }
            return returnTrue;
        }

        //離開
        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Changed事件

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)comboBoxEx1.SelectedItem;
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                row.Cells[7].Value = kvp.Value;
            }
        }

        private void comboBoxEx1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBoxEx1.Focus();
                string comText = comboBoxEx1.Text;
                comText = comText.Remove(0, comText.IndexOf('-') + 1);

                string reasonValue = GetReason(comText);

                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    row.Cells[7].Value = reasonValue;
                }
            }
        }

        private void comboBoxEx1_TextChanged(object sender, EventArgs e)
        {
            string comText = comboBoxEx1.Text;
            comText = comText.Remove(0, comText.IndexOf('-') + 1);

            string reasonValue = GetReason(comText);

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                row.Cells[7].Value = reasonValue;
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
                if (ReasonDic.ContainsKey(each1))
                {
                    list.Add(ReasonDic[each1]);
                }
                else
                {
                    list.Add(each1);
                }
            }

            reasonValue = string.Join(",", list);
            return reasonValue;
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX1.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    row.Cells[4].Value = textBoxX1.Text;
                }
                errorProvider1.Clear();
            }
            else
            {
                errorProvider1.SetError(textBoxX1, "輸入內容非數字!!");
            }
        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX2.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    row.Cells[5].Value = textBoxX2.Text;
                }
                errorProvider2.Clear();
            }
            else
            {
                errorProvider2.SetError(textBoxX2, "輸入內容非數字!!");
            }

        }

        private void textBoxX3_TextChanged(object sender, EventArgs e)
        {
            if (intParse(textBoxX3.Text))
            {
                foreach (DataGridViewRow row in dataGridViewX1.Rows)
                {
                    row.Cells[6].Value = textBoxX3.Text;
                }
                errorProvider3.Clear();
            }
            else
            {
                errorProvider3.SetError(textBoxX3, "輸入內容非數字!!");
            }
        }

        #endregion

        private void dataGridViewX1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //不是標題列
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                //缺曠數量
                if (e.ColumnIndex > 3 && e.ColumnIndex < 7)
                {
                    DataGridViewCell cell = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (intParse("" + cell.Value))
                    {
                        cell.ErrorText = "";
                    }
                    else
                    {
                        cell.ErrorText = "內容非數字!!";
                    }
                }

                //事由替換
                if (e.ColumnIndex == 7)
                {
                    DataGridViewCell cell = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.Value = GetReason("" + cell.Value);
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
    }
}
