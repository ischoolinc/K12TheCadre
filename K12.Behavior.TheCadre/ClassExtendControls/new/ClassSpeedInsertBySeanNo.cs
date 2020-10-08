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
using FISCA.Data;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    public partial class ClassSpeedInsertBySeanNo : BaseForm
    {
        //問題:        
        //當學生被記多筆幹部記錄
        //因為_SchoolObjDic設計為只存一個幹部記錄
        //因此會造成修改時,只能刪除其中一筆

        //row.Tag = 學生
        //row.cell[0].Tag = 學生班級幹部記錄
        internal AccessHelper _accessHelper = new AccessHelper();

        /// <summary>
        /// 座號對照字典
        /// </summary>
        internal Dictionary<string, StudentRecord> _SeatNoDic = new Dictionary<string, StudentRecord>();

        /// <summary>
        /// 幹部UDT記錄對照字典
        /// </summary>
        internal Dictionary<string, SchoolObject> _SchoolObjDic = new Dictionary<string, SchoolObject>();

        internal List<SchoolObject> CadreList { get; set; }

        internal Dictionary<string, bool> CadreDic { get; set; }

        internal List<CadreDataRow> _RowList { get; set; }

        internal BackgroundWorker BGW;

        /// <summary>
        /// 班級Record
        /// </summary>
        internal ClassRecord _classRecord;

        //private int DefSchoolYear { get; set; }
        //private int DefSemester { get; set; }

        public ClassSpeedInsertBySeanNo()
        {
            InitializeComponent();
        }

        private void CadreByStudentSean_Load(object sender, EventArgs e)
        {
            // 設定全形半形
            List<string> cols = new List<string>() { "座號" };
            Campus.Windows.DataGridViewImeDecorator dec = new Campus.Windows.DataGridViewImeDecorator(this.dataGridViewX1, cols);

            BGW = new BackgroundWorker();
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            //學年期
            //lbSchoolYear.Text = "學年度「" + School.DefaultSchoolYear + "」學期「" + School.DefaultSemester + "」班級「" + _classRecord.Name + "」";
            intSchoolYear.Value = int.Parse(School.DefaultSchoolYear);
            intSemester.Value = int.Parse(School.DefaultSemester);

            dataGridViewX1.AutoGenerateColumns = false;

            Reset();
        }

        private void Reset()
        {
            if (!BGW.IsBusy)
            {
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

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            //依選取ID,填入幹部名稱
            GetStudentAndCadre();
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //取得班級幹部設定,建置畫面
            ChangeForm();

            this.intSchoolYear.ValueChanged += new System.EventHandler(this.intSchoolYear_ValueChanged);
            this.intSemester.ValueChanged += new System.EventHandler(this.intSemester_ValueChanged);
        }

        /// <summary>
        /// 取得班級學生,且篩選出學生幹部
        /// </summary>
        private void GetStudentAndCadre()
        {
            QueryHelper _queryhelper = new QueryHelper();
            _classRecord = Class.SelectByID(K12.Presentation.NLDPanels.Class.SelectedSource[0]);

            //取得學生,狀態為一般,相同班級,有座號

            #region 取得班級學生
            _SeatNoDic.Clear();

            string sqlStr2 = string.Format("select student.id from student where status='1' and ref_class_id='{0}' and seat_no is not null", K12.Presentation.NLDPanels.Class.SelectedSource[0]);
            DataTable dt = _queryhelper.Select(sqlStr2);
            List<string> studentList = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                studentList.Add("" + row["id"]);
            }

            List<StudentRecord> ClassStudentList = Student.SelectByIDs(studentList);

            List<string> StudentIdList = new List<string>();
            foreach (StudentRecord student in ClassStudentList)
            {
                //本功能會排除無座號之學生
                string SeatNo = student.SeatNo.HasValue ? student.SeatNo.Value.ToString() : "";

                if (!_SeatNoDic.ContainsKey(SeatNo))
                {
                    _SeatNoDic.Add(SeatNo, student);
                }
                //學生ID
                if (!StudentIdList.Contains(student.ID))
                {
                    StudentIdList.Add(student.ID);
                }
            }
            #endregion

            #region 取得本學期所有班級幹部,篩選出本班之幹部清單(CadreList)
            CadreList = new List<SchoolObject>();
            List<SchoolObject> CadreList1 = _accessHelper.Select<SchoolObject>(string.Format("SchoolYear = '{0}' and Semester = '{1}' and ReferenceType = '{2}'", intSchoolYear.Value, intSemester.Value, "班級幹部"));
            foreach (SchoolObject each in CadreList1)
            {
                if (!StudentIdList.Contains(each.StudentID))
                    continue;

                CadreList.Add(each);
            }
            #endregion

            SetObj();
        }

        /// <summary>
        /// 依幹部類別清單,建立判斷依據
        /// </summary>
        private void SetObj()
        {
            CadreDic = new Dictionary<string, bool>();
            foreach (SchoolObject each in CadreList)
            {
                if (!CadreDic.ContainsKey(each.UID))
                {
                    CadreDic.Add(each.UID, false);
                }
            }
        }

        /// <summary>
        /// 依幹部名稱更新畫面
        /// </summary>
        private void ChangeForm()
        {
            _RowList = new List<CadreDataRow>();

            //取得班級幹部類型
            List<ClassCadreNameObj> ClassCadreNameList = _accessHelper.Select<ClassCadreNameObj>(string.Format("NameType = '{0}'", "班級幹部"));

            ClassCadreNameList = SortCadreNameList(ClassCadreNameList);

            if (ClassCadreNameList.Count != 0) //是否有字典內容
            {
                foreach (ClassCadreNameObj each in ClassCadreNameList)
                {
                    //依照有幾個幹部名單,而增加DataGridViewRow
                    for (int x = 1; x <= each.Number; x++)
                    {
                        SchoolObject obj = Getobj(each.CadreName);

                        //如果有幹部資料,則填入
                        //無幹部資料,填入空Row
                        if (obj != null)
                        {
                            StudentRecord student = Student.SelectByID(obj.StudentID);

                            CadreDataRow cdr = new CadreDataRow(student, obj, each.Index, this, intSchoolYear.Value, intSemester.Value);
                            _RowList.Add(cdr);
                        }
                        else
                        {
                            CadreDataRow cdr = new CadreDataRow(each.CadreName, each.Index, this, intSchoolYear.Value, intSemester.Value);
                            _RowList.Add(cdr);
                        }
                    }
                }

                //排序
                _RowList.Sort(SortRow);

                dataGridViewX1.DataSource = new BindingList<CadreDataRow>(_RowList);
            }
            else
            {
                DialogResult dr = MsgBox.Show("您未設定班級幹部清單\n是否要現在設定?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    CadreNameChange(); //開啟設定檔畫面
                }
            }

            this.intSchoolYear.ValueChanged += new System.EventHandler(this.intSchoolYear_ValueChanged);
            this.intSemester.ValueChanged += new System.EventHandler(this.intSemester_ValueChanged);
        }

        private int SortRow(CadreDataRow a, CadreDataRow b)
        {
            string indexA = a._index.ToString().PadLeft(3, '0');
            string indexB = b._index.ToString().PadLeft(3, '0');
            string SeatNoA = "" + a._StudentSeatNo.PadLeft(3, '0');
            string SeatNoB = "" + b._StudentSeatNo.PadLeft(3, '0');
            indexA += SeatNoA;
            indexB += SeatNoB;
            return indexA.CompareTo(indexB);
        }

        /// <summary>
        /// 拋出尚未填入的幹部資料
        /// </summary>
        private SchoolObject Getobj(string CadreName)
        {
            foreach (SchoolObject each in CadreList)
            {
                //1.名稱相同
                //2.字典內是false
                if (CadreName == each.CadreName && !CadreDic[each.UID])
                {
                    CadreDic[each.UID] = true;
                    return each;
                }
            }
            return null;

        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CadreDataRow c = dataGridViewX1.Rows[e.RowIndex].DataBoundItem as CadreDataRow;

            dataGridViewX1.Rows[e.RowIndex].Cells[2].ErrorText = c.Error;
        }

        /// <summary>
        /// 儲存
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //畫面設定為鎖定
            SetObjType = false;

            #region 資料整理
            List<SchoolObject> listInsert = new List<SchoolObject>();
            List<SchoolObject> listDelete = new List<SchoolObject>();
            List<SchoolObject> listDef = new List<SchoolObject>();

            List<CadreDataRow> insert = new List<CadreDataRow>();
            List<CadreDataRow> delete = new List<CadreDataRow>();
            List<CadreDataRow> Def = new List<CadreDataRow>();

            foreach (CadreDataRow data in _RowList)
            {
                if (data._CadreRecord != null && data._CadreRecord.UID == "")
                {
                    insert.Add(data);
                }

                if (data._CadreRecordDel != null)
                {
                    delete.Add(data);
                }

                if (data._CadreRecord != null && data._CadreRecord.UID != "")
                {
                    Def.Add(data);
                }
            }

            foreach (CadreDataRow data in Def)
            {
                listDef.Add(data._CadreRecord);
            } 
            #endregion

            #region Log
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("學年度「" + intSchoolYear.Value + "」學期「" + intSemester.Value + "」");

            if (insert.Count != 0)
            {
                sb.AppendLine("班級「" + _classRecord.Name + "」已進行班級幹部登錄：");

                foreach (CadreDataRow data in insert)
                {
                    //新增
                    listInsert.Add(data._CadreRecord);

                    sb.AppendLine("學生「" + data._StudentRecord.Name + "」擔任「班級幹部」幹部名稱「" + data._CadreName + "」已新增");

                }
            }

            if (delete.Count != 0)
            {
                sb.AppendLine("班級「" + _classRecord.Name + "」同步刪除舊有班級幹部記錄：");

                foreach (CadreDataRow data in delete)
                {
                    listDelete.Add(data._CadreRecordDel);

                    if (data._StudentRecord == null) //如果是學生完全清空
                    {
                        StudentRecord sr = Student.SelectByID(data._CadreRecordDel.StudentID);
                        sb.AppendLine("學生「" + sr.Name + "」擔任「班級幹部」幹部名稱「" + data._CadreName + "」已被刪除");
                    }
                    else
                    {
                        sb.AppendLine("學生「" + data._StudentRecord.Name + "」擔任「班級幹部」幹部名稱「" + data._CadreName + "」已被刪除");
                    }
                }

            }
            #endregion

            #region 資料儲存至資料庫
            List<string> listCadreID = new List<string>();
            try
            {
                listCadreID = this._accessHelper.InsertValues(listInsert.ToArray());
                this._accessHelper.DeletedValues(listDelete.ToArray());
            }
            catch
            {
                MsgBox.Show("新增資料發生錯誤!!");
                SetObjType = true;
                return;
            }
            if (listInsert.Count + listDelete.Count > 0)
            {
                ApplicationLog.Log("幹部外掛模組", "班級幹部登錄", "class", _classRecord.ID, sb.ToString());
                MsgBox.Show("幹部記錄,儲存成功!!");
            }
            else
            {
                MsgBox.Show("未修改資料!!");
            } 
            #endregion

            SetObjType = true;

            //如果勾選登錄幹部敘獎
            if (checkBoxX1.Checked)
            {
                #region 開啟「幹部敘獎作業」

                (new CadreMeritManage.CadreMeritManage(intSchoolYear.Value,intSemester.Value, CadreType.ClassCadre, K12.Presentation.NLDPanels.Class.SelectedSource[0])).ShowDialog();

                // 舊-幹部敘獎作業
                //List<SchoolObject> list = new List<SchoolObject>();

                //if (listCadreID.Count != 0)
                //{
                //    String sb123 = string.Join(",", listCadreID.ToArray());
                //    list = _accessHelper.Select<SchoolObject>(string.Format("UID in (" + sb123 + ")"));
                //}

                //list.AddRange(listDef);

                //if (list.Count != 0)
                //{
                //    //進行敘獎操作
                //    //ps:絮獎模式有兩種
                //    //1.將學生基本資料導入獎勵功能
                //    //2.透過設定值,以幹部為基準進行絮獎(較方便) <--
                //    MutiMeritDemerit mmd = new MutiMeritDemerit("獎勵", list, "班級幹部", intSchoolYear.Value, intSemester.Value);
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


        #region 較不重要的程式碼

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
        /// 管理幹部名稱
        /// </summary>
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
        /// 一般是儲存狀況時使用
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

        private int SortIndex(ClassCadreNameObj x, ClassCadreNameObj y)
        {
            return x.Index.CompareTo(y.Index);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnReset_Click(object sender, EventArgs e)
        {
            //btnReset.ForeColor = this.ControlText;
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
