﻿using Proiect.CoursesWebServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect.Faculties
{
    public partial class FormViewFaculties : Form
    {
        public static CoursesWebService webService = new CoursesWebService();
        private FormDashboard parent;

        public FormViewFaculties()
        {
            InitializeComponent();
        }

        private void FormViewFaculties_Load(object sender, EventArgs e)
        {
            parent = (FormDashboard)Owner;

            UpdateData(sender, e);
        }

        private void toolStripButtonAddFaculty_Click(object sender, EventArgs e)
        {
            FormCreateFaculty formCreateFaculty = new FormCreateFaculty();
            formCreateFaculty.Show(this);
            this.Hide();
        }

        private void FormViewFaculties_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Show();
        }

        private void viewFaculty(object sender, EventArgs e, Faculty faculty)
        {
            FormViewFaculty formViewFaculty = new FormViewFaculty(this, faculty.id);
            this.Hide();
            formViewFaculty.Show();
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            this.Close();
            parent.Show();
        }

        private void FormViewFaculties_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                UpdateData(sender, e);
            }
        }

        private void UpdateData(object sender, EventArgs e)
        {
            panel.Controls.Clear();

            int width = panel.Size.Width;

            int widthOffset = 5;
            int heightOffset = 0;

            int btnWidth = 200;
            int btnHeight = 100;

            string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));

            webService.GetFaculties().ToList().ForEach((faculty) =>
            {

                if ((widthOffset + btnWidth) >= width)
                {
                    widthOffset = 5;
                    heightOffset = heightOffset + btnHeight;

                    var button = new Button();
                    button.Size = new Size(btnWidth, btnHeight);
                    button.Name = "buttonFaculty" + faculty.id;
                    button.Text = faculty.name;
                    button.Click += new EventHandler((ss, ee) => viewFaculty(sender, e, faculty));
                    button.Location = new Point(widthOffset, heightOffset);
                    panel.Controls.Add(button);

                    widthOffset = widthOffset + (btnWidth);
                }

                else
                {
                    var button = new Button();
                    button.Size = new Size(btnWidth, btnHeight);
                    button.Name = "buttonFaculty" + faculty.id;
                    button.Text = faculty.name;
                    button.Click += new EventHandler((ss, ee) => viewFaculty(sender, e, faculty));
                    button.Location = new Point(widthOffset, heightOffset);

                    panel.Controls.Add(button);

                    widthOffset = widthOffset + (btnWidth);
                }
            });

            this.Height = heightOffset + btnHeight + 90;
            panel.Height = heightOffset + btnHeight + 90;
        }
    }
}
