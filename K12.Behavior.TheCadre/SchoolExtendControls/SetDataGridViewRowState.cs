using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace K12.Behavior.TheCadre
{
    class SetDataGridViewRowState
    {
        //設定DataGridViewROW的目前狀態
        //包含學生不存在
        //DataGridView應該設定正確的狀態

        /// <summary>
        /// 設定DataGridViewRow為初始化狀態
        /// </summary>
        /// <param name="row"></param>
        public void IsNewRow(DataGridViewRow row)
        {
            row.Tag = null; //學生Record
            row.Cells[1].Tag = null; //幹部UDT資料
            row.Cells[1].Value = ""; //班級
            row.Cells[2].Value = ""; //學生姓名
            row.Cells[3].Value = ""; //座號

            row.Cells[1].ErrorText = ""; //座號
            row.Cells[2].ErrorText = ""; //座號
            row.Cells[3].ErrorText = ""; //座號
        }

        /// <summary>
        /// 當DataGridViewRow班級資料輸入錯誤
        /// </summary>
        public void IsErrorRow(DataGridViewRow row)
        {
            row.Tag = null; //學生Record
            row.Cells[1].Tag = null; //幹部UDT資料
            row.Cells[1].Value = ""; //學生姓名
            //row.Cells[2].Value = ""; //班級名稱錯誤不清空
            row.Cells[3].Value = ""; //座號

            row.Cells[1].ErrorText = ""; //姓名
            //row.Cells[2].ErrorText = ""; //班級
            row.Cells[3].ErrorText = ""; //座號
        }
    }
}
