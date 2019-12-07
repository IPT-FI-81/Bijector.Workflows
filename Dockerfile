FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY /src .
RUN dotnet restore "Bijector.Workflows.csproj"
RUN dotnet build "Bijector.Workflows.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bijector.Workflows.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bijector.Workflows.dll"]
