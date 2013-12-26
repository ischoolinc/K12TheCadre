using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using System.Diagnostics;
using FISCA.LogAgent;
using System.Xml;
using FISCA.DSAUtil;

namespace K12.Behavior.TheCadre
{
    public partial class TheCadreByClassForm : BaseForm
    {

        //幹部社定檔:幹部模組_幹部名稱清單
        //幹部名稱 - 用以儲存幹部名稱
        //幹部是否進行敘獎 - 是否於幹部登錄後進行敘獎
        //幹部自動帶入 - 幹部記錄(資料項目)是否自動填入學年度學期
        //自動填入班級名稱 - 班級幹部管理,是否自動代入班級名稱


        //班級學生ID
        List<StudentRecord> studentIDList = new List<StudentRecord>();

        //學生ID 物件
        Dictionary<string, SchoolObject> Dic = new Dictionary<string, SchoolObject>();

        //UDT功能物件
        private AccessHelper _accessHelper = new AccessHelper();

        //目前幹部清單
        private List<SchoolObject> UDTSchoolList = new List<SchoolObject>();

        string _ClassID;

        bool Refalse = false;

        public TheCadreByClassForm(string ClassID)
        {
            InitializeComponent();

            _ClassID = ClassID;

            integerInput1.Text = School.DefaultSchoolYear;
            integerInput2.Text = School.DefaultSemester;

            ChengeCadreList();

            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            if (!string.IsNullOrEmpty(DateConfig["幹部自動帶入"]))
            {
                checkBoxX2.Checked = bool.Parse(DateConfig["幹部自動帶入"]);
            }

            //由班級ID取得全班學生
            foreach (StudentRecord each in Student.SelectByClassID(_ClassID))
            {
                if (each.Status == K12.Data.StudentRecord.StudentStatus.一般)
                {
                    if (each.RefClassID == ClassID)
                    {
                        studentIDList.Add(each);
                    }
                }
            }

            studentIDList.Sort(StudentSort);

            BingData();

            this.integerInput1.ValueChanged += new System.EventHandler(this.integerInput1_ValueChanged);
            this.integerInput2.ValueChanged += new System.EventHandler(this.integerInput2_ValueChanged);
        }

        private void ChengeCadreList()
        {
            List<ClassCadreNameObj> ClassCadreNameList = _accessHelper.Select<ClassCadreNameObj>(string.Format("NameType = '{0}'", "班級幹部"));
            if (ClassCadreNameList.Count != 0) //是否有字典內容
            {
                var list = from Record in ClassCadreNameList orderby Record.Index select Record;

                foreach (ClassCadreNameObj each in list)
                {
                    Column8.Items.Add(each.CadreName);
                }
            }
        }

        private void BingData()
        {
            this.Text = "班級幹部批次登錄";
            Refalse = false;
            dataGridViewX1.Rows.Clear();
            Dic.Clear();

            foreach (StudentRecord each in studentIDList)
            {
                if (!Dic.ContainsKey(each.ID))
                {
                    Dic.Add(each.ID, null);
                }

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Tag = each.ID;
                row.Cells[0].Value = each.StudentNumber;
                row.Cells[1].Value = each.Class != null ? each.Class.Name : "";
                row.Cells[2].Value = each.SeatNo.HasValue ? each.SeatNo.Value.ToString() : "";
                row.Cells[3].Value = each.Name;
                dataGridViewX1.Rows.Add(row);
            }
            UDTSchoolList.Clear();
            UDTSchoolList = _accessHelper.Select<SchoolObject>(string.Format("SchoolYear='{0}' and Semester='{1}'", integerInput1.Text, integerInput2.Text));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("以下學生在同一學年度學期,有多筆班級幹部記錄\n如為合理狀態,可忽略此提示\n");
            List<string> ErrorList = new List<string>();

            foreach (SchoolObject each in UDTSchoolList)
            {
                //(學號,學年度,學期)相同
                if (Dic.ContainsKey(each.StudentID) && each.SchoolYear == integerInput1.Text && each.Semester == integerInput2.Text && each.ReferenceType == "班級幹部")
                {
                    if (Dic[each.StudentID] == null)
                    {
                        Dic[each.StudentID] = each;
                    }
                    else
                    {
                        StudentRecord sr = Student.SelectByID(each.StudentID);
                        ErrorList.Add("座號：" + (sr.SeatNo.HasValue ? sr.SeatNo.Value.ToString().PadLeft(2,'0') : "") + "　姓名：" + sr.Name);
                    }
                }
            }

            if (ErrorList.Count != 0)
            {
                ErrorList.Sort();
                sb.Append(string.Join("\n", ErrorList.ToArray()));
                MsgBox.Show(sb.ToString());
            }

            foreach (DataGridViewRow each in dataGridViewX1.Rows)
            {
                if (!each.IsNewRow)
                {
                    if (Dic["" + each.Cells[0].Tag] != null) //傳入學號
                    {
                        //幹部物件
                        each.Tag = Dic["" + each.Cells[0].Tag];
                        each.Cells[4].Value = Dic["" + each.Cells[0].Tag].CadreName;
                        each.Cells[5].Value = Dic["" + each.Cells[0].Tag].Text;
                    }
                }
            }
        }

