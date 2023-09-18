using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K12.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace K12.Behavior.TheCadre
{
    class CadreDataRow:System.ComponentModel.INotifyPropertyChanged
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

        private string _student_seat_no;
        /// <summary>
        /// 學生座號
        /// </summary>
        public string _StudentSeatNo
        {
            get { return _student_seat_no; }
            set
            {
                Error = string.Empty;

                _student_seat_no = value;

                if (string.IsNullOrWhiteSpace(_student_seat_no))
                    _student_seat_no = string.Empty;

                if (_Context._SeatNoDic.ContainsKey(_student_seat_no))
                {
                    _StudentName = _Context._SeatNoDic[_student_seat_no].Name;
                    _StudentRecord = _Context._SeatNoDic[_student_seat_no];

                    //從刪除狀態內找是否為原有資料
                    //且學生ID相同,即為原有幹部記錄
                    if (_CadreRecordDel != null && _CadreRecordDel.StudentID == _StudentRecord.ID)
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
                        _CadreRecord.ReferenceType = "班級幹部"; //本功能寫死為班級幹部記錄
                        _CadreRecord.SchoolYear = _DefSchoolYear.ToString(); //學年度
                        _CadreRecord.Semester = _DefSemester.ToString(); //學期
                        _CadreRecord.StudentID = _StudentRecord.ID; //學生ID
                        _CadreRecord.Text = _Context._classRecord.Name; //班級名稱
                    }
                }
                else if (string.IsNullOrEmpty(_student_seat_no))//如果沒有這名學生
                {
                    _StudentName = "";
                    _StudentRecord = null;

                    if (_CadreRecord != null && _CadreRecord.UID != "")
                        _CadreRecordDel = _CadreRecord;

                    _CadreRecord = null;
                }
                else
                {
                    _StudentName = "";
                    _StudentRecord = null;

                    Error = "本班級,查無此學生";

                    if (_CadreRecord != null && _CadreRecord.UID != "")
                        _CadreRecordDel = _CadreRecord;

                    _CadreRecord = null;
                }

            }
        }

        public string Error { get; set; }

        /// <summary>
        /// 班級幹部Record
        /// </summary>
        public SchoolObject _CadreRecord { get; set; }

        public SchoolObject _CadreRecordDel { get; set; }

        /// <summary>
        /// 學生Record
        /// </summary>
        public StudentRecord _StudentRecord { get; set; }

        private ClassSpeedInsertBySeanNo _Context { get; set; }

        /// <summary>
        /// 幹部排序
        /// </summary>
        public int _index { get; set; }

        /// <summary>
        /// 未傳入任何參數
        /// </summary>
        public CadreDataRow(string CadreName, int index, ClassSpeedInsertBySeanNo Context,int DefSchoolYear,int DefSemester)
        {
            _DefSchoolYear = DefSchoolYear;
            _DefSemester = DefSemester;

            _Context = Context;
            _CadreName = CadreName;
            _index = index;

            _StudentName = "";
            _student_seat_no = ""; 
        }

        /// <summary>
        /// 傳入(學生/幹部/Index)記錄
        /// </summary>
        /// <param name="stud"></param>
        /// <param name="obj"></param>
        public CadreDataRow(StudentRecord stud, SchoolObject obj, int index, ClassSpeedInsertBySeanNo Context, int DefSchoolYear, int DefSemester)
        {
            _DefSchoolYear = DefSchoolYear;
            _DefSemester = DefSemester;

            _Context = Context;
            _StudentRecord = stud;
            _StudentName = _StudentRecord.Name;
            _student_seat_no = _StudentRecord.SeatNo.HasValue ? _StudentRecord.SeatNo.Value.ToString() : "";

            _CadreRecord = obj;
            _CadreName = _CadreRecord.CadreName;

            _index = index;
        }       

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
