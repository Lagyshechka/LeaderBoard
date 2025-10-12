using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace LeaderBoard
{
    public partial class Form1 : Form
    {
        private string dataFilePath;
        private System.Windows.Forms.Timer updateTimer;
        private const string PhantomStudentName = "+";

        public Form1()
        {
            InitializeComponent();
            dataFilePath = Path.Combine(Application.StartupPath, "students_data.json");
            SetupUpdateTimer();
            SetupDataGridView();
            LoadData();
            AddPhantomStudent();
        }

        private void SetupUpdateTimer()
        {
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 100;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            RecalculateAllTotals();
            MaintainPhantomStudent();
        }

        private void SetupDataGridView()
        {
            dataGridView.Columns.Clear();
            
            dataGridView.Columns.Add("Name", "Student Name");
            dataGridView.Columns.Add("Group", "Group");
            
            for (int i = 1; i <= 3; i++)
            {
                dataGridView.Columns.Add($"Grade{i}", $"Grade {i}");
            }
            
            dataGridView.Columns.Add("Total", "Total");
            
            dataGridView.Columns["Name"].Width = 150;
            dataGridView.Columns["Group"].Width = 100;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.StartsWith("Grade"))
                    column.Width = 70;
            }
            dataGridView.Columns["Total"].Width = 70;
            
            dataGridView.AllowUserToAddRows = false;
            dataGridView.CellEndEdit += DataGridView_CellEndEdit;
            dataGridView.CellClick += DataGridView_CellClick;
        }

        private void AddPhantomStudent()
        {
            int rowIndex = dataGridView.Rows.Add();
            DataGridViewRow row = dataGridView.Rows[rowIndex];
            
            row.Cells["Name"].Value = PhantomStudentName;
            row.Cells["Group"].Value = "";
            
            for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
            {
                row.Cells[i].Value = "";
            }
            
            row.Cells[dataGridView.Columns.Count - 1].Value = "";
            
            row.DefaultCellStyle.ForeColor = Color.Gray;
            row.DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Italic);
        }

        private void MaintainPhantomStudent()
        {
            if (dataGridView.Rows.Count == 0) return;

            DataGridViewRow lastRow = dataGridView.Rows[dataGridView.Rows.Count - 1];
            
            if (lastRow.Cells["Name"].Value?.ToString() != PhantomStudentName)
            {
                AddPhantomStudent();
            }
            int phantomCount = 0;
            for (int i = dataGridView.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView.Rows[i].Cells["Name"].Value?.ToString() == PhantomStudentName)
                {
                    phantomCount++;
                    if (phantomCount > 1)
                    {
                        dataGridView.Rows.RemoveAt(i);
                    }
                }
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                DataGridViewRow clickedRow = dataGridView.Rows[e.RowIndex];
                
                if (clickedRow.Cells["Name"].Value?.ToString() == PhantomStudentName)
                {
                    if (e.ColumnIndex == 0)
                    {
                        AddNewStudentFromPhantom();
                    }
                }
            }
        }

        private void AddNewStudentFromPhantom()
        {
            int phantomRowIndex = dataGridView.Rows.Count - 1;
            
            using (var dialog = new AddStudentDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DataGridViewRow newRow = dataGridView.Rows[phantomRowIndex];
                    
                    newRow.Cells["Name"].Value = dialog.StudentName;
                    newRow.Cells["Group"].Value = dialog.GroupName;
                    
                    for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
                    {
                        newRow.Cells[i].Value = 0;
                    }
                    
                    newRow.DefaultCellStyle.ForeColor = dataGridView.DefaultCellStyle.ForeColor;
                    newRow.DefaultCellStyle.Font = dataGridView.DefaultCellStyle.Font;
                    
                    AddPhantomStudent();
                    SaveData();
                }
            }
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                
                if (row.Cells["Name"].Value?.ToString() == PhantomStudentName && e.ColumnIndex == 0)
                {
                    AddNewStudentFromPhantom();
                }
                else
                {
                    SaveData();
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nameTextBox.Text))
            {
                AddRegularStudent(nameTextBox.Text, groupTextBox.Text);
                nameTextBox.Text = "";
                groupTextBox.Text = "";
            }
        }

        private void AddRegularStudent(string name, string group)
        {
            int rowIndex = dataGridView.Rows.Add();
            DataGridViewRow row = dataGridView.Rows[rowIndex];
            
            row.Cells["Name"].Value = name;
            row.Cells["Group"].Value = group;
            
            for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
            {
                row.Cells[i].Value = 0;
            }
            
            SaveData();
        }

        private void addGradeColumnButton_Click(object sender, EventArgs e)
        {
            int gradeColumnCount = dataGridView.Columns.Count - 3;
            string newColumnName = $"Grade{gradeColumnCount + 1}";
            
            var newColumn = new DataGridViewTextBoxColumn
            {
                Name = newColumnName,
                HeaderText = $"Grade {gradeColumnCount + 1}",
                Width = 70
            };
            
            dataGridView.Columns.Insert(dataGridView.Columns.Count - 1, newColumn);
            
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["Name"].Value?.ToString() != PhantomStudentName)
                {
                    row.Cells[newColumnName].Value = 0;
                }
                else
                {
                    row.Cells[newColumnName].Value = "";
                }
            }
            
            SaveData();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (!row.IsNewRow && row.Cells["Name"].Value?.ToString() != PhantomStudentName)
                    {
                        dataGridView.Rows.Remove(row);
                    }
                }
                SaveData();
            }
        }

        private void RecalculateTotalForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count - 1) return;
            
            DataGridViewRow row = dataGridView.Rows[rowIndex];
            
            if (row.Cells["Name"].Value?.ToString() == PhantomStudentName) return;
            
            int total = 0;
            
            for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
            {
                if (row.Cells[i].Value != null && int.TryParse(row.Cells[i].Value.ToString(), out int grade))
                {
                    total += grade;
                }
            }
            
            row.Cells[dataGridView.Columns.Count - 1].Value = total;
        }

        private void RecalculateAllTotals()
        {
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                RecalculateTotalForRow(i);
            }
        }

        private void SaveData()
        {
            try
            {
                var students = new List<StudentData>();
                
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (row.Cells["Name"].Value?.ToString() == PhantomStudentName) continue;
                    
                    var student = new StudentData
                    {
                        Name = row.Cells["Name"].Value?.ToString() ?? "",
                        Group = row.Cells["Group"].Value?.ToString() ?? "",
                        Grades = new List<int>()
                    };
                    
                    for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
                    {
                        if (row.Cells[i].Value != null && int.TryParse(row.Cells[i].Value.ToString(), out int grade))
                        {
                            student.Grades.Add(grade);
                        }
                        else
                        {
                            student.Grades.Add(0);
                        }
                    }
                    
                    students.Add(student);
                }
                
                var data = new SaveData
                {
                    Students = students,
                    GradeColumnsCount = dataGridView.Columns.Count - 3
                };
                
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                if (!File.Exists(dataFilePath)) return;
                
                string json = File.ReadAllText(dataFilePath);
                var data = JsonSerializer.Deserialize<SaveData>(json);
                
                if (data == null) return;
                
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                
                dataGridView.Columns.Add("Name", "Student Name");
                dataGridView.Columns.Add("Group", "Group");
                
                for (int i = 1; i <= data.GradeColumnsCount; i++)
                {
                    dataGridView.Columns.Add($"Grade{i}", $"Grade {i}");
                }
                
                dataGridView.Columns.Add("Total", "Total");
                
                dataGridView.Columns["Name"].Width = 150;
                dataGridView.Columns["Group"].Width = 100;
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Name.StartsWith("Grade"))
                        column.Width = 70;
                }
                dataGridView.Columns["Total"].Width = 70;
                
                dataGridView.AllowUserToAddRows = false;
                dataGridView.CellEndEdit += DataGridView_CellEndEdit;
                dataGridView.CellClick += DataGridView_CellClick;
                
                foreach (var student in data.Students)
                {
                    AddRegularStudent(student.Name, student.Group);
                    
                    DataGridViewRow row = dataGridView.Rows[dataGridView.Rows.Count - 1];
                    for (int i = 0; i < student.Grades.Count; i++)
                    {
                        row.Cells[i + 2].Value = student.Grades[i];
                    }
                }
                
                AddPhantomStudent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
    }

    public class StudentData
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public List<int> Grades { get; set; }
    }

    public class SaveData
    {
        public List<StudentData> Students { get; set; }
        public int GradeColumnsCount { get; set; }
    }
}