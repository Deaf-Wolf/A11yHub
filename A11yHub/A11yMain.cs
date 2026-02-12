using static A11yHub.AudioSplitterUtil;

namespace A11yHub;

public partial class A11yMain : Form
{
    private AudioSplitterUtil audioUtil;
    private List<AudioSplitterUtil.OutputDevice> allDevices = new List<AudioSplitterUtil.OutputDevice>();
    private bool isEnabled = false;

    public A11yMain()
    {
        InitializeComponent();
        audioUtil = new AudioSplitterUtil();
        LoadDevices();
        BuildTrayMenu();
    }

    private void LoadDevices()
    {
        allDevices = audioUtil.GetOutputDevices();
        
        sourceDeviceCombo.Items.Clear();
        deviceListBox.Items.Clear();

        foreach (var device in allDevices)
        {
            sourceDeviceCombo.Items.Add(device.Device.FriendlyName);
            deviceListBox.Items.Add(device.Device.FriendlyName);
        }

        if (sourceDeviceCombo.Items.Count > 0)
        {
            // Set the System Current Default Output as Source
            var defaultOutput = audioUtil.GetDefaultOutputDevice();
            sourceDeviceCombo.SelectedItem = defaultOutput;

        }
    }

    private void BuildTrayMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Open", null, (s, e) => ShowWindow());
        menu.Items.Add("Exit", null, (s, e) => Application.Exit());
        notifyIcon.ContextMenuStrip = menu;
    }

    private void BtnEnable_Click(object sender, EventArgs e)
    {
        if (!isEnabled)
        {
            if (sourceDeviceCombo.SelectedIndex == -1)
            {
                MessageBox.Show("Select source device!");
                return;
            }

            var sourceDevice = allDevices[sourceDeviceCombo.SelectedIndex];
            var outputDevices = new List<AudioSplitterUtil.OutputDevice>();

            foreach (int i in deviceListBox.CheckedIndices)
            {
                outputDevices.Add(allDevices[i]);
            }

            if (outputDevices.Count > 0)
            {
                audioUtil.RouteAudioToDevices(sourceDevice.Device, outputDevices);
                btnEnable.Text = "Disable";
                isEnabled = true;
                UpdateTrayIcon(true);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Select at least one output device!");
            }
        }
        else
        {
            audioUtil.StopRouting();
            btnEnable.Text = "Enable";
            isEnabled = false;
            UpdateTrayIcon(false);
        }
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        if (isEnabled)
        {
            audioUtil.StopRouting();
            isEnabled = false;
            btnEnable.Text = "Enable";
            UpdateTrayIcon(false);
        }
        LoadDevices();
    }

    private void NotifyIcon_DoubleClick(object sender, EventArgs e)
    {
        ShowWindow();
    }

    private void ShowWindow()
    {
        this.Show();
        this.WindowState = FormWindowState.Normal;
    }

    private void UpdateTrayIcon(bool active)
    {
        if (active)
        {
            using (Bitmap bmp = new Bitmap(16, 16))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.FillEllipse(Brushes.LimeGreen, 2, 2, 12, 12);
                notifyIcon.Icon = Icon.FromHandle(bmp.GetHicon());
            }
        }
        else
        {
            notifyIcon.Icon = SystemIcons.Application;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && isEnabled)
        {
            e.Cancel = true;
            this.Hide();
        }
        base.OnFormClosing(e);
    }
}