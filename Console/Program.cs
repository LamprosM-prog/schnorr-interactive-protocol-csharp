using System;
using System.Diagnostics;
using System.Numerics;
using SchnorrLibrary;

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        Console.WriteLine("=== Schnorr Protocol Demo ===\n");

        //Parameters
        var param = new SchnorrParameters
        {
            P = 23,
            Q = 11,
            G = 3
        };   //Note these parameters arent cryptographically secure. Nor is the randomness used.
             //These are used for demostration purposes only.

        //Key generation 
        var (x, y) = SchnorrSetup.GenerateKeys(param);

        Console.WriteLine($"Private key x: {x}");
        Console.WriteLine($"Public key  y: {y}\n");

        IProver prover = new HonestProver(x);
        IVerifier verifier = new Grandma();
        // Commitment
        BigInteger t = prover.GenerateCommitment(param);
        Console.Write("Grandchild:");
        Console.WriteLine($"I choose random number  r={t}");

        // Challenge
        BigInteger c = verifier.GenerateChallenge(param);
        Console.WriteLine($"Challenge c: {c}");

        // Response
        BigInteger s = prover.Respond(c, param);
        Console.WriteLine($"Response s: {s}\n");

        // Verification
        bool result = verifier.Verify(param, y, t, c, s);

        Console.WriteLine($"Verification result: {result}");
        stopwatch.Stop();
        TimeSpan timeSpan = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0000}", timeSpan.Milliseconds);
        Console.WriteLine($"Runtime: {elapsedTime} ms");
        Console.ReadKey();
        
    }
}