using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _carRentalEntities;

        public ManageUsers()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("AddUser"))
            {
                AddUser addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }

        }

        public void PopulateGrid()
        {
            var users = _carRentalEntities.Users
                .Select(d => new
                {
                    d.Id,
                    d.username,
                    d.password,
                    d.isActive,
                    d.UserRoles.FirstOrDefault().Role.name
                })
                .ToList();

            dgvUsersList.DataSource = users;
            dgvUsersList.Columns["Id"].Visible = false;
            dgvUsersList.Columns["password"].Visible = false;
            dgvUsersList.Columns["username"].HeaderText = "Username";
            dgvUsersList.Columns["isActive"].HeaderText = "Active";
            dgvUsersList.Columns["name"].HeaderText = "Role Name";
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                int id = (int)dgvUsersList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                User user = _carRentalEntities.Users.FirstOrDefault(q => q.Id == id);

                string hashedPassword = Utils.getDefaultHashedPassword();

                user.password = hashedPassword;

                _carRentalEntities.SaveChanges();

                MessageBox.Show($"{user.username}'s password has been reset.");

                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show("There was an error", "Error");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                int id = (int)dgvUsersList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                User user = _carRentalEntities.Users.FirstOrDefault(q => q.Id == id);

                //user.isActive = false;
                user.isActive = user.isActive == true ? false : true;

                _carRentalEntities.SaveChanges();

                MessageBox.Show($"{user.username}'s active status has been changed.");

                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show("There was an error", "Error");
            }
        }

    }
}
