namespace LLRB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();

            Random rand = new Random(0);

            for (int i = 0; i < 500; i++)
            {
                tree.Insert(rand.Next(0, 25));
            }
            //Check Insert
            //Make Remove
            bool work = tree.DoesWork();

            Console.WriteLine(work);

            tree.Clear();

            //Trying to manuall break tree below
            //Successful in breaking

            tree.root = new Node<int>(5) { Red = false };
            tree.root.Right = new Node<int>(7) { Red = false };
            tree.root.Left = new Node<int>(5) { Red = false};

            Console.WriteLine(tree.DoesWork());
        }
        
    }
}