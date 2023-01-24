using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLRB
{
    public class Node<T> where T : IComparable<T>
    {
        public List<Node<T>> Children { get; set; }

        public List<T> Keys { get; set; }
        public bool Red { get; set; } = true;
        
        public Node(List<T> keys)
        {
            Keys = keys;
        }

    }
}
