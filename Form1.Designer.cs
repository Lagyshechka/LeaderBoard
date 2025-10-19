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
        private System.Windows.Forms.Button renameColumnsButton;

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
            this.SuspendLayout();
            
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridView.Location = new System.Drawing.Point(12, 50);
            this.dataGridView.Size = new System.Drawing.Size(960, 520);
            this.dataGridView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | 
                                      System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox.Location = new System.Drawing.Point(12, 12);
            this.nameTextBox.Size = new System.Drawing.Size(150, 23);
            this.nameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            this.groupTextBox = new System.Windows.Forms.TextBox();
            this.groupTextBox.Location = new System.Drawing.Point(168, 12);
            this.groupTextBox.Size = new System.Drawing.Size(100, 23);
            this.groupTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            this.addButton = new System.Windows.Forms.Button();
            this.addButton.Location = new System.Drawing.Point(274, 12);
            this.addButton.Size = new System.Drawing.Size(100, 25);
            this.addButton.Text = "Add Student";
            this.addButton.UseVisualStyleBackColor = false;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            
            this.addGradeColumnButton = new System.Windows.Forms.Button();
            this.addGradeColumnButton.Location = new System.Drawing.Point(380, 12);
            this.addGradeColumnButton.Size = new System.Drawing.Size(120, 25);
            this.addGradeColumnButton.Text = "Add Grade Column";
            this.addGradeColumnButton.UseVisualStyleBackColor = false;
            this.addGradeColumnButton.Click += new System.EventHandler(this.addGradeColumnButton_Click);
            
            this.deleteButton = new System.Windows.Forms.Button();
            this.deleteButton.Location = new System.Drawing.Point(506, 12);
            this.deleteButton.Size = new System.Drawing.Size(100, 25);
            this.deleteButton.Text = "Delete Selected";
            this.deleteButton.UseVisualStyleBackColor = false;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            
            this.renameColumnsButton = new System.Windows.Forms.Button();
            this.renameColumnsButton.Location = new System.Drawing.Point(612, 12);
            this.renameColumnsButton.Size = new System.Drawing.Size(120, 25);
            this.renameColumnsButton.Text = "Rename Columns";
            this.renameColumnsButton.UseVisualStyleBackColor = false;
            this.renameColumnsButton.Click += new System.EventHandler(this.renameColumnsButton_Click);
            
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 582);
            this.Text = "Student Leaderboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.groupTextBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.addGradeColumnButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.renameColumnsButton);
            
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}