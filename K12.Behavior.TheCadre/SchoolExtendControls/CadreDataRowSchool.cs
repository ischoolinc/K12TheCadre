using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K12.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace K12.Behavior.TheCadre
{
    class CadreDataRowSchool:System.ComponentModel.INotifyPropertyChanged
    {
  
        private string _care_name;

        private int _DefSchoolYear { get; set; }
        private int _DefSemester { get; set; }

        /// <summary>
        /// 幹部名稱
        /// </summary>
        public string _CadreName
        {
            get { return _care_name; }
            set
            {
                _care_name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("_CadreName"));
            }
        }

        private string _student_name;

        /// <summary>
        /// 學生姓名
        /// </summary>
        public string _StudentName
        {
            get { return _student_name; }
            set
            {
                _student_name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("_StudentName"));
            }
        }

        private string _student_class_name;

        /// <summary>
        /// 取得班級名稱
        /// </summary>
        public string _StudentClassName
        {
            get { return _student_class_name; }
            set
            {
                //無錯誤內容
                ClassNameError = string.Empty;
                _student_class_name = value;
                //輸入班級名稱
                if (string.IsNullOrWhiteSpace(_StudentClassName))
                {
                    _student_class_name = string.Empty;
                    ClassNameError = string.Empty;
                    SeatNoError = string.Empty;
                    _StudentName = string.Empty;
                    _StudentSeatNo = string.Empty;
                    _SRecord = null;
                    if (_CadreRecord != null && _CadreRecord.UID != "")
                        _CadreRecordDel = _CadreRecord;
                    _CadreRecord = null; //清空原Record
                }
                else
                {
                    //是否有班級名稱
                    if (_Context.GetStudent.GetClassIsNull(_student_class_name))
                    {
                        //有此班級名稱,可清空錯誤訊息
                        ClassNameError = string.Empty; //班級錯誤
                        SeatNoError = string.Empty; //座號錯誤
                        _StudentName = string.Empty; //學生姓名
                        _StudentSeatNo = string.Empty; //學生座號
                        _SRecord = null;

                        if (_CadreRecord != null && _CadreRecord.UID != "")
                            _CadreRecordDel = _CadreRecord;
                        _CadreRecord = null; //清空原Record
                    }
                    else
                    {
                        _StudentName = string.Empty;
                        _SRecord = null;

                        ClassNameError = "查無此班級!!";
                        //如果幹部記錄不為空,且幹部ID不為空,此幹部記錄需列入刪除內容
                        if (_CadreRecord != null && _CadreRecord.UID != "")
                            _CadreRecordDel = _CadreRecord;
                        _CadreRecord = null; //清空原Record
                    }
                }
            }
        }

        private string _student_seat_no;
        /// <summary>
        /// 學生座號
        /// </summary>
        public string _StudentSeatNo
        {
            //取得內部座號
            get { return _student_seat_no; }
            set
            {
                SeatNoError = string.Empty;
                //輸入座號時,需要有班級名稱,否則資料將會為錯誤

                _student_seat_no = value; //填值

                //如果是空白或多空格或是null,清空資料
                if (string.IsNullOrWhiteSpace(_student_seat_no))
                    _student_seat_no = string.Empty;

                //如果班級不為空值
                if (!string.IsNullOrEmpty(_student_class_name))
                {
                    //如果座號不為空值
                    if (!string.IsNullOrEmpty(_student_seat_no))
                    {
                        Srecord s = _Context.GetStudent.GetStudentBySeatNo(_student_class_name,_student_seat_no);
                        if (s == null)
                        {
                            ReStart("本班級,查無此學生");
                        }
                        else
                        {
                            _StudentName = s.StudentName;
                            _SRecord = s;

                            //從刪除狀態內找是否為原有資料
                            //且學生ID相同,即為原有幹部記錄
                            if (_CadreRecordDel != null && _CadreRecordDel.StudentID == s.StudedntID)
                            {
                                _CadreRecord = _CadreRecordDel;
                                _CadreRecordDel = null;
                            }
                            else
                            {
                                if (_CadreRecord != null && _CadreRecord.UID != "")
                                    _CadreRecordDel = _CadreRecord;
                                _CadreRecord = new SchoolObject();
                                _CadreRecord.CadreName = _CadreName;
                                _CadreRecord.ReferenceType = "學校幹部"; //本功能寫死為班級幹部記錄
                                _CadreRecord.SchoolYear = _DefSchoolYear.ToString(); //預設學年度
                                _CadreRecord.Semester = _DefSemester.ToString(); //預設學期
                                _CadreRecord.StudentID = s.StudedntID; //學生ID

                                //原本 - 學校幹部不輸入內容
                                //因采威 抓取資料是依據註解內的班級名稱
                                //因此解除此限制
                                //2021/3/26 - By Dylan
                                _CadreRecord.Text = _student_class_name;
                            }
                        }
                    }
                    else //如果沒有這名學生
                    {
                        ReStart("本班級,查無此學生");
                    } 
                }
                else //沒有輸入班級資料
                {
                    if (!string.IsNullOrEmpty(_student_seat_no))
                    {
                        ReStart("您僅輸入座號\n因無班級資訊無法查找學生!!");
                    }
                }
            }
        }

        public void ReStart(string Message)
        {
            _StudentName = "";
            _SRecord = null;
            SeatNoError = Message;
            if (_CadreRecord != null && _CadreRecord.UID != "")
                _CadreRecordDel = _CadreRecord;
            _CadreRecord = null;
        }

        public string SeatNoError { get; set; }

        public string ClassNameError { get; set; }

        /// <summary>
        /// 班級幹部Record
        /// </summary>
        public SchoolObject _CadreRecord { get; set; }

        public SchoolObject _CadreRecordDel { get; set; }

        /// <summary>
        /// 學生Record
        /// </summary>
        public Srecord _SRecord { get; set; }

        private SchoolSpeedInsertByClassSeanNo _Context { get; set; }

        /// <summary>
        /// 幹部排序
        /// </summary>
        public int _index { get; set; }

        /// <summary>
        /// 未傳入任何參數
        /// </summary>
        public CadreDataRowSchool(string CadreName, int index, SchoolSpeedInsertByClassSeanNo Context, int DefSchoolYear, int DefSemester)
        {
            _DefSchoolYear = DefSchoolYear;
            _DefSemester = DefSemester;

            _Context = Context;
            _CadreName = CadreName;
            _index = index;

            _StudentName = "";
            _student_seat_no = "";
            _student_class_name = "";
        }

        /// <summary>
        /// 傳入(學生/幹部/Index)記錄
        /// </summary>
        /// <param name="stud"></param>
        /// <param name="obj"></param>
        public CadreDataRowSchool(Srecord stud, SchoolObject obj, int index, SchoolSpeedInsertByClassSeanNo Context, int DefSchoolYear, int DefSemester)
        {
            _DefSchoolYear = DefSchoolYear;
            _DefSemester = DefSemester;

            _Context = Context;
            _SRecord = stud;
            _StudentName = _SRecord.StudentName;
            _student_seat_no = _SRecord.SeatNo;
            _student_class_name = _SRecord.ClassName;

            _CadreRecord = obj;
            _CadreName = _CadreRecord.CadreName;

            _index = index;
        }       

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
