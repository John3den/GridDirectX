using DevExpress.XtraEditors;
using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridRender
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public RenderLoopControl loopControl;
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hScrollBar1_Click(object sender, EventArgs e)
        {

        }

        private void hScrollBar3_Click(object sender, EventArgs e)
        {

        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void trackBarControl2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            loopControl.Grid.ChangeProperty(labelControl3, true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            loopControl.Grid.ChangeProperty(labelControl3, false);
        }
        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }
    }
}