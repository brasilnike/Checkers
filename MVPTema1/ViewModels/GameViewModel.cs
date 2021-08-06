using MVPTema1.Commands;
using MVPTema1.Models;
using MVPTema1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVPTema1.ViewModels
{
    class GameViewModel : BaseNotification
    {
        public GameViewModel()
        {
            LoadData();
            gameServices = new GameServices(this);
            menuServices = new MenuServices(this);
            gameServices.GameRound();
        }

        private GameServices gameServices;
        private MenuServices menuServices;

        private int redWins;

        public int RedWins
        {
            get { return redWins; }
            set { redWins = value; }
        }

        private int whiteWins;

        public int WhiteWins
        {
            get { return whiteWins; }
            set { whiteWins = value; }
        }

        private ObservableCollection<Board> board;

        public ObservableCollection<Board> Board
        {

            get { return board; }
            set { board = value; OnPropertyChanged("Board"); }
        }

        /*private ObservableCollection<Piece> pieceList;

        public ObservableCollection<Piece> PieceList
        {

            get { return pieceList; }
            set { pieceList = value; OnPropertyChanged("PieceList"); }
        }*/

        private string currentPlayer = Player.Red.ToString();

        public string CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; OnPropertyChanged("CurrentPlayer"); }
        }

        private string winner = "";

        public string Winner
        {
            get { return winner; }
            set { winner = value; OnPropertyChanged("Winner"); }
        }

        private ICommand cellClickedCommand;
        public ICommand CellClickedCommand
        {
            get
            {
                if (cellClickedCommand == null)
                {
                    cellClickedCommand = new RelayCommand(gameServices.CellClicked);
                }
                return cellClickedCommand;
            }
        }

        private ICommand saveGameCommand;
        public ICommand SaveGameCommand
        {
            get
            {
                if (saveGameCommand == null)
                {
                    saveGameCommand = new RelayCommand(menuServices.SaveGame);
                }
                return saveGameCommand;
            }
        }

        private ICommand openGameCommand;
        public ICommand OpenGameCommand
        {
            get
            {
                if (openGameCommand == null)
                {
                    openGameCommand = new RelayCommand(menuServices.OpenGame);
                }
                return openGameCommand;
            }
        }

        private ICommand newGameCommand;
        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                {
                    newGameCommand = new RelayCommand(gameServices.NewGame);
                }
                return newGameCommand;
            }
        }

        private ICommand statisticsCommand;
        public ICommand StatisticsCommand
        {
            get
            {
                if (statisticsCommand == null)
                {
                    statisticsCommand = new RelayCommand(menuServices.Statistics);
                }
                return statisticsCommand;
            }
        }

        private ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new RelayCommand(menuServices.About);
                }
                return aboutCommand;
            }
        }

        public void LoadData()
        {
            board = new ObservableCollection<Board>();

            for (int index_i = 0; index_i < 8; index_i++)
                for (int index_j = 0; index_j < 8; index_j++)
                {
                    if (index_i < 3 && (index_i + index_j) % 2 == 1)
                    {
                        Board cellBoard = new Board();
                        cellBoard.Cell = CellColor.Brown;
                        cellBoard.CellPos = new Point(index_i, index_j);
                        cellBoard.Piece = new Piece { Type = PieceType.Pawn, Player = Player.White };
                        cellBoard.Resource = cellBoard.SetResource();
                        board.Add(cellBoard);
                    }
                    else if (index_i > 4 && (index_i + index_j) % 2 == 1)
                    {
                        Board cellBoard = new Board();
                        cellBoard.Cell = CellColor.Brown;
                        cellBoard.CellPos = new Point(index_i, index_j);
                        cellBoard.Piece = new Piece { Type = PieceType.Pawn, Player = Player.Red };
                        cellBoard.Resource = cellBoard.SetResource();
                        board.Add(cellBoard);
                    }
                    else if ((index_i + index_j) % 2 == 0)
                    {
                        Board cellBoard = new Board();
                        cellBoard.Cell = CellColor.Tan;
                        cellBoard.CellPos = new Point(index_i, index_j);
                        board.Add(cellBoard);
                    }
                    else
                    {
                        Board cellBoard = new Board();
                        cellBoard.Cell = CellColor.Brown;
                        cellBoard.CellPos = new Point(index_i, index_j);
                        board.Add(cellBoard);
                    }
                }
        }
    }
}
