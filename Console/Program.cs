using System;
using System.Diagnostics;
using System.Numerics;
using SchnorrLibrary;
using ConsoleSchnorr;

public class Program
{
    ///<summary>
    ///Notation:
    ///y = public key
    ///x = secret key
    ///P/p = random prime number used in parameters
    ///Q/q = a (P-1)/2
    ///G = Generator
    ///t = commitment
    ///c = challenge
    ///s = response
    /// </summary>





    public static void Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        var param = new SchnorrParameters { P = 23, Q = 11, G = 3 };
        // These parameters arent cryptographically secure. Nor is the randomness used.
        // These are just for demo purposes.
        Console.WriteLine("Notation and Preliminaries:\n" +
            "y = Public Key\n" +
            "x = Secret Key\n" +
            "P/p = Random Prime\n" +
            "Q = (P-1)/2\n" +
            "G = Generator\n" +
            "t = Commitment\n" +
            "c = Challenge\n" +
            "s = Response\n" +
            "All Arithmetic is modular.\n\n\n");
        
        Console.WriteLine($"Grandma: I choose a random prime number P = {param.P}\n" +
            $"Grandma: And a Q = (P-1)/2 = {param.Q}\n" +
            $"Grandma: And a random generator G<Q G = {param.G}\n" +
            $"Grandma: I am ready to accept commitments!");
        var (x, y) = SchnorrSetup.GenerateKeys(param);
        var rng = Random.Shared;
        var fakeSecretKey = rng.Next(1, (int)param.Q);
        var verifier = new Grandma();
    
        var sessions = new List<ProverSession>
        {
            new ProverSession
            {
                DisplayName  = $"Prover 1",
                Identity = "Neighbour (Attacker)",
                Strategy = "Random Number Guesses/ Uninformed Adversary",
                Prover = new Attacker1()
            },
            new ProverSession
            {
                DisplayName = "Prover 2",
                Identity = "Cousin (Attacker)",
                Strategy = "Fake Secret Key",
                Prover = new Attacker2(fakeSecretKey)
            },
            new ProverSession   
            {
                DisplayName = "Prover 3",
                Identity = "Grandchild (honest)",
                Strategy = "Honest",
                Prover = new HonestProver(x)
                    
            }

        };
        foreach (var session in sessions)
        {

            SchnorrRunner.Run(session, param, verifier, y);
        }
        foreach (var session in sessions)
        {
            Console.WriteLine($"\n=== {session.DisplayName} ===\n");

            foreach (var step in session.Trace.Steps)
            {
                Console.WriteLine($"{step.Speaker}: {step.Message}");
            }
        }
        Console.WriteLine("\n=== Identity Reveal ===\n");

        foreach (var s in sessions)
        {
            Console.WriteLine($"{s.DisplayName}: {s.Identity}\n" +
                $"Strategy: {s.Strategy}");
        }
        stopwatch.Stop();
        TimeSpan timeSpan = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0000}", timeSpan.Milliseconds);
        Console.WriteLine($"Runtime: {elapsedTime} ms");
        Console.ReadKey();
        
    }
}