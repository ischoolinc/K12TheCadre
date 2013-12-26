using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using System.Xml;
using K12.Data;
using FISCA.DSAUtil;
using Framework.Feature;
using Aspose.Cells;
using FISCA.UDT;
using Campus.Windows;

namespace K12.Behavior.TheCadre
{
    //幹部與敘獎 - 管理視窗
    public partial class NewCadreSetup : BaseForm
    {

        private ChangeListener DataListener { get; set; }
        private bool DataGridViewDataInChange = false; //資料是否更動檢查

        /// <summary>
        /// 獎勵事由代碼
        /// </summary>
        private Dictionary<string, string> CodeDic = new Dictionary<string, string>();

        /// <summary>
        /// UDT操作物件
        /// </summary>
        private AccessHelper accessHelper = new AccessHelper();

        /// <summary>
        /// 資料檢查器
        /// </summary>
        private DataGridViewErrorCheck check = new DataGridViewErrorCheck();

        /// <summary>
        /// Config設定器
        /// </summary>
        ConfigFormMethod cfm = new ConfigFormMethod();

        //先進先出,說明文字
        private Queue<string> timerList2 = new Queue<string>();

        private List<string> CadreTypeList = new List<string>();
        
        /// <summary>
        /// 傳入幹部型態(班級幹部/學校幹部/社團幹部/空字串為所有類型)
        /// </summary>
        /// <param name="NameType"></param>
        public NewCadreSetup()
        {
            InitializeComponent();

            DataListener = new ChangeListener();
            DataListener.Add(new DataGridViewSource(dataGridViewX1));
            DataListener.StatusChanged += new EventHandler<ChangeEventArgs>(DataListener_StatusChanged);

            this.Text = "幹部名稱管理(班級幹部 / 學校幹部 / 社團幹部)";

            CadreTypeList.Add("班級幹部");
            CadreTypeList.Add("社團幹部");
            CadreTypeList.Add("學校幹部");

            timerString();

            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            //DateConfig["幹部名稱_輸入敘獎資料"] = KeyInMerit.Checked.ToString();
            if (DateConfig["幹部名稱管理畫面_輸入敘獎資料"] != "")
            {
                KeyInMerit.Checked = bool.Parse(DateConfig["幹部名稱管理畫面_輸入敘獎資料"]);
            }

            CodeDic = cfm.GetDisciplineReason();

            SetData();
        }

        /// <summary>
        /// 設定畫面
        /// </summary>
        private void SetData()
        {
            //取得班級幹部類型
            DataListener.SuspendListen(); //終止變更判斷
            DataGridViewDataInChange = false;
            dataGridViewX1.Rows.Clear();
            List<ClassCadreNameObj> ClassCadreNameList = accessHelper.Select<ClassCadreNameObj>();

            if (ClassCadreNameList.Count != 0) //是否有字典內容
            {
                var list = from Record in ClassCadreNameList orderby Record.NameType, Record.Index select Record;

                foreach (ClassCadreNameObj each in list)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewX1);
                    row.Cells[colCadreType.Index].Value = each.NameType;
                    row.Cells[colCadreIndex.Index].Value = each.Index.ToString();
                    row.Cells[colCadreName.Index].Value = each.CadreName;
                    if (each.Number == 0)
                        row.Cells[colNumber.Index].Value = 1;
                    else
                        row.Cells[colNumber.Index].Value = each.Number;
                    row.Cells[colMeritA.Index].Value = each.MeritA;
                    row.Cells[colMeritB.Index].Value = each.MeritB;
                    row.Cells[colMeritC.Index].Value = each.MeritC;
                    row.Cells[colMeritReason.Index].Value = each.Reason;
                    dataGridViewX1.Rows.Add(row);
                }
            }

            ChangeKeyInMerit();

            DataListener.Reset();
            DataListener.ResumeListen(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckBox())
            {
                Campus.Windows.MsgBox.Show("資料尚有錯誤,請修改後再儲存!!");
                return;
            }

            List<ClassCadreNameObj> InsertList = new List<ClassCadreNameObj>();

            foreach (DataGridViewRow eachRow in dataGridViewX1.Rows)
            {
                if (eachRow.IsNewRow)
                    continue;

                ClassCadreNameObj obj = new ClassCadreNameObj();
                obj.NameType = "" + eachRow.Cells[colCadreType.Index].Value; //幹部類型
                obj.Index = check.CheckDataGridViewNotInt(eachRow.Cells[colCadreIndex.Index]); //排序
                obj.CadreName = "" + eachRow.Cells[colCadreName.Index].Value; //幹部名稱
                obj.Number = check.CheckNotIntNumber(eachRow.Cells[colNumber.Index]); //擔任人數
                if (KeyInMerit.Checked)
                {
                    obj.MeritA = check.CheckDataGridViewNotInt(eachRow.Cells[colMeritA.Index]);
                    obj.MeritB = check.CheckDataGridViewNotInt(eachRow.Cells[colMeritB.Index]);
                    obj.MeritC = check.CheckDataGridViewNotInt(eachRow.Cells[colMeritC.Index]);
                    obj.Reason = "" + eachRow.Cells[colMeritReason.Index].Value;
                }

                InsertList.Add(obj);
            }

