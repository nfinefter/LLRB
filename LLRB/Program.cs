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

            int Ceiling = tree.Ceiling(5);
            int Floor = tree.Floor(300);

            Console.WriteLine($"Ceiling: {Ceiling}");
            Console.WriteLine($"Floor: {Floor}");

            foreach (var item in tree)
            {
                Console.WriteLine(item);
            }
           
            Tree<int> newTree = new Tree<int>();

            rand = new Random(1);

            randoms = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                num = rand.Next(0, 25);
                if (rand.Next(0, 4) == 3)
                {
                    randoms.Add(num);
                }
                newTree.Add(num);
            }

            Console.WriteLine("end");

            foreach (var item in newTree)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("end");

            Tree<int> finalTree = (Tree<int>)tree.Union(newTree);

            foreach (var item in finalTree)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("end");

            Tree<int> final = (Tree<int>)tree.Intersection(finalTree);

            foreach (var item in final)
            {
                Console.WriteLine(item);
            }
        }
        
    }
}