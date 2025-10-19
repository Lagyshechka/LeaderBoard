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
        private const string PhantomStudentName = "+";

        private Color PrimaryColor = Color.FromArgb(41, 128, 185);
        private Color SecondaryColor = Color.FromArgb(52, 152, 219);
        private Color AccentColor = Color.FromArgb(46, 204, 113);
        private Color DangerColor = Color.FromArgb(231, 76, 60);
        private Color DarkColor = Color.FromArgb(44, 62, 80);
        private Color LightColor = Color.FromArgb(236, 240, 241);

        private Color[] ProgressColors = new Color[]
        {
            Color.FromArgb(231, 76, 60),
            Color.FromArgb(230, 126, 34),
            Color.FromArgb(241, 196, 15),
            Color.FromArgb(46, 204, 113),
            Color.FromArgb(52, 152, 219),
            Color.FromArgb(155, 89, 182)
        };

        public Form1()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);

            InitializeComponent();
            dataFilePath = Path.Combine(Application.StartupPath, "students_data.json");
            ApplyModernStyling();
            SetupDataGridView();
            SubscribeToGridEvents();
            LoadData();
            AddPhantomStudent();
        }

        private void SubscribeToGridEvents()
        {
            dataGridView.CellPainting += DataGridView_CellPainting;
            dataGridView.CellEndEdit += DataGridView_CellEndEdit;
            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.CellValueChanged += DataGridView_CellValueChanged;
        }

        private void ApplyModernStyling()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.White;
            this.ForeColor = DarkColor;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.Size = new Size(1000, 600);

            var controls = new Control[] {
                addButton, addGradeColumnButton, deleteButton, renameColumnsButton, deleteColumnButton,
                nameTextBox, groupTextBox, dataGridView
            };

            foreach (var control in controls)
            {
                if (control != null)
                {
                    control.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
                }
            }

            var buttons = new[] { addButton, addGradeColumnButton, deleteButton, renameColumnsButton, deleteColumnButton };
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = PrimaryColor;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
                    button.Size = new Size(120, 32);
                    button.Cursor = Cursors.Hand;
                    button.UseVisualStyleBackColor = false;
                }
            }

            if (addButton != null)
                addButton.BackColor = AccentColor;
            if (deleteButton != null)
                deleteButton.BackColor = DangerColor;
             if (deleteColumnButton != null)
                deleteColumnButton.BackColor = DangerColor;

            var textBoxes = new[] { nameTextBox, groupTextBox };
            foreach (var textBox in textBoxes)
            {
                if (textBox != null)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.White;
                    textBox.ForeColor = DarkColor;
                    textBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
                }
            }

            if (dataGridView != null)
            {
                dataGridView.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            }

            this.Text = "🎓 Student Leaderboard";
        }

        private void SetupDataGridView()
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("Name", "👤 Student Name");
            dataGridView.Columns.Add("Group", "🏫 Group");

            for (int i = 1; i <= 3; i++)
            {
                dataGridView.Columns.Add($"Grade{i}", $"Grade {i}");
            }

            dataGridView.Columns.Add("Total", "📊 Total");
            dataGridView.Columns.Add("Progress", "📈 Progress");
            dataGridView.Columns.Add("Grade", "🎓 Grade");

            ApplyDataGridViewStyling();
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView.Columns[e.ColumnIndex].Name == "Progress" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                var row = dataGridView.Rows[e.RowIndex];

                if (row.Cells["Name"].Value?.ToString() == PhantomStudentName)
                {
                    e.Graphics.DrawString("-", e.CellStyle.Font, Brushes.Gray,
                        e.CellBounds.X + 5, e.CellBounds.Y + (e.CellBounds.Height - 12) / 2);
                    e.Handled = true;
                    return;
                }

                if (row.Cells["Total"].Value != null && int.TryParse(row.Cells["Total"].Value.ToString(), out int totalScore))
                {
                    int gradeColumnsCount = GetGradeColumnsCount();
                    int maxPossibleScore = 100;

                    if (maxPossibleScore > 0)
                    {
                        float percentage = (float)totalScore / maxPossibleScore;
                        percentage = Math.Max(0, Math.Min(1, percentage));

                        var progressColor = GetProgressColor(percentage * 100);

                        var progressBounds = new Rectangle(
                            e.CellBounds.X + 5,
                            e.CellBounds.Y + 8,
                            e.CellBounds.Width - 10,
                            e.CellBounds.Height - 16
                        );

                        e.Graphics.FillRectangle(Brushes.LightGray, progressBounds);

                        var filledWidth = (int)(progressBounds.Width * percentage);
                        if (filledWidth > 0)
                        {
                            var filledBounds = new Rectangle(
                                progressBounds.X,
                                progressBounds.Y,
                                filledWidth,
                                progressBounds.Height
                            );
                            using (var brush = new SolidBrush(progressColor))
                            {
                                e.Graphics.FillRectangle(brush, filledBounds);
                            }
                        }

                        string progressText = $"{totalScore}/{maxPossibleScore} ({(percentage * 100):F0}%)";
                        var textSize = e.Graphics.MeasureString(progressText, e.CellStyle.Font);
                        var textLocation = new PointF(
                            e.CellBounds.X + (e.CellBounds.Width - textSize.Width) / 2,
                            e.CellBounds.Y + (e.CellBounds.Height - textSize.Height) / 2
                        );

                        var textColor = filledWidth > progressBounds.Width / 2 ? Brushes.White : Brushes.Black;
                        e.Graphics.DrawString(progressText, e.CellStyle.Font, textColor, textLocation);
                    }
                }

                e.Handled = true;
            }
        }

        private Color GetProgressColor(float percentage)
        {
            if (percentage <= 25) return ProgressColors[0];
            if (percentage <= 50) return ProgressColors[1];
            if (percentage <= 60) return ProgressColors[2];
            if (percentage <= 70) return ProgressColors[3];
            if (percentage <= 80) return ProgressColors[4];
            return ProgressColors[5];
        }

        private string GetGradeBadge(int totalScore, int maxPossibleScore)
        {
            if (maxPossibleScore == 0) return "-";

            float percentage = (float)totalScore / maxPossibleScore * 100;

            if (percentage <= 25) return "G 🔴";
            if (percentage <= 50) return "F 🟠";
            if (percentage <= 60) return "3/E 🟡";
            if (percentage <= 70) return "3+/D 🟢";
            if (percentage <= 80) return "4/C 🔵";
            if (percentage <= 89) return "4+/B 🟣";
            return "5/A 💜";
        }

        private int GetGradeColumnsCount()
        {
            int count = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.StartsWith("Grade") && column.Name != "Grade")
                    count++;
            }
            return count;
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count)
            {
                DataGridViewRow clickedRow = dataGridView.Rows[e.RowIndex];

                if (clickedRow.Cells["Name"].Value?.ToString() == PhantomStudentName && e.ColumnIndex == 0)
                {
                    AddNewStudentFromPhantom();
                }
            }
        }

        private void AddNewStudentFromPhantom()
        {
            using (var dialog = new AddStudentDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataGridViewRow existingRow in dataGridView.Rows)
                    {
                        if (existingRow.Cells["Name"].Value?.ToString() == PhantomStudentName)
                        {
                            existingRow.Cells["Name"].Value = dialog.StudentName;
                            existingRow.Cells["Group"].Value = dialog.GroupName;

                            int gradeColumnsCount = GetGradeColumnsCount();

                            for (int i = 0; i < gradeColumnsCount; i++)
                            {
                                string columnName = $"Grade{i + 1}";
                                existingRow.Cells[columnName].Value = 0;
                            }

                            existingRow.DefaultCellStyle = dataGridView.DefaultCellStyle;
                            break;
                        }
                    }

                    AddPhantomStudent();
                    SaveData();
                }
            }
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveData();
        }

        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView.Rows.Count && dataGridView.Rows[e.RowIndex].IsNewRow) return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                 var column = dataGridView.Columns[e.ColumnIndex];
                 if(column.Name.StartsWith("Grade") && column.Name != "Grade")
                 {
                    RecalculateTotalForRow(e.RowIndex);
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
            int phantomRowIndex = -1;
            for(int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if(dataGridView.Rows[i].Cells["Name"].Value?.ToString() == PhantomStudentName)
                {
                    phantomRowIndex = i;
                    break;
                }
            }

            if (phantomRowIndex != -1)
            {
                DataGridViewRow newRow = dataGridView.Rows[phantomRowIndex];
                newRow.Cells["Name"].Value = name;
                newRow.Cells["Group"].Value = group;

                int gradeColumnsCount = GetGradeColumnsCount();
                for (int i = 0; i < gradeColumnsCount; i++)
                {
                    string columnName = $"Grade{i + 1}";
                    newRow.Cells[columnName].Value = 0;
                }
                newRow.DefaultCellStyle = dataGridView.DefaultCellStyle;
                RecalculateTotalForRow(phantomRowIndex);
                AddPhantomStudent();
                SaveData();
            }
        }

        private void addGradeColumnButton_Click(object sender, EventArgs e)
        {
            int gradeColumnCount = GetGradeColumnsCount();
            string newColumnName = $"Grade{gradeColumnCount + 1}";

            var newColumn = new DataGridViewTextBoxColumn
            {
                Name = newColumnName,
                HeaderText = $"Grade {gradeColumnCount + 1}",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    ForeColor = DarkColor
                }
            };

            int insertIndex = dataGridView.Columns.Count - 3;
            dataGridView.Columns.Insert(insertIndex, newColumn);

            foreach (DataGridViewRow gridRow in dataGridView.Rows)
            {
                if (gridRow.IsNewRow) continue;
                if (gridRow.Cells["Name"].Value?.ToString() != PhantomStudentName)
                {
                    gridRow.Cells[newColumnName].Value = 0;
                }
                else
                {
                    gridRow.Cells[newColumnName].Value = "";
                }
            }

            RecalculateAllTotals();
            SaveData();
        }

        private void renameColumnsButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new RenameColumnsDialog(GetGradeColumnNames()))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var newNames = dialog.GetNewNames();
                    for (int i = 0; i < newNames.Length; i++)
                    {
                        if (i < GetGradeColumnsCount())
                        {
                            string columnName = $"Grade{i + 1}";
                            dataGridView.Columns[columnName].HeaderText = newNames[i];
                        }
                    }
                    SaveData();
                }
            }
        }
        
        private void deleteColumnButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell == null)
            {
                MessageBox.Show("Please select a column to delete by clicking any cell in it.", "No Column Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewColumn columnToDelete = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex];

            if (!columnToDelete.Name.StartsWith("Grade") || columnToDelete.Name == "Grade")
            {
                MessageBox.Show("You can only delete grade columns.", "Action Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the '{columnToDelete.HeaderText}' column?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dataGridView.Columns.Remove(columnToDelete);
                ReindexGradeColumns(); 
                RecalculateAllTotals();
                SaveData();
            }
        }

        private void ReindexGradeColumns()
        {
            int gradeIndex = 1;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.StartsWith("Grade") && column.Name != "Grade")
                {
                    string newName = $"Grade{gradeIndex}";
                    if (column.Name != newName)
                    {
                        if (column.HeaderText.StartsWith("Grade "))
                        {
                            column.HeaderText = $"Grade {gradeIndex}";
                        }
                        column.Name = newName;
                    }
                    gradeIndex++;
                }
            }
        }

        private string[] GetGradeColumnNames()
        {
            int count = GetGradeColumnsCount();
            string[] names = new string[count];
            for (int i = 0; i < count; i++)
            {
                string columnName = $"Grade{i + 1}";
                names[i] = dataGridView.Columns[columnName].HeaderText;
            }
            return names;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Delete selected students?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var rowsToDelete = new List<DataGridViewRow>();
                    foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
                    {
                        if (!selectedRow.IsNewRow && selectedRow.Cells["Name"].Value?.ToString() != PhantomStudentName)
                        {
                            rowsToDelete.Add(selectedRow);
                        }
                    }

                    foreach (var row in rowsToDelete)
                    {
                        dataGridView.Rows.Remove(row);
                    }

                    SaveData();
                }
            }
        }

        private void RecalculateTotalForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count) return;

            DataGridViewRow currentRow = dataGridView.Rows[rowIndex];

            if (currentRow.IsNewRow || currentRow.Cells["Name"].Value?.ToString() == PhantomStudentName) return;

            int total = 0;
            int gradeColumnsCount = GetGradeColumnsCount();

            for (int i = 0; i < gradeColumnsCount; i++)
            {
                string columnName = $"Grade{i + 1}";
                if (currentRow.Cells[columnName].Value != null &&
                    int.TryParse(currentRow.Cells[columnName].Value.ToString(), out int grade))
                {
                    total += grade;
                }
            }

            currentRow.Cells["Total"].Value = total;

            int maxPossibleScore = 100;
            currentRow.Cells["Grade"].Value = GetGradeBadge(total, maxPossibleScore);

            dataGridView.InvalidateRow(rowIndex);
        }

        private void RecalculateAllTotals()
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                 if(!dataGridView.Rows[i].IsNewRow)
                    RecalculateTotalForRow(i);
            }
            dataGridView.Refresh();
        }

        private void AddPhantomStudent()
        {
            foreach (DataGridViewRow existingRow in dataGridView.Rows)
            {
                if (existingRow.Cells["Name"].Value?.ToString() == PhantomStudentName)
                    return;
            }

            int rowIndex = dataGridView.Rows.Add();
            DataGridViewRow phantomRow = dataGridView.Rows[rowIndex];

            phantomRow.Cells["Name"].Value = PhantomStudentName;
            phantomRow.Cells["Group"].Value = "";

            int gradeColumnsCount = GetGradeColumnsCount();
            for (int i = 0; i < gradeColumnsCount; i++)
            {
                phantomRow.Cells[$"Grade{i+1}"].Value = "";
            }

            phantomRow.Cells["Total"].Value = "";
            phantomRow.Cells["Grade"].Value = "";

            phantomRow.DefaultCellStyle.ForeColor = Color.Gray;
            phantomRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            phantomRow.DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            phantomRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 200, 200);
            phantomRow.DefaultCellStyle.SelectionForeColor = Color.Gray;
        }

        private void SaveData()
        {
            try
            {
                var students = new List<StudentData>();

                foreach (DataGridViewRow studentRow in dataGridView.Rows)
                {
                    if (studentRow.IsNewRow) continue;
                    if (studentRow.Cells["Name"].Value?.ToString() == PhantomStudentName) continue;
                    if (studentRow.Cells["Name"].Value == null) continue;

                    var student = new StudentData
                    {
                        Name = studentRow.Cells["Name"].Value?.ToString() ?? "",
                        Group = studentRow.Cells["Group"].Value?.ToString() ?? "",
                        Grades = new List<int>()
                    };

                    int gradeColumnsCount = GetGradeColumnsCount();

                    for (int i = 1; i <= gradeColumnsCount; i++)
                    {
                        string columnName = $"Grade{i}";
                        if (studentRow.Cells[columnName].Value != null &&
                            int.TryParse(studentRow.Cells[columnName].Value.ToString(), out int grade))
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

                var gradeColumnNames = new string[GetGradeColumnsCount()];
                for (int i = 0; i < gradeColumnNames.Length; i++)
                {
                    string columnName = $"Grade{i + 1}";
                    gradeColumnNames[i] = dataGridView.Columns[columnName].HeaderText;
                }

                var data = new SaveData
                {
                    Students = students,
                    GradeColumnsCount = GetGradeColumnsCount(),
                    GradeColumnNames = gradeColumnNames
                };

                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                if (!File.Exists(dataFilePath)) return;

                string json = File.ReadAllText(dataFilePath);
                if (string.IsNullOrWhiteSpace(json)) return;
                
                var data = JsonSerializer.Deserialize<SaveData>(json);

                if (data == null) return;

                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();

                dataGridView.Columns.Add("Name", "👤 Student Name");
                dataGridView.Columns.Add("Group", "🏫 Group");

                if(data.GradeColumnNames != null)
                {
                    for (int i = 1; i <= data.GradeColumnNames.Length; i++)
                    {
                        string columnName = $"Grade{i}";
                        string headerText = data.GradeColumnNames[i - 1];
                        dataGridView.Columns.Add(columnName, headerText);
                    }
                }

                dataGridView.Columns.Add("Total", "📊 Total");
                dataGridView.Columns.Add("Progress", "📈 Progress");
                dataGridView.Columns.Add("Grade", "🎓 Grade");

                ApplyDataGridViewStyling();

                foreach (var student in data.Students)
                {
                    int rowIndex = dataGridView.Rows.Add();
                    DataGridViewRow newRow = dataGridView.Rows[rowIndex];

                    newRow.Cells["Name"].Value = student.Name;
                    newRow.Cells["Group"].Value = student.Group;

                    int gradeColumnCount = data.GradeColumnNames?.Length ?? 0;
                    for (int i = 0; i < student.Grades.Count && i < gradeColumnCount; i++)
                    {
                        string columnName = $"Grade{i + 1}";
                        newRow.Cells[columnName].Value = student.Grades[i];
                    }
                }
                
                RecalculateAllTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyDataGridViewStyling()
        {
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = LightColor;
            dataGridView.GridColor = Color.FromArgb(200, 200, 200);

            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersHeight = 35;

            dataGridView.DefaultCellStyle.BackColor = Color.White;
            dataGridView.DefaultCellStyle.ForeColor = DarkColor;
            dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dataGridView.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = true;
            
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name == "Name") column.Width = 180;
                else if (column.Name == "Group") column.Width = 120;
                else if (column.Name.StartsWith("Grade") && column.Name != "Grade") column.Width = 80;
                else if (column.Name == "Total") column.Width = 80;
                else if (column.Name == "Progress") column.Width = 200;
                else if (column.Name == "Grade") column.Width = 100;

                if (column.Name.StartsWith("Grade") || column.Name == "Total")
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
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
        public string[] GradeColumnNames { get; set; }
    }
}