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
        public List<TraceStep> Steps { get; set; } = new();
        public void Add(string speaker, string message)
        {
            Steps.Add(new TraceStep
            {
                Speaker = speaker,
                Message = message
            });
        }
    }
    public class TraceStep
    {
        public string Speaker { get; set; }
        public string Message { get; set; }
    }




}
