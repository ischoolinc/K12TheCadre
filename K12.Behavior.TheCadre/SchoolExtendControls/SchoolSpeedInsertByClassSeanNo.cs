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
using System.Xml;
using FISCA.DSAUtil;
using FISCA.LogAgent;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    public partial class SchoolSpeedInsertByClassSeanNo : BaseForm
    {
        internal AccessHelper _accessHelper = new AccessHelper();
        internal GetAllStudent GetStudent { get; set; }
        internal SetDataGridViewRowState SetDataGrid = new SetDataGridViewRowState();
        internal BackgroundWorker BGW;
        internal List<SchoolObject> _listCadre { get; set; }
        internal Dictionary<string, bool> _dicCadre { get; set; }
        internal List<CadreDataRowSchool> _listRow { get; set; }
        //private int DefSchoolYear { get; set; }
        //private int DefSemester { get; set; }

        public SchoolSpeedInsertByClassSeanNo()
        {
            InitializeComponent();
        }

        private void CadreByStudentSean_Load(object sender, EventArgs e)
        {
            BGW = new BackgroundWorker();
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            //學年期
            intSchoolYear.Value = int.Parse(School.DefaultSchoolYear);
            intSemester.Value = int.Parse(School.DefaultSemester);
            this.Text = "學校幹部登錄(全校資料讀取中...)";

            dataGridViewX1.AutoGenerateColumns = false;

            Reset();
        }

        private void Reset()
        {
            if (!BGW.IsBusy)
            {
                SetObjType = false;

                this.intSchoolYear.ValueChanged -= new System.EventHandler(this.intSchoolYear_ValueChanged);
                this.intSemester.ValueChanged -= new System.EventHandler(this.intSemester_ValueChanged);

                //DefSchoolYear = intSchoolYear.Value;
                //DefSemester = intSemester.Value;

                BGW.RunWorkerAsync();
            }
            else
            {
                MsgBox.Show("系統忙碌中...");
            }
        }

        //取得全校學生
        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            GetStudent = new GetAllStudent();

            GetStudentAndCadre();
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ChangeForm();

            this.intSchoolYear.ValueChanged += new System.EventHandler(this.intSchoolYear_ValueChanged);
            this.intSemester.ValueChanged += new System.EventHandler(this.intSemester_ValueChanged);

            this.Text = "學校幹部登錄";

            SetObjType = true;
        }

        /// <summary>
        /// 取得班級學生,且篩選出學生幹部
        /// </summary>
        private void GetStudentAndCadre()
        {
            //取得本學期所有學校幹部
            _listCadre = _accessHelper.Select<SchoolObject>(string.Format("SchoolYear = '{0}' and Semester = '{1}' and ReferenceType = '{2}'", intSchoolYear.Value, intSemester.Value, "學校幹部"));

            SetObj();

            #region 註解
            //foreach (DataGridViewRow row in dataGridViewX1.Rows)
            //{
            //    //文字記錄
            //    StringBuilder ErrorString = new StringBuilder();

            //    foreach (SchoolObject cadreRecord in CadreList)
            //    {
            //        if (cadreRecord.ReferenceType != "學校幹部")
            //            continue;

            //        if (cadreRecord.CadreName == "" + row.Cells[0].Value)
            //        {
            //            StudentRecord student = Student.SelectByID(cadreRecord.StudentID);
            //            //如果RowTag是空的,表示該Row已經存有其他幹部類型
            //            if (row.Tag == null)
            //            {
            //                if (student != null) //避免取出之幹部資料,學生ID為錯
            //                {
            //                    //學生Record
            //                    row.Tag = student;
            //                    //幹部記錄
            //                    row.Cells[1].Tag = cadreRecord;
            //                    //姓名
            //                    row.Cells[1].Value = student.Name;
            //                    //班級(照理說班級名稱沒有無法登錄)
            //                    row.Cells[2].Value = student.Class != null ? student.Class.Name : "";
            //                    //座號
            //                    row.Cells[3].Value = student.SeatNo.HasValue ? student.SeatNo.Value.ToString() : "";
            //                }
            //            }
            //            else
            //            {
            //                ErrorString.AppendLine("學生「" + student.Name + "」已有職稱「" + cadreRecord.CadreName + "」幹部記錄");
            //            }
            //        }
            //    }
            //    if (ErrorString.ToString() != "")
            //    {
            //        ErrorString.AppendLine("建議確認幹部記錄是否正確!!");
            //        row.ErrorText = ErrorString.ToString();
            //    }
            //} 
            #endregion
        }

        /// <summary>
        /// 依幹部類別清單,建立判斷依據
        /// </summary>
        private void SetObj()
        {
            _dicCadre = new Dictionary<string, bool>();
            foreach (SchoolObject each in _listCadre)
            {
                if (!_dicCadre.ContainsKey(each.UID))
                {
                    _dicCadre.Add(each.UID, false);
                }
            }
        }

        /// <summary>
        /// 排序後回傳資料
        /// </summary>
        private List<ClassCadreNameObj> SortCadreNameList(List<ClassCadreNameObj> ClassCadreNameList)
        {
            //依順序(index內容)排序
            List<ClassCadreNameObj> neo = new List<ClassCadreNameObj>();
            var list = from Record in ClassCadreNameList orderby Record.Index select Record;

            foreach (ClassCadreNameObj each in list)
            {
                neo.Add(each);
            }
            return neo;
        }

        /// <summary>
        /// 拋出尚未填入的幹部資料
        /// </summary>
        private SchoolObject Getobj(string CadreName)
        {
            foreach (SchoolObject each in _listCadre)
            {
                //判斷null,是為了確認學生來自有班座之學生
                Srecord sr = GetStudent.GetSrecord(each.StudentID);
                if (sr != null)
                {
                    //1.名稱相同
                    //2.字典內是false
                    if (CadreName == each.CadreName && !_dicCadre[each.UID])
                    {
                        _dicCadre[each.UID] = true;
                        return each;
                    }
                }
            }
            return null;

        }

        /// <summary>
        /// 依幹部名稱更新畫面
        /// </summary>
        private void ChangeForm()
        {
            _listRow = new List<CadreDataRowSchool>();

            //取得學校幹部類型
            List<ClassCadreNameObj> SchoolCadreNameList = _accessHelper.Select<ClassCadreNameObj>(string.Format("NameType = '{0}'", "學校幹部"));

            SchoolCadreNameList = SortCadreNameList(SchoolCadreNameList);

            if (SchoolCadreNameList.Count != 0) //是否有字典內容
            {
                foreach (ClassCadreNameObj each in SchoolCadreNameList)
                {
                    //依照有幾個幹部名單,而增加DataGridViewRow
                    for (int x = 1; x <= each.Number; x++)
                    {
                        //取得幹部記錄
                        SchoolObject obj = Getobj(each.CadreName);

                        //尚未填入欄位
                        if (obj != null)
                        {
                            //取得學生擔任資料
                            Srecord student = GetStudent.GetSrecord(obj.StudentID);

                            if (student != null) //如果為null表示學生無座號,或是無班級,或是狀態不是一般生
                            {
                                CadreDataRowSchool cdr = new CadreDataRowSchool(student, obj, each.Index, this, intSchoolYear.Value, intSemester.Value);
                                _listRow.Add(cdr);
                            }
                            else
                            {
                                CadreDataRowSchool cdr = new CadreDataRowSchool(each.CadreName, each.Index, this, intSchoolYear.Value, intSemester.Value);
                                _listRow.Add(cdr);
                            }
                        }
                        else
                        {
                            CadreDataRowSchool cdr = new CadreDataRowSchool(each.CadreName, each.Index, this, intSchoolYear.Value, intSemester.Value);
                            _listRow.Add(cdr);
                        }
                    }
                }

                //排序
                _listRow.Sort(SortRow);

                dataGridViewX1.DataSource = new BindingList<CadreDataRowSchool>(_listRow);
            }
            else
            {
                DialogResult dr = MsgBox.Show("您未設定學校幹部清單\n是否要現在設定?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    CadreNameChange(); //開啟設定檔畫面
                }
            }

        }

        private int SortRow(CadreDataRowSchool a, CadreDataRowSchool b)
        {
            string indexA = a._index.ToString().PadLeft(3, '0');
            string indexB = b._index.ToString().PadLeft(3, '0');
            string SeatNoA = "" + a._StudentSeatNo.PadLeft(3, '0');
            string SeatNoB = "" + b._StudentSeatNo.PadLeft(3, '0');
            indexA += SeatNoA;
            indexB += SeatNoB;
            return indexA.CompareTo(indexB);
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CadreDataRowSchool c = dataGridViewX1.Rows[e.RowIndex].DataBoundItem as CadreDataRowSchool;

            dataGridViewX1.Rows[e.RowIndex].Cells[2].ErrorText = c.ClassNameError;
            dataGridViewX1.Rows[e.RowIndex].Cells[3].ErrorText = c.SeatNoError;
        }

        private bool CheckClassColumn(DataGridViewRow row)
        {
            //姓名欄位內容
            DataGridViewCell cellStudentName = row.Cells[colStudentName.Index];
            //班級名稱欄位內容
            DataGridViewCell cellClassName = row.Cells[colClassName.Index];
            //座號欄位內容
            DataGridViewCell cellSeatNo = row.Cells[colSeatNo.Index];

            //班級名稱"不是"空值
            if (!string.IsNullOrEmpty("" + cellClassName.Value))
            {
                //檢查班級名稱是否存在
                if (!GetStudent.GetClassIsNull("" + cellClassName.Value))
                {
                    //當班級不存在,將所有資料重置
                    cellClassName.ErrorText = "班級不存在!!";
                    SetDataGrid.IsErrorRow(row);
                    return true;
                }
                else //存在則清空錯誤訊息(正確狀態)
                {
                    cellClassName.ErrorText = "";
                    return false;
                }
            }
            else //班級名稱"是"空值,整個Row應初始化為空資料
            {
                //傳入Row以初始化資料
                SetDataGrid.IsNewRow(row);
                return true;
            }
        }

        /// <summary>
        /// 儲存
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //鎖定儲存時畫面
            SetObjType = false;

            #region 資料整理
            List<SchoolObject> InsertList = new List<SchoolObject>();
            List<SchoolObject> DeleteList = new List<SchoolObject>();
            List<SchoolObject> DefList = new List<SchoolObject>();

            List<CadreDataRowSchool> insert = new List<CadreDataRowSchool>();
            List<CadreDataRowSchool> delete = new List<CadreDataRowSchool>();
            List<CadreDataRowSchool> Def = new List<CadreDataRowSchool>();

            foreach (CadreDataRowSchool data in _listRow)
            {
                //Record不是null,但是沒有UID就是新增資料
                if (data._CadreRecord != null && data._CadreRecord.UID == "")
                {
                    insert.Add(data);
                }

                //Del內不為null,就是有刪除資料
                if (data._CadreRecordDel != null)
                {
                    delete.Add(data);
                }

                //有資料,有ID,用來登錄獎勵使用
                if (data._CadreRecord != null && data._CadreRecord.UID != "")
                {
                    Def.Add(data);
                }
            }

            foreach (CadreDataRowSchool data in Def)
            {
                DefList.Add(data._CadreRecord);
            } 
            #endregion

            #region Log
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("學年度「" + intSchoolYear.Value + "」學期「" + intSemester.Value + "」");

            if (insert.Count != 0)
            {
                sb.AppendLine("新增學校幹部登錄：");

                foreach (CadreDataRowSchool data in insert)
                {
                    InsertList.Add(data._CadreRecord);

                    sb.AppendLine("學生「" + data._StudentName + "」擔任「學校幹部」幹部名稱「" + data._CadreName + "」已新增");
                }
            }

            if (delete.Count != 0)
            {
                sb.AppendLine("刪除舊有學校幹部記錄：");

                foreach (CadreDataRowSchool data in delete)
                {
                    DeleteList.Add(data._CadreRecordDel);
                    StudentRecord sr = Student.SelectByID(data._CadreRecordDel.StudentID);

                    sb.AppendLine("學生「" + sr.Name + "」擔任「學校幹部」幹部名稱「" + data._CadreName + "」已被刪除");
                }
            }
            #endregion

            #region 資料儲存至資料庫
            List<string> listCadreID = new List<string>();
            try
            {
                listCadreID = this._accessHelper.InsertValues(InsertList.ToArray());
                this._accessHelper.DeletedValues(DeleteList.ToArray());
            }
            catch
            {
                MsgBox.Show("新增資料發生錯誤!!");
                SetObjType = true;
                return;
            }
            if (InsertList.Count + DeleteList.Count > 0)
            {
                ApplicationLog.Log("幹部外掛模組", "學校幹部登錄", sb.ToString());
                MsgBox.Show("幹部記錄,儲存成功!!");
            }
            else
            {
                MsgBox.Show("未修改資料!!");
            } 
            #endregion

            //敘獎模式
            if (checkBoxX1.Checked)
            {
                #region 開啟「敘獎作業」功能畫面

                (new CadreMeritManage.CadreMeritManage(intSchoolYear.Value,intSemester.Value,CadreType.SchoolCadre)).ShowDialog();

                // 舊 幹部敘獎功能畫面
                //List<SchoolObject> list = new List<SchoolObject>();

                //if (CadreIDList.Count != 0)
                //{
                //    String sb123 = string.Join(",", CadreIDList.ToArray());
                //    list = _accessHelper.Select<SchoolObject>(string.Format("UID in (" + sb123 + ")"));
                //}

                //list.AddRange(DefList);

                //if (list.Count != 0)
                //{
                //    //進行敘獎操作
                //    //ps:絮獎模式有兩種
                //    //1.將學生基本資料導入獎勵功能
                //    //2.透過設定值,以幹部為基準進行絮獎(較方便) <--
                //    MutiMeritDemerit mmd = new MutiMeritDemerit("獎勵", list, "學校幹部", intSchoolYear.Value, intSemester.Value);
                //    mmd.ShowDialog();
                //}
                //else
                //{
                //    MsgBox.Show("因無學生擔任幹部\n敘獎畫面將不會開啟!!");
                //} 
                #endregion
            }

            Reset();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CadreNameChange();
        }

        /// <summary>
        /// 開啟幹部名稱管理清單
        /// </summary>
        private void CadreNameChange()
        {
            NewCadreSetup TCN = new NewCadreSetup();
            TCN.ShowDialog();

            Reset();

        }

        /// <summary>
        /// 設定特定畫面為Enabled
        /// </summary>
        private bool SetObjType
        {
            set
            {
                linkLabel1.Enabled = value;
                btnSave.Enabled = value;
                btnExit.Enabled = value;
                checkBoxX1.Enabled = value;
                dataGridViewX1.Enabled = value;
            }
        }

        //排序學校幹部名稱物件
        private int SortIndex(ClassCadreNameObj x, ClassCadreNameObj y)
        {
            return x.Index.CompareTo(y.Index);
        }

        /// <summary>
        /// 離開
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            //K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            //DateConfig["學校幹部_是否進行敘獎"] = checkBoxX1.Checked.ToString();
            //DateConfig.Save();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.FromName("ControlText");
            Reset();
        }

        private void intSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Red;
            btnReset.Pulse(5);
        }

        private void intSemester_ValueChanged(object sender, EventArgs e)
        {
            btnReset.ForeColor = Color.Red;
            btnReset.Pulse(5);
        }
    }
}
