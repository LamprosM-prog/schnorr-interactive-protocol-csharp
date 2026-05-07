using System.Numerics;
using SchnorrLibrary;
using Xunit;

namespace Schnorr.Tests;

public class SchnorrProtocolTests
{
    // P = 23 (prime), Q = 11 (prime factor of P-1 = 22), G = 2 (generator of order 11 mod 23)
    // Verify: 2^11 mod 23 = 2048 mod 23 = 1 ✓
    private static readonly SchnorrParameters Params = new()
    {
        P = new BigInteger(23),
        Q = new BigInteger(11),
        G = new BigInteger(2)
    };

    // SchnorrSetup.GenerateKeys

    [Fact]
    public void GenerateKeys_PrivateKeyInValidRange()
    {
        var (x, _) = SchnorrSetup.GenerateKeys(Params);

        Assert.True(x >= BigInteger.One);
        Assert.True(x < Params.Q);
    }

    [Fact]
    public void GenerateKeys_PublicKeyIsGPowXModP()
    {
        var (x, y) = SchnorrSetup.GenerateKeys(Params);

        var expected = Arithmetic.ModMath.Pow(Params.G, x, Params.P);
        Assert.Equal(expected, y);
    }

    [Fact]
    public void GenerateKeys_PublicKeyInValidRange()
    {
        var (_, y) = SchnorrSetup.GenerateKeys(Params);

        Assert.True(y >= BigInteger.One);
        Assert.True(y < Params.P);
    }

    [Fact]
    public void GenerateKeys_ProducesDistinctKeysAcrossMultipleCalls()
    {
        // With Q=11 there are only 10 possible values, so a few calls
        // will almost certainly produce at least two different private keys.
        var xs = Enumerable.Range(0, 20)
            .Select(_ => SchnorrSetup.GenerateKeys(Params).x)
            .ToHashSet();

        Assert.True(xs.Count > 1, "RNG appears to be returning the same value every time.");
    }

    //SchnorrProtocol.Commit 

    [Fact]
    public void Commit_NonceTIsGPowRModP()
    {
        var (r, t) = SchnorrProtocol.Commit(Params);

        var expected = Arithmetic.ModMath.Pow(Params.G, r, Params.P);
        Assert.Equal(expected, t);
    }

    [Fact]
    public void Commit_RandomnessRInValidRange()
    {
        var (r, _) = SchnorrProtocol.Commit(Params);

        Assert.True(r >= BigInteger.One);
        Assert.True(r < Params.Q);
    }

    [Fact]
    public void Commit_NonceTInValidRange()
    {
        var (_, t) = SchnorrProtocol.Commit(Params);

        Assert.True(t >= BigInteger.One);
        Assert.True(t < Params.P);
    }

    //SchnorrProtocol.Respond 

    [Theory]
    [InlineData(5, 2, 3)]   // s = (5 + 2*3) % 11 = 0
    [InlineData(7, 1, 3)]   // s = (7 + 1*3) % 11 = 10
    [InlineData(1, 4, 3)]   // s = (1 + 4*3) % 11 = 2
    [InlineData(9, 10, 3)]  // s = (9 + 10*3) % 11 = 6
    public void Respond_MatchesFormula(int r, int c, int x)
    {
        BigInteger rB = r, cB = c, xB = x;

        var s = SchnorrProtocol.Respond(rB, cB, xB, Params.Q);
        var expected = (rB + cB * xB) % Params.Q;

        Assert.Equal(expected, s);
    }

    [Fact]
    public void Respond_ResultIsNonNegative()
    {
        // Use values that keep the result positive with these small params
        BigInteger r = 1, c = 1, x = 1;
        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.True(s >= BigInteger.Zero);
    }

    // ── SchnorrProtocol.Verify ────────────────────────────────────────────────

    [Fact]
    public void Verify_ReturnsTrueForValidProof()
    {
        var (x, y) = KeysForX(3);
        BigInteger r = 5, c = 2;

        var t = CommitT(r);
        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.True(SchnorrProtocol.Verify(Params, y, t, c, s));
    }

    [Fact]
    public void Verify_ReturnsFalseForTamperedChallenge()
    {
        var (x, y) = KeysForX(3);
        BigInteger r = 5, c = 2;

        var t = CommitT(r);
        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.False(SchnorrProtocol.Verify(Params, y, t, c + 1, s));
    }

    [Fact]
    public void Verify_ReturnsFalseForTamperedResponse()
    {
        var (x, y) = KeysForX(3);
        BigInteger r = 5, c = 2;

        var t = CommitT(r);
        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.False(SchnorrProtocol.Verify(Params, y, t, c, s + 1));
    }

    [Fact]
    public void Verify_ReturnsFalseForTamperedCommitment()
    {
        var (x, y) = KeysForX(3);
        BigInteger r = 5, c = 2;
        BigInteger fakeT = 4;

        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.False(SchnorrProtocol.Verify(Params, y, fakeT, c, s));
    }

    [Fact]
    public void Verify_ReturnsFalseForWrongPublicKey()
    {
        var (x, _) = KeysForX(3);
        var (_, wrongY) = KeysForX(5); // different private key → different y
        BigInteger r = 5, c = 2;

        var t = CommitT(r);
        var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

        Assert.False(SchnorrProtocol.Verify(Params, wrongY, t, c, s));
    }


    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(5, 2, 3)]
    [InlineData(9, 10, 4)]
    [InlineData(3, 5, 7)]
    public void RoundTrip_VerifiesForKnownValues(int r, int c, int x)
    {
        BigInteger rB = r, cB = c;
        var (xB, y) = KeysForX(x);

        var t = CommitT(rB);
        var s = SchnorrProtocol.Respond(rB, cB, xB, Params.Q);

        Assert.True(SchnorrProtocol.Verify(Params, y, t, cB, s));
    }

    [Fact]
    public void RoundTrip_WithGeneratedKeys_AlwaysVerifies()
    {
        BigInteger c = 3;

        for (int i = 0; i < 30; i++)
        {
            var (x, y) = SchnorrSetup.GenerateKeys(Params);
            var (r, t) = SchnorrProtocol.Commit(Params);
            var s = SchnorrProtocol.Respond(r, c, x, Params.Q);

            Assert.True(
                SchnorrProtocol.Verify(Params, y, t, c, s),
                $"Verification failed on iteration {i} with x={x}, r={r}");
        }
    }


    /// <summary>
    /// Deterministically derive a key pair for a fixed private key,
    /// matching exactly what SchnorrSetup.GenerateKeys does internally.
    /// </summary>
    private static (BigInteger x, BigInteger y) KeysForX(int x)
    {
        BigInteger xB = x;
        BigInteger yB = Arithmetic.ModMath.Pow(Params.G, xB, Params.P);
        return (xB, yB);
    }

    /// <summary>
    /// Compute the commitment nonce t for a fixed r,
    /// bypassing Random so tests remain deterministic.
    /// </summary>
    private static BigInteger CommitT(BigInteger r) =>
        Arithmetic.ModMath.Pow(Params.G, r, Params.P);
}