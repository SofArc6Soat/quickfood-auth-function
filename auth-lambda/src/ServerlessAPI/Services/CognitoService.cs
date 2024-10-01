using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using ServerlessAPI.Configurations;
using ServerlessAPI.Models.Response;

namespace ServerlessAPI.Services
{
    public class CognitoService : ICognitoService
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClientIdentityProvider;
        private readonly ICognitoFactory _cognitoFactory;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userPoolId;

        public CognitoService(IAmazonCognitoIdentityProvider cognitoClientIdentityProvider, ICognitoFactory cognitoFactory, ICognitoSettings cognitoSettings)
        {
            _cognitoClientIdentityProvider = cognitoClientIdentityProvider;
            _cognitoFactory = cognitoFactory;

            var _cognitoSettings = cognitoSettings;

            _clientId = _cognitoSettings.ClientId;
            _clientSecret = _cognitoSettings.ClientSecret;
            _userPoolId = _cognitoSettings.UserPoolId;
        }

        public async Task<TokenUsuarioResponse?> IdentifiqueSeAsync(string? email, string? cpf, string senha, CancellationToken cancellationToken)
        {
            var userPool = new CognitoUserPool(_userPoolId, _clientId, _cognitoClientIdentityProvider);

            var userId = string.Empty;

            if (email is null && cpf is null)
            {
                return ResponseExcecao(email, cpf);
            }

            if (email is not null)
            {
                userId = await ObertUsuarioCognitoPorEmailAsync(email, cancellationToken);
            }

            if (cpf is not null)
            {
                userId = await ObterUserIdPorCpfAsync(cpf, cancellationToken);

            }

            if (!string.IsNullOrEmpty(userId))
            {
                var cognitoUser = new CognitoUser(userId, _clientId, userPool, _cognitoClientIdentityProvider, _clientSecret);

                var authRequest = _cognitoFactory.CreateInitiateSrpAuthRequest(senha);

                try
                {
                    var respose = await cognitoUser.StartWithSrpAuthAsync(authRequest, cancellationToken);

                    if (respose is null)
                    {
                        return ResponseExcecao(email, cpf);
                    }

                    if (respose.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                    {
                        return ResponseExcecao(email, cpf, "Por favor, troque sua senha.");
                    }

                    var timeSpan = TimeSpan.FromSeconds(respose.AuthenticationResult.ExpiresIn);
                    var expiry = DateTimeOffset.UtcNow + timeSpan;

                    return new()
                    {
                        Email = email,
                        Cpf = cpf,
                        AccessToken = respose.AuthenticationResult.AccessToken,
                        RefreshToken = respose.AuthenticationResult.RefreshToken,
                        Expiry = expiry,
                        Errors = null
                    };
                }
                catch (NotAuthorizedException)
                {
                    return ResponseExcecao(email, cpf);
                }
                catch (UserNotConfirmedException)
                {
                    return ResponseExcecao(email, cpf, "Usuário não confirmado. Por favor, verifique seu e-mail.");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return ResponseExcecao(email, cpf);
        }

        private async Task<string?> ObterUserIdPorCpfAsync(string cpf, CancellationToken cancellationToken)
        {
            var usuarios = await ObterTodosUsuariosCognitoAsync(cancellationToken);

            var usuariosComCpf = usuarios.FirstOrDefault(usuario =>
                    usuario.Attributes.Any(attribute =>
                        attribute.Name == "custom:cpf" && attribute.Value == cpf));

            if (usuariosComCpf == null)
            {
                return string.Empty;
            }

            var emailAttribute = usuariosComCpf.Attributes.FirstOrDefault(attr => attr.Name == "email");
            return emailAttribute?.Value;
        }

        private async Task<List<UserType>> ObterTodosUsuariosCognitoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var usuarios = new List<UserType>();
                string? paginationToken = null;

                do
                {
                    var request = _cognitoFactory.CreateListUsersRequestByAll(_userPoolId, paginationToken);

                    var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

                    usuarios.AddRange(response.Users);

                    paginationToken = response.PaginationToken;
                }
                while (!string.IsNullOrEmpty(paginationToken));

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        private async Task<string?> ObertUsuarioCognitoPorEmailAsync(string email, CancellationToken cancellationToken)
        {
            var request = _cognitoFactory.CreateListUsersRequestByEmail(_userPoolId, email);

            var response = await _cognitoClientIdentityProvider.ListUsersAsync(request, cancellationToken);

            var usuario = response.Users.FirstOrDefault();
            return usuario?.Username;
        }

        private static TokenUsuarioResponse ResponseExcecao(string? email, string? cpf, string mensagem = "Usuário ou credenciais inválidas.")
        {
            var response = new TokenUsuarioResponse()
            {
                Email = email,
                Cpf = cpf,
                AccessToken = null,
                RefreshToken = null,
                Expiry = null
            };
            response.AddError(mensagem);

            return response;
        }
    }
}