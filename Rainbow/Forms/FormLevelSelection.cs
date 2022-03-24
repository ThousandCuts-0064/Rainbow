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
        private IColorModel _colorModel = new RGB();
        private int _selectedLevel = 0;

        public FormLevelSelection() => InitializeComponent();

        private void FormLevelSelection_Load(object sender, EventArgs e) => CenterToParent();
        private void ButtonLevelClicked(object sender, EventArgs e) => _selectedLevel = int.Parse(((Control)sender).Text.Split(' ').Last());

        private void radioButtonRGB_CheckedChanged(object sender, EventArgs e) => _colorModel = new RGB();
        private void radioButtonRYB_CheckedChanged(object sender, EventArgs e) => _colorModel = new RYB();
        private void radioButtonCMY_CheckedChanged(object sender, EventArgs e) => _colorModel = new CMY();

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (_selectedLevel == 0) return;
            //FormPlay.Owner = this.Owner
            new FormPlay(_colorModel, _selectedLevel) { Owner = Owner }.Show();
            Close();
        }
    }
}
