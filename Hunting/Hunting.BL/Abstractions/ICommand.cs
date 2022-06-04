namespace Hunting.BL.Abstractions;

public interface ICommand
{
    string CommandText { get; }
}

public interface ICommand<TParams, TResult> : ICommand 
    where TResult : System.Enum 
    where TParams : IContract
{
    Func<TParams, bool> CanExecute { get; }
    Action<TParams> Execute { get; }
    TResult State { get; set; }
    TParams Contract { get; }
}