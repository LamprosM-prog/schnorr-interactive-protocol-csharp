using System;
using System.Diagnostics;
using System.Numerics;
using SchnorrLibrary;

public class Program
{
    public static void Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        var trace = new SchnorrTrace();
        trace.ProverType = "HonestProver";

        //Parameters
        var param = new SchnorrParameters
        {
            P = 23,
            Q = 11,
            G = 3
        };   //Note these parameters arent cryptographically secure. Nor is the randomness used.
             //These are used for demostration purposes.
        Console.WriteLine($"Grandma: I choose, a random prime P = 23\n" +
            $"Grandma: And a Q = P-1 = 11\n" +
            $"Grandma: And our Generator G = 3\n" +
            $"Grandma: I am ready to accept commitments!");
        //Key generation 
        var (x, y) = SchnorrSetup.GenerateKeys(param);
        trace.Y = y;
        IProver prover2 = new Attacker1(trace);
        IProver prover = new HonestProver(x, trace);
        IVerifier verifier = new Grandma(trace);
        //attacker1 
       
        //Commitment
        BigInteger t1 = prover2.GenerateCommitment(param);
        trace.T = t1;

        //Challenge 
        BigInteger c1 = verifier.GenerateChallenge(param);
        trace.C = c1;

        //Response
        BigInteger s1 = prover2.Respond(c1 ,param);
        trace.S = s1;

        //Verify
        bool result1 = verifier.Verify(param, y, t1, c1, s1);
        trace.Result = result1;
        
        //-----------------------------------------------------------------------------
        
        // Commitment
        BigInteger t = prover.GenerateCommitment(param);
        trace.T = t;

        // Challenge
        BigInteger c = verifier.GenerateChallenge(param);
        trace.C = c;

        // Response
        BigInteger s = prover.Respond(c, param);
        trace.S = s;
        // Verification
        bool result = verifier.Verify(param, y, t, c, s);
        trace.Result = result;
        
        ConsoleSchnorr.SchnorrTracePrinter.Print(trace);    // I have no idea why the prefix ConsoleSchnorr is needed

        stopwatch.Stop();
        TimeSpan timeSpan = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0000}", timeSpan.Milliseconds);
        Console.WriteLine($"Runtime: {elapsedTime} ms");
        Console.ReadKey();
        
    }
}