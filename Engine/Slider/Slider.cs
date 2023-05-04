using TrackBar = DevExpress.XtraEditors.TrackBarControl;
using CheckBox = DevExpress.XtraEditors.CheckEdit;
using GridRender;

namespace Engine
{
    public class Slider
    {
        bool _isEnabledX = true;
        bool _isEnabledY = true;
        bool _isEnabledZ = true;
        bool _isFullGrid = true;
        int _currentX = 3;
        int _currentY = 3;
        int _currentZ = 3;
        TrackBar _scrollX;
        TrackBar _scrollY;
        TrackBar _scrollZ;
        CheckBox _CheckX;
        CheckBox _CheckY;
        CheckBox _CheckZ;
        CheckBox _CheckFull;
        public Slider(XtraForm1 form,Grid grid)
        {
            _scrollX = form.GetTrackBar1();
            _scrollY = form.GetTrackBar2();
            _scrollZ = form.GetTrackBar3();
            _CheckX = form.GetCheck1();
            _CheckY = form.GetCheck2();
            _CheckZ = form.GetCheck3();
            _CheckFull = form.GetFullCheck();

            _scrollX.Properties.Maximum = grid.GetSize().x - 1;
            _scrollY.Properties.Maximum = grid.GetSize().y - 1;
            _scrollZ.Properties.Maximum = grid.GetSize().z - 1;
        }
        public bool IncludesCell(int x, int y, int z)
        {
            return (_isFullGrid) || ((z == _currentZ && _isEnabledZ) || (y == _currentY && _isEnabledY) || (x == _currentX && _isEnabledX));
        }
        public void Update()
        {
            _currentX = _scrollX.Value;
            _currentY = _scrollY.Value;
            _currentZ = _scrollZ.Value;
            _isEnabledX = _CheckX.Checked;
            _isEnabledY = _CheckY.Checked;
            _isEnabledZ = _CheckZ.Checked;
            _isFullGrid = _CheckFull.Checked;
        }
    }
}