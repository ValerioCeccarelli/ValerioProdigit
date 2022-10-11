FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ValerioProdigit.Api/ValerioProdigit.Api.csproj", "ValerioProdigit.Api/"]
RUN dotnet restore "ValerioProdigit.Api/ValerioProdigit.Api.csproj"
COPY . .
WORKDIR "/src/ValerioProdigit.Api"
RUN dotnet build "ValerioProdigit.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ValerioProdigit.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ValerioProdigit.Api.dll"]