            List<ClassCadreNameObj> DeleteList = accessHelper.Select<ClassCadreNameObj>();

            try
            {
                accessHelper.DeletedValues(DeleteList.ToArray());
                accessHelper.InsertValues(InsertList.ToArray());
            }
            catch(Exception ex)
            {
                Campus.Windows.MsgBox.Show("新增資料錯誤!!\n" + ex.Message);
            }
            DataGridViewDataInChange = false;
            Campus.Windows.MsgBox.Show("儲存資料成功!!");
        }

        /// <summary>
        /// 檢查功過是否為數字
        /// 回傳true即為錯誤
        /// </summary>
        /// <returns></returns>
        private bool CheckBox()
        {
            bool DateError = false;
            foreach (DataGridViewRow eachRow in dataGridViewX1.Rows)
            {
                if (eachRow.IsNewRow)
                    continue;

                CheckCell(eachRow);
            }

            foreach (DataGridViewRow eachRow in dataGridViewX1.Rows)
            {
                //如果是新行則離開
                if (eachRow.IsNewRow)
                    continue;

                //Row - 是否有錯誤訊息
                if (!string.IsNullOrEmpty(eachRow.ErrorText))
                {
                    DateError = true;
                }

                //所有的Cell是否有錯誤訊息
                foreach (DataGridViewCell eachCell in eachRow.Cells)
                {
                    if (!string.IsNullOrEmpty(eachCell.ErrorText))
                    {
                        DateError = true;
                    }
                }
            }
            return DateError;
        }

        private void CheckCell(DataGridViewRow eachRow)
        {
            //幹部類型
            check.CheckCadreType(eachRow.Cells[colCadreType.Index], "錯誤:必須選擇幹部類型");

            //排序
            check.CheckCellIsInt(eachRow.Cells[colCadreIndex.Index], "錯誤:排序必須輸入數字");
            //幹部名稱
            check.CheckCellIsEmptyError(eachRow.Cells[colCadreName.Index], "錯誤:幹部名稱必須填入內容!!");

            check.CheckCellIsIntEmpty(eachRow.Cells[colNumber.Index], "錯誤:擔任人數內容非數字"); 
            //內容不是空的才檢查
            check.CheckCellIsIntEmpty(eachRow.Cells[colMeritA.Index], "錯誤:大功內容非數字"); //大功
            check.CheckCellIsIntEmpty(eachRow.Cells[colMeritB.Index], "錯誤:小功內容非數字"); //小功
            check.CheckCellIsIntEmpty(eachRow.Cells[colMeritC.Index], "錯誤:嘉獎內容非數字"); //嘉獎

            if (KeyInMerit.Checked)
            {
                //大功+小功+嘉獎不可等於0
                check.CheckRowTotalEqualToZero(eachRow, colMeritA.Index, colMeritB.Index, colMeritC.Index, "大功+小功+嘉獎不可為0");
                //事由必須輸入內容
                check.CheckCellIsEmptyWarning(eachRow.Cells[colMeritReason.Index]);
            }
        }

        private void btnPrintOut_Click(object sender, EventArgs e)
        {
            #region 匯出
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            DataGridViewExport export = new DataGridViewExport(dataGridViewX1);
            export.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            #endregion
        }

        private void btnPrintIn_Click(object sender, EventArgs e)
        {
            #region 匯入
            DialogResult dr = FISCA.Presentation.Controls.MsgBox.Show("匯入管理名稱與敘獎資料\n將完全覆蓋目前之資料狀態\n(建議可將原資料匯出備份)\n\n請確認繼續?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dr != DialogResult.Yes)
                return;

            Workbook wb = new Workbook();

            #region OpenFileDialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "選擇要匯入的缺曠類別設定值";
            ofd.Filter = "Excel檔案 (*.xls)|*.xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    wb.Open(ofd.FileName);
                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "開啟檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }
            #endregion

            List<ClassCadreNameObj> InsertList = new List<ClassCadreNameObj>();

            //必要欄位
            List<string> requiredHeaders;

            if (KeyInMerit.Checked) //如果提供輸入預設獎勵清單
            {
                requiredHeaders = new List<string>(new string[] { "幹部類型", "排序", "幹部名稱", "擔任人數", "大功", "小功", "嘉獎", "獎勵事由" });

                #region 匯入包含敘獎內容

                //欄位標題的索引
                Dictionary<string, int> headers = new Dictionary<string, int>();
                Worksheet ws = wb.Worksheets[0];

                //蒐集excel清單上的標題欄位
                for (int i = 0; i <= 7; i++)
                {
                    string header = ws.Cells[0, i].StringValue;
                    if (requiredHeaders.Contains(header))
                        headers.Add(header, i);
                }

                //如果使用者匯入檔的欄位與必要欄位不符，則停止匯入
                if (headers.Count != requiredHeaders.Count)
                {
                    StringBuilder builder = new StringBuilder(string.Empty);
                    builder.AppendLine("匯入格式不符合。");
                    builder.AppendLine("匯入資料標題必須包含：");
                    builder.AppendLine(string.Join(",", requiredHeaders.ToArray()));
                    FISCA.Presentation.Controls.MsgBox.Show(builder.ToString());
                    return;
                }

                for (int x = 1; x <= wb.Worksheets[0].Cells.MaxDataRow; x++) //每一Row
                {

                    if (string.IsNullOrEmpty(ws.Cells[x, headers["幹部名稱"]].StringValue.Trim()))
                    {
                        continue;
                    }

                    //"排序", "幹部名稱", "大功", "小功", "嘉獎", "獎勵事由
                    ClassCadreNameObj obj = new ClassCadreNameObj();
                    obj.Index = check.CheckAsposeNotInt(ws.Cells[x, headers["排序"]]);
                    obj.MeritA = check.CheckAsposeNotInt(ws.Cells[x, headers["大功"]]);
                    obj.MeritB = check.CheckAsposeNotInt(ws.Cells[x, headers["小功"]]);
                    obj.MeritC = check.CheckAsposeNotInt(ws.Cells[x, headers["嘉獎"]]);

                    if (obj.MeritA + obj.MeritB + obj.MeritC == 0)
                    {
                        Campus.Windows.MsgBox.Show("大功+小功+嘉獎不可等於0,匯入失敗.");
                        return;
                    }

                    obj.NameType = ws.Cells[x, headers["幹部類型"]].StringValue.Trim();
                    if (!CadreTypeList.Contains(obj.NameType)) //必須是三種幹部類型之一
                    {
                        Campus.Windows.MsgBox.Show("幹部類型僅提供班級幹部,社團幹部,學校幹部..等..三種類型,匯入失敗.");
                        return;
                    }

                    obj.CadreName = ws.Cells[x, headers["幹部名稱"]].StringValue.Trim();
                    if (string.IsNullOrEmpty(obj.CadreName))
                    {
                        Campus.Windows.MsgBox.Show("幹部名稱必須輸入內容,匯入失敗.");
                        return;
                    }
                    obj.Reason = ws.Cells[x, headers["獎勵事由"]].StringValue.Trim(); //去除前後空白字元
                    if (string.IsNullOrEmpty(obj.Reason))
                    {
                        Campus.Windows.MsgBox.Show("獎勵事由必須輸入內容,匯入失敗.");
                        return;
                    }

                    obj.Number = check.CheckNotIntNumber_1(ws.Cells[x, headers["擔任人數"]]);

                    InsertList.Add(obj);
                }
                #endregion
            }
            else
            {
                requiredHeaders = new List<string>(new string[] { "幹部類型", "排序", "幹部名稱", "擔任人數" });
               
                #region 不匯入敘獎內容
                
                //欄位標題的索引
                Dictionary<string, int> headers = new Dictionary<string, int>();
                Worksheet ws = wb.Worksheets[0];

                //蒐集excel清單上的標題欄位
                for (int i = 0; i <= ws.Cells.MaxDataColumn; i++)
                {
                    string header = ws.Cells[0, i].StringValue;
                    if (requiredHeaders.Contains(header))
                        headers.Add(header, i);
                }

                //如果使用者匯入檔的欄位與必要欄位不符，則停止匯入
                if (headers.Count != requiredHeaders.Count)
                {
                    StringBuilder builder = new StringBuilder(string.Empty);
                    builder.AppendLine("匯入格式不符合。");
                    builder.AppendLine("匯入資料標題必須包含：");
                    builder.AppendLine(string.Join(",", requiredHeaders.ToArray()));
                    FISCA.Presentation.Controls.MsgBox.Show(builder.ToString());
                    return;
                }

                for (int x = 1; x <= wb.Worksheets[0].Cells.MaxDataRow; x++) //每一Row
                {
                    //幹部類型為空就跳過
                    if (string.IsNullOrEmpty(ws.Cells[x, headers["幹部名稱"]].StringValue.Trim()))
                    {
                        continue;
                    }

                    ClassCadreNameObj obj = new ClassCadreNameObj();

                    obj.Index = check.CheckAsposeNotInt(ws.Cells[x, headers["排序"]]);
                    obj.CadreName = ws.Cells[x, headers["幹部名稱"]].StringValue;
                    if (string.IsNullOrEmpty(obj.CadreName))
                    {
                        Campus.Windows.MsgBox.Show("幹部名稱必須輸入內容,匯入失敗.");
                        return;
                    }
                    obj.NameType = ws.Cells[x, headers["幹部類型"]].StringValue;

                    if (!CadreTypeList.Contains(obj.NameType)) //必須是三種幹部類型之一
                    {
                        Campus.Windows.MsgBox.Show("幹部類型僅提供班級幹部,社團幹部,學校幹部..等..三種類型");
                        return;
                    }

                    obj.Number = check.CheckNotIntNumber_1(ws.Cells[x, headers["擔任人數"]]);

                    InsertList.Add(obj);
                } 
                #endregion
            }

            List<ClassCadreNameObj> DeleteList = accessHelper.Select<ClassCadreNameObj>();

            try
            {
                accessHelper.DeletedValues(DeleteList.ToArray());
                accessHelper.InsertValues(InsertList.ToArray());
            }
            catch (Exception ex)
            {
                Campus.Windows.MsgBox.Show("新增資料錯誤!!" + ex.Message);
            }

            Campus.Windows.MsgBox.Show("匯入成功!!");

            SetData(); 
            #endregion            
        }

        #region 提示訊息的切換

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTip();
        }

        private void UpdateTip()
        {
            string msg = timerList2.Dequeue(); //移出字串(最前端)
            labelX1.Text = msg; //修改說明
            timerList2.Enqueue(msg); //加回字串(最後)
        }

        private void timerString()
        {
            timerList2.Enqueue("說明：(舊有)幹部清單已提供匯出，可於匯出整理後匯入新畫面清單中。");
            timerList2.Enqueue("說明：幹部類型分為(班級幹部,社團幹部,學校幹部)。");
            timerList2.Enqueue("說明：(獎勵事由)欄位可直接輸入(獎勵事由)代碼。");
            timerList2.Enqueue("說明：排序是依(幹部名稱)分類後才依(排序)數字進行排序。");
        } 

        #endregion

        /// <summary>
        /// 是否輸入敘獎之資料切換
        /// </summary>
        private void ChangeKeyInMerit()
        {
            //當輸入敘獎資料
            if (KeyInMerit.Checked)
            {
                colMeritA.Visible = true;
                colMeritB.Visible = true;
                colMeritC.Visible = true;
                colMeritReason.Visible = true;
            }
            else //不進行敘獎
            {
                //foreach (DataGridViewRow row in dataGridViewX1.Rows)
                //{
                //    if (row.IsNewRow)
                //        continue;

                //    row.Cells[colMeritA.Index].Value = 0;
                //    row.Cells[colMeritB.Index].Value = 0;
                //    row.Cells[colMeritC.Index].Value = 0;
                //    row.Cells[colMeritReason.Index].Value = "";
                //}
                colMeritA.Visible = false;
                colMeritB.Visible = false;
                colMeritC.Visible = false;
                colMeritReason.Visible = false;
            }
        }

        //資料更新事件
        void DataListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            DataGridViewDataInChange = true;
        }

        //當設定被勾選
        private void KeyInMerit_CheckedChanged(object sender, EventArgs e)
        {
            K12.Data.Configuration.ConfigData DateConfig = K12.Data.School.Configuration["幹部模組_幹部名稱清單"];
            DateConfig["幹部名稱管理畫面_輸入敘獎資料"] = KeyInMerit.Checked.ToString();
            DateConfig.Save();

            ChangeKeyInMerit();
        }

        //舊有幹部資料畫面
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OldCadreSetup Tcn = new OldCadreSetup();
            Tcn.ShowDialog();
        }

        //檢查新增之Row是否未輸入資料
        private void dataGridViewX1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            CheckCell(dataGridViewX1.Rows[e.Row.Index - 1]);
        }

        //DataGridView結束編輯
        private void dataGridViewX1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CheckCell(dataGridViewX1.CurrentRow);

            string CurrentCellValue = "" + dataGridViewX1.CurrentCell.Value;
            if (e.ColumnIndex == 7)
            {
                //事由替換
                if (CodeDic.ContainsKey(CurrentCellValue))
                {
                    dataGridViewX1.CurrentCell.Value = CodeDic[CurrentCellValue];
                }
            }
        }

        //離開
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (DataGridViewDataInChange)
            {
                DialogResult dr = FISCA.Presentation.Controls.MsgBox.Show("資料已被修改,請確認是否要離開?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
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
    }
}
