using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public  class HonestProver : IProver
    {
        public BigInteger GenerateCommitment(SchnorrParameters param)
        {
            return SchnorrProtocol.Commit(param);
        }
        public BigInteger Respond(BigInteger challenge, BigInteger r, BigInteger c, BigInteger x, BigInteger q)
        {
            return Respond( challenge, r, c,  x,  q);
        }
    }
}
