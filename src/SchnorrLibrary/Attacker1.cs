using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    //Random number strategy
    //Neighbour
    
    public class Attacker1 : IProver
    {
 
        public BigInteger GenerateCommitment(SchnorrParameters param, SchnorrTrace trace)
        {
            var rng = Random.Shared;
            var t = rng.Next(1, (int)param.P);
            trace.Add("Prover", $"Uhhh t = {t}");
            return t;
           
        
        }
        public BigInteger Respond(BigInteger c,SchnorrParameters param, SchnorrTrace trace)
        {
            var rng = Random.Shared;
            var s = rng.Next(1, 50); // arbitrary guess, no knowledge of protocol
            trace.Add("Prover", $"Hmmm, s = {s}");
            return s;
        }
    
    }
}
