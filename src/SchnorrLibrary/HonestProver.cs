using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public class HonestProver : IProver //Grandchild
    {
        private readonly BigInteger _x;
        private BigInteger _r; 
 
        public HonestProver(BigInteger x) => _x = x;

        public BigInteger GenerateCommitment(SchnorrParameters param)
        {
            var (r, t) = SchnorrProtocol.Commit(param);
            _r = r; 
            return t;
        }

        public BigInteger Respond(BigInteger c, SchnorrParameters param)
        {
            return SchnorrProtocol.Respond(_r, c, _x, param.Q);
        }
    }
}
