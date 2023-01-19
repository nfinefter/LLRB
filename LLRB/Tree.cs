using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLRB
{
    public class Tree<T> where T : IComparable<T>
    {
        private Node<T> root;

        public Tree()
        {
            root = null;
        }


    }
}
