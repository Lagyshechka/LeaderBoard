using System;
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
            this.Size = new Size(300, 150);
            this.Text = "Add New Student";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var nameLabel = new Label { Text = "Student Name:", Location = new Point(10, 15), Size = new Size(100, 20) };
            nameTextBox = new TextBox { Location = new Point(120, 12), Size = new Size(150, 20) };

            var groupLabel = new Label { Text = "Group:", Location = new Point(10, 45), Size = new Size(100, 20) };
            groupTextBox = new TextBox { Location = new Point(120, 42), Size = new Size(150, 20) };

            okButton = new Button { Text = "OK", Location = new Point(120, 75), Size = new Size(70, 25), DialogResult = DialogResult.OK };
            cancelButton = new Button { Text = "Cancel", Location = new Point(200, 75), Size = new Size(70, 25), DialogResult = DialogResult.Cancel };

            okButton.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                {
                    StudentName = nameTextBox.Text;
                    GroupName = groupTextBox.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            cancelButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { nameLabel, nameTextBox, groupLabel, groupTextBox, okButton, cancelButton });
        }
    }
}