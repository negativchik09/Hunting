namespace Hunting.BL.Abstractions;

public interface ICommand
{
    string CommandText { get; }
}

public interface ICommand<TParams, TResult> : ICommand
    where TParams : IContract
    where TResult : System.Enum
{
    Func<TParams, bool> CanExecute { get; }
    Action<TParams> Execute { get; }
    TResult State { get; set; }
    TParams Contract { get; }
}