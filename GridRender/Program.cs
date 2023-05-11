using System;
namespace Main
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            var program = new Engine.RenderLoopControl();
        }
    }
}