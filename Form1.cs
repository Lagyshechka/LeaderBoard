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

        private Color PrimaryColor = Color.FromArgb(0, 123, 255);
        private Color SecondaryColor = Color.FromArgb(227, 242, 253);
        private Color AccentColor = Color.FromArgb(40, 167, 69);
        private Color DangerColor = Color.FromArgb(220, 53, 69);
        private Color DarkColor = Color.FromArgb(52, 58, 64);
        private Color LightColor = Color.FromArgb(248, 249, 250);

        private Color[] ProgressColors = new Color[]
        {
            Color.FromArgb(220, 53, 69),    // Danger
            Color.FromArgb(255, 193, 7),    // Warning
            Color.FromArgb(40, 167, 69),    // Success
            Color.FromArgb(0, 123, 255),    // Primary
            Color.FromArgb(111, 66, 193)    // Indigo
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
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.Size = new Size(1200, 700);

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

            var buttons = new[] { addGradeColumnButton, renameColumnsButton };
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = PrimaryColor;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point);
                    button.Size = new Size(140, 32);
                    button.Cursor = Cursors.Hand;
                    
                    Color hoverColor = Color.FromArgb(0, 100, 220);
                    Color originalColor = PrimaryColor;
                    button.MouseEnter += (s, e) => { button.BackColor = hoverColor; };
                    button.MouseLeave += (s, e) => { button.BackColor = originalColor; };
                }
            }
            
            if (addButton != null)
            {
                addButton.FlatStyle = FlatStyle.Flat;
                addButton.FlatAppearance.BorderSize = 0;
                addButton.BackColor = AccentColor;
                addButton.ForeColor = Color.White;
                addButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point);
                addButton.Size = new Size(120, 32);
                addButton.Cursor = Cursors.Hand;
                Color hoverColor = Color.FromArgb(35, 140, 60);
                addButton.MouseEnter += (s, e) => { addButton.BackColor = hoverColor; };
                addButton.MouseLeave += (s, e) => { addButton.BackColor = AccentColor; };
            }

            var dangerButtons = new[] { deleteButton, deleteColumnButton };
            foreach (var button in dangerButtons)
            {
                if (button != null)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = DangerColor;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Regular, GraphicsUnit.Point);
                    button.Size = new Size(130, 32);
                    button.Cursor = Cursors.Hand;
                    Color hoverColor = Color.FromArgb(200, 45, 60);
                    button.MouseEnter += (s, e) => { button.BackColor = hoverColor; };
                    button.MouseLeave += (s, e) => { button.BackColor = DangerColor; };
                }
            }
            
            if (deleteButton != null) deleteButton.Text = "Delete Student";
            
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

        private void ApplyDataGridViewStyling()
        {
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.FromArgb(222, 226, 230);
            
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = LightColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = DarkColor;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 60; // Увеличена высота заголовка
            dataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);

            dataGridView.DefaultCellStyle.BackColor = Color.White;
            dataGridView.DefaultCellStyle.ForeColor = DarkColor;
            dataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dataGridView.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dataGridView.DefaultCellStyle.SelectionForeColor = DarkColor;
            dataGridView.RowTemplate.Height = 35;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = LightColor;

            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = true;
            
            // Настройка ширины колонок
            if (dataGridView.Columns["Name"] != null) dataGridView.Columns["Name"].Width = 280; // Увеличена ширина
            if (dataGridView.Columns["Group"] != null) dataGridView.Columns["Group"].Width = 120;
            if (dataGridView.Columns["Total"] != null) dataGridView.Columns["Total"].Width = 80;
            if (dataGridView.Columns["Progress"] != null) dataGridView.Columns["Progress"].Width = 200;
            if (dataGridView.Columns["Grade"] != null) dataGridView.Columns["Grade"].Width = 100;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.StartsWith("Grade") && column.Name != "Grade")
                    column.Width = 80;
            }
        }

        private (Color, Color) GetGradeColors(int score)
        {
            if (score <= 25) return (Color.FromArgb(253, 237, 238), Color.FromArgb(220, 53, 69)); 
            if (score <= 50) return (Color.FromArgb(255, 243, 224), Color.FromArgb(255, 193, 7));
            if (score <= 70) return (Color.FromArgb(232, 245, 233), Color.FromArgb(40, 167, 69));
            if (score <= 89) return (Color.FromArgb(227, 242, 253), Color.FromArgb(0, 123, 255));
            return (Color.FromArgb(243, 229, 245), Color.FromArgb(111, 66, 193));
        }
        
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView.Columns[e.ColumnIndex].Name == "Progress" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                var row = dataGridView.Rows[e.RowIndex];

                if (row.Cells["Name"].Value?.ToString() == PhantomStudentName)
                {
                    e.Graphics.DrawString("-", e.CellStyle.Font, Brushes.Gray, e.CellBounds.X + 5, e.CellBounds.Y + (e.CellBounds.Height - 12) / 2);
                    e.Handled = true;
                    return;
                }

                if (row.Cells["Total"].Value != null && int.TryParse(row.Cells["Total"].Value.ToString(), out int totalScore))
                {
                    int maxPossibleScore = 100;

                    float percentage = (float)totalScore / maxPossibleScore;
                    percentage = Math.Max(0, Math.Min(1, percentage));
                    
                    var progressColor = GetProgressColor(totalScore);

                    var progressBounds = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 10, e.CellBounds.Width - 10, e.CellBounds.Height - 20);
                    
                    using(var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddArc(progressBounds.X, progressBounds.Y, progressBounds.Height, progressBounds.Height, 90, 180);
                        path.AddArc(progressBounds.Right - progressBounds.Height, progressBounds.Y, progressBounds.Height, progressBounds.Height, -90, 180);
                        path.CloseFigure();
                        e.Graphics.FillPath(new SolidBrush(LightColor), path);
                    }

                    var filledWidth = (int)(progressBounds.Width * percentage);
                    if (filledWidth > 0)
                    {
                         using(var path = new System.Drawing.Drawing2D.GraphicsPath())
                        {
                            path.AddArc(progressBounds.X, progressBounds.Y, progressBounds.Height, progressBounds.Height, 90, 180);
                            path.AddArc(progressBounds.X + filledWidth - progressBounds.Height, progressBounds.Y, progressBounds.Height, progressBounds.Height, -90, 180);
                            path.CloseFigure();
                            e.Graphics.FillPath(new SolidBrush(progressColor), path);
                        }
                    }

                    string progressText = $"{totalScore}%";
                    var textSize = e.Graphics.MeasureString(progressText, e.CellStyle.Font);
                    var textLocation = new PointF(e.CellBounds.X + (e.CellBounds.Width - textSize.Width) / 2, e.CellBounds.Y + (e.CellBounds.Height - textSize.Height) / 2);
                    using (var textBrush = new SolidBrush(DarkColor))
                    {
                        e.Graphics.DrawString(progressText, e.CellStyle.Font, textBrush, textLocation);
                    }
                }
                e.Handled = true;
            }
        }
        
        private Color GetProgressColor(float percentage)
        {
            if (percentage <= 25) return ProgressColors[0];
            if (percentage <= 50) return ProgressColors[1];
            if (percentage <= 70) return ProgressColors[2];
            if (percentage <= 89) return ProgressColors[3];
            return ProgressColors[4];
        }

        private string GetGradeBadge(int totalScore, int maxPossibleScore)
        {
            if (maxPossibleScore == 0) return "-";
            float percentage = (float)totalScore / maxPossibleScore * 100;
            if (percentage <= 25) return "G";
            if (percentage <= 50) return "F";
            if (percentage <= 60) return "E";
            if (percentage <= 70) return "D";
            if (percentage <= 80) return "C";
            if (percentage <= 89) return "B";
            return "A";
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
                if (currentRow.Cells[columnName].Value != null && int.TryParse(currentRow.Cells[columnName].Value.ToString(), out int grade))
                {
                    total += grade;
                }
            }

            total = Math.Max(0, Math.Min(100, total));

            currentRow.Cells["Total"].Value = total;

            var gradeCell = currentRow.Cells["Grade"];
            gradeCell.Value = GetGradeBadge(total, 100);
            
            var (bgColor, fgColor) = GetGradeColors(total);
            gradeCell.Style.BackColor = bgColor;
            gradeCell.Style.ForeColor = fgColor;
            gradeCell.Style.Font = new Font("Segoe UI Semibold", 9.5F);

            dataGridView.InvalidateRow(rowIndex);
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
                            RecalculateTotalForRow(existingRow.Index);
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

            int insertIndex = dataGridView.Columns["Total"].Index;
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
            var gradeColumns = dataGridView.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("Grade") && c.Name != "Grade").ToList();

            foreach (var column in gradeColumns)
            {
                string newName = $"Grade{gradeIndex}";
                column.Name = newName;
                if (column.HeaderText.StartsWith("Grade "))
                {
                    column.HeaderText = $"Grade {gradeIndex}";
                }
                gradeIndex++;
            }
        }

        private string[] GetGradeColumnNames()
        {
            return dataGridView.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Name.StartsWith("Grade") && c.Name != "Grade")
                .Select(c => c.HeaderText).ToArray();
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
            
            foreach (DataGridViewCell cell in phantomRow.Cells)
            {
                if(cell.OwningColumn.Name != "Name")
                    cell.Value = "";
            }

            phantomRow.DefaultCellStyle.ForeColor = Color.Gray;
            phantomRow.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            phantomRow.DefaultCellStyle.BackColor = LightColor;
            phantomRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
            phantomRow.DefaultCellStyle.SelectionForeColor = Color.Gray;
        }

        private void SaveData()
        {
            try
            {
                var students = new List<StudentData>();
                var gradeColumns = dataGridView.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Name.StartsWith("Grade") && c.Name != "Grade").ToList();

                foreach (DataGridViewRow studentRow in dataGridView.Rows)
                {
                    if (studentRow.IsNewRow || studentRow.Cells["Name"].Value == null || studentRow.Cells["Name"].Value.ToString() == PhantomStudentName)
                        continue;

                    var student = new StudentData
                    {
                        Name = studentRow.Cells["Name"].Value?.ToString() ?? "",
                        Group = studentRow.Cells["Group"].Value?.ToString() ?? "",
                        Grades = new List<int>()
                    };

                    foreach (var col in gradeColumns)
                    {
                         if (studentRow.Cells[col.Name].Value != null && int.TryParse(studentRow.Cells[col.Name].Value.ToString(), out int grade))
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
                    GradeColumnNames = GetGradeColumnNames()
                };

                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                dataGridView.Columns.Clear();
                dataGridView.Rows.Clear();

                dataGridView.Columns.Add("Name", "👤 Student Name");
                dataGridView.Columns.Add("Group", "🏫 Group");

                if(data.GradeColumnNames != null)
                {
                    for (int i = 0; i < data.GradeColumnNames.Length; i++)
                    {
                        dataGridView.Columns.Add($"Grade{i + 1}", data.GradeColumnNames[i]);
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
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public string[] GradeColumnNames { get; set; }
    }
}