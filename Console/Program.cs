using System;
using System.Numerics;
using SchnorrLibrary;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Schnorr Protocol Demo ===\n");

        // 1. Parameters
        var param = new SchnorrParameters
        {
            P = 23,
            Q = 11,
            G = 3
        };

        // 2. Key generation 
        var (x, y) = SchnorrSetup.GenerateKeys(param);

        Console.WriteLine($"Private key x: {x}");
        Console.WriteLine($"Public key  y: {y}\n");


        IProver prover = new HonestProver(x);
        IVerifier verifier = new Grandma();


        // Commitment
        BigInteger t = prover.GenerateCommitment(param);
        Console.WriteLine($"Commitment t: {t}");

        // Challenge
        BigInteger c = verifier.GenerateChallenge(param);
        Console.WriteLine($"Challenge c: {c}");

        // Response
        BigInteger s = prover.Respond(c, param);
        Console.WriteLine($"Response s: {s}\n");

        // Verification
        bool result = verifier.Verify(param, y, t, c, s);

        Console.WriteLine($"Verification result: {result}");
        Console.ReadKey();
    
    }
}