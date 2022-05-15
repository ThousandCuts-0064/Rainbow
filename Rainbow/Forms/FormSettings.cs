using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow
{
    public partial class FormSettings : Form
    {
        private const string ON = "On";
        private const string OFF = "Off";
        private static readonly TextRenderingHint[] _textRenderingHints;
        private static Settings _settingsOld = DefaultSettings;
        private readonly FormPause _formPause;
        private Settings _settingsNew;
        public static Settings DefaultSettings { get; } = new Settings()
        {
            AntiAliasing = false,
            PixelOffset = false,
            TextRenderingHint = TextRenderingHint.SystemDefault
        };

        public static event Action<Settings> SettingsChanged;

        static FormSettings()
        {
            SettingsChanged += settings => _settingsOld = settings;
            _textRenderingHints = Enum.GetValues(typeof(TextRenderingHint)).Cast<TextRenderingHint>().ToArray();
        }

        public FormSettings(FormPause formPause)
        {
            InitializeComponent();
            _formPause = formPause;
            Owner = formPause;
            _settingsNew = _settingsOld;
            VisualizeSettings();
        }

        private void FormSettings_Load(object sender, EventArgs e) => CenterToParent();

        private void ButtonAntiAliasing_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                case MouseButtons.Left:
                    _settingsNew.AntiAliasing ^= true;
                    buttonAntiAliasing.Text = BoolToText(_settingsNew.AntiAliasing);
                    break;

                case MouseButtons.Middle:
                    _settingsNew.AntiAliasing = DefaultSettings.AntiAliasing;
                    buttonAntiAliasing.Text = BoolToText(_settingsNew.AntiAliasing);
                    break;
            }
        }

        private void ButtonPixelOffset_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                case MouseButtons.Left:
                    _settingsNew.PixelOffset ^= true;
                    buttonPixelOffset.Text = BoolToText(_settingsNew.PixelOffset);
                    break;

                case MouseButtons.Middle:
                    _settingsNew.PixelOffset = DefaultSettings.PixelOffset;
                    buttonPixelOffset.Text = BoolToText(_settingsNew.PixelOffset);
                    break;
            }
        }

        private void ButtonTextRenderingHint_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _settingsNew.TextRenderingHint =
                        _settingsNew.TextRenderingHint == _textRenderingHints.Last()
                            ? _settingsNew.TextRenderingHint = _textRenderingHints.First()
                            : _settingsNew.TextRenderingHint + 1;
                    buttonTextRenderingHint.Text = EnumToText(_settingsNew.TextRenderingHint);
                    break;

                case MouseButtons.Right:
                    _settingsNew.TextRenderingHint =
                        _settingsNew.TextRenderingHint == _textRenderingHints.First()
                            ? _settingsNew.TextRenderingHint = _textRenderingHints.Last()
                            : _settingsNew.TextRenderingHint - 1;
                    buttonTextRenderingHint.Text = EnumToText(_settingsNew.TextRenderingHint);
                    break;

                case MouseButtons.Middle:
                    _settingsNew.TextRenderingHint = DefaultSettings.TextRenderingHint;
                    buttonTextRenderingHint.Text = EnumToText(_settingsNew.TextRenderingHint);
                    break;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if (_settingsNew != _settingsOld)
                SettingsChanged(_settingsNew);
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            _settingsNew = _settingsOld;
            VisualizeSettings();
        }

        private void ButtonDefault_Click(object sender, EventArgs e)
        {
            _settingsNew = DefaultSettings;
            VisualizeSettings();
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Close();
            _formPause.Show();
        }

        private void FormSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                _formPause.Close();
        }

        private void VisualizeSettings()
        {
            buttonAntiAliasing.Text = BoolToText(_settingsNew.AntiAliasing);
            buttonPixelOffset.Text = BoolToText(_settingsNew.PixelOffset);
            buttonTextRenderingHint.Text = EnumToText(_settingsNew.TextRenderingHint);
        }

        private string BoolToText(bool @bool) => @bool ? ON : OFF;

        private string EnumToText(Enum @enum)
        {
            List<char> chars = @enum.ToString().ToList();
            for (int i = 1; i < chars.Count; i++)
            {
                if (!char.IsUpper(chars[i])) continue;

                chars.Insert(i, ' ');
                i++;
            }
            return new string(chars.ToArray());
        }
    }
}
