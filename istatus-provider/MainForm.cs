using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace istatus_provider
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Implicit cast from IStatusProvider to Control
            foreach (Control control in UserControls)
            {
                flowLayoutPanel.Controls.Add(control);
            }

            // IStatusProvider
            foreach (var statusProvider in UserControls)
            {
                statusProvider.StatusUpdated += onAnyStatusUpdated;
            }
        }

        private void onAnyStatusUpdated(object sender, EventArgs e)
        {
            foreach (var statusProvider in UserControls)
            {
                textBoxMultiline.AppendText($"{statusProvider.Status}{Environment.NewLine}");
            }
            textBoxMultiline.AppendText(Environment.NewLine);
        }

        IStatusProvider[] UserControls = new IStatusProvider[]
        {
            new UserControlTypeA(),
            new UserControlTypeB(),
            new UserControlTypeC(),
        };
    }
    interface IStatusProvider
    {
        string Status { get; }
        event EventHandler StatusUpdated;
    }
    class UserControlTypeA : UserControl, IStatusProvider
    {
        public UserControlTypeA()
        {
            _checkBox =
                new CheckBox
                {
                    Text = "CheckBox",
                    Size = new Size(150, 50),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Appearance = Appearance.Button,
                };
            _checkBox.CheckedChanged += (sender, e) => 
                StatusUpdated?.Invoke(this, EventArgs.Empty);
            Controls.Add(_checkBox);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }
        private readonly CheckBox _checkBox;

        public event EventHandler StatusUpdated;

        public string Status => $"{GetType().Name}: {_checkBox.Checked}";
    }
    class UserControlTypeB : UserControl, IStatusProvider
    {
        int _clickCount = 0;
        public UserControlTypeB()
        {
            _button =
                new Button
                {
                    Text = "Button",
                    Size = new Size(150, 50),
                };
            _button.Click += (sender, e) => 
                { _clickCount++; StatusUpdated?.Invoke(this, EventArgs.Empty); };
            Controls.Add(_button);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }
        private readonly Button _button;

        public event EventHandler StatusUpdated;

        public string Status => $"{GetType().Name}: {_clickCount} clicks";
    }
    class UserControlTypeC : UserControl, IStatusProvider
    {
        Color[] _colorArray =
            Enum.GetValues(typeof(KnownColor))
            .Cast<KnownColor>()
            .Select(known=>Color.FromKnownColor(known))
            .ToArray();
        int _index = 102;
        public UserControlTypeC()
        {
            _panel =
                new Panel
                {
                    Size = new Size(150, 50),
                    BackColor = Color.LightCyan,
                };
            _panel.Click += (sender, e) => 
                { _panel.BackColor = _colorArray[_index++]; StatusUpdated?.Invoke(this, EventArgs.Empty); };
            Controls.Add(_panel);
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
        }
        private Panel _panel;
        public event EventHandler StatusUpdated;
        public string Status => $"{GetType().Name}: {_panel.BackColor}";
    }
}
