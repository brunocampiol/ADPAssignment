using System.Threading.Tasks;

namespace ADP.Assignment.Domain.Interfaces
{
    public interface IMathService
    {
        Task<string> CalculateInstructionAsync();
    }
}