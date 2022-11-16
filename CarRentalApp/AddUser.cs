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
    public partial class AddUser : Form
    {
        private ManageUsers _manageUsers;
        private readonly CarRentalEntities _carRentalEntities;

        public AddUser()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
        }

        public AddUser(ManageUsers manageUsers) : this()
        {
            _manageUsers = manageUsers;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                int roleId = (int)cbRole.SelectedValue;

                User user = new User();
                user.username = username;
                user.password = Utils.getDefaultHashedPassword();
                user.isActive = true;

                _carRentalEntities.Users.Add(user);
                _carRentalEntities.SaveChanges();

                int userId = user.Id;

                UserRole userRole = new UserRole
                {
                    userId = userId,
                    roleId = roleId
                };

                _carRentalEntities.UserRoles.Add(userRole);
                _carRentalEntities.SaveChanges();

                if (_manageUsers != null)
                    _manageUsers.PopulateGrid();

                MessageBox.Show("New user added successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't add new user.");
            }
            Close();
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _carRentalEntities.Roles
                .Select(d => new { Id = d.Id, d.name, d.shortName })
                .ToList();

            cbRole.DisplayMember = "name";
            cbRole.ValueMember = "Id";
            cbRole.DataSource = roles;

        }
    }
}
