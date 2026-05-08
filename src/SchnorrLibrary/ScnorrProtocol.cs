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
            trace?.Add("Prover", $"t = g^r mod p\n{param.G}^{r} mod {param.P}\n = {t}");
            return (r, t);
        }

        public static BigInteger Respond(BigInteger r, BigInteger c, BigInteger x, BigInteger q, SchnorrTrace trace)
        {
            BigInteger s = (r + c * x) % q;
            trace?.Add("Prover", $"(r + c * x) mod q\n({r}+{c}*{x}) mod {q}\n= {s}");
            return s; //The response to the verifier's challenge

        }

        public static bool Verify( SchnorrParameters param,
            BigInteger y,
            BigInteger t,
            BigInteger c,
            BigInteger s,
            SchnorrTrace trace)  // Verify: G^s ≡ t * y^c (mod P)
        {
            trace?.Add("Grandma", $"Let me check your response!\n");
            trace?.Add("Grandma", $"If you are indeed my grandson who knows the secret key x" +
                $"\nthen the equation G^s = t * y^c  will be true!");
            var left = ModMath.Pow(param.G, s, param.P); //Left is what the prover sent. 
            trace?.Add("Grandma", $"\nLeft side first!\nG^s mod P\n" +
                $"{param.G}^{s} mod {param.P}=\n" +
                $"{(int)left}");

            var right = ModMath.Multiply(t, ModMath.Pow(y, c, param.P), param.P); //This is what the verifier already knows.
            trace?.Add("Grandma", $"And now right side\n" +
                $"(t * y ^ c ) mod P\n" +
                $"({t}*{y}^{c}) mod {param.P}\n" +
                $"={(int)right}");

            return left == right; // Schnorr!  if the the equality is true the prover is honest!
        }
    }
}
