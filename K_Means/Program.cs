using System;

namespace K_Means
{
    class Program
    {
        static void Main(string[] args)
        {
            K_Means k_Means = new K_Means(7,5,3);

            #region
            k_Means.Start();
   
            #endregion
            Console.WriteLine("Hello World!");
        }
    }
}
