using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Windows;
using MVPTema1.ViewModels;
using MVPTema1.Models;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace MVPTema1.Services
{
    class MenuServices
    {
        private readonly string filePath = "../../Resources/Statistics.txt";
        private string text;
        private static ObservableCollection<Board> game;
        GameViewModel gameView;

        public MenuServices(GameViewModel gameView)
        {
            this.gameView = gameView;
        }

        public void SaveGame(object param)
        {
            game = gameView.Board;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(game, Newtonsoft.Json.Formatting.Indented);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json file (*.json)|*.json";
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                File.WriteAllText(saveFileDialog.FileName, output);
            }
        }

        public void OpenGame(object param)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Json file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                text = File.ReadAllText(openFileDialog.FileName);
                gameView.Board = JsonConvert.DeserializeObject<ObservableCollection<Board>>(text);
            }

            for (int index = 0; index < gameView.Board.Count; index++)
            {
                Board piece = gameView.Board[index];
                
                if (piece.Piece != null)
                    if (piece.Piece.Player == Player.Red && piece.IsHitTestVisible == true)
                    {
                        gameView.CurrentPlayer = Player.Red.ToString();
                        break;
                    }
                    else
                    if (piece.Piece.Player == Player.White && piece.IsHitTestVisible == true)
                    {
                        gameView.CurrentPlayer = Player.White.ToString();
                        break;
                    }
            }
        }

        public void Statistics(object param)
        {
            string path = File.ReadAllText(filePath);
            string[] words = path.Split(' ');
            gameView.RedWins = Int32.Parse(words[0]);
            gameView.WhiteWins = Int32.Parse(words[1]);
            StringBuilder statistic = new StringBuilder();
            statistic.Append("Statistica Meciuri:\n");
            statistic.Append("Meciuri castigate jucator rosu: ").Append(words[0] + "\n");
            statistic.Append("Meciuri castigate jucator alb :").Append(words[1] + "\n");
            MessageBox.Show(statistic.ToString());
        }

        public void About(object param)
        {
            StringBuilder about = new StringBuilder();
            about.Append("Nume: Caliman Stefan-Daniel\n");
            about.Append("E-mail:stefan.caliman@student.unitbv.ro\n");
            about.Append("Grupa:10LF391\n");
            about.Append("Descriere joc:\n");
            about.Append("Checkers este un board game de strategie pentru doi jucatori care implica miscari diagonale ale pieselor de joc uniforme si capturi obligatorii prin sarituri peste piesele adversarului.\n");
            MessageBox.Show(about.ToString());
        }
    }
}
