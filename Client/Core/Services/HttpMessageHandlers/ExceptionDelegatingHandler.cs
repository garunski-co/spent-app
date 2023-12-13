using System.Net;

namespace Spent.Client.Core.Services.HttpMessageHandlers;

public class ExceptionDelegatingHandler(IStringLocalizer<AppStrings> localizer, HttpClientHandler httpClientHandler)
    : DelegatingHandler(httpClientHandler)
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var serverCommunicationSuccess = false;

        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            serverCommunicationSuccess = true;

            if (response.IsSuccessStatusCode is false &&
                response.Content.Headers.ContentType?.MediaType?.Contains("application/json",
                    StringComparison.InvariantCultureIgnoreCase) is true)
            {
                if (response.Headers.TryGetValues("Request-ID", out var values) && values.Any())
                {
                    var restError =
                        (await response.Content.ReadFromJsonAsync(AppJsonContext.Default.RestErrorInfo,
                            cancellationToken))!;

                    var exceptionType = typeof(RestErrorInfo).Assembly.GetType(restError.ExceptionType!) ??
                                        typeof(UnknownException);

                    var args = new List<object?>
                    {
                        typeof(KnownException).IsAssignableFrom(exceptionType)
                            ? new LocalizedString(restError.Key!, restError.Message!)
                            : restError.Message!
                    };

                    if (exceptionType == typeof(ResourceValidationException))
                    {
                        args.Add(restError.Payload);
                    }

                    var exp = (Exception)Activator.CreateInstance(exceptionType, args.ToArray())!;

                    throw exp;
                }
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(localizer[AppStrings.YouNeedToSignIn]);
                case HttpStatusCode.Forbidden:
                    throw new ForbiddenException(localizer[AppStrings.ForbiddenException]);
                default:
                    response.EnsureSuccessStatusCode();

                    return response;
            }
        }
        catch (Exception exp) when ((exp is HttpRequestException && serverCommunicationSuccess is false)
                                    || exp is TaskCanceledException { InnerException: TimeoutException })
        {
            throw new ServerConnectionException(nameof(AppStrings.ServerConnectionException), exp);
        }
    }
}
