# Schnorr Interactive Protocol — Proof of Concept

A C# demonstration of Schnorr's Interactive Proof of Knowledge. The protocol allows a prover to convince a verifier that they know a secret value `x`, without revealing `x` itself.

The scenario: Grandma wants to share her secret cookie recipe only with her grandchild. Many people might try to impersonate the grandchild to get the recipe. Fortunately, Grandma is a mathematical genius — she and her grandchild agreed on a secret key `x` long ago, and Grandma can verify knowledge of `x` without ever asking for it directly.

---

## How the Protocol Works

Schnorr's protocol is a three-move interaction:

1. **Commit** — The prover picks a random `r`, computes `t = G^r mod P`, and sends `t` to the verifier.
2. **Challenge** — The verifier picks a random challenge `c` from `[1, Q]` and sends it to the prover.
3. **Respond** — The prover computes `s = (r + c * x) mod Q` and sends `s` to the verifier.
4. **Verify** — The verifier checks whether `G^s ≡ t * y^c (mod P)`. If true, the prover knows `x`.

The key insight: a prover who does not know `x` cannot compute a valid `s` for an arbitrary challenge `c`. They would have to guess `c` before seeing it, which succeeds with probability `1/Q`.
The verifier never learns the secret x — only whether the prover’s behavior is consistent with knowing it. Making the protocol a zero-knowledge-proof.
---

## Notation

| Symbol | Meaning |
|--------|---------|
| `x` | Secret key (known only to the honest prover) |
| `y` | Public key — `y = G^x mod P` |
| `P` | A prime modulus |
| `Q` | `(P-1)/2` — the subgroup order; challenge and response space |
| `G` | A generator `G < Q` |
| `t` | Commitment sent by the prover |
| `c` | Challenge chosen by the verifier |
| `s` | Response sent by the prover |

All arithmetic is modular.

---

## The Three Provers

The demo runs three provers in sequence to illustrate how the protocol behaves:

| Prover | Identity | Strategy | Expected Outcome |
|--------|----------|----------|-----------------|
| Prover 1 | Neighbour (Attacker) | Random number guesses — no knowledge of protocol | Fails |
| Prover 2 | Cousin (Attacker) | Follows the protocol honestly but uses a fake `x` | Fails |
| Prover 3 | Grandchild (Honest) | Follows the protocol with the real `x` | Passes |

Prover 2 is the most instructive: it shows that knowing the protocol is not enough — you must know the secret.

---

## Security Disclaimer

**This is a proof of concept. Do not use it for anything real.**

- Parameters are tiny (`P = 23`, `Q = 11`, `G = 3`) for readability.
- Randomness uses `System.Random` / `Random.Shared`, which is not cryptographically secure. A real implementation would use `System.Security.Cryptography.RandomNumberGenerator`.
- There are `(int)` casts on `BigInteger` values in several places. These are safe given the small demo parameters but would silently fail with real cryptographic-sized numbers
- Attackers are bound to succeed 1/11 times ~= 10% of the time. This happens because of the small parameters chosen.

---

## Project Structure

```
SchnorrLibrary/
├── SchnorrParameters.cs     — Holds P, Q, G
├── SchnorrSetup.cs          — Generates the keypair (x, y)
├── SchnorrProtocol.cs       — Core protocol logic: Commit, Respond, Verify
├── SchnorrRunner.cs         — Orchestrates a full protocol run for a session
├── SchnorrTrace.cs          — Records each step of the protocol for printing
├── IProver.cs               — Prover interface
├── IVerifier.cs             — Verifier interface
├── HonestProver.cs          — Grandchild: knows real x
├── Attacker1.cs             — Neighbour: random guesses
├── Attacker2.cs             — Cousin: fake secret key
├── Grandma.cs               — The verifier
Arithmetic/
└── ModMath.cs               — Modular arithmetic helpers (Add, Multiply, Pow)
ConsoleSchnorr/
└── Program.cs               — Entry point and console output
```

---

## How to Run

```bash
git clone https://github.com/LamprosM-prog/schnorr-interactive-protocol-csharp
cd schnorr-interactive-protocol-csharp
dotnet run --project ConsoleSchnorr
```

Requires .NET 10 or later.

---

## Further Reading

- Schnorr, C.P. (1991). *Efficient Signature Generation by Smart Cards.* Journal of Cryptology.
- [Schnorr Protocol — Wikipedia](https://en.wikipedia.org/wiki/Schnorr_protocol)
