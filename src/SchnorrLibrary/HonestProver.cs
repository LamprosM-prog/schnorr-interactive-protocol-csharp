using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public class HonestProver : IProver //grandchild
    {
        private readonly BigInteger _x;
        private BigInteger _r;

        public HonestProver(BigInteger x)
        {
            _x = x;
        }

        public BigInteger GenerateCommitment(SchnorrParameters param, SchnorrTrace trace)
        {
            var (r, t) = SchnorrProtocol.Commit(param, trace);

            _r = r;

            trace.Add("Prover", $"Commitment t = {t}");

            return t;
        }

        public BigInteger Respond(BigInteger c, SchnorrParameters param, SchnorrTrace trace)
        {
            var s = SchnorrProtocol.Respond(_r, c, _x, param.Q, trace);

            trace.Add("Prover", $"Response s = {s}");

            return s;
        }
    }
}
