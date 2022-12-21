using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SapperOnWinForms.Properties;

namespace SapperOnWinForms
{
    public partial class SapperForm : Form
    {
        private List<SapperCell> Cells = new List<SapperCell>();
        private List<SapperCell> ClearCells = new List<SapperCell>();
        private Settings Data = Settings.Default;
        private bool IsLocked;
        private int Mines { get; set; }
        private int MarkedMines { get; set; }
        public SapperForm(int rows, int columns, int mines)
        {
            InitializeComponent();
            SuspendLayout();
            Mines = mines;
            
            int width = columns * (Data.ButtonSize + Data.ButtonIndent) + 
                (2 * Data.XMargin) -
                Data.ButtonIndent;
            int height = rows * (Data.ButtonSize + Data.ButtonIndent) +
                (2 * Data.YMargin) +
                Data.UpperBarSize +
                Data.ButtonIndent;
            ClientSize = new Size(width, height);
            for (int row = 1; row <= rows; row++)
            {
                for (int column = 1; column <= columns; column++)
                {
                    CreateButton(row, column);
                }
            }
            ResetButton.Location = new Point(width / 2 - ResetButton.Size.Width, ResetButton.Location.Y);
            MinesLabel.Location = new Point(width - Data.XMargin - MinesLabel.Size.Width, MinesLabel.Location.Y);
            MineCells();
            SetMinesLabel();
            ResumeLayout(false);
            PerformLayout();
        }
        private void SetMinesLabel()
        {
            MarkedMines = Mines;
            MinesLabel.Text = MarkedMines.ToString();
        }
        private void SetMinesLabel(int count)
        {
            MarkedMines = MarkedMines + count;
            MinesLabel.Text = MarkedMines.ToString();
        }
        private void ResetField()
        {
            ClearCells.Clear();
            foreach (var cell in Cells)
            {
                cell.Reset();
                ClearCells.Add(cell);
            }
            MineCells();
            IsLocked = false;
        }
        private void CreateButton(int row, int column)
        {
            var button = new SapperCell(row, column);
            Cells.Add(button);
            ClearCells.Add(button);
            button.Location = new Point(
                Data.XMargin + (Data.ButtonSize + Data.ButtonIndent) * (row - 1),
                Data.UpperBarSize + Data.YMargin + (Data.ButtonSize + Data.ButtonIndent) * (column - 1));
            button.Name = "B" + row.ToString() + column.ToString();
            button.Size = new Size(Data.ButtonSize, Data.ButtonSize);
            button.UseVisualStyleBackColor = true;
            button.MouseUp += new MouseEventHandler(Cell_MouseUp);
            Controls.Add(button);
        }
        private void MineCells()
        {
            Random rnd = new Random();
            for (int i = 0; i < Mines; i++)
            {
                int number = rnd.Next(0, ClearCells.Count - 1);
                var cell = ClearCells[number];
                cell.IsMined = true;
                ClearCells.Remove(cell);
            }
        }
        private void Cell_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsLocked)
            {
                return;
            }
            var cell = (SapperCell)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (cell.Status == CellStatus.Marked)
                {
                    return;
                }
                if (cell.IsMined)
                {
                    Boom(cell);
                }
                else
                {
                    OpenCell(cell);
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                cell.Mark();
                if (cell.Status == CellStatus.Marked)
                {
                    SetMinesLabel(-1); 
                }
                else
                {
                    SetMinesLabel(1);
                }
            }
        }
        private void Boom(SapperCell clickedCell)
        {
            IsLocked = true;
            foreach (var cell in Cells)
            {
                cell.Blow(cell == clickedCell);
            }
            MessageBox.Show("BOOOOOOOOM!");
        }
        private void OpenCell(SapperCell cell)
        {
            if (cell.Status == CellStatus.Opened)
            {
                return;
            }
            if (cell.Status == CellStatus.Marked)
            {
                cell.Image = null;
            }
            ClearCells.Remove(cell);
            if (ClearCells.Count == 0)
            {
                IsLocked = true;
                MessageBox.Show("You win!");
                return;
            }
            cell.Status = CellStatus.Opened;
            var nearestCells = Cells.FindAll(b => 
            b.Row >= (cell.Row - 1) &&
            b.Row <= (cell.Row + 1) &&
            b.Column >= (cell.Column - 1) &&
            b.Column <= (cell.Column + 1) &&
            b != cell);
            int mines = 0;
            foreach (var nearestCell in nearestCells)
            {
                mines = nearestCell.IsMined ? mines + 1 : mines;
            }
            if (mines == 0)
            {
                cell.BackColor = Data.ColorEmpty;
                foreach (var nearestCell in nearestCells)
                {
                    OpenCell(nearestCell);
                }
            }
            else
            {
                cell.SetText(mines);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetField();
        }
    }
}
