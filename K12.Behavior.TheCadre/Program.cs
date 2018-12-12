using FISCA;
using FISCA.Presentation;
using K12.Data;
using FISCA.Presentation.Controls;
using FISCA.Permission;

namespace K12.Behavior.TheCadre
{
    public class Program
    {
        [MainMethod("K12.Behavior.TheCadre")]
        static public void Main()
        {

            ServerModule.AutoManaged("http://module.ischool.com.tw/module/138/Cadre_Behavior/udm.xml");

            //幹部資料項目
            FISCA.Permission.FeatureAce UserPermission;
            UserPermission = FISCA.Permission.UserAcl.Current[Permissions.幹部記錄];
            if (UserPermission.Editable || UserPermission.Viewable)
                K12.Presentation.NLDPanels.Student.AddDetailBulider(new FISCA.Presentation.DetailBulider<StudentCadreItem>());

            string URL班級幹部總表 = "ischool/幹部系統/共用/學務/班級/報表/班級幹部總表";
            string URL學校幹部總表 = "ischool/幹部系統/共用/學務/班級/報表/學校幹部總表";
            //URL
            string URL學生幹部證明單 = "ischool/幹部系統/共用/學務/學生/報表/幹部證明單";
            string URL班級幹部登錄 = "ischool/幹部系統/共用/學務/班級/班級幹部登錄";
            string URL匯出擔任幹部記錄 = "ischool/幹部系統/共用/學務/學生/匯出/擔任幹部紀錄";
            string URL匯入擔任幹部記錄 = "ischool/幹部系統/共用/學務/學生/匯入/擔任幹部紀錄";
            string URL幹部名稱管理 = "ischool/幹部系統/共用/學務/學務作業/幹部名稱管理";
            string URL學校幹部登錄 = "ischool/幹部系統/共用/學務/學務作業/學校幹部登錄";
            string URL幹部批次修改 = "ischool/幹部系統/共用/學務/學務作業/幹部批次修改";
            string URL幹部敘獎作業 = "ischool/幹部系統/共用/學務/學務作業/幹部敘獎作業";
            //註冊功能
            #region URL班級幹部總表
            FISCA.Features.Register(URL班級幹部總表, arg =>
            {
                if (K12.Presentation.NLDPanels.Class.SelectedSource.Count != 0)
                {
                    StudentLeadersSummaryTable StudentRW = new StudentLeadersSummaryTable();
                    StudentRW.ShowDialog();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇班級!");
                }
            });

            RibbonBarItem rbItem7 = K12.Presentation.NLDPanels.Class.RibbonBarItems["資料統計"];
            rbItem7["報表"]["學務相關報表"]["班級幹部總表"].Enable = Permissions.班級幹部總表權限;
            rbItem7["報表"]["學務相關報表"]["班級幹部總表"].Click += delegate
            {
                Features.Invoke(URL班級幹部總表);
            };
            #endregion

            #region URL學校幹部總表
            FISCA.Features.Register(URL學校幹部總表, arg =>
            {
                SchoolLeadersSummaryTable StudentRW = new SchoolLeadersSummaryTable();
                StudentRW.ShowDialog();
            });

            RibbonBarItem rbItem8 = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "資料統計"];
            rbItem8["報表"].Image = Properties.Resources.paste_64;
            rbItem8["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            rbItem8["報表"]["學校幹部總表"].Enable = Permissions.學校幹部總表權限;
            rbItem8["報表"]["學校幹部總表"].Click += delegate
            {
                Features.Invoke(URL學校幹部總表);
            };
            #endregion

            #region URL學生幹部證明單
            FISCA.Features.Register(URL學生幹部證明單, arg =>
            {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count != 0)
                {
                    CadreProveReport StudentRW = new CadreProveReport();
                    StudentRW.ShowDialog();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請選擇學生!");
                }
            });

