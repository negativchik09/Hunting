using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Hunting.BL.Special;
using Timer = System.Timers.Timer;

namespace Hunting.BL;

public class HuntingWorkflow
{
    private Timer _timer;

    public HuntingWorkflow(Timer timer)
    {
        _timer = timer;
        _timer.AutoReset = true;
        _timer.Elapsed += (sender, args) => ExecuteOneTurn();
    }

    public void ExecuteOneTurn()
    {
        throw new NotImplementedException();
    }

    public void StartContinuousExecution(int seconds)
    {
        _timer.Interval = seconds * 1000;
        _timer.Start();
    }

    public void StopContinuousExecution()
    {
        _timer.Stop();
    }

    public event EventHandler<HuntingWorkflowEventParameters>? TurnPassed;

    protected void OnTurnPassed(HuntingWorkflowEventParameters e)
    {
        TurnPassed?.Invoke(this, e);
    }
}