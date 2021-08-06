using MVPTema1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVPTema1.Utils
{
    public class Node
    {
        public Board piece;
        public List<Node> child;
        public Board pieceRemove;

        // Parameterized Constructor
        public Node(Board piece, Board pieceRemove)
        {
            this.piece = piece;
            this.pieceRemove = pieceRemove;
            this.child = new List<Node>();
        }

        public void Insert(Node root, Board parent, Node node)
        {

            // Root is empty then the node wil
            // l become the root
            if (root == null)
            {
                root = node;
            }
            else
            {
                if (root.piece == parent)
                {
                    root.child.Add(node);
                }
                else
                {
                    // Recursive approach to
                    // insert the child
                    int l = root.child.Count();

                    for (int i = 0; i < l; i++)
                    {
                        if (root.child[i].piece == parent)
                            Insert(root.child[i], parent, node);
                        else
                            Insert(root.child[i], parent, node);
                    }
                }
            }
        }

        public List<List<Board>> PrintPaths(Node node)
        {
            Board[] path = new Board[1000];
            List<List<Board>> paths = new List<List<Board>>();
            PrintPathsRecur(node, path, 0, paths);
            return paths;
        }

        /* Recursive helper function -- given a node, and an array 
           containing the path from the root node up to but not  
           including this node, print out all the root-leaf paths.*/

        public void PrintPathsRecur(Node node, Board[] path, int pathLen, List<List<Board>> paths)
        {
            if (node.piece.CellPos == new Point(0,0))
            {
                return;
            }

            /* append this node to the path array */
            path[pathLen] = node.piece; //adaug piesa curenta
            pathLen++;
            path[pathLen] = node.pieceRemove; // adaug piesa pe care trebuie sa o scot
            pathLen++;

            /* it's a leaf, so print the path that led to here  */
            if (node.child.Count==0) // daca nu am niciun copil, printez lista
            {
               paths.Add(PrintArray(path, pathLen));
            }
            else
            {
                //altfel, o iau pe subcategorii, pana ajung la frunza
                // o sa fac recursiv pentru fiecare copil
                for (int index = 0; index < node.child.Count(); index++)
                    PrintPathsRecur(node.child[index], path, pathLen,paths);
            }
        }

        /* Utility function that prints out an array on a line. */
        public List<Board> PrintArray(Board[] ints, int len)
        {
            List<Board> path = new List<Board>();
            for (int i = 0; i < len; i++)
            {
                path.Add(ints[i]);
            }
            return path;
        }

    }
}
