namespace MPTeste.API.Exceptions
{
    /// <summary>
    /// Exceção lançada quando uma regra de negócio é violada.
    /// </summary>
    public class BusinessRuleViolationException(string message) : Exception(message)
    {
    }
}
