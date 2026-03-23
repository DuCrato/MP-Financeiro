namespace MPTeste.API.Exceptions
{
    /// <summary>
    /// Exceção lançada quando há erro de validação de dados.
    /// </summary>
    public class ValidationException(string message) : Exception(message)
    {
    }
}
