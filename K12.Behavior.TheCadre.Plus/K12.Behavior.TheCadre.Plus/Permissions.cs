using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Permission;

namespace K12.Behavior.TheCadre.Plus
{
    /// <summary>
    /// 代表目前使用者的相關權限資訊。
    /// </summary>
    public static class Permissions
    {
       

        public static string 幹部登錄時間設定 { get { return "K12.Behavior.TheCadre.CadreInputDateForm"; } }

        public static bool 幹部登錄時間設定權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[幹部登錄時間設定].Executable;
                return check1;
            }
        }

    }
}
