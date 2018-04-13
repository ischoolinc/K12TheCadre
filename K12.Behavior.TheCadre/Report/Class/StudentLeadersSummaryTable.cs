using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data.Configuration;
using Aspose.Cells;
using System.IO;
using K12.Data;

namespace K12.Behavior.TheCadre
{
    public partial class StudentLeadersSummaryTable : BaseForm
    {
        string stringPrintClassCadre = "列印班級幹部";
        string stringPrintSchoolCadre = "列印學校幹部";
        string stringPrintandsetCadre = "列印社團幹部";

        string stringConfigName = "班級幹部總表_K12";

        ConfigData cd;

        BackgroundWorker BGW = new BackgroundWorker();

        slsTConfig _config { get; set; }

        public StudentLeadersSummaryTable()
        {
            InitializeComponent();

            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
        }

        private void StudentLeadersSummaryTable_Load(object sender, EventArgs e)
        {
            CheckConfig();
        }

        /// <summary>
        /// 列印學生幹部總表
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!cbClassCadre.Checked && !cbSchoolCadre.Checked && !cbAndCadre.Checked)
            {
                MsgBox.Show("請選擇一個幹部類型!!");
                return;
            }


            if (!BGW.IsBusy)
            {
                _config = new slsTConfig();
                _config.PrintAllSchoolYear = cbPrintAllSchoolYear.Checked; //列印所有學年期

                _config.SchoolYear = intSchoolYear.Value; //學年度
                _config.Semester = intSemester.Value; //學期

                _config.CheckClassCadre = cbClassCadre.Checked; //列印班級幹部類型
                _config.CheckSchoolCadre = cbSchoolCadre.Checked; //列印學校幹部類型
                _config.CheckAndCadre = cbAndCadre.Checked; //列印社團幹部類型

                RunWorker = false;

                BGW.RunWorkerAsync(K12.Presentation.NLDPanels.Class.SelectedSource);
            }
            else
            {
                MsgBox.Show("系統忙碌中..稍後再試");
            }
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> classidlist = (List<string>)e.Argument;

            //取得所選班級學生資料
            SQLselect sql = new SQLselect(classidlist);

            sql.AccessUDTData(_config);

            //取得範本
            Workbook template = new Workbook();
            template.Worksheets.Clear();
            //template.Open(new MemoryStream(Properties.Resources.班級幹部總表_範本), FileFormatType.Xlsx);
            template = new Workbook(new MemoryStream(Properties.Resources.班級幹部總表_範本), new LoadOptions());

            Worksheet ptws = template.Worksheets[0];
            //建立Range
            Range ptHeader = ptws.Cells.CreateRange(0, 2, false);
            Range ptEachRow = ptws.Cells.CreateRange(2, 1, false);

            Workbook wb = new Workbook();
            wb.Copy(template);
            Worksheet ws = wb.Worksheets[0];

