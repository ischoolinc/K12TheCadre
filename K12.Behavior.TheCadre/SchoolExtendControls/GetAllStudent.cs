using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K12.Data;
using FISCA.Data;
using System.Data;

namespace K12.Behavior.TheCadre
{
    class GetAllStudent
    {
        Dictionary<string, Dictionary<string, Srecord>> dic = new Dictionary<string, Dictionary<string, Srecord>>();

        Dictionary<string, Srecord> StudentDic = new Dictionary<string, Srecord>();
        /// <summary>
        /// FISCA Query物件
        /// </summary>
        QueryHelper _queryhelper = new QueryHelper();

        public GetAllStudent()
        {
            string sqlStr2 = string.Format("select s1.id as student_id,s1.name as student_name,s1.seat_no as student_seatno,c1.class_name as classname from student s1,class c1 where s1.ref_class_id = c1.id and s1.status=1 and s1.seat_no is not null");
            //id
            //name
            //seatno
            //classname

            DataTable dt = _queryhelper.Select(sqlStr2);
            dic.Clear();
            StudentDic.Clear();
            foreach (DataRow row in dt.Rows)
            {
                Srecord s = new Srecord();
                s.StudedntID = "" + row[0];
                s.StudentName = "" + row[1];
                s.SeatNo = "" + row[2];
                s.ClassName = "" + row[3];

                //提供用ID查找學生
                if (!StudentDic.ContainsKey(s.StudedntID))
                {
                    StudentDic.Add(s.StudedntID, s);
                }

                //提供用班級/座號查找學生
                if (!dic.ContainsKey(s.ClassName))
                {
                    dic.Add(s.ClassName, new Dictionary<string, Srecord>());
                }

                if (!dic[s.ClassName].ContainsKey(s.SeatNo))
                {
                    dic[s.ClassName].Add(s.SeatNo, s);
                }
            }
        }

        public Srecord GetSrecord(string id)
        {
            if (StudentDic.ContainsKey(id))
            {
                return StudentDic[id];
            }
            return null;
        }

        public Srecord GetStudentBySeatNo(string ClassName, string SeatNo)
        {
            if (dic.ContainsKey(ClassName))
            {
                if (dic[ClassName].ContainsKey(SeatNo))
                {
                    return dic[ClassName][SeatNo];
                }
            }
            return null;
        }

        /// <summary>
        /// True為有此班級名稱
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public bool GetClassIsNull(string ClassName)
        {
            if (dic.ContainsKey(ClassName))
            {
                return true;
            }
            return false; ;
        }
    }

    class Srecord
    {
        public string StudedntID { get; set; }
        public string ClassName { get; set; }
        public string SeatNo { get; set; }
        public string StudentName { get; set; }
    }
}
