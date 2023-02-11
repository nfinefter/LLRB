namespace LLRB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();

            Random rand = new Random(0);

            int num = 0;

            List<int> randoms = new List<int>();

            for (int i = 0; i < 24; i++)
            {   
                num = rand.Next(0, 25);
                if (rand.Next(0, 5) == 3)
                {
                    randoms.Add(num);
                }
                tree.Insert(num);
            }
            //Check Insert
            //Make Remove
            bool work = tree.DoesWork();

            Console.WriteLine(work);

            //Trying to manuall break tree below
            //Successful in breaking

            for (int i = 0; i < randoms.Count; i++)
            {
                tree.Remove(randoms[i]);
                Console.WriteLine(tree.DoesWork());
            }

            
        }
        
    }
}