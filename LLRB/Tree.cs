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
        public Node<T> root;
        public int Count { get; private set;}

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
            if (root == null)
            {
                throw new ArgumentException("Root was null");
            }

            Node<T> curr = root;

            root = Remove(curr, key);
        }

        private Node<T> Remove(Node<T> curr, T key)
        {
            if (curr == null)
            {
                return null;
            }

            //Will Crash if deleting value doesnt exist

            if (curr.Key.CompareTo(key) > 0)
            {
                if (TwoNode(curr.Left))
                {
                    curr = MoveRedLeft(curr);
                }

                curr.Left = Remove(curr.Left, key);
            }
            else
            {
                //You only need to check if the left child is red then rotate right.
                if (IsRed(curr.Left))
                {
                    curr = RotateRight(curr);
                }
                //If the current node still has the value we want to delete
                if (curr.Key.CompareTo(key) == 0)
                {
                    //and the current node is a leaf node, we can remove it by cutting it's connection to the tree
                    if (curr.Left == null)
                    {
                        return null;
                    }

                    curr.Left = FindDeleteLeftMax(curr.Left, out T newKey);

                    curr.Key = newKey;

                }
                //Either the value still exists on the right
                else if (curr.Key.CompareTo(key) < 0)
                {
                    if (TwoNode(curr.Right))
                    {
                        curr = MoveRedRight(curr);
                    }

                    if (key.CompareTo(curr.Key) == 0)
                    {
                        while (curr.Left != null)
                        {
                            curr = curr.Left;
                        }

                        curr.Right = Remove(curr.Right, curr.Key);
                    }
                    else
                    {
                        curr.Right = Remove(curr.Right, key);
                    }
                }
                //We found the value as part of an internal 3-node or 4-node.
                else if (ThreeNode(curr) || FourNode(curr))
                {
                    //We still want to MoveRedRight if required
                    if (TwoNode(curr.Right))
                    {
                        curr = MoveRedRight(curr);
                    }
                    
                    if (curr.Left != null)
                    {
                        curr = curr.Left;
                    }
                    curr = Remove(curr, curr.Key);
                }
            }

            return FixUp(curr);
        }

        public Node<T> FindDeleteLeftMax(Node<T> curr, out T key)
        {
            if (curr.Right == null)
            {
                key = curr.Key;

                if (curr.Left != null)
                {
                    return curr.Left;
                }

                curr = null;
                return curr;
            }

            if (TwoNode(curr))
            {
                curr = MoveRedLeft(curr);
            }

            curr.Right = FindDeleteLeftMax(curr.Right, out key);

            return FixUp(curr);
        }

        public bool DoesWork()
        {
            if (root == null)
            {
                return false;
            }
            if (root.Red)
            {
                return false;
            }

            int firstBlackCount = -1;

            bool work = DoesWork(root, 0, ref firstBlackCount);
            //Check where tree breaks
            return work;
        }

        private bool DoesWork(Node<T> curr, int blackCount, ref int firstBlackCount)
        {
            if (IsRed(curr) && (IsRed(curr.Left) || IsRed(curr.Right)))
            {
                return false;
            }

            if (curr == null)
            {
                if (firstBlackCount != -1)
                {
                    return blackCount == firstBlackCount;
                }

                firstBlackCount = blackCount;
                
                return true;
            }

            if (IsRed(curr.Right) && !IsRed(curr.Left))
            {
                return false;
            }

            if (!curr.Red)
            {
                blackCount++;
            }

            bool good =  DoesWork(curr.Left, blackCount, ref firstBlackCount) & DoesWork(curr.Right, blackCount, ref firstBlackCount);
            return good;

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

        public Node<T> FixUp(Node<T> node)
        {
            if (IsRed(node.Right))
            {
                node = RotateLeft(node);
            }
            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }

            if (FourNode(node))
            {
                FlipColor(node);
            }

            if ((node.Left != null) && IsRed(node.Left.Right) && !IsRed(node.Left.Left))
            {
                node.Left = RotateLeft(node.Left);

                if (IsRed(node.Left))
                {
                    node = RotateRight(node);
                }
            }

            return node;
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
       
        public void Clear()
        {
            root = null;
        }

        public bool TwoNode(Node<T> curr)
        {
            return !IsRed(curr.Left) && !IsRed(curr);
        }

        public bool ThreeNode(Node<T> curr)
        {
            return IsRed(curr.Left) && !IsRed(curr.Right);
        }

        public bool FourNode(Node<T> curr)
        {
            return IsRed(curr.Left) && IsRed(curr.Right);
        }
    }
}
