using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public interface IProver
    {
        BigInteger GenerateCommitment(
            SchnorrParameters param,
            SchnorrTrace trace);

        BigInteger Respond(
            BigInteger c,
            SchnorrParameters param,
            SchnorrTrace trace);
    }
}
