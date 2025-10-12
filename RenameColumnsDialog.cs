using System;
using System.Drawing;
using System.Windows.Forms;

namespace LeaderBoard
{
    public partial class RenameColumnsDialog : Form
    {
        private TextBox[] nameTextBoxes;

        public RenameColumnsDialog(string[] currentNames)
        {
            InitializeComponent(currentNames);
        }

        private void InitializeComponent(string[] currentNames)
        {
            this.Size = new Size(400, 200 + currentNames.Length * 40);
            this.Text = "Rename Grade Columns";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Padding = new Padding(20);

            var titleLabel = new Label 
            { 
                Text = "Rename Grade Columns", 
                Location = new Point(20, 20), 
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            this.Controls.Add(titleLabel);

            nameTextBoxes = new TextBox[currentNames.Length];
            
            for (int i = 0; i < currentNames.Length; i++)
            {
                var label = new Label 
                { 
                    Text = $"Column {i + 1}:", 
                    Location = new Point(20, 60 + i * 40), 
                    Size = new Size(80, 25),
                    Font = new Font("Segoe UI", 9)
                };
                
                nameTextBoxes[i] = new TextBox 
                { 
                    Text = currentNames[i],
                    Location = new Point(110, 58 + i * 40), 
                    Size = new Size(250, 25),
                    Font = new Font("Segoe UI", 9),
                    BorderStyle = BorderStyle.FixedSingle
                };

                this.Controls.Add(label);
                this.Controls.Add(nameTextBoxes[i]);
            }

            var okButton = new Button 
            { 
                Text = "OK", 
                Location = new Point(200, 70 + currentNames.Length * 40), 
                Size = new Size(80, 32),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            
            var cancelButton = new Button 
            { 
                Text = "Cancel", 
                Location = new Point(290, 70 + currentNames.Length * 40), 
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

            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        public string[] GetNewNames()
        {
            string[] names = new string[nameTextBoxes.Length];
            for (int i = 0; i < nameTextBoxes.Length; i++)
            {
                names[i] = string.IsNullOrWhiteSpace(nameTextBoxes[i].Text) ? $"Grade {i + 1}" : nameTextBoxes[i].Text;
            }
            return names;
        }
    }
}