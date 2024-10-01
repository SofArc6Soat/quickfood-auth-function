using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace ServerlessAPI.Services
{
    public interface ICognitoFactory
    {
        ListUsersRequest CreateListUsersRequestByEmail(string userPoolId, string email);
        ListUsersRequest CreateListUsersRequestByAll(string userPoolId, string? paginationToken);
        InitiateSrpAuthRequest CreateInitiateSrpAuthRequest(string password);
    }
}
