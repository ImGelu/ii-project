using Proiect.CoursesWebServiceReference;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Proiect
{
    public partial class FormCreateUser : Form
    {
        public static CoursesWebService webService = new CoursesWebService();
        private FormViewUsers parent;

        public FormCreateUser()
        {
            InitializeComponent();
        }

        private void FormCreateUser_Load(object sender, EventArgs e)
        {
            parent = (FormViewUsers)Owner;
            listBoxRoles.ValueMember = "name";
            comboBoxRoles.ValueMember = "name";

            webService.GetRoles().ToList().ForEach((role) =>
            {
                comboBoxRoles.Items.Add(role);
            });
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string name, email, password, passwordConfirmation;

            name = textBoxName.Text;
            email = textBoxEmail.Text;
            password = textBoxPassword.Text;
            passwordConfirmation = textBoxPasswordConfirmation.Text;

            if (name != String.Empty && email != String.Empty && password != String.Empty && passwordConfirmation != String.Empty)
            {
                if (Utils.IsValidEmail(email))
                {
                    if (!Utils.EmailAlreadyTaken(email))
                    {
                        if (password.Equals(passwordConfirmation))
                        {
                            User newUser = new User();
                            newUser.name = name;
                            newUser.email = email;
                            newUser.password = password;

                            try
                            {
                                Role[] rolesToBeUpdated = new Role[100];
                                int i = 0;

                                listBoxRoles.Items.Cast<Role>().ToList().ForEach((role) =>
                                {
                                    rolesToBeUpdated[i++] = role;
                                });

                                webService.AddUser(newUser);

                                User addedUser = webService.GetUserByEmail(newUser.email);

                                webService.UpdateUserRoles(addedUser.id, rolesToBeUpdated);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("A apărut o eroare\n" + ex.Message.ToString());
                            }

                            MessageBox.Show("Contul a fost creat cu succes!");
                            textBoxEmail.Text = String.Empty;
                            textBoxName.Text = String.Empty;
                            textBoxPassword.Text = String.Empty;
                            textBoxPasswordConfirmation.Text = String.Empty;

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Parolele nu se potrivesc. Încearcă din nou!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Acest email este deja luat. Încearcă altul!");
                    }
                }
                else
                {
                    MessageBox.Show("Acest email nu este unul valid. Încearcă altul!");
                }
            }
            else
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii!");
            }
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            this.Close();
            parent.Show();
        }

        private void FormCreateUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Show();
        }

        private void buttonAddRole_Click(object sender, EventArgs e)
        {
            Role selectedItem = (Role)comboBoxRoles.SelectedItem;

            if (comboBoxRoles.SelectedItem != null)
            {
                listBoxRoles.Items.Add(selectedItem);
            }

            comboBoxRoles.Items.Remove(selectedItem);
        }

        private void listBoxRoles_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Back))
            {
                Role selectedItem = (Role)listBoxRoles.SelectedItem;

                e.Handled = true;
                if (selectedItem != null)
                {
                    listBoxRoles.Items.Remove(selectedItem);
                    comboBoxRoles.Items.Add(selectedItem);
                }
            }
        }
    }
}
