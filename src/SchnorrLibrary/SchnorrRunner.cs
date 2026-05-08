using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public static class SchnorrRunner
    {
        public static ProverSession Run(
            ProverSession session,
            SchnorrParameters param,
            IVerifier verifier,
            BigInteger y)
        {
            var trace = session.Trace;
            session.T = session.Prover.GenerateCommitment(param,trace);
            session.C = verifier.GenerateChallenge(param,trace);
            session.S = session.Prover.Respond(session.C, param, trace);
            session.Result = verifier.Verify(param, y, session.T, session.C, session.S, trace);

            return session;
        }
    }
    
    public class ProverSession
    {
        public string DisplayName { get; set; }      // "Prover 1"
        public string Identity { get; set; }         // "Neighbour / Honest / etc"
        public IProver Prover { get; set; }
        public SchnorrTrace Trace { get; set; } = new();

        public BigInteger T;
        public BigInteger C;
        public BigInteger S;
        public bool Result;
    }


}
