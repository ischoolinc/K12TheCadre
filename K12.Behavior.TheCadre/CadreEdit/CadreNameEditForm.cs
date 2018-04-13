using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;

namespace K12.Behavior.TheCadre.CadreEdit
{
    public partial class CadreNameEditForm : BaseForm
    {
        public string _cadreName { get; set; }

        public CadreNameEditForm(string cardType)
        {
            InitializeComponent();
            // Init CardNameCbx
            AccessHelper access = new AccessHelper();
            List<ClassCadreNameObj> cadreList = access.Select<ClassCadreNameObj>("NameType = " + "'" + cardType + "'");

            foreach (ClassCadreNameObj cadre in cadreList)
            {
                cadreNameCbx.Items.Add(cadre.CadreName);
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            _cadreName = cadreNameCbx.Text;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
