using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Permission;

namespace K12.Behavior.TheCadre
{
    /// <summary>
    /// 代表目前使用者的相關權限資訊。
    /// </summary>
    public static class Permissions
    {

        public static string 幹部記錄 { get { return "K12.Student.TheCadre.Detail00040"; } }
        //public static string 幹部記錄_國中 { get { return "Behavior.TheCadre.Detail00040"; } }

        public static string 學生幹部證明單 { get { return "K12.Student.TheCadre.Report00060"; } }
        //public static string 學生幹部證明單_國中 { get { return "Behavior.TheCadre.Report00060"; } }

        public static string 班級幹部總表 { get { return "K12.class.TheCadre.Report00060.5"; } }

        public static string 學校幹部總表 { get { return "K12.class.TheCadre.Report00060.8"; } }

        public static string 班級幹部管理 { get { return "K12.class.TheCadre.Report00070"; } }
        //public static string 班級幹部管理_國中 { get { return "Behavior.TheCadre.Report00070"; } }

        public static string 班級幹部登錄 { get { return "K12.class.TheCadre.Report00070.1"; } }
        //public static string 班級幹部登錄_國中 { get { return "Behavior.TheCadre.Report00070.1"; } }

        public static string 匯出擔任幹部記錄 { get { return "K12.Student.Student.Ribbon0167"; } }
        //public static string 匯出擔任幹部記錄_國中 { get { return "JHSchool.Student.Ribbon0167"; } }

        public static string 匯入擔任幹部記錄 { get { return "K12.Student.Student.Ribbon0168"; } }
        //public static string 匯入擔任幹部記錄_國中 { get { return "JHSchool.Student.Ribbon0168"; } }

        public static string 幹部名稱管理 { get { return "K12.class.TheCadre.Report00070.5"; } }
        //public static string 幹部名稱管理_國中 { get { return "Behavior.TheCadre.Report00070.5"; } }

        public static string 學校幹部登錄 { get { return "K12.class.TheCadre.Report00080"; } }
        //public static string 學校幹部登錄_國中 { get { return "Behavior.TheCadre.Report00080"; } }

        public static bool 學校幹部總表權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[學校幹部總表].Executable;
                return check1;
            }
        }

        public static bool 班級幹部總表權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[班級幹部總表].Executable;
                return check1;
            }
        }

        public static bool 學校幹部登錄權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[學校幹部登錄].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[學校幹部登錄_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 幹部名稱管理權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[幹部名稱管理].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[幹部名稱管理_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 幹部記錄權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[幹部記錄].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[幹部記錄_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 學生幹部證明單權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[學生幹部證明單].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[學生幹部證明單_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 班級幹部管理權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[班級幹部管理].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[班級幹部管理_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 班級幹部登錄權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[班級幹部登錄].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[班級幹部登錄_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 匯出擔任幹部記錄權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[匯出擔任幹部記錄].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[匯出擔任幹部記錄_國中].Executable;
                return check1;// || check2;
            }
        }

        public static bool 匯入擔任幹部記錄權限
        {
            get
            {
                bool check1 = FISCA.Permission.UserAcl.Current[匯入擔任幹部記錄].Executable;
                //bool check2 = FISCA.Permission.UserAcl.Current[匯入擔任幹部記錄_國中].Executable;
                return check1;// || check2;
            }
        }

    }
}
