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
         
            Console.WriteLine($"Prover Type : {trace.ProverType}");
            foreach (var step in trace.Steps)
            {
                Console.WriteLine(step.ToString());
            }

            Console.WriteLine($"Commitment t = {trace.T}");
            Console.WriteLine($"Challenge c = {trace.C}");
            Console.WriteLine($"Response s = {trace.S}");

            Console.WriteLine($"Public key y = {trace.Y}");
            Console.WriteLine($"Verification result = {trace.Result}");




        }
    
    }
}
