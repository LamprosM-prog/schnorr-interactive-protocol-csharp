using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SchnorrLibrary
{
    public  class SchnorrTrace //Trace to be able to write the computations
    {
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
