using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLRB
{
    public class Tree<T> where T : IComparable<T>
    {
        private Node<T> root;
        const int SplitDegree = 3;

        public Tree()
        {
            root = null;
        }

        public void Insert(T key)
        {
            if (root == null)
            {
                root = new Node<T>(new List<T> { key });
                return;
            }

            Node<T> curr = root;

            Insert(curr, key);
        }
        private void Insert(Node<T> curr, T key)
        {
            if (curr == null)
            {
                return;
            }

            if (curr.Children != null)
            {
                for (int i = 0; i < curr.Keys.Count; i++)
                {
                    if (key.CompareTo(curr.Keys[i]) < 0)
                    { 
                        SplitCheck(curr, i);
                      
                        Insert(curr.Children[i], key);

                        return;
                    }
                    else if (curr.Keys.Count == i + 1 || key.CompareTo(curr.Keys[i + 1]) < 0)
                    {
                        SplitCheck(curr, i + 1);
                     
                        Insert(curr.Children[i + 1], key);

                        return;
                    }
                }
            }

            for (int i = 0; i < curr.Keys.Count; i++)
            {
                if (key.CompareTo(curr.Keys[i]) < 0)
                {
                    curr.Keys.Insert(i, key);
                    return;
                }
                else if (curr.Keys.Count == i + 1 || key.CompareTo(curr.Keys[i + 1]) < 0)
                {
                    curr.Keys.Insert(i + 1, key);
                    return;
                }
            }

        }

        private void Split(Node<T> curr, int i)
        {

            return;
        }
        public void SplitCheck(Node<T> curr, int i)
        {
            if (curr.Left.Red == true && curr.Right.Red == true)
            {
                Split(curr, i);
            }
        }

        public Node<T> RotateLeft(Node<T> node)
        {
            Node<T> newHead = node.Right;

            node.Right = node.Right.Left;

            newHead.Left = node;

            return newHead;
        }

        public Node<T> RotateRight(Node<T> node)
        {
            Node<T> newHead = node.Left;

            node.Right = node.Left.Right;

            newHead.Right = node;

            return newHead;
        }

        public void FlipColor(Node<T> node)
        {
            if (node.Red)
            {
                node.Red = false;
            }
            else
            {
                node.Red = true;
            }

            if (node.Left.Red)
            {
                node.Left.Red = false;
            }
            else
            {
                node.Left.Red = true;
            }

            if (node.Right.Red)
            {
                node.Right.Red = false;
            }
            else
            {
                node.Right.Red = true;
            }

        }

        public bool IsRed(Node<T> node)
        {
            if (node == null)
            {
                return false;
            }
            return true;
        }


    }
}