        //學生座號排序
        private int StudentSort(StudentRecord x,StudentRecord y)
        {
            int SeatNoX = x.SeatNo.HasValue ? x.SeatNo.Value : 0;
            int SeatNoY = y.SeatNo.HasValue ? y.SeatNo.Value : 0;
            return SeatNoX.CompareTo(SeatNoY);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewCadreSetup TCN = new NewCadreSetup();
            TCN.ShowDialog();

            ChengeCadreList();
        }

        private void integerInput1_ValueChanged(object sender, EventArgs e)
        {
            labelX4.Visible = true;
            buttonX3.Pulse(10);
        }

        private void integerInput2_ValueChanged(object sender, EventArgs e)
        {
            labelX4.Visible = true;
            buttonX3.Pulse(10);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            labelX4.Visible = false;
            BingData();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (!Refalse)
                return;
            //怎麼儲存???
            //怎麼Log

            #region 將畫面的資料刪除
            List<SchoolObject> DeleteList = new List<SchoolObject>();
            List<SchoolObject> InsertList = new List<SchoolObject>();

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.Tag != null)
                {
                    DeleteList.Add(row.Tag as SchoolObject);
                }
            }

            _accessHelper.DeletedValues(DeleteList.ToArray()); 
            #endregion

            #region 將畫面的資料新增
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("班級：" + Class.SelectByID(_ClassID).Name);
            sb.AppendLine("已進行班級幹部登錄作業");
            sb.AppendLine("詳細資料如下：");
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (!string.IsNullOrEmpty("" + row.Cells[4].Value))
                {
                    StudentRecord sr = Student.SelectByID("" + row.Cells[0].Tag);
                    sb.AppendLine("學生：「" + sr.Name + "」");
                    sb.AppendLine("於" + integerInput1.Text + "學年度/第" + integerInput2.Text + "學期");
                    sb.AppendLine("擔任班級幹部：「" + row.Cells[4].Value + "」" + "說明：「" + row.Cells[5].Value + "」");

                    SchoolObject obj = new SchoolObject();
                    obj.ReferenceType = "班級幹部";
                    obj.SchoolYear = integerInput1.Text;
                    obj.Semester = integerInput2.Text;
                    obj.StudentID = "" + row.Cells[0].Tag; //取得學生ID
                    obj.CadreName = "" + row.Cells[4].Value;
                    obj.Text = "" + row.Cells[5].Value;
                    InsertList.Add(obj);
                }
                else
                {
                    if (!string.IsNullOrEmpty("" + row.Cells[5].Value))
                    {
                        MsgBox.Show("注意：\n學生：" + Student.SelectByID("" + row.Cells[0].Tag).Name + "\n僅輸入說明欄位無法儲存為幹部資料!!\n(已略過此筆資料)");
                    }
                }
            }
            _accessHelper.InsertValues(InsertList.ToArray());

            ApplicationLog.Log("幹部外掛模組", "班級幹部批次登錄", "class", _ClassID, sb.ToString());
            
            
            
            MsgBox.Show("幹部資料儲存成功!!");
            #endregion

            //3.更新畫面資料
            BingData();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (Refalse)
            {
                DialogResult dr = MsgBox.Show("資料已變更,是否離開?\n(按下否,可於原畫面進行儲存)", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
            
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == Column8.Index)
            {
                if (checkBoxX2.Checked)
                {
                    dataGridViewX1.Rows[e.RowIndex].Cells[5].Value = Class.SelectByID(_ClassID).Name;
                }
            }
        }

        private void dataGridViewX1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.Text = "班級幹部批次登錄(＊)";
            Refalse = true;
        }

        private void checkBoxX2_CheckedChanged(object sender, EventArgs e)
        {
            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            DateConfig["幹部自動帶入"] = checkBoxX2.Checked.ToString();
            DateConfig.Save();
        }
    }
}
