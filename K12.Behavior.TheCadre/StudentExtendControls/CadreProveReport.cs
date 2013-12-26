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
using FISCA.Presentation;
using System.Diagnostics;
using Aspose.Words;
using System.IO;
using K12.Data;
using Aspose.Words.Drawing;
using System.Xml;

namespace K12.Behavior.TheCadre
{
    public partial class CadreProveReport : BaseForm
    {
        private BackgroundWorker BGW = new BackgroundWorker();

        private AccessHelper _accessHelper = new AccessHelper();

        private List<SchoolObject> UDTList = new List<SchoolObject>();

        private string CadreConfig = "K12.Behavior.TheCadre.Config";

        // 入學照片
        Dictionary<string, string> _PhotoPDict = new Dictionary<string, string>();

        // 畢業照片
        Dictionary<string, string> _PhotoGDict = new Dictionary<string, string>();

        private string _ChancellorChineseName = "";

        //主文件
        private Document _doc;
        //單頁範本
        private Document _template;
        //移動使用
        private Run _run;

        //K12.Data.Configuration.ConfigData DateConfig;

        public CadreProveReport()
        {
            InitializeComponent();
        }

        private void StudentCadreProve_Load(object sender, EventArgs e)
        {
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            //DateConfig = K12.Data.School.Configuration["幹部模組_學生幹部證明單"];
            //if (!string.IsNullOrEmpty(DateConfig["校長姓名"]))
            //{
            //    lbChancellorChineseName.Text = DateConfig["校長姓名"];
            //}

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //取得設定檔
            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(CadreConfig);
            //畫面內容(範本內容,預設樣式
            Campus.Report.TemplateSettingForm TemplateForm;
            if (ConfigurationInCadre.Template != null)
            {
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.學生幹部證明單, Campus.Report.TemplateType.Word));
            }
            else
            {
                ConfigurationInCadre.Template = new Campus.Report.ReportTemplate(Properties.Resources.學生幹部證明單, Campus.Report.TemplateType.Word);
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.學生幹部證明單, Campus.Report.TemplateType.Word));
            }

            //預設名稱
            TemplateForm.DefaultFileName = "學生幹部證明單(範本)";
            //如果回傳為OK
            if (TemplateForm.ShowDialog() == DialogResult.OK)
            {
                //設定後樣試,回傳
                ConfigurationInCadre.Template = TemplateForm.Template;
                //儲存
                ConfigurationInCadre.Save();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;

            List<string> CadreTypeList = new List<string>();
            if (checkBoxX1.Checked)
                CadreTypeList.Add(checkBoxX1.Text);
            if (checkBoxX2.Checked)
                CadreTypeList.Add(checkBoxX2.Text);
            if (checkBoxX3.Checked)
                CadreTypeList.Add(checkBoxX3.Text);

            if (CadreTypeList.Count == 0)
            {
                MsgBox.Show("未選擇列印類別,無法列印資料!");
                btnPrint.Enabled = true;
                return;
            }

            //DateConfig["校長姓名"] = _ChancellorChineseName = lbChancellorChineseName.Text;
            //DateConfig.Save();
            BGW.RunWorkerAsync(CadreTypeList);
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 密技密技哀哀叫

            List<string> CadreTypeList = (List<string>)e.Argument;
            List<string> StudentIDList = K12.Presentation.NLDPanels.Student.SelectedSource;

            List<string> ids = new List<string>();
            foreach (string id in StudentIDList)
                ids.Add("'" + id + "'");

            List<string> Types = new List<string>();
            foreach (string id in CadreTypeList)
                Types.Add("'" + id + "'");

            string where = string.Format("StudentID in ({0}) and ReferenceType in ({1})", string.Join(",", ids.ToArray()), string.Join(",", Types.ToArray()));


            UDTList.Clear();
            UDTList = _accessHelper.Select<SchoolObject>(where);
            UDTList.Sort(new Comparison<SchoolObject>(SortUDT)); //排序

            #endregion

            _doc = new Document();
            _doc.Sections.Clear(); //清空此Document

            //取得設定檔
            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(CadreConfig);
            if (ConfigurationInCadre.Template == null)
            {
                //如果範本為空,則建立一個預設範本
                Campus.Report.ReportConfiguration ConfigurationInCadre_1 = new Campus.Report.ReportConfiguration(CadreConfig);
                ConfigurationInCadre_1.Template = new Campus.Report.ReportTemplate(Properties.Resources.學生幹部證明單, Campus.Report.TemplateType.Word);
                //ConfigurationInCadre_1.Template = new Campus.Report.ReportTemplate(Properties.Resources.社團點名表_合併欄位總表, Campus.Report.TemplateType.Word);
                _template = ConfigurationInCadre_1.Template.ToDocument();
            }
            else
            {
                //如果已有範本,則取得樣板
                _template = ConfigurationInCadre.Template.ToDocument();
            }

            //整理學生UDT資料
            Dictionary<string, List<SchoolObject>> StudentInfoList = new Dictionary<string, List<SchoolObject>>();
            List<string> StudentOKList = new List<string>();
            foreach (SchoolObject each in UDTList)
            {
                if (!StudentInfoList.ContainsKey(each.StudentID))
                {
                    StudentOKList.Add(each.StudentID);
                    StudentInfoList.Add(each.StudentID, new List<SchoolObject>());
                }

                StudentInfoList[each.StudentID].Add(each);
            }

            List<StudentRecord> StudList = Student.SelectByIDs(StudentInfoList.Keys);
            StudList.Sort(new Comparison<StudentRecord>(SortStudent));

            // 入學照片
            _PhotoPDict.Clear();
            _PhotoPDict = K12.Data.Photo.SelectFreshmanPhoto(StudentOKList);

            // 畢業照片
            _PhotoGDict.Clear();
            _PhotoGDict = K12.Data.Photo.SelectGraduatePhoto(StudentOKList);


            foreach (StudentRecord student in StudList)
            {
                #region MailMerge
                List<string> name = new List<string>();
                List<object> value = new List<object>();

                name.Add("學校名稱");
                value.Add(School.ChineseName);

                if (student.Class != null)
                {
                    name.Add("班級");
                    value.Add(student.Class.Name);
                }
                else
                {
                    name.Add("班級");
                    value.Add("");
                }

                name.Add("座號");
                value.Add(student.SeatNo.HasValue ? student.SeatNo.Value.ToString() : "");

                name.Add("姓名");
                value.Add(student.Name);

                name.Add("學號");
                value.Add(student.StudentNumber);

                name.Add("校長");
                if (K12.Data.School.Configuration["學校資訊"].PreviousData != null)
                {
                    if (K12.Data.School.Configuration["學校資訊"].PreviousData.SelectSingleNode("ChancellorChineseName") != null)
                    {
                        value.Add(K12.Data.School.Configuration["學校資訊"].PreviousData.SelectSingleNode("ChancellorChineseName").InnerText);
                    }
                    else
                    {
                        value.Add("");
                    }
                }
                else
                {
                    value.Add("");
                }

                name.Add("資料");
                if (StudentInfoList.ContainsKey(student.ID))
                    value.Add(StudentInfoList[student.ID]);
                else
                    value.Add(new List<SchoolObject>());

                if (_PhotoPDict.ContainsKey(student.ID))
                {
                    name.Add("新生照片1");
                    value.Add(_PhotoPDict[student.ID]);

                    name.Add("新生照片2");
                    value.Add(_PhotoPDict[student.ID]);
                }

                if (_PhotoGDict.ContainsKey(student.ID))
                {
                    name.Add("畢業照片1");
                    value.Add(_PhotoGDict[student.ID]);

                    name.Add("畢業照片2");
                    value.Add(_PhotoGDict[student.ID]);
                }

                //取得範本樣式
                Document PageOne = (Document)_template.Clone(true);

                PageOne.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
                PageOne.MailMerge.Execute(name.ToArray(), value.ToArray());
                #endregion

                _doc.Sections.Add(_doc.ImportNode(PageOne.FirstSection, true));

            }

            e.Result = _doc;
        }

        void MailMerge_MergeField(object sender, Aspose.Words.Reporting.MergeFieldEventArgs e)
        {
            if (e.FieldName == "新生照片1" || e.FieldName == "新生照片2")
            {
                if (!string.IsNullOrEmpty(e.FieldValue.ToString()))
                {
                    byte[] photo = Convert.FromBase64String(e.FieldValue.ToString()); //e.FieldValue as byte[];

                    if (photo != null && photo.Length > 0)
                    {
                        DocumentBuilder photoBuilder = new DocumentBuilder(e.Document);
                        photoBuilder.MoveToField(e.Field, true);
                        e.Field.Remove();
                        //Paragraph paragraph = photoBuilder.InsertParagraph();// new Paragraph(e.Document);
                        Shape photoShape = new Shape(e.Document, ShapeType.Image);
                        photoShape.ImageData.SetImage(photo);
                        photoShape.WrapType = WrapType.Inline;
                        //Cell cell = photoBuilder.CurrentParagraph.ParentNode as Cell;
                        //cell.CellFormat.LeftPadding = 0;
                        //cell.CellFormat.RightPadding = 0;
                        if (e.FieldName == "新生照片1")
                        {
                            // 1吋
                            photoShape.Width = ConvertUtil.MillimeterToPoint(25);
                            photoShape.Height = ConvertUtil.MillimeterToPoint(35);
                        }
                        else
                        {
                            //2吋
                            photoShape.Width = ConvertUtil.MillimeterToPoint(35);
                            photoShape.Height = ConvertUtil.MillimeterToPoint(45);
                        }
                        //paragraph.AppendChild(photoShape);
                        photoBuilder.InsertNode(photoShape);
                    }
                }
            }
            else if (e.FieldName == "畢業照片1" || e.FieldName == "畢業照片2")
            {
                if (!string.IsNullOrEmpty(e.FieldValue.ToString()))
                {
                    byte[] photo = Convert.FromBase64String(e.FieldValue.ToString()); //e.FieldValue as byte[];

                    if (photo != null && photo.Length > 0)
                    {
                        DocumentBuilder photoBuilder = new DocumentBuilder(e.Document);
                        photoBuilder.MoveToField(e.Field, true);
                        e.Field.Remove();
                        //Paragraph paragraph = photoBuilder.InsertParagraph();// new Paragraph(e.Document);
                        Shape photoShape = new Shape(e.Document, ShapeType.Image);
                        photoShape.ImageData.SetImage(photo);
                        photoShape.WrapType = WrapType.Inline;
                        //Cell cell = photoBuilder.CurrentParagraph.ParentNode as Cell;
                        //cell.CellFormat.LeftPadding = 0;
                        //cell.CellFormat.RightPadding = 0;
                        if (e.FieldName == "畢業照片1")
                        {
                            // 1吋
                            photoShape.Width = ConvertUtil.MillimeterToPoint(25);
                            photoShape.Height = ConvertUtil.MillimeterToPoint(35);
                        }
                        else
                        {
                            //2吋
                            photoShape.Width = ConvertUtil.MillimeterToPoint(35);
                            photoShape.Height = ConvertUtil.MillimeterToPoint(45);
                        }
                        //paragraph.AppendChild(photoShape);
                        photoBuilder.InsertNode(photoShape);
                    }
                }
            }
            else if (e.FieldName == "資料")
            {
                List<SchoolObject> records = e.FieldValue as List<SchoolObject>;
                records.Sort(SortSchoolObject);
                Document PageOne = e.Document; // (Document)_template.Clone(true);
                _run = new Run(PageOne);
                DocumentBuilder builder = new DocumentBuilder(PageOne);
                builder.MoveToMergeField("資料");
                ////取得目前Cell
                Cell cell = (Cell)builder.CurrentParagraph.ParentNode;
                ////取得目前Row
                Row row = (Row)builder.CurrentParagraph.ParentNode.ParentNode;

                //建立新行
                for (int x = 1; x < records.Count; x++)
                {
                    (cell.ParentNode.ParentNode as Table).InsertAfter(row.Clone(true), cell.ParentNode);
                }

                foreach (SchoolObject obj in records)
                {

                    List<string> list = new List<string>();
                    list.Add(obj.SchoolYear);
                    list.Add(obj.Semester);
                    list.Add(obj.ReferenceType);
                    list.Add(obj.CadreName);
                    list.Add(obj.Text);

                    foreach (string listEach in list)
                    {
                        Write(cell, listEach);

                        if (cell.NextSibling != null) //是否最後一格
                            cell = cell.NextSibling as Cell; //下一格
                    }

                    Row Nextrow = cell.ParentRow.NextSibling as Row; //取得下一個Row
                    if (Nextrow == null)
                        break;
                    cell = Nextrow.FirstCell; //第一格Cell 
                }
            }
            else
            {
                //...
            }
        }

        private int SortSchoolObject(SchoolObject r1, SchoolObject r2)
        {
            string school_1 = r1.SchoolYear.PadLeft(3, '0');
            school_1 += r1.Semester.PadLeft(1, '0');

            string school_2 = r2.SchoolYear.PadLeft(3, '0');
            school_2 += r2.Semester.PadLeft(1, '0');

            return school_1.CompareTo(school_2);
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Document inResult = (Document)e.Result;
            btnPrint.Enabled = true;

            try
            {
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();

                SaveFileDialog1.Filter = "Word (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                SaveFileDialog1.FileName = "學生幹部證明單";

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    inResult.Save(SaveFileDialog1.FileName);
                    Process.Start(SaveFileDialog1.FileName);
                    MotherForm.SetStatusBarMessage("學生幹部證明單,列印完成!!");
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("檔案未儲存");
                    return;
                }
            }
            catch
            {
                FISCA.Presentation.Controls.MsgBox.Show("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                MotherForm.SetStatusBarMessage("檔案儲存錯誤,請檢查檔案是否開啟中!!");
            }
        }

        private int SortUDT(SchoolObject x, SchoolObject y)
        {
            string schoolYearAndSemester1 = x.SchoolYear + x.Semester;
            string schoolYearAndSemester2 = y.SchoolYear + y.Semester;
            return schoolYearAndSemester1.CompareTo(schoolYearAndSemester2);
        }

        private int SortStudent(StudentRecord x, StudentRecord y)
        {
            string student1 = "";
            string student2 = "";

            if (x.Class != null)
            {
                student1 += x.Class.Name.PadLeft(8, '0');
            }
            else
            {
                student1 += "00000000";
            }

            student1 += x.SeatNo.HasValue ? x.SeatNo.Value.ToString().PadLeft(8, '0') : "00000000";

            if (y.Class != null)
            {
                student2 += y.Class.Name.PadLeft(8, '0');
            }
            else
            {
                student2 += "00000000";
            }

            student2 += y.SeatNo.HasValue ? y.SeatNo.Value.ToString().PadLeft(8, '0') : "00000000";

            return student1.CompareTo(student2);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 寫入資料
        /// </summary>
        private void Write(Cell cell, string text)
        {
            if (cell.FirstParagraph == null)
                cell.Paragraphs.Add(new Paragraph(cell.Document));
            cell.FirstParagraph.Runs.Clear();
            _run.Text = text;
            _run.Font.Size = 12;
            _run.Font.Name = "標楷體";
            cell.FirstParagraph.Runs.Add(_run.Clone(true));
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "另存新檔";
            sfd.FileName = "學生幹部證明單_合併欄位總表.doc";
            sfd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    fs.Write(Properties.Resources.合併欄位總表, 0, Properties.Resources.合併欄位總表.Length);
                    fs.Close();
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "另存檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}
