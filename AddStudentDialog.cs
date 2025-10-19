using System;
using System.Drawing;
using System.Windows.Forms;

namespace LeaderBoard
{
    public partial class AddStudentDialog : Form
    {
        public string StudentName { get; private set; }
        public string GroupName { get; private set; }

        private TextBox nameTextBox;
        private TextBox groupTextBox;
        private Button okButton;
        private Button cancelButton;

        public AddStudentDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(350, 180);
            this.Text = "Add New Student";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            var nameLabel = new Label 
            { 
                Text = "Student Name:", 
                Location = new Point(20, 20), 
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9)
            };

            nameTextBox = new TextBox 
            { 
                Location = new Point(130, 18), 
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 9)
            };

            var groupLabel = new Label 
            { 
                Text = "Group:", 
                Location = new Point(20, 60), 
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9)
            };

            groupTextBox = new TextBox 
            { 
                Location = new Point(130, 58), 
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 9)
            };

            okButton = new Button 
            { 
                Text = "OK", 
                Location = new Point(150, 100), 
                Size = new Size(80, 32),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            
            cancelButton = new Button 
            { 
                Text = "Cancel", 
                Location = new Point(240, 100), 
                Size = new Size(80, 32),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };

            okButton.FlatAppearance.BorderSize = 0;
            cancelButton.FlatAppearance.BorderSize = 0;

            okButton.Click += OkButton_Click;

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(groupLabel);
            this.Controls.Add(groupTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Please enter student name", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return;
            }
            
            if (string.IsNullOrWhiteSpace(groupTextBox.Text))
            {
                MessageBox.Show("Please enter a group name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                groupTextBox.Focus();
                return;
            }

            StudentName = nameTextBox.Text.Trim();
            GroupName = groupTextBox.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }
    }
}