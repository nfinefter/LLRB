using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLRB
{
    public class Tree<T> where T : IComparable<T>
    {
        private Node<T> root;

#pragma warning disable

        public Tree()
        {
            root = null;
        }
#pragma warning enable

        public void Insert(T key)
        {

            Node<T> curr = root;

            root = Insert(curr, key);

            root.Red = false;
        }
        private Node<T> Insert(Node<T> curr, T key)
        {
            if (curr == null)
            {
                return new Node<T>(key);
            }

            SplitCheck(curr);

            if (curr.Key.CompareTo(key) <= 0)
            {
                curr.Right = Insert(curr.Right, key);
            }
            else if (curr.Key.CompareTo(key) > 0)
            {
                curr.Left = Insert(curr.Left, key);
            }

            if (IsRed(curr.Right))
            {
                curr = RotateLeft(curr);
            }

            if (IsRed(curr.Left) && IsRed(curr.Left.Left))
            {
                curr = RotateRight(curr);
            }

            return curr;
        }
        
        public void Remove(T key)
        {
            if (root.Key.CompareTo(key) == 0)
            {
                root = null;
            }
        }

        private Node<T> Remove(Node<T> curr, T key)
        {

        }

        private void Split(Node<T> curr)
        {
            FlipColor(curr);
        }
        public void SplitCheck(Node<T> curr)
        {
            if (IsRed(curr.Left) && IsRed(curr.Right))
            {
                Split(curr);
            }
        }

        public void FixUp(Node<T> node)
        {
            if (IsRed(node.Right))
            {
                node = RotateLeft(node);
            }
            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }

            //if (IsRed())
            //{
            //    node = RotateRight(node);
            //}

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                FlipColor(node);
            }

            if (node.Left != null)
            {
                if (node.Left.Right != null && node.Left.Left == null && node.Left.Right.Right != null && node.Left.Right.Left == null)
                {
                    node.Left = RotateLeft(node.Left);
                    node.Left = RotateRight(node.Left);
                }
            }
        }

        public Node<T> MoveRedRight(Node<T> node)
        {
            FlipColor(node);

            if (IsRed(node.Left.Left))
            {
                node = RotateRight(node);
                FlipColor(node);
            }

            return node;
        }

        public Node<T> MoveRedLeft(Node<T> node)
        {
            FlipColor(node);

            if (IsRed(node.Right.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                FlipColor(node);

                if (IsRed(node.Right.Right))
                {
                    node.Right = RotateLeft(node.Right);
                }
            }

            return node;
        }

        public Node<T> RotateLeft(Node<T> node)
        {
            Node<T> newHead = node.Right;

            node.Right = node.Right.Left;

            newHead.Left = node;

            newHead.Red = node.Red;
            node.Red = true;

            return newHead;
        }

        public Node<T> RotateRight(Node<T> node)
        {
            Node<T> newHead = node.Left;

            node.Left = node.Left.Right;

            newHead.Right = node;

            newHead.Red = node.Red;
            node.Red = true;

            return newHead;
        }

        public void FlipColor(Node<T> node)
        {
            //Flips node and children colors

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
            if (node == null || node.Red == false)
            {
                return false;
            }

            return true;

        }


    }
}
