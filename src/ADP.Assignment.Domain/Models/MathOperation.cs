using System;

namespace ADP.Assignment.Domain.Models
{
    public class MathOperation
    {
        public Guid Id { get; set; }
        public string Operation { get; set; }
        public long Left { get; set; }
        public long Right { get; set; }
    }
}