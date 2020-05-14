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
using System.Windows.Shapes;
using static CoordinationTraining.Controls.CustomTact;

namespace CoordinationTraining.Controls.MiniNotes
{
    public partial class SelectNoteType : Window
    {
        CustomTactForAutoPlay_mini _losc;

        public SelectNoteType()
        {
            InitializeComponent();
        }

        public SelectNoteType(CustomTactForAutoPlay_mini ll)
        {
            _losc = ll;
            InitializeComponent();
        }
        private void lbNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lbNotes.SelectedItem != null)
            {
                _losc._type = (TactType)(lbNotes.SelectedIndex + 1);
                Close();
            }
        }
    }
}
