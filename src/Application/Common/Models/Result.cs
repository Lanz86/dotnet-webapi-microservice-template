namespace MicroserviceTemplate.Application.Common.Models;

public enum ResultState : byte
{
    Faulted,
    Success
}
public readonly struct Result<A>
{
    public static readonly Result<A> Bottom = default;

    public readonly ResultState State;
    public readonly A Value;
    public readonly Exception exception;

    public Result(A value)
    {
        State = ResultState.Success;
        Value = value;
        exception = null;
    }

    public Result(Exception e)
    {
        State = ResultState.Faulted;
        exception = e;
        Value = default(A);
    }

    public static implicit operator Result<A>(A value) =>
        new Result<A>(value);

    public bool IsFaulted =>
        State == ResultState.Faulted;

    public bool IsSuccess =>
        State == ResultState.Success;

    public override string ToString() =>
        IsFaulted
            ? exception?.ToString() ?? "(Bottom)"
            : Value?.ToString() ?? "(null)";

    public override bool Equals(object obj) =>
        obj is Result<A> rhs && Equals(rhs);

    public A IfFail(A defaultValue) =>
        IsFaulted
            ? defaultValue
            : Value;
}
