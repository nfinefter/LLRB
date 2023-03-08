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

            for (int i = 0; i < 10; i++)
            {   
                num = rand.Next(0, 25);
                if (rand.Next(0, 4) == 3)
                {
                    randoms.Add(num);
                }
                tree.Add(num);
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
                work = tree.DoesWork();
            }
            Console.WriteLine(work);

            int max = tree.Max();
            int min = tree.Min();

            Console.WriteLine($"Max: {max}");
            Console.WriteLine($"Min: {min}");

            int Ceiling = tree.Ceiling(15);
            int Floor = tree.Floor(23);

            Console.WriteLine($"Ceiling: {Ceiling}");
            Console.WriteLine($"Floor: {Floor}");

            foreach (var item in tree)
            {
                Console.WriteLine(item);
            }
        }
        
    }
}