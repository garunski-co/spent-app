# spent-app


# Secrets

1. initialize secret manager for dotnet
    ```sh
    dotnet user-secrets init --project Server/Server.csproj
    ```
1. set the Plaid Client ID, technically not a secret
    ```sh
    dotnet user-secrets set "AppSettings:PlaidSettings:ClientId" "12345" --project Server/Server.csproj
    ```
1. set the Plaid Secret
    ```sh
    dotnet user-secrets set "AppSettings:PlaidSettings:Secret" "12345" --project Server/Server.csproj
    ```
