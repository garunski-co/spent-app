namespace Spent.Client.Core.Services;

public partial class ClientSideAuthTokenProvider : IAuthTokenProvider
{
    [AutoInject]
    private readonly IStorageService _storageService = default!;

    public bool IsInitialized => true;

    public async Task<string?> GetAccessTokenAsync()
    {
        return await _storageService.GetItem("access_token");
    }
}
