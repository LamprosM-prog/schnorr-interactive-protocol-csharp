using SchnorrLibrary;
using System;
using System.Numerics;
using Arithmetic;

namespace SchnorrLibrary
{

    public static class SchnorrSetup
    {
        public static (BigInteger x, BigInteger y) GenerateKeys(SchnorrParameters param)
        {
            var rng =  Random.Shared;
            BigInteger x = rng.Next(1, (int)param.Q);
            BigInteger y = ModMath.Pow(param.G, x, param.P);

            return (x, y);
        }
    }

}

