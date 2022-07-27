using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace istatus_provider
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(_panel);
            foreach (Control control in UserControls)
            {
                _panel.Controls.Add(control);
            } 
        }
        FlowLayoutPanel _panel;
        IStatusProvider[] UserControls = new []
        {
            new UserControlTypeA(),
        };
    }
    interface IStatusProvider
    {
        public string Status { get; }
    }
    class UserControlTypeA : UserControl, IStatusProvider
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if(!DesignMode || _isHandleInitialized)
            {
                _isHandleInitialized = true;
                Controls.Add(
                    new Button
                    {
                        Text = "Button",
                        Size = new Size(150, 50),
                    }
                );
            }
        }
        bool _isHandleInitialized = false;
        public string Status => GetType().Name;
    }
}
