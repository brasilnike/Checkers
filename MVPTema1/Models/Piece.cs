using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVPTema1.Models
{
    public enum PieceType
    {
        Pawn,
        King
    }

    public enum Player
    {
        Red,
        White
    }


    public class Piece : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private PieceType pieceType;

        public PieceType Type
        {
            get { return pieceType; }
            set { pieceType = value; OnPropertyChanged("Type"); }
        }

        private Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; OnPropertyChanged("Player"); }
        }
    }
}
