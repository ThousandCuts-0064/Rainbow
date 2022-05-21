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
        private const string _fileName = nameof(Settings) + ".dat";
        private const string ON = "On";
        private const string OFF = "Off";
        private static readonly TextRenderingHint[] _textRenderingHints;
        private static Settings SettingsOld
        {
            get => (Settings)Save.Deserialize(_fileName);
            set => Save.Serialize(_fileName, value);
        }
        private Settings _settingsNew;
        public static Settings SettingsDefault { get; } = new Settings()
        {
            AntiAliasing = false,
            PixelOffset = false,
            TextRenderingHint = TextRenderingHint.SystemDefault
        };

        public static event Action<Settings> SettingsChanged;

        static FormSettings()
        {
            if (!Save.FileExists(_fileName)) SettingsOld = SettingsDefault;
            SettingsChanged += settings => SettingsOld = settings;
            _textRenderingHints = Enum.GetValues(typeof(TextRenderingHint)).Cast<TextRenderingHint>().ToArray();
        }

        public FormSettings(FormPause formPause)
        {
            InitializeComponent();
            Owner = formPause;
            _settingsNew = SettingsOld;
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
                    _settingsNew.AntiAliasing = SettingsDefault.AntiAliasing;
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
                    _settingsNew.PixelOffset = SettingsDefault.PixelOffset;
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
                    _settingsNew.TextRenderingHint = SettingsDefault.TextRenderingHint;
                    buttonTextRenderingHint.Text = EnumToText(_settingsNew.TextRenderingHint);
                    break;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e) => 
            SettingsChanged(_settingsNew);

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            _settingsNew = SettingsOld;
            VisualizeSettings();
        }

        private void ButtonDefault_Click(object sender, EventArgs e)
        {
            _settingsNew = SettingsDefault;
            VisualizeSettings();
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void FormSettings_KeyDown(object sender, KeyEventArgs e) => ButtonBack_Click(sender, e);
            // Alternative:
            //if (e.KeyCode == Keys.Escape)
            //    Owner.Close();
            

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
