using System;
using System.Threading;

namespace PizzaConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new MainCommand();

            return app.Execute(args);
        }
    }
}
