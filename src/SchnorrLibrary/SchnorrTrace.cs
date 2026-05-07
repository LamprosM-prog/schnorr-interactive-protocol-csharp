using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public  class SchnorrTrace //Trace to be able to write the computations
    {
        public BigInteger T {  get; set; }
        public BigInteger C { get; set; }
        public BigInteger S { get; set; }
        public BigInteger Y { get; set; }

        public bool Result { get; set; }

        public string ProverType { get; set; } = "Honest";
        public List<string> Steps { get; set; } = new();

    }
}