            RibbonBarItem rbItem2 = K12.Presentation.NLDPanels.Student.RibbonBarItems["資料統計"];
            rbItem2["報表"]["學務相關報表"]["學生幹部證明單"].Enable = Permissions.學生幹部證明單權限;
            rbItem2["報表"]["學務相關報表"]["學生幹部證明單"].Click += delegate
            {
                Features.Invoke(URL學生幹部證明單);
            };
            #endregion

            RibbonBarItem rbItem3 = K12.Presentation.NLDPanels.Class.RibbonBarItems["學務"];

            #region URL班級幹部登錄
            FISCA.Features.Register(URL班級幹部登錄, arg =>
            {
                if (K12.Presentation.NLDPanels.Class.SelectedSource.Count == 1)
                {
                    ClassSpeedInsertBySeanNo CBC = new ClassSpeedInsertBySeanNo();
                    CBC.ShowDialog();
                }
                else if (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 1)
                {
                    MsgBox.Show("本功能僅提供對單一班級進行幹部登錄作業!");
                }
                else
                {
                    MsgBox.Show("請選擇一個班級!!");
                }
            });

            rbItem3["班級幹部登錄"].Enable = false;
            rbItem3["班級幹部登錄"].Image = Properties.Resources.stamp_paper_fav_128;
            rbItem3["班級幹部登錄"].Size = RibbonBarButton.MenuButtonSize.Medium;
            rbItem3["班級幹部登錄"].Click += delegate
            {
                Features.Invoke(URL班級幹部登錄);
            };
            #endregion

            #region URL匯出擔任幹部記錄
            FISCA.Features.Register(URL匯出擔任幹部記錄, arg =>
            {
                SmartSchool.API.PlugIn.Export.Exporter exporter = new ExportSchoolObject();
                ExportStudentV2 wizard = new ExportStudentV2(exporter.Text, exporter.Image);
                exporter.InitializeExport(wizard);
                wizard.ShowDialog();
            });

            MenuButton rbItemExport = K12.Presentation.NLDPanels.Student.RibbonBarItems["資料統計"]["匯出"]["學務相關匯出"];
            rbItemExport["匯出擔任幹部記錄"].Enable = Permissions.匯出擔任幹部記錄權限;
            rbItemExport["匯出擔任幹部記錄"].Click += delegate
            {
                Features.Invoke(URL匯出擔任幹部記錄);
            };
            #endregion

            #region URL匯入擔任幹部記錄
            FISCA.Features.Register(URL匯入擔任幹部記錄, arg =>
            {
                SmartSchool.API.PlugIn.Import.Importer importer = new ImportSchoolObject();
                ImportStudentV2 wizard = new ImportStudentV2(importer.Text, importer.Image);
                importer.InitializeImport(wizard);
                wizard.ShowDialog();
            });

            MenuButton rbItemImport = K12.Presentation.NLDPanels.Student.RibbonBarItems["資料統計"]["匯入"]["學務相關匯入"];
            rbItemImport["匯入擔任幹部記錄"].Enable = Permissions.匯入擔任幹部記錄權限;
            rbItemImport["匯入擔任幹部記錄"].Click += delegate
            {
                Features.Invoke(URL匯入擔任幹部記錄);
            };
            #endregion

            #region URL幹部名稱管理
            FISCA.Features.Register(URL幹部名稱管理, arg =>
             {
                 NewCadreSetup cs1 = new NewCadreSetup();
                 cs1.ShowDialog();
             });

            RibbonBarItem RibbonItem = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "基本設定"];
            RibbonItem["管理"]["幹部名稱管理"].Enable = Permissions.幹部名稱管理權限;
            RibbonItem["管理"]["幹部名稱管理"].Click += delegate
            {
                Features.Invoke(URL幹部名稱管理);
            };
            #endregion

            #region URL學校幹部登錄
            FISCA.Features.Register(URL學校幹部登錄, arg =>
            {
                SchoolSpeedInsertByClassSeanNo cs1 = new SchoolSpeedInsertByClassSeanNo();
                cs1.ShowDialog();
            });

