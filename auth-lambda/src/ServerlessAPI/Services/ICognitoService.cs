using ServerlessAPI.Models.Response;

namespace ServerlessAPI.Services
{
    public interface ICognitoService
    {
        Task<TokenUsuarioResponse?> IdentifiqueSeAsync(string? email, string? cpf, string senha, CancellationToken cancellationToken);
    }
}
