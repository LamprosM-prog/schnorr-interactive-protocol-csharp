using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public class HonestProver : IProver
    {
        private readonly BigInteger _x;
        private readonly SchnorrTrace _trace;
        private BigInteger _r;

        public HonestProver(BigInteger x, SchnorrTrace trace)
        {
            _x = x;
            _trace = trace;
        }

        public BigInteger GenerateCommitment(SchnorrParameters param)
        {
            var (r, t) = SchnorrProtocol.Commit(param, _trace);

            _r = r;

            _trace.Add("Prover", $"Commitment t = {t}");

            return t;
        }

        public BigInteger Respond(BigInteger c, SchnorrParameters param)
        {
            var s = SchnorrProtocol.Respond(_r, c, _x, param.Q, _trace);

            _trace.Add("Prover", $"Response s = {s}");

            return s;
        }
    }
}
