using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public class Grandma : IVerifier
    {
        public  BigInteger GenerateChallenge(SchnorrParameters param)
        {
            return Random.Shared.Next(1, (int)param.Q); 
        }
        public bool Verify(SchnorrParameters  param,BigInteger y, BigInteger t,BigInteger c,BigInteger s)
        {
            return SchnorrProtocol.Verify(param ,y, t, c, s);
        }
    }
}
