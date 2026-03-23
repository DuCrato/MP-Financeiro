namespace MPTeste.API.Exceptions
{
    /// <summary>
    /// Exceção lançada quando um recurso não é encontrado no banco de dados.
    /// </summary>
    public class NotFoundException(string message) : Exception(message)
    {
    }
}
