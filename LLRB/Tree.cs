using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LLRB
{
    public class Tree<T> : ISortedSet<T> where T : IComparable<T> 
    {
        public Node<T> root;
        public int Count { get; private set;}

        public IComparer<T> Comparer => throw new NotImplementedException();

#pragma warning disable

        public Tree()
        {
            root = null;
        }
#pragma warning enable

        public bool Add(T key)
        {
            Node<T> curr = root;

            int beforeCount = Count;

            root = Add(curr, key);

            root.Red = false;

            return Count != beforeCount;
        }

        private Node<T> Add(Node<T> curr, T key)
        {
            if (curr == null)
            {
                return new Node<T>(key);
            }

            SplitCheck(curr);

            if (curr.Key.CompareTo(key) <= 0)
            {
                curr.Right = Add(curr.Right, key);
            }
            else if (curr.Key.CompareTo(key) > 0)
            {
                curr.Left = Add(curr.Left, key);
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

        public bool Remove(T key)
        {
            if (root == null)
            {
                throw new ArgumentException("Root was null");
            }

            Node<T> curr = root;

            int beforeCount = Count;

            root = Remove(curr, key);

            return Count != beforeCount;

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

                }
                //Either the value still exists on the right

                else if (curr.Key.CompareTo(key) < 0)
                {
                    if (TwoNode(curr.Right))
                    {
                        curr = MoveRedRight(curr);
                    }

                    //In the case of an internal 3-node or 4-node, we still want to MoveRedRight if required then perform a BST delete for an internal node
                    if (ThreeNode(curr.Right) && FourNode(curr.Right))
                    {
                        //WORK ON THIS PART
                        curr = MoveRedRight(curr);

                        curr = FindDeleteLeftMax(curr.Left, out T newKey);

                        curr.Key = newKey;

                        curr = Remove(curr.Right, curr.Key);
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


        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Contains(T item)
        {
            var curr = root;

            Contains(curr, item, out bool contained);

            return contained;
        }

        public Node<T> Contains(Node<T> curr, T item, out bool contained)
        {
            contained = false;

            if (curr == null)
            {
                return null;
            }

            if (curr.Key.CompareTo(item) > 0)
            {
                return Contains(curr.Left, item, out contained);
            }
            else
            {
                if (curr.Key.CompareTo(item) == 0)
                {
                    contained = true;
                    return null;
                }

                else if (curr.Key.CompareTo(item) < 0)
                {
                    return Contains(curr.Left, item, out contained);
                }
            }

            return null;
        }

        public T Max()
        {
            Node<T> curr = root;

            while (curr.Right != null)
            {
                curr = curr.Right;
            }

            return curr.Key;
        }

        public T Min()
        {
            Node<T> curr = root;

            while (curr.Left != null)
            {
                curr = curr.Left;
            }

            return curr.Key;
        }

        public T Ceiling(T item)//Finds value or value directly above it
        {
            Node<T> curr = root;

            curr = Ceiling(curr, item);

            return curr.Key;

        }

        public Node<T> Ceiling(Node<T> curr, T item)
        {
            if (curr.Key.CompareTo(item) > 0)//curr > item
            {
                if (curr.Left.Key.CompareTo(item) < 0)// Left < item
                {
                    //NOT WORKING
                    return curr;
                }

                return Ceiling(curr.Left, item);
            }
            else
            {
                if (curr.Key.CompareTo(item) == 0)
                {
                    return curr;
                }

                else if (curr.Key.CompareTo(item) < 0)
                {
                    return Ceiling(curr.Right, item);
                }
            }
            return null;

        }

        public T Floor(T item)//Finds value or value directly below it, seems to be working
        {
            Node<T> curr = root;

            curr = Floor(curr, item);

            return curr.Key;

        }

        public Node<T> Floor(Node<T> curr, T item)
        {
            if (curr.Key.CompareTo(item) > 0)
            {
                return Floor(curr.Left, item);
            }
            else
            {
                if (curr.Key.CompareTo(item) == 0)
                {
                    return curr;
                }

                else//string check = "if our item is larger than our current value"; (joke)
                {
                    if (curr.Right.Key.CompareTo(item) > 0)
                    {
                        return curr;
                    }

                    return Floor(curr.Right, item);
                }
            }
        }

        public ISortedSet<T> Union(ISortedSet<T> other)
        {
            AddRange(other);

            return this;
        }

        public ISortedSet<T> Intersection(ISortedSet<T> other)
        {
            ISortedSet<T> set = new Tree<T>();

            foreach (var item in this)
            {
                //Tell me about Intersection
            }
            
            return set;
        }

        public struct Enumerator : IEnumerator<T>
        {
            object IEnumerator.Current => Current;

            public T Current => throw new NotImplementedException();

            int state;

            Tree<T> Tree;

            public Enumerator(Tree<T> tree)
            {
                state = 0;
                Tree = tree;
            }

            public void Dispose()
            {
                Tree = null;
            }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                state = 0;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
