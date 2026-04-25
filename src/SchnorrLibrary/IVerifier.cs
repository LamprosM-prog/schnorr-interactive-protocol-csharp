using System;
using System.Numerics;

namespace SchnorrLibrary
{
    public interface IVerifier
    {
        BigInteger GenerateChallenge(SchnorrParameters param);

        bool Verify(SchnorrParameters param,BigInteger y, BigInteger t, BigInteger c, BigInteger s);
    }
}
