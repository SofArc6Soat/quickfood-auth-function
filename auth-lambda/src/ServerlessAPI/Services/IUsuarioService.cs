using ServerlessAPI.Models.Request;
using ServerlessAPI.Models.Response;

namespace ServerlessAPI.Services
{
    public interface IUsuarioService
    {
        Task<TokenUsuarioResponse?> IdentificarFuncionarioAsync(FuncinarioIdentifiqueSeRequestDto funcinarioIdentifiqueSeRequestDto, CancellationToken cancellationToken);
        Task<TokenUsuarioResponse?> IdentificarClienteCpfAsync(ClienteIdentifiqueSeRequestDto clienteIdentifiqueSeRequestDto, CancellationToken cancellationToken);
    }
}
