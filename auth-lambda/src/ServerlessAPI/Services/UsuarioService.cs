using ServerlessAPI.Models.Request;
using ServerlessAPI.Models.Response;

namespace ServerlessAPI.Services
{
    public class UsuarioService(ICognitoService cognitoService) : IUsuarioService
    {
        public async Task<TokenUsuarioResponse?> IdentificarFuncionarioAsync(FuncinarioIdentifiqueSeRequestDto funcinarioIdentifiqueSeRequestDto, CancellationToken cancellationToken) =>
            await cognitoService.IdentifiqueSeAsync(funcinarioIdentifiqueSeRequestDto.Email, null, funcinarioIdentifiqueSeRequestDto.Senha, cancellationToken);

        public async Task<TokenUsuarioResponse?> IdentificarClienteCpfAsync(ClienteIdentifiqueSeRequestDto clienteIdentifiqueSeRequestDto, CancellationToken cancellationToken) =>
            await cognitoService.IdentifiqueSeAsync(null, clienteIdentifiqueSeRequestDto.Cpf, clienteIdentifiqueSeRequestDto.Senha, cancellationToken);
    }
}
