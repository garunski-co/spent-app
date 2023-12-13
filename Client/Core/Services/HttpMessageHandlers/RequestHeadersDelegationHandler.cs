using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Spent.Client.Core.Services.HttpMessageHandlers;

public class RequestHeadersDelegationHandler(AuthDelegatingHandler handler)
    : DelegatingHandler(handler)
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Omit);
        request.SetBrowserResponseStreamingEnabled(true);

#if MultilingualEnabled
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
#endif

        return base.SendAsync(request, cancellationToken);
    }
}
