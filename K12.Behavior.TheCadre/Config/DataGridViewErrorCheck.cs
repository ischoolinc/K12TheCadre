using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Aspose.Cells;

namespace K12.Behavior.TheCadre
{
    class DataGridViewErrorCheck
    {

        /// <summary>
        /// 檢查Cell內容是否有輸入文字,未輸入文字將提示錯誤訊息,需輸入錯誤訊息
        /// </summary>
        public bool CheckCellIsEmptyError(DataGridViewCell cell, string ErrorText)
        {
            if (string.IsNullOrEmpty(("" + cell.Value).Trim()))
            {
                cell.ErrorText = ErrorText;
                return true;
            }
            else
            {
                cell.ErrorText = "";
                return false;
            } 
        }

        /// <summary>
        /// 檢查Cell內容是否有輸入文字,並修改欄位為黃色警告
        /// </summary>
        public bool CheckCellIsEmptyWarning(DataGridViewCell cell)
        {
            if (string.IsNullOrEmpty(("" + cell.Value).Trim()))
            {
                cell.Style.BackColor = Color.Yellow;
                return true;
            }
            else
            {
                cell.Style.BackColor = Color.White;
                return false;
            } 
        }

        /// <summary>
        /// 欄位內容是否為數字,需輸入錯誤訊息
        /// </summary>
        public bool CheckCellIsInt(DataGridViewCell cell,string ErrorText)
        {
            int IntName;
            if (!int.TryParse(("" + cell.Value).Trim(), out IntName))
            {
                cell.ErrorText = ErrorText;
                return true;
            }
            else
            {
                cell.ErrorText = "";
                return false;
            }
        }

        /// <summary>
        /// 欄位內容是否為數字,空白為正確內容
        /// </summary>
        public bool CheckCellIsIntEmpty(DataGridViewCell cell, string ErrorText)
        {
            int IntName;
            if (string.IsNullOrEmpty("" + cell.Value))
            {
                cell.ErrorText = "";
                return false;
            }
            else if (!int.TryParse("" + cell.Value, out IntName))
            {
                cell.ErrorText = ErrorText;
                return true;
            }
            else
            {
                cell.ErrorText = "";
                return false;
            }
        }

        /// <summary>
        /// 欄位內容是否為數字
        /// </summary>
        public bool CheckCellIsInt(DataGridViewCell cell)
        {
            int IntName;
            if (int.TryParse(("" + cell.Value).Trim(), out IntName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 欄位內容是否為數字,回傳數字(錯誤為0)(Aspose)
        /// </summary>
        public int CheckAsposeNotInt(Cell CellObj)
        {
            int CellValue = 0;

            //檢查IsNullOrEmpty
            if (!string.IsNullOrEmpty(CellObj.StringValue))
            {
                //檢查是否為數字
                int intString;
                if (int.TryParse(CellObj.StringValue, out intString))
                {
                    CellValue = intString;
                }
            }

            return CellValue;
        }

        /// <summary>
        /// 欄位內容是否為數字,回傳數字(錯誤為0)(DataGridView)
        /// </summary>
        public int CheckDataGridViewNotInt(DataGridViewCell CellObj)
        {
            int CellValue = 0;

            //檢查IsNullOrEmpty
            if (!string.IsNullOrEmpty("" + CellObj.Value))
            {
                //檢查是否為數字
                int intString;
                if (int.TryParse("" + CellObj.Value, out intString))
                {
                    CellValue = intString;
                }
            }

            return CellValue;
        }

        /// <summary>
        /// 欄位內容是否為數字,回傳數字(錯誤預設為1)(DataGridView)
        /// </summary>
        public int CheckNotIntNumber(DataGridViewCell CellObj)
        {
            int CellValue = 1;

            //檢查IsNullOrEmpty
            if (!string.IsNullOrEmpty("" + CellObj.Value))
            {
                //檢查是否為數字
                int intString;
                if (int.TryParse("" + CellObj.Value, out intString))
                {
                    CellValue = intString;
                }
            }

            return CellValue;
        }

        /// <summary>
        /// 欄位內容是否為數字,回傳數字(錯誤預設為1)(DataGridView)
        /// </summary>
        public int CheckNotIntNumber_1(Cell CellObj)
        {
            int CellValue = 1;

            //檢查IsNullOrEmpty
            if (!string.IsNullOrEmpty("" + CellObj.Value))
            {
                //檢查是否為數字
                int intString;
                if (int.TryParse("" + CellObj.Value, out intString))
                {
                    CellValue = intString;
                }
            }

            return CellValue;
        }

        /// <summary>
        /// 3個欄位相加是否為0
        /// </summary>
        public bool CheckRowTotalEqualToZero(DataGridViewRow row, int Index1, int Index2, int Index3, string ErrorText)
        {
            //是否為數字

            #region 相加是否為零
            string Cell1 = "" + row.Cells[Index1].Value;
            string Cell2 = "" + row.Cells[Index2].Value;
            string Cell3 = "" + row.Cells[Index3].Value;
            int a = 0;
            int b = 0;
            int c = 0;
            int.TryParse(Cell1, out a);
            int.TryParse(Cell2, out b);
            int.TryParse(Cell3, out c);

            if (a + b + c == 0)
            {
                row.Cells[Index1].ErrorText = ErrorText;
                row.Cells[Index2].ErrorText = ErrorText;
                row.Cells[Index3].ErrorText = ErrorText;
                return true;
            }
            else
            {
                row.ErrorText = "";
                return false;
            }
            #endregion
        }

        public bool CheckCadreType(DataGridViewCell cell, string ErrorText)
        {
            if (string.IsNullOrEmpty("" + cell.Value))
            {
                cell.ErrorText = ErrorText;
                return true;
            }
            else
            {
                cell.ErrorText = "";
                return false;
            }
        }
    }
}
