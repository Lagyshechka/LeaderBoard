using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace LeaderBoard
{
    public partial class Form1 : Form
    {
        private string _dataFilePath;
        
        public Form1()
        {
            InitializeComponent();
            _dataFilePath = Path.Combine(Application.StartupPath, "students_data.json");
            SetupDataGridView();
            
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
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nameTextBox.Text))
            {
                var row = new object[dataGridView.Columns.Count];
                row[0] = nameTextBox.Text;
                row[1] = groupTextBox.Text;
                
                for (int i = 2; i < dataGridView.Columns.Count - 1; i++)
                {
                    row[i] = 0;
                }
                
                row[dataGridView.Columns.Count - 1] = 0;
                
                dataGridView.Rows.Add(row);
                
                nameTextBox.Text = "";
                groupTextBox.Text = "";
            }
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
                row.Cells[newColumnName].Value = 0;
            }
            
            RecalculateAllTotals();
        }

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 2 && e.ColumnIndex < dataGridView.Columns.Count - 1)
            {
                RecalculateTotalForRow(e.RowIndex);
            }
        }

        private void RecalculateTotalForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count - 1) return;
            
            int total = 0;
            var row = dataGridView.Rows[rowIndex];
            
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
    }
}