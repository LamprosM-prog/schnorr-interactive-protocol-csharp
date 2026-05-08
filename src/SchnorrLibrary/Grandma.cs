using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public class Grandma : IVerifier
    {
        private readonly SchnorrTrace _trace;

        public Grandma(SchnorrTrace trace)
        {
            _trace = trace;
        }

        public BigInteger GenerateChallenge(SchnorrParameters param)
        {
            var c = Random.Shared.Next(1, (int)param.Q);

            _trace.Add("Grandma", $"I choose randomly from a number between and including 1 to {param.Q}" +
                $"\nThe Challenge is c = {c}" +
                $"\nI await responses!");

            return c;
        }

        public bool Verify(SchnorrParameters param, BigInteger y, BigInteger t, BigInteger c, BigInteger s)
        {
            var result = SchnorrProtocol.Verify(param, y, t, c, s, _trace);
            if (result == false) 
            {
                _trace.Add("Grandma", $"Left and right side aren't equal! You aren't my grandchild!");
            } else if(result == true)
            {
                _trace.Add("Grandma", $"Left and right side are equal! You are my grandchild!");
            }

            return result;
        }
    }
}
