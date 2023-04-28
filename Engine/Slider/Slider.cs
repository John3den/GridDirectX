using TrackBar = DevExpress.XtraEditors.TrackBarControl;
using CheckBox = DevExpress.XtraEditors.CheckEdit;
using GridRender;

namespace Engine
{
    public class Slider
    {
        bool isEnabledX = true;
        bool isEnabledY = true;
        bool isEnabledZ = true;
        bool isFullGrid = true;
        int currentX = 3;
        int currentY = 3;
        int currentZ = 3;
        TrackBar scrollX;
        TrackBar scrollY;
        TrackBar scrollZ;
        CheckBox CheckX;
        CheckBox CheckY;
        CheckBox CheckZ;
        CheckBox CheckFull;
        public Slider(XtraForm1 form,Grid grid)
        {
            scrollX = form.GetTrackBar1();
            scrollY = form.GetTrackBar2();
            scrollZ = form.GetTrackBar3();
            CheckX = form.GetCheck1();
            CheckY = form.GetCheck2();
            CheckZ = form.GetCheck3();
            CheckFull = form.GetFullCheck();

            scrollX.Properties.Maximum = grid.size.x - 1;
            scrollY.Properties.Maximum = grid.size.y - 1;
            scrollZ.Properties.Maximum = grid.size.z - 1;
        }
        public bool Includes(int x, int y, int z)
        {
            return (isFullGrid) || ((z == currentZ && isEnabledZ) || (y == currentY && isEnabledY) || (x == currentX && isEnabledX));
        }
        public void Update()
        {
            currentX = scrollX.Value;
            currentY = scrollY.Value;
            currentZ = scrollZ.Value;
            isEnabledX = CheckX.Checked;
            isEnabledY = CheckY.Checked;
            isEnabledZ = CheckZ.Checked;
            isFullGrid = CheckFull.Checked;
        }
    }
}