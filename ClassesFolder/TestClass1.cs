using System;

namespace MyCode
{
    public class MyClass
    {
        public void FirstMethod()
        {
            Console.WriteLine("First method");
        }
        
        public void SecondMethod()
        {
            Console.WriteLine("Second method");
        }
        
        public void ThirdMethod(int a)
        {
            Console.WriteLine("Third method (int)");
        }
        
        public void ThirdMethod(double a)
        {
            Console.WriteLine("Third method (double)");
        }

        private string SomeMethod1()
        {
            return string.Empty;
        }

        public static async Task<string> NewMethodTest()
        {
            return await File.ReadAllTextAsync("");
        }
    }
}