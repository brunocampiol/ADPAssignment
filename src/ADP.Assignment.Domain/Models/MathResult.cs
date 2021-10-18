namespace ADP.Assignment.Domain.Models
{
    public class MathResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorInformation { get; set; }
        public MathOperation MathOperation { get; set; }
        public MathOperationResult MathOperationResult { get; set; }
    }
}
