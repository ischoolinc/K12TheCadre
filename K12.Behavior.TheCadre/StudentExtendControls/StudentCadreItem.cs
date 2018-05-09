using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.UDT;
using FISCA.LogAgent;
using Framework;
using K12.Data;
using System.Xml;
using FISCA.DSAUtil;
using Campus.Windows;

namespace K12.Behavior.TheCadre
{
    [FISCA.Permission.FeatureCode("6B1F4596-D2C8-44E2-ADFD-D01E9E61B796", "幹部記錄")]
    public partial class StudentCadreItem : DetailContentBase
    {
        internal static FISCA.Permission.FeatureAce UserPermission;

        private Campus.Windows.ChangeListener DataListener { get; set; }
        //目前幹部清單
        private List<SchoolObject> UDTSchoolList = new List<SchoolObject>();

        private List<SchoolObject> DeleteSchoolList = new List<SchoolObject>();

        private BackgroundWorker BgW = new BackgroundWorker();

        //UDT功能物件
        private AccessHelper _accessHelper = new AccessHelper();

        private bool BkWBool = false;

        Dictionary<string, List<ClassCadreNameObj>> cadreList = new Dictionary<string, List<ClassCadreNameObj>>();

        public StudentCadreItem()
        {
            InitializeComponent();

            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            
            ChangeCadreList();

            if (!string.IsNullOrEmpty(DateConfig["幹部自動帶入"]))
            {
                checkBoxX1.Checked = bool.Parse(DateConfig["幹部自動帶入"]);
            }

            UserPermission = FISCA.Permission.UserAcl.Current[FISCA.Permission.FeatureCodeAttribute.GetCode(GetType())];
            this.Enabled = UserPermission.Editable;

            this.Group = "幹部記錄";

            BgW.DoWork += new DoWorkEventHandler(BgW_DoWork);
            BgW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgW_RunWorkerCompleted);

