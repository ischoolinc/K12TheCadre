using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FISCA.UDT;

namespace K12.Behavior.TheCadre
{
    class SQLselect
    {
        public Dictionary<string, List<StudentCadre>> _StudentObjDic = new Dictionary<string, List<StudentCadre>>();

        public Dictionary<string, string> _TeacherNameDic = new Dictionary<string, string>();
        private List<string> StudentIDList = new List<string>();
        private Dictionary<string, string> CadreSortDic = new Dictionary<string, string>();
        /// <summary>
        /// 傳入班級清單,以取得學生資料
        /// </summary>
        public SQLselect(List<string> ClassIDList)
        {
            FISCA.Data.QueryHelper _queryHelper = new FISCA.Data.QueryHelper();
            //篩選學生資料
            StringBuilder sb = new StringBuilder();
            //取得學生與班級Table
            sb.Append("select class.id as class_id,class.class_name,student.id as student_id,student.name,student.student_number,student.seat_no from student,class ");
            //Join班級與學生
            sb.Append("where student.ref_class_id = class.id ");
            //狀態為一般
            sb.Append("and student.status = 1 ");
            //所選班級
            sb.Append(string.Format("and class.id in('{0}') ", string.Join("','", ClassIDList)));
            //排序(年級,班級順序,班級名稱,座號,姓名)排序
            sb.Append("order by class.grade_year,class.display_order,class.class_name,student.seat_no,student.name");

            DataTable dt = _queryHelper.Select(sb.ToString());
            foreach (DataRow row in dt.Rows)
            {
                StudentCadre sc = new StudentCadre(row);

                if (!_StudentObjDic.ContainsKey(sc.Class_Name))
                {
                    _StudentObjDic.Add(sc.Class_Name, new List<StudentCadre>());
                }

                _StudentObjDic[sc.Class_Name].Add(sc);

                //所有學生清單(取得幹部資料用)
                if (!StudentIDList.Contains(sc.Student_ID))
                {
                    StudentIDList.Add(sc.Student_ID);
                }
            }

            string setTeacher = "select class.class_name,teacher.teacher_name from class,teacher where class.ref_teacher_id = teacher.id";
            DataTable dTeacher = _queryHelper.Select(setTeacher);
            foreach (DataRow row in dTeacher.Rows)
            {
                string ClassName = "" + row["class_name"];
                string TeacheName = "" + row["teacher_name"];
                if (!_TeacherNameDic.ContainsKey(ClassName))
                {
                    _TeacherNameDic.Add(ClassName, TeacheName);
                }
            }
        }

        /// <summary>
        /// 依據設定進行UDT資料取得
        /// </summary>
        /// <param name="config"></param>
        public void AccessUDTData(slsTConfig config)
        {
            //取得學生擔任幹部資料
            AccessHelper _accessHelper = new AccessHelper();

            List<SchoolObject> CadreList = new List<SchoolObject>();

            StringBuilder sb = new StringBuilder();

            if (!config.PrintAllSchoolYear)
            {
                //條件式 - 學年度/學期/學生/幹部類型

                sb.Append(string.Format("SchoolYear in ('{0}') and Semester in('{1}') ", config.SchoolYear.ToString(), config.Semester.ToString()));

                List<string> list = new List<string>();
                if (config.CheckClassCadre)
                    list.Add("班級幹部");

                if (config.CheckSchoolCadre)
                    list.Add("學校幹部");

                if (config.CheckAndCadre)
                    list.Add("社團幹部");

                sb.Append("and ReferenceType in ('" + string.Join("','", list) + "') ");

                sb.Append("and StudentID in ('" + string.Join("','", StudentIDList) + "')");

                CadreList = _accessHelper.Select<SchoolObject>(sb.ToString());
            }
            else
            {
                List<string> list = new List<string>();
                if (config.CheckClassCadre)
                    list.Add("班級幹部");

                if (config.CheckSchoolCadre)
                    list.Add("學校幹部");

                if (config.CheckAndCadre)
                    list.Add("社團幹部");

                sb.Append("ReferenceType in ('" + string.Join("','", list) + "') ");

                sb.Append("and StudentID in ('" + string.Join("','", StudentIDList) + "')");
                //列印所有學年期
                CadreList = _accessHelper.Select<SchoolObject>(sb.ToString());
            }

            foreach (SchoolObject each in CadreList)
            {
                bool IsTrue = false; //是否找到學生

                foreach (string className in _StudentObjDic.Keys)
                {
                    if (IsTrue) //前一個迴圈有找到學生,則跳出
                        break;

                    foreach (StudentCadre card in _StudentObjDic[className])
                    {
                        if (card.Student_ID != each.StudentID)
                            continue;

                        card.CadreList.Add(each);
                        IsTrue = true;
                        break;

                    }

                }
            }

            //取得幹部設定檔,取得排序依據
            List<ClassCadreNameObj> ClassCadreNameList = _accessHelper.Select<ClassCadreNameObj>();

            foreach (ClassCadreNameObj each in ClassCadreNameList)
            {
                if (!CadreSortDic.ContainsKey(each.NameType + each.CadreName))
                {
                    CadreSortDic.Add(each.NameType + each.CadreName, each.NameType + each.Index.ToString().PadLeft(4, '0'));
                }
            }

            foreach (string className in _StudentObjDic.Keys)
            {
                foreach (StudentCadre card in _StudentObjDic[className])
                {
                    card.CadreList.Sort(SortCadre);
                }
            }

        }

        private int SortCadre(SchoolObject a1, SchoolObject b1)
        {
            string aa2 = a1.SchoolYear.PadLeft(4, '0');
            aa2 += a1.Semester.PadLeft(4, '0');
            string aa1 = "矓矓矓矓" + 9999;
            if (CadreSortDic.ContainsKey(a1.ReferenceType + a1.CadreName))
            {
                aa1 = CadreSortDic[a1.ReferenceType + a1.CadreName];
            }
            aa2 += aa1;

            string bb2 = b1.SchoolYear.PadLeft(4, '0');
            bb2 += b1.Semester.PadLeft(4, '0');
            string bb1 = "矓矓矓矓" + 9999;

            if (CadreSortDic.ContainsKey(b1.ReferenceType + b1.CadreName))
            {
                bb1 = CadreSortDic[b1.ReferenceType + b1.CadreName];
            }
            bb2 += bb1;
            return aa2.CompareTo(bb2);
        }
    }

    class StudentCadre
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

        /// <summary>
        /// 學生所擔任之幹部清單
        /// </summary>
        public List<SchoolObject> CadreList { get; set; }

        public StudentCadre(DataRow row)
        {
            Class_ID = "" + row["class_id"];
            Class_Name = "" + row["class_name"];
            Student_ID = "" + row["student_id"];
            Student_Name = "" + row["name"];
            Student_SeatNo = "" + row["seat_no"];
            Student_Number = "" + row["student_number"];
            CadreList = new List<SchoolObject>();
        }
    }
}
