using Hunting.BL.Enum;

namespace Hunting.BL.Abstractions;

public interface ICommand<in TParams, out TResult> where TResult : System.Enum
{
    public Func<TParams, bool> CanExecute { get; }
    public Action<TParams> Execute { get; }
    public TResult State { get; }
}