using LLRB;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Insert()
        {
            Tree<int> tree = new Tree<int>();

            tree.Insert(1);
            tree.Insert(2);
            tree.Insert(3);
            tree.Insert(4);
            tree.Insert(5);
        }
    }
}