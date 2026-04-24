using System;
using System.Numerics;
using Arithmetic;

namespace SchnorrLibrary
{
    public static class SchnorrProtocol
    {
        public static (BigInteger r, BigInteger t) Commit(SchnorrParameters param)
        {
            var rng = Random.Shared;
            BigInteger r = rng.Next(1, (int)param.Q);
            BigInteger t = ModMath.Pow(param.G, r, param.P);
            return (r, t);
        }

        public static BigInteger Respond(BigInteger r, BigInteger c, BigInteger x, BigInteger q)
        {
            return (r + c * x) % q;
        }

        public static bool Verify( SchnorrParameters param,
            BigInteger y,
            BigInteger t,
            BigInteger c,
            BigInteger s)
        {
            var left = ModMath.Pow(param.G, s, param.P);

            var right = ModMath.Multiply(t, ModMath.Pow(y, c, param.P), param.P);
            
            return left == right;
        }
    }
}
