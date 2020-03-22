using System.Threading.Tasks;

namespace Archersoft.RunR
{
    public interface ICommand
    {
        Task Execute();
    }
}