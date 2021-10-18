using ADP.Assignment.Domain.Models;
using System.Threading.Tasks;

namespace ADP.Assignment.Domain.Interfaces
{
    public interface IMathService
    {
        Task<MathResult> CalculateInstructionAsync();
    }
}