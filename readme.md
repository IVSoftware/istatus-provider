# istatus-provider
Implement interface for a consistent way to call different classes.


Implementing an `interface` is flexible and extensible. There's no requirement to inherit any particular base class, and it's possible to mix-and-match interfaces as a form of de facto multiple inheritance. This interface defines a class that can be polled for its status, and can also send notifications when something has occurred that might require an status update.

    interface IStatusProvider
    {
        string Status { get; }
        event EventHandler StatusUpdated;
    }

***
**Example of a `CustomUserControl : UserControl, IStatusProvider**

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

***
Make a collection of them like this:

    IStatusProvider[] UserControls = new IStatusProvider[]
    {
        new UserControlTypeA(),
        new UserControlTypeB(),
        new UserControlTypeC(),
    };

You can use an [implicit cast](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions) in a `foreach`, for example to use it as a `Control`:

    // Implicit cast from IStatusProvider to Control
    foreach (Control control in UserControls)
    {
        flowLayoutPanel.Controls.Add(control);
    }

Otherwise, the `foreach` will evaluate to IStatusProvider and this is where the different user controls will provide different responses to `Status`:

    // IStatusProvider
    foreach (var statusProvider in UserControls)
    {
        statusProvider.StatusUpdated += onAnyStatusUpdated;
    }

    private void onAnyStatusUpdated(object sender, EventArgs e)
    {
        foreach (var statusProvider in UserControls)
        {
            textBoxMultiline.AppendText($"{statusProvider.Status}{Environment.NewLine}");
        }
        textBoxMultiline.AppendText(Environment.NewLine);
    }

***

The result of iterating the `UserControls` collection is shown here, where the `flowLayoutPanel` is on the left and the status updates are shown on the right.

![screenshot](https://github.com/IVSoftware/istatus-provider/blob/master/istatus-provider/ReadMe/screenshot.png)
