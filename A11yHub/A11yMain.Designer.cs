using System.Windows.Forms;

namespace A11yHub;

partial class A11yMain
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox sourceDeviceCombo;
    private CheckedListBox deviceListBox;
    private Button btnEnable;
    private Button btnRefresh;
    private NotifyIcon notifyIcon;

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.sourceDeviceCombo = new ComboBox();
        this.deviceListBox = new CheckedListBox();
        this.btnEnable = new Button();
        this.btnRefresh = new Button();
        this.notifyIcon = new NotifyIcon(this.components);
        this.SuspendLayout();

        // sourceDeviceCombo
        this.sourceDeviceCombo.Location = new System.Drawing.Point(12, 12);
        this.sourceDeviceCombo.Size = new System.Drawing.Size(360, 25);
        this.sourceDeviceCombo.DropDownStyle = ComboBoxStyle.DropDownList;

        // deviceListBox
        this.deviceListBox.Location = new System.Drawing.Point(12, 45);
        this.deviceListBox.Size = new System.Drawing.Size(360, 167);

        // btnEnable
        this.btnEnable.Location = new System.Drawing.Point(12, 220);
        this.btnEnable.Size = new System.Drawing.Size(170, 30);
        this.btnEnable.Text = "Enable";
        this.btnEnable.Click += BtnEnable_Click;

        // btnRefresh
        this.btnRefresh.Location = new System.Drawing.Point(202, 220);
        this.btnRefresh.Size = new System.Drawing.Size(170, 30);
        this.btnRefresh.Text = "Refresh Devices";
        this.btnRefresh.Click += BtnRefresh_Click;

        // notifyIcon
        this.notifyIcon.Icon = SystemIcons.Application;
        this.notifyIcon.Visible = true;
        this.notifyIcon.Text = "A11yHub";
        this.notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

        // A11yMain
        this.ClientSize = new System.Drawing.Size(384, 262);
        this.Controls.Add(this.sourceDeviceCombo);
        this.Controls.Add(this.deviceListBox);
        this.Controls.Add(this.btnEnable);
        this.Controls.Add(this.btnRefresh);
        this.Text = "A11yHub - Audio Router";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }
}