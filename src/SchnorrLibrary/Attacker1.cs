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
        private readonly SchnorrTrace _trace;

        public Attacker1(SchnorrTrace trace) {
            _trace = trace;
        }

        public BigInteger GenerateCommitment(SchnorrParameters param)
        {
            var rng = Random.Shared;
            var t = rng.Next(1, (int)param.P);
            _trace.Add("Attacker", $"Uhhh t = {t}");
            return t;
           
        
        }
        public BigInteger Respond(BigInteger c,SchnorrParameters param)
        {
            var rng = Random.Shared;
            var s = rng.Next(1, 50);
            _trace.Add("Attacker", $"Hmmm, s = {s}");
            return s;
        }
    
    }
}
