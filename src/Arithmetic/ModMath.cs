using System;
using System.Numerics;


namespace Arithmetic
{
    public class ModMath
    {
        public static BigInteger Add(BigInteger a, BigInteger b, BigInteger p) => (a + b) % p;
        public static BigInteger Multiply(BigInteger a, BigInteger b, BigInteger p) => (a *b) % p;
        public static BigInteger Pow(BigInteger a, BigInteger b, BigInteger p) => BigInteger.ModPow(a, b, p);
    }
}