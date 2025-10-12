namespace LeaderBoard
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox groupTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button addGradeColumnButton;
        private System.Windows.Forms.Button deleteButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.groupTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.addGradeColumnButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            
            this.dataGridView.Location = new System.Drawing.Point(10, 60);
            this.dataGridView.Size = new System.Drawing.Size(860, 480);
            this.dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            
            this.nameTextBox.Location = new System.Drawing.Point(10, 10);
            this.nameTextBox.Size = new System.Drawing.Size(150, 20);
            this.nameTextBox.Text = "";
            
            this.groupTextBox.Location = new System.Drawing.Point(170, 10);
            this.groupTextBox.Size = new System.Drawing.Size(100, 20);
            this.groupTextBox.Text = "";
            
            this.addButton.Location = new System.Drawing.Point(280, 10);
            this.addButton.Size = new System.Drawing.Size(100, 25);
            this.addButton.Text = "Add Student";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            
            this.addGradeColumnButton.Location = new System.Drawing.Point(390, 10);
            this.addGradeColumnButton.Size = new System.Drawing.Size(120, 25);
            this.addGradeColumnButton.Text = "Add Grade Column";
            this.addGradeColumnButton.Click += new System.EventHandler(this.addGradeColumnButton_Click);
            
            this.deleteButton.Location = new System.Drawing.Point(520, 10);
            this.deleteButton.Size = new System.Drawing.Size(100, 25);
            this.deleteButton.Text = "Delete Selected";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Text = "Student Leaderboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.groupTextBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.addGradeColumnButton);
            this.Controls.Add(this.deleteButton);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }
    }
}