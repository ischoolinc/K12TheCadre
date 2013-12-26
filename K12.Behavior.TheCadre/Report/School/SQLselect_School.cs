using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    class SQLselect_School
    {
        public Dictionary<string, StudentCadre_School> _StudentObjDic = new Dictionary<string, StudentCadre_School>();

        private List<string> StudentIDList = new List<string>();
        private Dictionary<string, string> CadreSortDic = new Dictionary<string, string>();

        /// <summary>
        /// 幹部資料
        /// </summary>
        public List<SchoolObject> CadreList;

        /// <summary>
        /// 取得全校學生資料
        /// </summary>
        public SQLselect_School(slsTConfig_School config)
        {

            #region 取得學生擔任幹部資料
            AccessHelper _accessHelper = new AccessHelper();

            CadreList = new List<SchoolObject>();

            StringBuilder sb = new StringBuilder();

            //條件式 - 學年度/學期/學生/幹部類型
            sb.Append(string.Format("SchoolYear in ('{0}') and Semester in('{1}') ", config.SchoolYear.ToString(), config.Semester.ToString()));

            sb.Append("and ReferenceType in ('學校幹部') ");

            CadreList = _accessHelper.Select<SchoolObject>(sb.ToString());
            #endregion

            #region 取得幹部設定檔,取得排序依據
            List<ClassCadreNameObj> ClassCadreNameList = _accessHelper.Select<ClassCadreNameObj>();

            foreach (ClassCadreNameObj each in ClassCadreNameList)
            {
                if (!CadreSortDic.ContainsKey(each.NameType + each.CadreName))
                {
                    CadreSortDic.Add(each.NameType + each.CadreName, each.NameType + each.Index.ToString().PadLeft(4, '0'));
                }
            }
            #endregion

            #region 取得學生清單
            List<string> list = new List<string>();

            foreach (SchoolObject each in CadreList)
            {
                if (!list.Contains(each.StudentID))
                {
                    list.Add(each.StudentID);
                }
            }
            #endregion

            FISCA.Data.QueryHelper _queryHelper = new FISCA.Data.QueryHelper();
            //篩選學生資料
            StringBuilder sb1 = new StringBuilder();
            //取得學生與班級Table
            sb1.Append("select class.id as class_id,class.class_name,student.id as student_id,student.name,student.student_number,student.seat_no,class.display_order ");

            //Join班級與學生
            sb1.Append("from student left join class on student.ref_class_id=class.id ");

            sb1.Append(string.Format("where student.id in('{0}') ", string.Join("','", list.ToArray())));

            //排序(年級,班級順序,班級名稱,座號,姓名)排序
            sb1.Append("order by class.grade_year,class.display_order,class.class_name,student.seat_no,student.name");

            DataTable dt = _queryHelper.Select(sb1.ToString());
            foreach (DataRow row in dt.Rows)
            {
                StudentCadre_School sc = new StudentCadre_School(row);

                if (!_StudentObjDic.ContainsKey(sc.Student_ID))
                {
                    _StudentObjDic.Add(sc.Student_ID, sc);
                }
            }

            //排序
            CadreList.Sort(SortCadre);
        }

        private int SortCadre(SchoolObject a1, SchoolObject b1)
        {
            StudentCadre_School sc1 = _StudentObjDic[a1.StudentID];

            string aa2 = sc1.Class_display_order.PadLeft(4, '0');
            aa2 += sc1.Class_Name.PadLeft(4, '0');
            aa2 += sc1.Student_SeatNo.PadLeft(4, '0');
            string aa1 = "矓矓矓矓" + 9999;
            if (CadreSortDic.ContainsKey(a1.ReferenceType + a1.CadreName))
            {
                aa1 = CadreSortDic[a1.ReferenceType + a1.CadreName];
            }
            aa1 += aa2;

            StudentCadre_School sc2 = _StudentObjDic[b1.StudentID];
            string bb2 = sc2.Class_display_order.PadLeft(4, '0');
            bb2 += sc2.Class_Name.PadLeft(4, '0');
            bb2 += sc2.Student_SeatNo.PadLeft(4, '0');
            string bb1 = "矓矓矓矓" + 9999;

            if (CadreSortDic.ContainsKey(b1.ReferenceType + b1.CadreName))
            {
                bb1 = CadreSortDic[b1.ReferenceType + b1.CadreName];
            }
            bb1 += bb2;
            return aa1.CompareTo(bb1);
        }
    }

    class StudentCadre_School
    {
        /// <summary>
        /// 班級系統編號
        /// </summary>
        public string Class_ID { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string Class_Name { get; set; }

        /// <summary>
        /// 班級排序
        /// </summary>
        public string Class_display_order { get; set; }

        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string Student_ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Student_Name { get; set; }

        /// <summary>
        /// 座號
        /// </summary>
        public string Student_SeatNo { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        public string Student_Number { get; set; }

        public StudentCadre_School(DataRow row)
        {
            Class_ID = "" + row["class_id"];
            Class_Name = "" + row["class_name"];
            Student_ID = "" + row["student_id"];
            Student_Name = "" + row["name"];
            Student_SeatNo = "" + row["seat_no"];
            Student_Number = "" + row["student_number"];
            Class_display_order = "" + row["display_order"];
        }
    }
}
