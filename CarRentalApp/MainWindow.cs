using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private readonly Login _login;
        // by making these public any sub-window (class) can see it
        public string _roleName = String.Empty;
        public List<UserRole> _roles;
        public List<string> _roleNames = new List<string>();
        public User _user;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login login) : this()
        {
            _login = login;
        }

        public MainWindow(Login login, string roleName) : this(login)
        {
            _roleName = roleName;
        }

        public MainWindow(Login login, List<UserRole> roles) : this(login)
        {
            _roles = roles;
        }

        public MainWindow(Login login, List<string> roleNames) : this(login)
        {
            _roleNames = roleNames;
        }

        public MainWindow(Login login, User user) : this(login)
        {
            _user = user;
            _roleName=user.UserRoles.FirstOrDefault().Role.shortName;
            _roleNames = user.UserRoles.ToList()
                            .Select(d => (string)d.Role.shortName).ToList();
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("AddEditRentalRecord"))
            {
                AddEditRentalRecord addRentalRecord = new AddEditRentalRecord();
                addRentalRecord.MdiParent = this;
                addRentalRecord.Show(); 
            }
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("ManageVehicleListing"))
            {
                ManageVehicleListing manageVehicleListing = new ManageVehicleListing();
                manageVehicleListing.MdiParent = this;
                manageVehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("ManageRentalRecords"))
            {
                ManageRentalRecords manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();

            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("ManageUsers"))
            {
                ManageUsers manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();

            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if(_user.password==Utils.getDefaultHashedPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }

            tsiLabel.Text=$"Username: {_user.username}";

            if (_roleName != "admin" && !_roleNames.Contains("admin"))
                manageUsersToolStripMenuItem.Visible = false;
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resetPassword = new ResetPassword(_user);
            resetPassword.ShowDialog();
        }
    }
}
