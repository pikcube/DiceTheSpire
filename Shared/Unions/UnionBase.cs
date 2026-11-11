#pragma warning disable IDE0130
namespace System.Runtime.CompilerServices
#pragma warning restore IDE0130
{
    public interface IUnion
    {
        object? Value { get; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class UnionAttribute : Attribute
    {
    }
}