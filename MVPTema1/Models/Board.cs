using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVPTema1.Models
{
    public enum CellColor
    {
        Brown,
        Tan,
        CadetBlue
    }

    public class Board : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private CellColor cell;

        public CellColor Cell
        {
            get { return cell; }
            set { cell = value; OnPropertyChanged("Cell"); }
        }

        private Point cellPos;

        public Point CellPos
        {
            get { return cellPos; }
            set { cellPos = value; OnPropertyChanged("CellPos"); }
        }

        private bool isHitTestVisible = true;

        public bool IsHitTestVisible
        {
            get { return isHitTestVisible; }
            set { isHitTestVisible = value; OnPropertyChanged("IsHitTestVisible"); }
        }

        private Piece piece = null;

        public Piece Piece
        {
            get { return piece; }
            set { piece = value; OnPropertyChanged("Piece"); }
        }

        private string resource = "../Resources/Transparent.png";

        public string Resource
        {
            get { return resource; }
            set { resource = value; OnPropertyChanged("Resource"); }
        }

        public string SetResource()
        {
            string resource = null;

            switch (Piece.Player)
            {
                case Player.White:
                    switch (Piece.Type)
                    {
                        case PieceType.Pawn:
                            resource = "../Resources/WhiteCircle.png";
                            break;
                        case PieceType.King:
                            resource = "../Resources/WhiteCircleKing.png";
                            break;
                    }
                    break;
                case Player.Red:
                    switch (Piece.Type)
                    {
                        case PieceType.Pawn:
                            resource = "../Resources/RedCircle.png";
                            break;
                        case PieceType.King:
                            resource = "../Resources/RedCircleKing.png";
                            break;
                    }
                    break;
                default:
                    resource = null;
                    break;
            }

            return resource;
        }

    }
}
