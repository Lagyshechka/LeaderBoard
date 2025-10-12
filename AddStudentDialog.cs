using System;
using System.Drawing;
using System.Windows.Forms;

namespace LeaderBoard
{
    public partial class AddStudentDialog : Form
    {
        public string StudentName { get; private set; }
        public string GroupName { get; private set; }

        public AddStudentDialog()
        {
            InitializeComponent();
            ApplyDialogStyling();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(450, 250);
            this.Text = "Add New Student";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
        }

        private void ApplyDialogStyling()
        {
            var titleLabel = new Label 
            { 
                Text = "Add New Student", 
                Location = new Point(20, 15), 
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            var nameLabel = new Label 
            { 
                Text = "Student Name:", 
                Location = new Point(20, 50), 
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9)
            };
            
            var nameTextBox = new TextBox 
            { 
                Location = new Point(150, 48), 
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle
            };

            var groupLabel = new Label 
            { 
                Text = "Group:", 
                Location = new Point(20, 80), 
                Size = new Size(120, 20),
                Font = new Font("Segoe UI", 9)
            };
            
            var groupTextBox = new TextBox 
            { 
                Location = new Point(150, 78), 
                Size = new Size(160, 25),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle
            };

            var okButton = new Button 
            { 
                Text = "Add", 
                Location = new Point(150, 115), 
                Size = new Size(80, 32),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            
            var cancelButton = new Button 
            { 
                Text = "Cancel", 
                Location = new Point(240, 115), 
                Size = new Size(80, 32),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            okButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatAppearance.BorderSize = 0;

            okButton.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                {
                    StudentName = nameTextBox.Text;
                    GroupName = groupTextBox.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter student name", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { titleLabel, nameLabel, nameTextBox, groupLabel, groupTextBox, okButton, cancelButton });

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }
    }
}