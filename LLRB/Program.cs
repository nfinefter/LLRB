namespace LLRB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();

            Random rand = new Random(0);

            for (int i = 0; i < 10; i++)
            {
                tree.Insert(rand.Next(0, 25));
            }
            //Check Insert
            //Make Remove
        }
    }
}