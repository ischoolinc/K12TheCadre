using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using SmartSchool.API.PlugIn;

namespace K12.Behavior.TheCadre
{
    class ExportSchoolObject :SmartSchool.API.PlugIn.Export.Exporter 
    {
        private AccessHelper helper = new AccessHelper();

        //建構子
        public ExportSchoolObject()
        {
            this.Image = null;
            this.Text = "匯出擔任幹部記錄";
        }

        //覆寫
        public override void InitializeExport(SmartSchool.API.PlugIn.Export.ExportWizard wizard)
        {
            List<SchoolObject> allrecords = helper.Select<SchoolObject>();

            wizard.ExportableFields.AddRange("學年度", "學期", "幹部類別", "幹部名稱", "說明");

            wizard.ExportPackage += (sender,e)=>
            {
                List<SchoolObject> records = new List<SchoolObject>();

                records = allrecords.Where(x => e.List.Contains(x.StudentID)).ToList();

                for (int i = 0; i < records.Count; i++)
                {
                    RowData row = new RowData();
                    row.ID = records[i].StudentID;
                    foreach (string field in e.ExportFields)
                    {
                        if (wizard.ExportableFields.Contains(field))
                        {
                            switch (field)
                            {
                                case "學年度": row.Add(field, "" + records[i].SchoolYear); break;
                                case "學期": row.Add(field, "" + records[i].Semester); break;
                                case "幹部類別": row.Add(field, "" + records[i].ReferenceType); break;
                                case "幹部名稱": row.Add(field, "" + records[i].CadreName); break;
                                case "說明": row.Add(field, "" + records[i].Text); break;
                            }
                        }
                    }

                    e.Items.Add(row);
                }
            };
        }
    }
}