            int studentCount = 0;
            int cutPageIndex = 49;
            //依據 學年度/學期/座號/姓名/學號/類型/幹部名稱/說明 進行列印
            foreach (string class_Name in sql._StudentObjDic.Keys) //班級名稱
            {
                int cutStudentIndex = 0;
                int count = 0;
                foreach (StudentCadre cadre in sql._StudentObjDic[class_Name]) //每一個學生幹部資料
                {
                    count += cadre.CadreList.Count;
                }
                if (count == 0) continue;

                //int SetOutline = 1;
                //複製 Header
                ws.Cells.CreateRange(studentCount, 2, false).Copy(ptHeader);

                string SchoolNameAndTitle = "班級幹部總表";
                ws.Cells[studentCount, 0].PutValue("班級：" + class_Name); //班級名稱
                ws.Cells[studentCount, 2].PutValue(SchoolNameAndTitle); //學校名稱 與 報表名稱
                if (sql._TeacherNameDic.ContainsKey(class_Name))
                    ws.Cells[studentCount, 7].PutValue("導師：" + sql._TeacherNameDic[class_Name]); //班導師
                else
                    ws.Cells[studentCount, 7].PutValue("導師："); //班導師

                //把班級標頭拷貝下來
                Range ClassHeader = ws.Cells.CreateRange(studentCount, 2, false);

                studentCount += 2;
                foreach (StudentCadre cadre in sql._StudentObjDic[class_Name]) //每一個學生幹部資料
                {
                    foreach (SchoolObject obj in cadre.CadreList)
                    {
                        cutStudentIndex++;
                        if (cutStudentIndex == cutPageIndex)
                        {
                            cutStudentIndex = 1;
                            //ws.HPageBreaks.Add(studentCount, 7);
                            ws.HorizontalPageBreaks.Add(studentCount, 7);
                            ws.Cells.CreateRange(studentCount, 2, false).Copy(ClassHeader);
                            studentCount += 2;
                        }


                        ws.Cells.CreateRange(studentCount, 1, false).Copy(ptEachRow);
                        //if (SetOutline % 5 == 0 && SetOutline != 0)
                        //{
                        //    ws.Cells.CreateRange(studentCount, 0, 1, 8).SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Medium, Color.Black);
                        //}

                        ws.Cells[studentCount, 0].PutValue(cadre.Student_SeatNo); //座號
                        ws.Cells[studentCount, 1].PutValue(cadre.Student_Name); //姓名
                        ws.Cells[studentCount, 2].PutValue(cadre.Student_Number); //學號

                        ws.Cells[studentCount, 3].PutValue(obj.SchoolYear); //學年度
                        ws.Cells[studentCount, 4].PutValue(obj.Semester); //學期
                        ws.Cells[studentCount, 5].PutValue(obj.ReferenceType); //幹部類型
                        ws.Cells[studentCount, 6].PutValue(obj.CadreName); //幹部名稱
                        ws.Cells[studentCount, 7].PutValue(obj.Text); //說明
                        //SetOutline++;
                        studentCount++;
                    }
                }
                //ws.Cells.CreateRange(studentCount - 1, 0, 1, 8).SetOutlineBorder(BorderType.BottomBorder, CellBorderType.Medium, Color.Black);
                //ws.HPageBreaks.Add(studentCount, 7);
                ws.HorizontalPageBreaks.Add(studentCount, 7);
            }

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, "班級幹部總表" + ".xlt");
            e.Result = new object[] { "班級幹部總表", path, wb };
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string BarMessage = "";
            if (e.Error == null)
            {
                string reportName;
                string path;
                Workbook wb;

                object[] result = (object[])e.Result;
                reportName = (string)result[0];
                path = (string)result[1];
                wb = (Workbook)result[2];

                if (File.Exists(path))
                {
                    int i = 1;
                    while (true)
                    {
                        string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                        if (!File.Exists(newPath))
                        {
                            path = newPath;
                            break;
                        }
                    }
                }

                try
                {
                    //wb.Save(path, FileFormatType.Excel2003);
                    wb.Save(path,SaveFormat.Xlsx);
                    FISCA.Presentation.MotherForm.SetStatusBarMessage(reportName + "產生完成");
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.Title = "另存新檔";
                    sd.FileName = reportName + ".xlsx";
                    sd.Filter = "Excel檔案 (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";
                    if (sd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            //wb.Save(sd.FileName, FileFormatType.Excel2003);
                            wb.Save(sd.FileName,SaveFormat.Xlsx);
                        }
                        catch
                        {
                            MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                BarMessage = "班級幹部總表 列印成功!!";
                FISCA.Presentation.MotherForm.SetStatusBarMessage(BarMessage);
            }
            else
            {
                SmartSchool.ErrorReporting.ReportingService.ReportException(e.Error);
                BarMessage = "班級幹部總表 列印失敗!!";
                FISCA.Presentation.MotherForm.SetStatusBarMessage(BarMessage + e.Error.Message);
            }

            RunWorker = true;
            FISCA.Presentation.MotherForm.SetStatusBarMessage(BarMessage);
        }

        /// <summary>
        /// 判斷與設定檔
        /// </summary>
        private void CheckConfig()
        {
            //取得設定值
            cd = K12.Data.School.Configuration[stringConfigName];

            //班級幹部
            bool op = true;
            if (cd.Contains(stringPrintClassCadre)) //是否包含此字串
            {
                bool.TryParse(cd[stringPrintClassCadre], out op);
            }
            cbClassCadre.Checked = op;

            //學校幹部
            op = true;
            if (cd.Contains(stringPrintSchoolCadre)) //是否包含此字串
            {
                bool.TryParse(cd[stringPrintSchoolCadre], out op);
            }
            cbSchoolCadre.Checked = op;

            //社團幹部
            op = true;
            if (cd.Contains(stringPrintandsetCadre)) //是否包含此字串
            {
                bool.TryParse(cd[stringPrintandsetCadre], out op);
            }
            cbAndCadre.Checked = op;

            //學年度
            int schoolYearSemester = int.Parse(School.DefaultSchoolYear);
            if (cd.Contains("學年度"))
            {
                int.TryParse(cd["學年度"], out schoolYearSemester);
            }
            intSchoolYear.Value = schoolYearSemester;

            //學期
            schoolYearSemester = int.Parse(School.DefaultSemester);
            if (cd.Contains("學期"))
            {
                int.TryParse(cd["學期"], out schoolYearSemester);
            }
            intSemester.Value = schoolYearSemester;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbPrintAllSchoolYear_CheckedChanged(object sender, EventArgs e)
        {
            intSchoolYear.Enabled = !cbPrintAllSchoolYear.Checked;
            intSemester.Enabled = !cbPrintAllSchoolYear.Checked;
        }

        private void StudentLeadersSummaryTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            cd[stringPrintClassCadre] = cbClassCadre.Checked.ToString();
            cd[stringPrintSchoolCadre] = cbSchoolCadre.Checked.ToString();
            cd[stringPrintandsetCadre] = cbAndCadre.Checked.ToString();
            cd["學年度"] = intSchoolYear.Value.ToString();
            cd["學期"] = intSemester.Value.ToString();
            cd.SaveAsync();
        }

        bool RunWorker
        {
            set
            {
                gbSelectPrintFom.Enabled = value;
                gbSelectPrintType.Enabled = value;
                btnPrint.Enabled = value;
                if (!value)
                {
                    FISCA.Presentation.MotherForm.SetStatusBarMessage("開始列印 班級幹部總表...");
                }
            }
        }
    }

    class slsTConfig
    {
        /// <summary>
        /// 列印所有學年期
        /// </summary>
        public bool PrintAllSchoolYear { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        public int SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public int Semester { get; set; }

        /// <summary>
        /// 列印班級幹部類型
        /// </summary>
        public bool CheckClassCadre { get; set; }


        /// <summary>
        /// 列印學校幹部類型
        /// </summary>
        public bool CheckSchoolCadre { get; set; }

        /// <summary>
        /// 列印社團幹部類型
        /// </summary>
        public bool CheckAndCadre { get; set; }


    }
}
