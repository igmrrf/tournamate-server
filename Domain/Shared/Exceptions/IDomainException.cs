

namespace Domain.Shared.Exceptions
{
    public  class DomainException(string message, string? name = null) : Exception(message)
    {
        public string? Name { get; } = name;
    }
}
