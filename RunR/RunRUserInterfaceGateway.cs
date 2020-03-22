using System.Threading.Tasks;
using System.Windows.Threading;

namespace Archersoft.RunR
{
    public class RunRUserInterfaceGateway
    {
        private readonly RunRControl _runRControl;
        private readonly Dispatcher _dispatcher;

        public RunRUserInterfaceGateway(RunRControl runRControl, Dispatcher dispatcher)
        {
            _runRControl = runRControl;
            _dispatcher = dispatcher;
        }

        public async Task ReportProgress(double progressStep)
            => await _dispatcher.InvokeAsync(() => _runRControl.CommandProgressBar.Value += progressStep);
    }
}