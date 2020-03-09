using Helper;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sample = args[0];
            AnimReader anim = new AnimReader(sample);
            anim.SaveRecord();
        }
    }
}
