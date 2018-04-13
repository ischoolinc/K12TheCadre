using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;

namespace K12.Behavior.TheCadre.CadreEdit
{
    public partial class SchoolYearSemesterEditForm : BaseForm
    {
        public int _schoolYear { get; set; }

        public int _semester { get; set; }

        public SchoolYearSemesterEditForm()
        {
            InitializeComponent();

            // Init
            schoolYearIP.Value = int.Parse("" + School.DefaultSchoolYear);
            semesterIP.Value = int.Parse("" + School.DefaultSemester);
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            _schoolYear = schoolYearIP.Value;
            _semester = semesterIP.Value;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
