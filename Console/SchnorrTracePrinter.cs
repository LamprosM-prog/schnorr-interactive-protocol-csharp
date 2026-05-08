using System;
using System.Collections.Generic;
using System.Text;
using SchnorrLibrary;

namespace ConsoleSchnorr
{
    public static class SchnorrTracePrinter
    {
        public static void Print(SchnorrTrace trace)
        {
            foreach (var step in trace.Steps)
            {
                Console.WriteLine($"{step.Speaker}: {step.Message}");
            }
        }
    }
}
