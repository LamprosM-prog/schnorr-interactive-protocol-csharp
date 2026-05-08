using System;
using System.Diagnostics;
using System.Numerics;
using SchnorrLibrary;
using ConsoleSchnorr;

public class Program
{
    public static void Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
 
        var param = new SchnorrParameters { P = 23, Q = 11, G = 3 };
        // These parameters arent cryptographically secure. Nor is the randomness used.
        // These are just for demo purposes.

        var (x, y) = SchnorrSetup.GenerateKeys(param);

        var verifier = new Grandma();

        var sessions = new List<ProverSession>
        {
            new ProverSession
            {
                DisplayName  = $"Prover 1",
                Identity = "Neighbour (attacker)",
                Prover = new Attacker1()
            },
            new ProverSession
            {
                DisplayName = "Prover 2",
                Identity = "Grandchild (honest)",
                Prover = new HonestProver(x)
            }

        };
        foreach (var session in sessions)
        {
            session.Trace.Add("System", $"=== {session.DisplayName} begins ===");

            SchnorrRunner.Run(session, param, verifier, y);
        }
        foreach (var session in sessions)
        {
            Console.WriteLine($"\n=== {session.DisplayName} ===\n");

            foreach (var step in session.Trace.Steps)
            {
                Console.WriteLine($"{step.Speaker}: {step.Message}");
            }

            Console.WriteLine($"\nResult: {session.Result}");
        }
        Console.WriteLine("\n=== Identity Reveal ===\n");

        foreach (var s in sessions)
        {
            Console.WriteLine($"{s.DisplayName}: {s.Identity}");
        }
        stopwatch.Stop();
        TimeSpan timeSpan = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0000}", timeSpan.Milliseconds);
        Console.WriteLine($"Runtime: {elapsedTime} ms");
        Console.ReadKey();
        
    }
}