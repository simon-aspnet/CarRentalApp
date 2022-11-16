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
    public partial class Login : Form
    {
        private readonly CarRentalEntities _carRentalEntities;

        public Login()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                string hashedPassword = Utils.getHashedPassword(password);

                var user = _carRentalEntities.Users.FirstOrDefault(q => q.username == username && q.password == hashedPassword
                            && q.isActive == true);

                if (user == null)
                {
                    MessageBox.Show("Please provide valid credentials");
                }
                else
                {
                    string roleName = string.Empty;

                    UserRole role = user.UserRoles.FirstOrDefault();
                    if (role != null)
                        roleName = role.Role.shortName;

                    if (roleName != string.Empty)
                    {
                        // This will get all the roles - it's a bit of code by clever me
                        List<UserRole> roles = user.UserRoles.ToList();
                        // And also save them as a list of strings
                        List<string> roleShortNames = user.UserRoles.ToList()
                            .Select(d => (string)d.Role.shortName).ToList();

                        // And also save them as a list of strings
                        //List<string> roleShortNames = roles.Select(d => (string)d.Role.shortName).ToList();

                        // various constructors for info passed
                        var mainWindow = new MainWindow(this, user);
                        //var mainWindow = new MainWindow(this, roleShortNames);
                        //var mainWindow=new MainWindow(this,roles);
                        //var mainWindow=new MainWindow(this,roleName);

                        mainWindow.Show();
                        Hide();
                    }
                    else
                        MessageBox.Show("You can't do anything", "Bugger");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong.\r\nPlease try again.");
            }
        }
    }
}
