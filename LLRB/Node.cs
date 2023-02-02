using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLRB
{
    public class Node<T> where T : IComparable<T>
    {

        public Node<T> Left { get; set; }

        public Node<T> Right { get; set; }

        public T Key { get; set; }
        public bool Red { get; set; } = true;


#pragma warning disable
        public Node(T key)
        {
            Key = key;
        }

    }
}
