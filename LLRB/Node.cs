﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLRB
{
    public class Node<T> where T : IComparable<T>
    {
        public List<Node<T>> Children { get; set; }

        public Node<T> Left
        { 
            get
            {
                return Children[0];
            }
            set
            {
                Children[0] = value;    
            } 
        }

        public Node<T> Right
        {
            get
            {
                return Children[1];
            }
            set
            {
                Children[1] = value;
            }
        }


        public List<T> Keys { get; set; }
        public bool Red { get; set; } = true;
        
        public Node(List<T> keys)
        {
            Keys = keys;
        }

    }
}
