using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace ServerlessAPI.Services
{
    public class CognitoFactory : ICognitoFactory
    {
        public ListUsersRequest CreateListUsersRequestByEmail(string userPoolId, string email) =>
            new()
            {
                UserPoolId = userPoolId,
                Filter = $"email = \"{email}\""
            };

        public ListUsersRequest CreateListUsersRequestByAll(string userPoolId, string? paginationToken) =>
            new()
            {
                UserPoolId = userPoolId,
                PaginationToken = paginationToken
            };

        public InitiateSrpAuthRequest CreateInitiateSrpAuthRequest(string password) =>
            new()
            {
                Password = password
            };
    }
}
