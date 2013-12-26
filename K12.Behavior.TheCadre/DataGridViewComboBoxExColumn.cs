using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace K12.Behavior.TheCadre
{
   partial class DataGridViewComboBoxExColumn : DataGridViewColumn
   {
       public List<string> Items { get; set; }

       public DataGridViewComboBoxExColumn()
           : base(new DataGridViewComboBoxExCell())
       {
           this.Items = new List<string>();
       }

       public override DataGridViewCell CellTemplate
       {
           get
           {
               return base.CellTemplate;
           }
           set
           {
               // Ensure that the cell used for the template is a CalendarCell.
               if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewComboBoxExCell)))
               {
                   throw new InvalidCastException("Must be a ComboBoxExCell");
               }
               base.CellTemplate = value;
           }
       }
   }

   public class DataGridViewComboBoxExCell : DataGridViewTextBoxCell
   {
       public DataGridViewComboBoxExCell()
           : base()
       {

       }

       public override void InitializeEditingControl(int rowIndex, object
           initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
       {
           // Set the value of the editing control to the current cell value.

           try
           {
               base.InitializeEditingControl(rowIndex, initialFormattedValue,
                   dataGridViewCellStyle);

               if (this.OwningColumn is DataGridViewComboBoxExColumn && this.DataGridView.EditingControl is ComboBoxEx)
               {
                   ((ComboBoxEx)this.DataGridView.EditingControl).DataSource =((DataGridViewComboBoxExColumn)this.OwningColumn).Items;
                   //因為 EditControl 不是 TextBox,DataGridViewTextBoxCell 不會把initialFormattedValue 指派給 Text 屬性
                   
                   ((IDataGridViewEditingControl)this.DataGridView.EditingControl).EditingControlFormattedValue= initialFormattedValue.ToString();
               }
           }
           catch (Exception e)
           {
               MessageBox.Show(e.Message);
           }
       }

       public override Type EditType
       {
           get
           {
               // Return the type of the editing contol that CalendarCell uses.
               return typeof(ComboBoxExEditingControl);
           }
       }

       public override Type ValueType
       {
           get
           {
               // Return the type of the value that CalendarCell contains.
               return typeof(string);
           }
       }

       public override object DefaultNewRowValue
       {
           get
           {
               // Use the current date and time as the default value.
               return string.Empty;
           }
       }
   }

   class ComboBoxExEditingControl : ComboBoxEx, IDataGridViewEditingControl
   {
       DataGridView dataGridView;
       private bool valueChanged = false;
       int rowIndex;

       public ComboBoxExEditingControl()
       {
           this.DropDownStyle = ComboBoxStyle.DropDown;
       }

       protected override void OnTextChanged(EventArgs e)
       {

           base.OnTextChanged(e);

           this.valueChanged = true;

           if (this.dataGridView != null)
               this.dataGridView.NotifyCurrentCellDirty(true);
       }

       // Implements theIDataGridViewEditingControl.EditingControlFormattedValue
       // property.
       public object EditingControlFormattedValue
       {
           get
           {
               return this.Text;
           }
           set
           {
               if (value == null)
                   this.ResetText();
               else
                   this.Text = value.ToString();
           }
       }

       // Implements the
       // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
       public object GetEditingControlFormattedValue(
           DataGridViewDataErrorContexts context)
       {
           return EditingControlFormattedValue;
       }

       // Implements the
       // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
       public void ApplyCellStyleToEditingControl(
           DataGridViewCellStyle dataGridViewCellStyle)
       {
           this.Font = dataGridViewCellStyle.Font;
           this.ForeColor = dataGridViewCellStyle.ForeColor;
           this.BackColor = dataGridViewCellStyle.BackColor;
       }

       // Implements the IDataGridViewEditingControl.EditingControlRowIndex
       // property.
       public int EditingControlRowIndex
       {
           get
           {
               return rowIndex;
           }
           set
           {
               rowIndex = value;
           }
       }

       // Implements theIDataGridViewEditingControl.EditingControlWantsInputKey
       // method.
       public bool EditingControlWantsInputKey(
           Keys key, bool dataGridViewWantsInputKey)
       {
           return !dataGridViewWantsInputKey;
       }

       // Implements theIDataGridViewEditingControl.PrepareEditingControlForEdit
       // method.
       public void PrepareEditingControlForEdit(bool selectAll)
       {
           if (selectAll)
               this.SelectAll();
       }

       // Implements the IDataGridViewEditingControl
       // .RepositionEditingControlOnValueChange property.
       public bool RepositionEditingControlOnValueChange
       {
           get
           {
               return false;
           }
       }

       // Implements the IDataGridViewEditingControl
       // .EditingControlDataGridView property.
       public DataGridView EditingControlDataGridView
       {
           get
           {
               return dataGridView;
           }
           set
           {
               dataGridView = value;
           }
       }

       // Implements the IDataGridViewEditingControl
       // .EditingControlValueChanged property.
       public bool EditingControlValueChanged
       {
           get
           {
               return valueChanged;
           }
           set
           {
               valueChanged = value;
           }
       }

       // Implements the IDataGridViewEditingControl
       // .EditingPanelCursor property.
       public Cursor EditingPanelCursor
       {
           get
           {
               return base.Cursor;
           }
       }

       protected override void OnSelectedValueChanged(EventArgs eventargs)
       {
           // Notify the DataGridView that the contents of the cell
           // have changed.
           valueChanged = true;
           this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
           base.OnSelectedValueChanged(eventargs);
       }
   }
}