            RibbonBarItem RibbonSpeedInsert = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "批次作業/查詢"];
            RibbonSpeedInsert["學校幹部登錄"].Image = Properties.Resources.stamp_paper_fav_128;
            RibbonSpeedInsert["學校幹部登錄"].Enable = Permissions.學校幹部登錄權限;
            RibbonSpeedInsert["學校幹部登錄"].Click += delegate
            {
                Features.Invoke(URL學校幹部登錄);
            };
            #endregion

            //事件
            K12.Presentation.NLDPanels.Class.SelectedSourceChanged += delegate
            {
                rbItem3["班級幹部登錄"].Enable = (Permissions.班級幹部登錄權限 && (K12.Presentation.NLDPanels.Class.SelectedSource.Count == 1));
                rbItem7["報表"]["學務相關報表"]["班級幹部總表"].Enable = (Permissions.班級幹部總表權限 && (K12.Presentation.NLDPanels.Class.SelectedSource.Count >= 1));
            };

            #region 幹部批次修改
            FISCA.Features.Register(URL幹部批次修改, arg =>
            {
                (new CadreEdit.CadreEditForm()).ShowDialog();
            });
            RibbonSpeedInsert["幹部批次修改"].Enable = Permissions.幹部批次修改權限;
            RibbonSpeedInsert["幹部批次修改"].Image = Properties.Resources.niche_fav_64;
            RibbonSpeedInsert["幹部批次修改"].Click += delegate
            {
                Features.Invoke(URL幹部批次修改);
            };
            #endregion

            #region 幹部敘獎
            FISCA.Features.Register(URL幹部敘獎作業 , arg  =>
            {
                (new CadreMeritManage.CadreMeritManage()).ShowDialog();
            });
            RibbonSpeedInsert["幹部敘獎作業"].Enable = Permissions.幹部敘獎作業權限;
            RibbonSpeedInsert["幹部敘獎作業"].Image = Properties.Resources.diplom_fav_64;
            RibbonSpeedInsert["幹部敘獎作業"].Click += delegate
            {
                Features.Invoke(URL幹部敘獎作業);
            };

            #endregion
           
            #region 權限控管

            Catalog detail2;

            detail2 = RoleAclSource.Instance["學生"]["資料項目"];
            detail2.Add(new DetailItemFeature(Permissions.幹部記錄, "幹部記錄"));

            detail2 = RoleAclSource.Instance["學生"]["報表"];
            detail2.Add(new ReportFeature(Permissions.學生幹部證明單, "學生幹部證明單"));

            detail2 = RoleAclSource.Instance["學生"]["功能按鈕"];
            detail2.Add(new RibbonFeature(Permissions.匯出擔任幹部記錄, "匯出擔任幹部記錄"));

            detail2 = RoleAclSource.Instance["學生"]["功能按鈕"];
            detail2.Add(new RibbonFeature(Permissions.匯入擔任幹部記錄, "匯入擔任幹部記錄"));

            detail2 = RoleAclSource.Instance["班級"]["功能按鈕"];
            //detail2.Add(new ReportFeature(Permissions.班級幹部管理, "班級幹部管理"));
            detail2.Add(new ReportFeature(Permissions.班級幹部登錄, "班級幹部登錄"));

            detail2 = RoleAclSource.Instance["班級"]["報表"];
            detail2.Add(new ReportFeature(Permissions.班級幹部總表, "班級幹部總表"));

            detail2 = RoleAclSource.Instance["學務作業"];
            detail2.Add(new ReportFeature(Permissions.幹部名稱管理, "幹部名稱管理"));
            detail2.Add(new ReportFeature(Permissions.學校幹部登錄, "學校幹部登錄"));
            detail2.Add(new ReportFeature(Permissions.學校幹部總表, "學校幹部總表"));
            detail2.Add(new ReportFeature(Permissions.幹部批次修改, "幹部批次修改"));
            detail2.Add(new ReportFeature(Permissions.幹部敘獎作業, "幹部敘獎作業"));
            #endregion

        }
    }
}