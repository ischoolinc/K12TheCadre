using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K12.Behavior.TheCadre.Plus
{
    public class Program
    {
        [MainMethod("K12.Behavior.TheCadre.Plus")]
        static public void Main()
        {
            string URL幹部登錄時間設定 = "ischool/幹部系統/共用/學務/學務作業/幹部登錄時間設定";

            //New

            FISCA.Features.Register(URL幹部登錄時間設定, arg =>
            {
                CadreInputDateForm cdf = new CadreInputDateForm();
                cdf.ShowDialog();

            });

            RibbonBarItem RibbonSpeedInsert = FISCA.Presentation.MotherForm.RibbonBarItems["學務作業", "幹部作業"];
            RibbonSpeedInsert["幹部登錄時間設定"].Image = Properties.Resources.chief_of_staff_clock_64;
            RibbonSpeedInsert["幹部登錄時間設定"].Enable = Permissions.幹部登錄時間設定權限;
            RibbonSpeedInsert["幹部登錄時間設定"].Click += delegate
            {
                Features.Invoke(URL幹部登錄時間設定);
            };

            Catalog detail2 = RoleAclSource.Instance["學務作業"];
            detail2.Add(new ReportFeature(Permissions.幹部登錄時間設定, "幹部登錄時間設定"));

        }
    }
}