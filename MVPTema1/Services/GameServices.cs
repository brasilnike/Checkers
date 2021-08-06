using MVPTema1.Commands;
using MVPTema1.Models;
using MVPTema1.Utils;
using MVPTema1.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVPTema1.Services
{
    class GameServices
    {
        private readonly string filePath = "../../Resources/Statistics.txt";
        private GameViewModel gameView;
        private List<Board> moves = new List<Board>(); //miscarile pe care le fac
        private List<Board> remove = new List<Board>(); //verific daca am mai trecut o data prin ele(cand as avea ghinion sa fac un cerc)
        List<List<Board>> pathList = new List<List<Board>>();//drumul pe care il fac(asta ma ajuta atunci cand pot sari peste mai multe piese). 
        //Listele de pathuri, din ramura(rootlist) imi ia fiecare drum. Apoi verific si iau cel mai lung drum
        List<Node> rootList = new List<Node>(); // arborescenta. Pe baza arborelui fac lista, tine minte toata ramura
        private Board piece1;
        private Board piece2;
        public GameServices(GameViewModel gameView)
        {
            this.gameView = gameView;
            for (int index = 0; index < 4; index++)
            {
                pathList.Add(new List<Board>());
                rootList.Add(new Node(new Board(), new Board()));
            }
        }

        public void NewGame(object param)
        {
            gameView.LoadData();
        }

        public void CellClicked(object param)
        {
            ResetMoves();
            Board piece = param as Board;
            if (piece.Piece != null) // daca inca nu am apasat pe pieasa, adica sa vad mutarile
            {
                piece1 = piece;
                if (gameView.CurrentPlayer == Player.Red.ToString())
                {
                    switch (gameView.Board[(int)piece1.CellPos.X * 8 + (int)piece1.CellPos.Y].Piece.Type) // verific ce fel de piesa e
                    {
                        case PieceType.Pawn:
                            moves.Clear();
                            remove.Clear();
                            pathList[0].Clear();// le resetez pe toate
                            pathList[1].Clear();// 0 si 1 pentru ca noi avem stanga si dreapta
                            rootList[0] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[1] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            if (InBounds((int)(piece1.CellPos.X - 1), (int)piece1.CellPos.Y - 1)) // verific daca piesa de sus stanga se afla pe tabla 
                                if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1].Piece == null)//daca e patrat gol, o bag in lista de mutari simple
                                    moves.Add(gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1]);
                                else
                                    if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1].Piece.Player == Player.White // daca e piesa de culoare alba
                                    && InBounds((int)(piece1.CellPos.X - 2), (int)piece1.CellPos.Y - 2)) // si pot face saritura peste ea(daca e in harta)
                                    if (gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2].Piece == null)//verific daca unde vreau sa sar, e gol
                                    {
                                        rootList[0].piece = piece1; // nodul de start este piesa1
                                        remove.Add(piece1); // adaug in nodul de remove ce trebuie sa scoatem
                                        rootList[0].Insert(rootList[0], rootList[0].piece, new Node(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2], gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1]));
                                        //adaug arborestenta curenta, piesa in sine, si un nou nod care: prima piesa insasi, si cea pe care trebuie sa o elimin  
                                        
                                        
                                        Queue<Board> pieces = new Queue<Board>(); 
                                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2]); // piesa curenta o adaug intr-o coada, practic dupa ce am facut saritura
                                        while (pieces.Count() != 0)
                                        {
                                            Board currentPiece = pieces.Dequeue(); // aleg piesa
                                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y - 1)) // verific daca ma pot duce in stanga
                                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == Player.White
                                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y - 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1]));
                                                            }
                                                        }
                                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y + 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == Player.White
                                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y + 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1]));
                                                            }
                                                        }
                                        }
                                    }

                            if (InBounds((int)(piece1.CellPos.X - 1), (int)piece1.CellPos.Y + 1))
                                if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1].Piece == null)
                                    moves.Add(gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1]);
                                else
                                    if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1].Piece.Player == Player.White
                                    && InBounds((int)(piece1.CellPos.X - 2), (int)piece1.CellPos.Y + 2))
                                    if (gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2].Piece == null)
                                    {
                                        rootList[1].piece = piece1;
                                        remove.Add(piece1);
                                        rootList[1].Insert(rootList[1], rootList[1].piece, new Node(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2], gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1]));
                                        Queue<Board> pieces = new Queue<Board>();
                                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2]);
                                        while (pieces.Count() != 0)
                                        {
                                            Board currentPiece = pieces.Dequeue();
                                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y - 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == Player.White
                                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y - 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1]));
                                                            }
                                                        }

                                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y + 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == Player.White
                                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y + 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1]));
                                                            }
                                                        }
                                        }
                                    }
                            break;

                        case PieceType.King:
                            moves.Clear();
                            remove.Clear();
                            pathList[0].Clear();
                            pathList[1].Clear();
                            pathList[2].Clear();
                            pathList[3].Clear();
                            rootList[0] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[1] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[2] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[3] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            KingMove1(Player.White);
                            break;
                    }
                }
                else
                    if (gameView.CurrentPlayer == Player.White.ToString())
                {
                    switch (gameView.Board[(int)piece1.CellPos.X * 8 + (int)piece1.CellPos.Y].Piece.Type)
                    {
                        case PieceType.Pawn:
                            moves.Clear();
                            remove.Clear();
                            pathList[0].Clear();
                            pathList[1].Clear();
                            rootList[0] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[1] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            if (InBounds((int)(piece1.CellPos.X + 1), (int)piece1.CellPos.Y - 1))
                                if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1].Piece == null)
                                    moves.Add(gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1]);
                                else
                                    if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1].Piece.Player == Player.Red
                                    && InBounds((int)(piece1.CellPos.X + 2), (int)piece1.CellPos.Y - 2))
                                    if (gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2].Piece == null)
                                    {
                                        rootList[0].piece = piece1;
                                        remove.Add(piece1);
                                        rootList[0].Insert(rootList[0], rootList[0].piece, new Node(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2], gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1]));
                                        Queue<Board> pieces = new Queue<Board>();
                                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2]);
                                        while (pieces.Count() != 0)
                                        {
                                            Board currentPiece = pieces.Dequeue();
                                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y - 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == Player.Red
                                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y - 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1]));
                                                            }
                                                        }
                                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y + 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == Player.Red
                                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y + 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1]));
                                                            }
                                                        }
                                        }
                                    }

                            if (InBounds((int)(piece1.CellPos.X + 1), (int)piece1.CellPos.Y + 1))
                                if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1].Piece == null)
                                    moves.Add(gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1]);
                                else
                                    if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1].Piece.Player == Player.Red
                                    && InBounds((int)(piece1.CellPos.X + 2), (int)piece1.CellPos.Y + 2))
                                    if (gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2].Piece == null)
                                    {
                                        rootList[1].piece = piece1;
                                        remove.Add(piece1);
                                        rootList[1].Insert(rootList[1], rootList[1].piece, new Node(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2], gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1]));
                                        Queue<Board> pieces = new Queue<Board>();
                                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2]);
                                        while (pieces.Count() != 0)
                                        {
                                            Board currentPiece = pieces.Dequeue();
                                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y - 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == Player.Red
                                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y - 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                                                rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1]));
                                                            }
                                                        }

                                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y + 1))
                                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == Player.Red
                                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y + 2))
                                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                                        {
                                                            if (!remove.Contains(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]))
                                                            {
                                                                remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                                                rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1]));
                                                            }
                                                        }
                                        }
                                    }
                            break;

                        case PieceType.King:
                            moves.Clear();
                            remove.Clear();
                            pathList[0].Clear();
                            pathList[1].Clear();
                            pathList[2].Clear();
                            pathList[3].Clear();
                            rootList[0] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[1] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[2] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            rootList[3] = new Node(new Board { IsHitTestVisible = false }, new Board { IsHitTestVisible = false });
                            KingMove1(Player.Red);
                            break;
                    }
                }

                for (int index = 0; index < rootList.Count; index++) //aici suntem in lista de arbori
                {
                    if (rootList[index] != null) //verificam daca avem elemente
                    {
                        List<List<Board>> paths = rootList[index].PrintPaths(rootList[index]);
                        for (int index_i = 0; index_i < paths.Count; index_i++)
                        {
                            if (paths[index_i].Count > pathList[index].Count) //aici vrem sa luam drumul cel mai lung
                                pathList[index] = paths[index_i];
                        }

                        for (int index_j = 0; index_j < pathList[index].Count; index_j++)
                            Console.Write(pathList[index][index_j].CellPos + " ");
                    }
                    Console.WriteLine();
                }

                for (int index = 0; index < pathList.Count; index++)
                    for (int index_i = 2; index_i < pathList[index].Count; index_i = index_i + 2)
                        //aici merg din 2 in 2 pentru ca nu vreau sa colorez decat butoanele unde pot sa ma mut piesele 
                    {
                        pathList[index][index_i].IsHitTestVisible = true;// pentru fiecare lista, fac nodurile vizibile, ca sa putem sa apasam pe ele
                        pathList[index][index_i].Cell = CellColor.CadetBlue;// si schimbam culoara butonului 
                    }

                for (int index = 0; index < moves.Count; index++)
                {//nu mai numar din 2 in 2 pentru ca sunt mutari simple
                    moves[index].IsHitTestVisible = true; //aici este pentru mutarile simple
                    moves[index].Cell = CellColor.CadetBlue; 
                }


            }

            if (piece.Piece == null) //daca am apasat pe celula unde vreau sa ajung
            {
                piece2 = piece;

                RemovePiece();//scot piesa
                CheckKing();
                CheckWinner();
                FinishTurn();
            }

        }

        public void FinishTurn()
        {
            if (gameView.CurrentPlayer == Player.Red.ToString())
                gameView.CurrentPlayer = Player.White.ToString();
            else
                gameView.CurrentPlayer = Player.Red.ToString().ToString();

            ResetMoves();
            GameRound();
        }

        public void GameRound() // fac ca atunci cand e randul jucatorului cu piese rosii, doar acestea sa poata fi atinse, la fel si pentru cele albe
        {
            if (gameView.CurrentPlayer == Player.Red.ToString())
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (gameView.Board[i * 8 + j].Piece != null)
                            if (gameView.Board[i * 8 + j].Piece.Player == Player.Red)
                                gameView.Board[i * 8 + j].IsHitTestVisible = true;
                            else
                                gameView.Board[i * 8 + j].IsHitTestVisible = false;
                        else
                            gameView.Board[i * 8 + j].IsHitTestVisible = false;
            }
            else
            if (gameView.CurrentPlayer == Player.White.ToString())
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (gameView.Board[i * 8 + j].Piece != null)
                            if (gameView.Board[i * 8 + j].Piece.Player == Player.White)
                                gameView.Board[i * 8 + j].IsHitTestVisible = true;
                            else
                                gameView.Board[i * 8 + j].IsHitTestVisible = false;
                        else
                            gameView.Board[i * 8 + j].IsHitTestVisible = false;
            }
        }

        private bool InBounds(int x, int y)
        {
            if (x <= -1 || x >= 8)
                return false;
            if (y <= -1 || y >= 8)
                return false;
            return true;
        }

        private void KingMove1(Player player)
        {
            if (InBounds((int)(piece1.CellPos.X - 1), (int)piece1.CellPos.Y - 1))
                if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1].Piece == null)
                    moves.Add(gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1]);
                else
                    if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1].Piece.Player == player
                    && InBounds((int)(piece1.CellPos.X - 2), (int)piece1.CellPos.Y - 2))
                    if (gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2].Piece == null)
                    {
                        rootList[0].piece = piece1;
                        remove.Add(piece1);
                        rootList[0].Insert(rootList[0], rootList[0].piece, new Node(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2], gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y - 1]));
                        Queue<Board> pieces = new Queue<Board>();
                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y - 2]);
                        while (pieces.Count() != 0)
                        {
                            Board currentPiece = pieces.Dequeue();
                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y - 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y - 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1]));

                                        }
                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y + 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y + 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                        {
                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            rootList[0].Insert(rootList[0], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1]));

                                        }
                        }
                    }

            if (InBounds((int)(piece1.CellPos.X - 1), (int)piece1.CellPos.Y + 1))
                if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1].Piece == null)
                    moves.Add(gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1]);
                else
                    if (gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1].Piece.Player == player
                    && InBounds((int)(piece1.CellPos.X - 2), (int)piece1.CellPos.Y + 2))
                    if (gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2].Piece == null)
                    {
                        rootList[1].piece = piece1;
                        remove.Add(piece1);
                        rootList[1].Insert(rootList[1], rootList[1].piece, new Node(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2], gameView.Board[(int)(piece1.CellPos.X - 1) * 8 + (int)piece1.CellPos.Y + 1]));
                        Queue<Board> pieces = new Queue<Board>();
                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X - 2) * 8 + (int)piece1.CellPos.Y + 2]);
                        while (pieces.Count() != 0)
                        {
                            Board currentPiece = pieces.Dequeue();
                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y - 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y - 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                        {
                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y - 1]));

                                        }

                            if (InBounds((int)(currentPiece.CellPos.X - 1), (int)currentPiece.CellPos.Y + 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X - 2), (int)currentPiece.CellPos.Y + 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            rootList[1].Insert(rootList[1], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X - 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X - 1) * 8 + (int)currentPiece.CellPos.Y + 1]));

                                        }
                        }
                    }

            if (InBounds((int)(piece1.CellPos.X + 1), (int)piece1.CellPos.Y - 1))
                if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1].Piece == null)
                    moves.Add(gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1]);
                else
                    if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1].Piece.Player == player
                    && InBounds((int)(piece1.CellPos.X + 2), (int)piece1.CellPos.Y - 2))
                    if (gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2].Piece == null)
                    {
                        rootList[2].piece = piece1;
                        remove.Add(piece1);
                        rootList[2].Insert(rootList[2], rootList[2].piece, new Node(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2], gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y - 1]));
                        Queue<Board> pieces = new Queue<Board>();
                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y - 2]);
                        while (pieces.Count() != 0)
                        {
                            Board currentPiece = pieces.Dequeue();
                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y - 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y - 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            rootList[2].Insert(rootList[2], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1]));

                                        }
                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y + 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y + 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            rootList[2].Insert(rootList[2], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1]));

                                        }
                        }
                    }

            if (InBounds((int)(piece1.CellPos.X + 1), (int)piece1.CellPos.Y + 1))
                if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1].Piece == null)
                    moves.Add(gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1]);
                else
                    if (gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1].Piece.Player == player
                    && InBounds((int)(piece1.CellPos.X + 2), (int)piece1.CellPos.Y + 2))
                    if (gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2].Piece == null)
                    {
                        rootList[3].piece = piece1;
                        remove.Add(piece1);
                        rootList[3].Insert(rootList[3], rootList[3].piece, new Node(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2], gameView.Board[(int)(piece1.CellPos.X + 1) * 8 + (int)piece1.CellPos.Y + 1]));
                        Queue<Board> pieces = new Queue<Board>();
                        pieces.Enqueue(gameView.Board[(int)(piece1.CellPos.X + 2) * 8 + (int)piece1.CellPos.Y + 2]);
                        while (pieces.Count() != 0)
                        {
                            Board currentPiece = pieces.Dequeue();
                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y - 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y - 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2]);
                                            rootList[3].Insert(rootList[3], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y - 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y - 1]));

                                        }

                            if (InBounds((int)(currentPiece.CellPos.X + 1), (int)currentPiece.CellPos.Y + 1))
                                if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece != null)
                                    if (gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1].Piece.Player == player
                                    && InBounds((int)(currentPiece.CellPos.X + 2), (int)currentPiece.CellPos.Y + 2))
                                        if (gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2].Piece == null)
                                        {

                                            remove.Add(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            pieces.Enqueue(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2]);
                                            rootList[3].Insert(rootList[3], currentPiece, new Node(gameView.Board[(int)(currentPiece.CellPos.X + 2) * 8 + (int)currentPiece.CellPos.Y + 2], gameView.Board[(int)(currentPiece.CellPos.X + 1) * 8 + (int)currentPiece.CellPos.Y + 1]));

                                        }
                        }
                    }
        }

        private void ResetMoves() //reseteaza pathurile si lista de mutari
        {
            if (moves.Count != 0)
                for (int index = 0; index < moves.Count; index++)
                {
                    moves[index].IsHitTestVisible = false;
                    moves[index].Cell = CellColor.Brown;
                }

            for (int index = 0; index < pathList.Count; index++)
                if (pathList[index].Count != 0)
                    for (int index_i = 0; index_i < pathList[index].Count; index_i = index_i + 2)
                    {
                        pathList[index][index_i].IsHitTestVisible = false;
                        pathList[index][index_i].Cell = CellColor.Brown;
                    }

            moves.Clear();
        }

        private void RemovePiece()
        {
            for (int index = 0; index < pathList.Count; index++) //iau fiecare path
            {
                if (pathList[index].Contains(piece2)) //verific daca acel path contine piesa a doua, adica unde vreau sa ajung
                    for (int index_i = 2; index_i < pathList[index].Count; index_i = index_i + 2)
                    {
                        if (pathList[index][index_i] == piece2)
                        {
                            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Resource = gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Resource;
                            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Piece = gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Piece;
                            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].IsHitTestVisible = true;
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Piece = null; //piesele pe care trebuie sa le elimin, adica pozitia impara
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Resource = "../Resources/Transparent.png";
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].IsHitTestVisible = false;
                            break;
                        }
                        else // daca n am ajuns la final, pur si simplu le consider ca piese pe care trebuie sa le elimin
                        {
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Piece = null;
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].Resource = "../Resources/Transparent.png";
                            gameView.Board[(int)(pathList[index][index_i + 1].CellPos.X) * 8 + (int)pathList[index][index_i + 1].CellPos.Y].IsHitTestVisible = false;
                        }
                    }
            }

            moves.Clear();

            //fac interschimbarea, tot ce are piesa1, va lua piesa 2
            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Resource = gameView.Board[(int)(piece1.CellPos.X) * 8 + (int)piece1.CellPos.Y].Resource;
            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Piece = gameView.Board[(int)(piece1.CellPos.X) * 8 + (int)piece1.CellPos.Y].Piece;
            gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].IsHitTestVisible = true;

            //piesa 1 va fie goala
            gameView.Board[(int)(piece1.CellPos.X) * 8 + (int)piece1.CellPos.Y].Resource = "../Resources/Transparent.png";
            gameView.Board[(int)(piece1.CellPos.X) * 8 + (int)piece1.CellPos.Y].Piece = null;
            gameView.Board[(int)(piece1.CellPos.X) * 8 + (int)piece1.CellPos.Y].IsHitTestVisible = false;
        }

        private void CheckKing()
        {
            if (piece2.Piece.Player == Player.Red && piece2.CellPos.X == 0)
            {
                gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Resource = "../Resources/RedCircleKing.png";
                gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Piece.Type = PieceType.King;
            }
            if (piece2.Piece.Player == Player.White && piece2.CellPos.X == 7)
            {
                gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Resource = "../Resources/WhiteCircleKing.png";
                gameView.Board[(int)(piece2.CellPos.X) * 8 + (int)piece2.CellPos.Y].Piece.Type = PieceType.King;
            }
        }

        private void CheckWinner()
        {
            int nrRedPieces = 0;
            int nrWhitePieces = 0;
            for (int index = 0; index < gameView.Board.Count; index++)
            {
                if (gameView.Board[index].Piece != null)
                {
                    if (gameView.Board[index].Piece.Player == Player.Red)
                        ++nrRedPieces;

                    if (gameView.Board[index].Piece.Player == Player.White)
                        ++nrWhitePieces;
                }
            }

            if (nrWhitePieces == 0)
            {
                gameView.Winner = "Red Player Wins";
                UpdateStatistic(Player.Red);
                return;
            }

            if (nrRedPieces == 0)
            {
                gameView.Winner = "White Player Wins";
                UpdateStatistic(Player.White);
                return;
            }
        }

        private void UpdateStatistic(Player player)
        {
            string path = File.ReadAllText(filePath);
            string[] words = path.Split(' ');
            gameView.RedWins = Int32.Parse(words[0]);
            gameView.WhiteWins = Int32.Parse(words[1]);
            switch (player)
            {
                case Player.Red:
                    gameView.RedWins++;
                    break;
                case Player.White:
                    gameView.WhiteWins++;
                    break;
            }
            File.WriteAllText(filePath, gameView.RedWins.ToString() + " " + gameView.WhiteWins.ToString());
        }
    }
}
