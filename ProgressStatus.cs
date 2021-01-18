using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayerViewer
{
    public partial class ProgressStatus : Form
    {
        public ProgressStatus()
        {
            InitializeComponent();
        }
        public void Update(int value)
        {
            this.progressBar.Value = value;
        }
    }
}
