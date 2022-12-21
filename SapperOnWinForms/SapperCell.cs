using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SapperOnWinForms
{
    public enum CellStatus
    {
        Clear,
        Marked,
        Opened
    }
    public class SapperCell : Button
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsMined { get; set; }
        public CellStatus Status { get; set; }

        public SapperCell(int row, int column) : base()
        {
            Row = row;
            Column = column;
            Status = CellStatus.Clear;
            Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 
                20F, 
                System.Drawing.FontStyle.Regular, 
                System.Drawing.GraphicsUnit.Point, 
                ((byte)(204)));
            BackColor = System.Drawing.SystemColors.ControlLight;
        }
        public void SetText(int mineNumber)
        {
            Text = mineNumber.ToString();
            switch (mineNumber)
            {
                case 1:
                    ForeColor = Properties.Settings.Default.Color1;
                    break;
                case 2:
                    ForeColor = Properties.Settings.Default.Color2;
                    break;
                case 3:
                    ForeColor = Properties.Settings.Default.Color3;
                    break;
                case 4:
                    ForeColor = Properties.Settings.Default.Color4;
                    break;
                case 5:
                    ForeColor = Properties.Settings.Default.Color5;
                    break;
                case 6:
                    ForeColor = Properties.Settings.Default.Color6;
                    break;
                case 7:
                    ForeColor = Properties.Settings.Default.Color7;
                    break;
                case 8:
                    ForeColor = Properties.Settings.Default.Color8;
                    break;
                default:
                    ForeColor = Properties.Settings.Default.ColorEmpty;
                    break;
            }
            
        }
        public void Mark()
        {
            if (Status == CellStatus.Clear)
            {
                Image = Properties.Resources.Marker;
                Status = CellStatus.Marked;
            }
            else if(Status == CellStatus.Marked)
            {
                Image = null;
                Status = CellStatus.Clear;
            }
        }
        public void Blow(bool wasClicked)
        {
            if (IsMined)
            {
                Image = Properties.Resources.Mine;
                if (wasClicked)
                {
                    BackColor = System.Drawing.Color.Red;
                }
                return;
            }

        }
        public void Reset()
        {
            Image = null;
            IsMined = false;
            BackColor = System.Drawing.SystemColors.ControlLight;
            Status = CellStatus.Clear;
            Text = "";
        }
    }
}
