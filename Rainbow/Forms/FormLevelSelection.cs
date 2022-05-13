using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormLevelSelection : Form
    {
        private IColorModel _colorModel = null;
        private GameModifiers _gameModifiers = GameModifiers.None;
        private int _selectedLevel = 0;

        public FormLevelSelection() => InitializeComponent();

        private void FormLevelSelection_Load(object sender, EventArgs e) => CenterToParent();

        private void RadioButtonsLevel_CheckedChanged(object sender, EventArgs e)
        {
            var button = (RadioButton)sender;
            if (button.Checked)
                _selectedLevel = int.Parse(button.Text.Split(' ').Last());
        }

        private void RadioButtonColorModel_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked) return;

            if (radioButtonRGB.Checked)
                _colorModel = new RGB();
            else if (radioButtonCMY.Checked)
                _colorModel = new CMY();
            else if (radioButtonARC.Checked)
                _colorModel = new ARC();
            else if (radioButtonOSV.Checked)
                _colorModel = new OSV();
        }

        private void CheckBoxGameModifier_CheckedChanged(object sender, EventArgs e) =>
            _gameModifiers ^= (GameModifiers)Enum.Parse(
                typeof(GameModifiers),
                string.Concat(((Control)sender).Text.Split(' ')));

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (_selectedLevel == 0 || _colorModel == null) return;
            if (_selectedLevel < 2 && _gameModifiers.HasAnyFlag(
                GameModifiers.DoubleTiles | GameModifiers.ChessEvent)) return;
            if (_selectedLevel < 3 && _gameModifiers.HasAnyFlag(
                GameModifiers.TripleTiles | GameModifiers.DiamondEvent)) return;
            if (_selectedLevel < 6 && _gameModifiers.HasAnyFlag(
                GameModifiers.DiamondEvent)) return;
            
            new FormPlay(_colorModel, _gameModifiers, _selectedLevel) 
            {
                //FormPlay.Owner = this.Owner
                Owner = Owner
            }.Show();
            Close();
        }
    }
}
