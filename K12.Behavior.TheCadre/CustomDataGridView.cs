using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms;

namespace K12.Behavior.TheCadre
{
    public class CustomDataGridView : DataGridViewX
    {
        #region 此類別CustomDataGridView,繼承DataGridViewX物件,並改寫其內容,將Enter=Tab

        protected override bool ProcessDialogKey(Keys keyData)
        {
            // Extract the key code from the key value. 
            Keys key = (keyData & Keys.KeyCode);

            // Handle the ENTER key as if it were a RIGHT ARROW key. 
            if (key == Keys.Enter)
            {
                return this.ProcessTabKey(keyData);
            }
            return base.ProcessDialogKey(keyData);

        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {

            // Handle the ENTER key as if it were a RIGHT ARROW key. 
            if (e.KeyCode == Keys.Enter)
            {
                return this.ProcessTabKey(e.KeyData);
            }
            return base.ProcessDataGridViewKey(e);

        }
        #endregion
    }
}