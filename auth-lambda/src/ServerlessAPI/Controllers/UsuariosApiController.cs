using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Models.Request;
using ServerlessAPI.Services;

namespace ServerlessAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    [Route("usuarios")]
    public class UsuariosApiController(IUsuarioService usuarioService) : ControllerBase
    {
        [HttpPost("cliente/identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf(ClienteIdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await usuarioService.IdentificarClienteCpfAsync(request, cancellationToken);

            request.Senha = "*******";

            return result is null ? BadRequest(result) : Ok(result);
        }

        [HttpPost("funcionario/identifique-se")]
        public async Task<IActionResult> IdentificarFuncionario(FuncinarioIdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await usuarioService.IdentificarFuncionarioAsync(request, cancellationToken);

            request.Senha = "*******";

            return result is null ? BadRequest(result) : Ok(result);
        }
    }
}