            DataListener = new Campus.Windows.ChangeListener();
            DataListener.Add(new Campus.Windows.DataGridViewSource(dataGridViewX1));
            DataListener.StatusChanged += new EventHandler<Campus.Windows.ChangeEventArgs>(DataListener_StatusChanged);
        }

        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            if (this.PrimaryKey != "") //不是空的
            {
                this.Loading = true;


                if (BgW.IsBusy)
                {
                    BkWBool = true;
                }
                else
                {
                    BgW.RunWorkerAsync();
                }
            }
        }

        void BgW_DoWork(object sender, DoWorkEventArgs e)
        {
            UDTSchoolList.Clear();
            UDTSchoolList = _accessHelper.Select<SchoolObject>(string.Format("StudentID='{0}'", this.PrimaryKey));
            UDTSchoolList.Sort(new Comparison<SchoolObject>(SortCadre));

            DeleteSchoolList.Clear();
            foreach (SchoolObject each in UDTSchoolList)
            {
                DeleteSchoolList.Add(each);
            }
        }

        void BgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BkWBool)
            {
                BkWBool = false;
                BgW.RunWorkerAsync();
                return;
            }

            BuildData();

            this.SaveButtonVisible = false;
            this.CancelButtonVisible = false;
            DataListener.Reset();
            DataListener.ResumeListen();
            this.Loading = false;
        }

        void DataListener_StatusChanged(object sender, Campus.Windows.ChangeEventArgs e)
        {
            this.SaveButtonVisible = (e.Status == Campus.Windows.ValueStatus.Dirty);
            this.CancelButtonVisible = (e.Status == Campus.Windows.ValueStatus.Dirty);
        }

        /// <summary>
        /// 儲存
        /// </summary>
        protected override void OnSaveButtonClick(EventArgs e)
        {
            dataGridViewX1.EndEdit();

            if (CheckError())
            {
                FISCA.Presentation.Controls.MsgBox.Show("您輸入的資料尚有錯誤,請檢查後再進行儲存");
                return;
            }
            else
            {
                Save();
                this.SaveButtonVisible = false;
                this.CancelButtonVisible = false;
            }
            DataListener.Reset();
            DataListener.ResumeListen();
        }

        /// <summary>
        /// 儲存
        /// </summary>
        private void Save()
        {
            try
            {
                _accessHelper.DeletedValues(UDTSchoolList.ToArray());
            }
            catch (Exception ex)
            {
                FISCA.Presentation.Controls.MsgBox.Show("儲存失敗,請重試一次!\n" + ex.Message);
                return;
            }

            UDTSchoolList.Clear();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("學生「" + Student.SelectByID(this.PrimaryKey).Name + "」幹部記錄已修改");
            sb.AppendLine("詳細資料：");

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                SchoolObject obj = new SchoolObject();
                obj.StudentID = this.PrimaryKey;

                obj.ReferenceID = "" + row.Cells[0].Value; //已經沒有類型ID
                obj.ReferenceType = "" + row.Cells[1].Value; //幹部類型
                obj.SchoolYear = "" + row.Cells[2].Value;
                obj.Semester = "" + row.Cells[3].Value;
                obj.CadreName = "" + row.Cells[4].Value; //幹部名稱
                obj.Text = "" + row.Cells[5].Value;

                sb.AppendLine("學年度「" + obj.SchoolYear + "」學期「" + obj.Semester + "」擔任「" + obj.ReferenceType + "」幹部名稱「" + obj.CadreName + "」幹部說明「" + obj.Text + "」");

                UDTSchoolList.Add(obj);
            }

            try
            {
                _accessHelper.InsertValues(UDTSchoolList.ToArray());
            }
            catch (Exception ex)
            {
                FISCA.Presentation.Controls.MsgBox.Show("儲存失敗,請重試一次!\n" + ex.Message);

                _accessHelper.InsertValues(DeleteSchoolList.ToArray()); //把刪除資料重新建立回去

                return;
            }

            ApplicationLog.Log("幹部外掛模組", "新增幹部資料", "student", this.PrimaryKey, sb.ToString());

            Campus.Windows.MsgBox.Show("儲存成功!");

            this.Loading = true;

            BgW.RunWorkerAsync();

        }

        /// <summary>
        /// 針對DataGridView進行錯誤檢查
        /// </summary>
        private bool CheckError()
        {
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ErrorText != "")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 取消
        /// </summary>
        protected override void OnCancelButtonClick(EventArgs e)
        {
            this.SaveButtonVisible = false;
            this.CancelButtonVisible = false;

            DataListener.SuspendListen(); //終止變更判斷
            BgW.RunWorkerAsync(); //背景作業,取得並重新填入原資料
        }

        /// <summary>
        /// 更新畫面資料
        /// </summary>
        private void BuildData()
        {
            dataGridViewX1.Rows.Clear();
            foreach (SchoolObject each in UDTSchoolList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Value = each.ReferenceID; //參考ID
                row.Cells[1].Value = each.ReferenceType; //幹部類別
                row.Cells[2].Value = each.SchoolYear;
                row.Cells[3].Value = each.Semester;
                row.Cells[4].Value = each.CadreName;
                row.Cells[5].Value = each.Text;

                //if (each.ReferenceID != "")
                //{
                //    SetCellColor(row);
                //}

                dataGridViewX1.Rows.Add(row);
            }
        }

        //private void SetCellColor(DataGridViewRow row)
        //{
        //    foreach (DataGridViewCell cell in row.Cells)
        //    {
        //        cell.Style.BackColor = Color.LightCyan;
        //        cell.ReadOnly = true;

        //    }
        //}

        /// <summary>
        /// 基礎排序(學年度/學期)
        /// </summary>
        private int SortCadre(SchoolObject Cadrex, SchoolObject Cadrey)
        {
            string xx = Cadrex.SchoolYear + Cadrex.Semester;
            string yy = Cadrey.SchoolYear + Cadrey.Semester;

            return xx.CompareTo(yy);
        }

        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewX1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ErrorText = "";
            string check = "" + cell.Value;
            if (e.ColumnIndex == 1)
            {
                ColCadreName.Items.Clear();
                if (cadreList.ContainsKey("" + cell.Value)) //如果選擇了幹部類型,內容亦將篩選為該幹部內容
                {
                    foreach (ClassCadreNameObj each in cadreList["" + cell.Value])
                    {
                        ColCadreName.Items.Add(each.CadreName);
                    }
                }
                else //未選擇將會顯示全部幹部內容
                {
                    foreach (string each1 in cadreList.Keys)
                    {
                        foreach (ClassCadreNameObj each2 in cadreList[each1])
                        {
                            ColCadreName.Items.Add(each2.CadreName);
                        }
                    }
                }
            }
            else if (e.ColumnIndex == 2) //學年度
            {
                int ParseSchoolYear;
                if (!int.TryParse(check, out ParseSchoolYear))
                {
                    cell.ErrorText = "學年度必須為數字";
                }
            }
            else if (e.ColumnIndex == 3)
            {
                if (check != "1" && check != "2")
                {
                    cell.ErrorText = "學期必須為1或2";
                }
            }
        }

        private void dataGridViewX1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = dataGridViewX1.Rows[e.RowIndex].Cells[1];
            if (e.ColumnIndex == 4)
            {
                if (cadreList.ContainsKey("" + cell.Value))
                {
                    ColCadreName.Items.Clear();
                    foreach (ClassCadreNameObj each in cadreList["" + cell.Value])
                    {
                        ColCadreName.Items.Add(each.CadreName);
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewCadreSetup TCN = new NewCadreSetup();
            TCN.ShowDialog();

            ChangeCadreList();
        }

        /// <summary>
        /// 取得幹部名稱
        /// </summary>
        private void ChangeCadreList()
        {
            cadreList.Clear();
            cadreList.Add("班級幹部", new List<ClassCadreNameObj>());
            cadreList.Add("學校幹部", new List<ClassCadreNameObj>());
            cadreList.Add("社團幹部", new List<ClassCadreNameObj>());

            //取得幹部清單
            List<ClassCadreNameObj> ClassCadreNameList = _accessHelper.Select<ClassCadreNameObj>();
            var list = from Record in ClassCadreNameList orderby Record.NameType, Record.Index select Record;
            foreach (ClassCadreNameObj each in list)
            {
                cadreList[each.NameType].Add(each);
            }

            //將幹部清單加入Item
            ColCadreName.Items.Clear();
            foreach (string each1 in cadreList.Keys)
            {
                foreach (ClassCadreNameObj each2 in cadreList[each1])
                {
                    ColCadreName.Items.Add(each2.CadreName);
                }
            }
        }

        private void dataGridViewX1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (checkBoxX1.Checked)
            {
                dataGridViewX1.Rows[e.Row.Index - 1].Cells[2].Value = School.DefaultSchoolYear;
                dataGridViewX1.Rows[e.Row.Index - 1].Cells[3].Value = School.DefaultSemester;
            }
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            DateConfig["幹部自動帶入"] = checkBoxX1.Checked.ToString();
            DateConfig.Save();
        }
    }
}
