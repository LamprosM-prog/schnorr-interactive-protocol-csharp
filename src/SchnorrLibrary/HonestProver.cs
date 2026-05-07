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
        public SchnorrTrace _trace;
        public HonestProver(BigInteger x, SchnorrTrace trace)
        {
            _x = x;
            _trace = trace;
        }
        public BigInteger GenerateCommitment(SchnorrParameters param)
        {
            var (r, t) = SchnorrProtocol.Commit(param, _trace);
            _r = r; 
            return t;
        }

        public BigInteger Respond(BigInteger c, SchnorrParameters param)
        {
            return SchnorrProtocol.Respond(_r, c, _x, param.Q);
        }
    
    }
}
