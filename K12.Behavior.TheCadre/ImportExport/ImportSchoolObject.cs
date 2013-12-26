using System.Collections.Generic;
using System.Windows.Forms;
using FISCA.UDT;
using SmartSchool.API.PlugIn;

namespace K12.Behavior.TheCadre
{
    class ImportSchoolObject : SmartSchool.API.PlugIn.Import.Importer
    {
        AccessHelper helper = new AccessHelper();
        List<string> Types = new List<string>() {"社團幹部","學校幹部","班級幹部"};
        List<string> Keys = new List<string>();

        public ImportSchoolObject()
        {
            this.Image = null;
            this.Text = "匯入擔任幹部記錄";
        }

        public override void InitializeImport(SmartSchool.API.PlugIn.Import.ImportWizard wizard)
        {
            wizard.PackageLimit = 3000;
            //可匯入的欄位
            wizard.ImportableFields.AddRange("學年度", "學期", "幹部類別", "幹部名稱", "說明");
            //必需要有的欄位
            wizard.RequiredFields.AddRange("學年度", "學期", "幹部類別", "幹部名稱");
            //驗證開始事件
            //wizard.ValidateStart += (sender, e) => Keys.Clear();
            //驗證每行資料的事件
            wizard.ValidateRow += new System.EventHandler<SmartSchool.API.PlugIn.Import.ValidateRowEventArgs>(wizard_ValidateRow);
            //實際匯入資料的事件
            wizard.ImportPackage += new System.EventHandler<SmartSchool.API.PlugIn.Import.ImportPackageEventArgs>(wizard_ImportPackage);
            //匯入完成
            wizard.ImportComplete += (sender, e) => MessageBox.Show("匯入完成!");
        }

        void wizard_ValidateRow(object sender, SmartSchool.API.PlugIn.Import.ValidateRowEventArgs e)
        {
            #region 驗各欄位填寫格式
            int t;
            foreach (string field in e.SelectFields)
            {
                string value = e.Data[field];
                switch (field)
                {
                    default:
                        break;
                    case "學年度":
                        if (value == "" || !int.TryParse(value, out t))
                            e.ErrorFields.Add(field, "此欄為必填欄位，必須填入整數。");
                        break;
                    case "學期":
                        if (value == "" || !int.TryParse(value, out t))
                        {
                            e.ErrorFields.Add(field, "此欄為必填欄位，必須填入整數。");
                        }
                        else if (t != 1 && t != 2)
                        {
                            e.ErrorFields.Add(field, "必須填入1或2");
                        }
                        break;
                    case "幹部名稱":
                        if (string.IsNullOrEmpty(value))
                            e.ErrorFields.Add(field, "此欄為必填欄位。");
                        break;
                    case "幹部類別":
                        if (!Types.Contains(value))
                            e.ErrorFields.Add(field, "幹部類別必須為社團幹部、學校幹部或班級幹部");
                        break;
                }
            }
            #endregion
            #region 驗證主鍵
            string Key = e.Data.ID + "-" + e.Data["學年度"] + "-" + e.Data["學期"] + "-" + e.Data["幹部類別"] + "-" + e.Data["幹部名稱"];
            string errorMessage = string.Empty;

            if (Keys.Contains(Key))
                errorMessage = "學生編號、學年度、學期、幹部類別及幹部名稱的組合不能重覆!";
            else
                Keys.Add(Key);

            e.ErrorMessage = errorMessage;

            #endregion
        }

        void wizard_ImportPackage(object sender, SmartSchool.API.PlugIn.Import.ImportPackageEventArgs e)
        {
            List<ActiveRecord> InsertRecords = new List<ActiveRecord>();

            List<ActiveRecord> UpdateRecords = new List<ActiveRecord>();

            foreach (RowData Row in e.Items)
            {
                string strCondition = "StudentID='" + Row.ID + "' and SchoolYear='" + Row["學年度"] + "' and Semester='" + Row["學期"] + "' and ReferenceType='" + Row["幹部類別"] + "' and CadreName='" + Row["幹部名稱"] + "'";

                List<SchoolObject> records = helper.Select<SchoolObject>(strCondition);

                if (records.Count > 0)
                {
                    if (Row.ContainsKey("說明"))
                    {
                        records[0].Text = Row["說明"];
                        UpdateRecords.Add(records[0]);
                    }
                }
                else
                {
                    SchoolObject record = new SchoolObject();

                    record.StudentID = Row.ID;
                    record.SchoolYear = Row["學年度"];
                    record.Semester = Row["學期"];
                    record.ReferenceType = Row["幹部類別"];
                    record.CadreName = Row["幹部名稱"];

                    if (Row.ContainsKey("說明"))
                        record.Text = Row["說明"];

                    InsertRecords.Add(record);
                }
            }

            if (InsertRecords.Count > 0)
                helper.InsertValues(InsertRecords);

            if (UpdateRecords.Count > 0)
                helper.UpdateValues(UpdateRecords);
        }
    }
}