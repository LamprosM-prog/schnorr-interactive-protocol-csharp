using Arithmetic;
using System;
using System.Diagnostics;
using System.Numerics;

namespace SchnorrLibrary
{
    public static class SchnorrProtocol
    {
      
        public static (BigInteger r, BigInteger t) Commit(SchnorrParameters param, SchnorrTrace trace)
        {
            var rng = Random.Shared;
            BigInteger r = rng.Next(1, (int)param.Q);
            BigInteger t = ModMath.Pow(param.G, r, param.P); // The commit that the provers will send 
            trace?.Steps.Add($"t = g^r mod p\n{param.G}^{r} mod {param.P} = {t}");
            return (r, t);
        }

        public static BigInteger Respond(BigInteger r, BigInteger c, BigInteger x, BigInteger q)
        {
            return (r + c * x) % q; //The response to the verifier's challenge
        }

        public static bool Verify( SchnorrParameters param,
            BigInteger y,
            BigInteger t,
            BigInteger c,
            BigInteger s)  // Verify: G^s ≡ t · y^c (mod P)
        {
            var left = ModMath.Pow(param.G, s, param.P);

            var right = ModMath.Multiply(t, ModMath.Pow(y, c, param.P), param.P); 


            return left == right; // Schnorr!  if the the equality is true the prover is honest!
        }
    }
}
