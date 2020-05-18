using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CoordinationTraining.Controls.CustomTact;

namespace CoordinationTraining.Controls.MiniNotes
{
    public partial class CustomTactSelected : UserControl
    {
        public TactType _type = TactType.TT_SIXTEEN;
        public CustomTactSelected()
        {
            InitializeComponent();

            ct.SetTact(_type);
        }

        private void SelectedNote_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectNoteType snt = new SelectNoteType(this);
            snt.ShowDialog();
            ct.SetTact(_type);
        }
    }
}
