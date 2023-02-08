namespace LLRB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();

            Random rand = new Random(0);

            int num = 0;

            for (int i = 0; i < 500; i++)
            {
                num = rand.Next(0, 25);
                tree.Insert(num);
            }
            //Check Insert
            //Make Remove
            bool work = tree.DoesWork();

            Console.WriteLine(work);

            //Trying to manuall break tree below
            //Successful in breaking

            tree.Remove(num);

            Console.WriteLine(tree.DoesWork());
        }
        
    }
}