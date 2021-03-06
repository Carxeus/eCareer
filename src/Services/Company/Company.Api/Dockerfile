FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR ..
COPY ["src/Services/Company/Company.Api/Company.Api.csproj", "src/Services/Company/Company.Api/"]
COPY ["src/Services/Company/Company.Tests/Company.Tests.csproj", "src/Services/Company/Company.Tests/"]
COPY ["src/Services/Company/Company.Application/Company.Application.csproj", "src/Services/Company/Company.Application/"]
COPY ["src/Framework/Career.Exceptions/Career.Exceptions.csproj", "src/Framework/Career.Exceptions/"]
COPY ["src/Framework/Career.IoC/Career.IoC.csproj", "src/Framework/Career.IoC/"]
COPY ["src/Framework/Career.Logging/Career.Logging.csproj", "src/Framework/Career.Logging/"]
COPY ["src/Framework/Career.Mvc/Career.Mvc.csproj", "src/Framework/Career.Mvc/"]
COPY ["src/Framework/Career.EntityFramework/Career.EntityFramework.csproj", "src/Framework/Career.EntityFramework/"]
COPY ["src/Framework/Career.Swagger/Career.Swagger.csproj", "src/Framework/Career.Swagger/"]
RUN dotnet restore "src/Services/Company/Company.Api/Company.Api.csproj"
COPY . .
WORKDIR "/src/Services/Company/Company.Api"
RUN dotnet build "Company.Api.csproj" -c Release -o /app/build

FROM build AS test 
RUN dotnet test -c Release ../Company.Tests/Company.Tests.csproj

FROM build AS publish
RUN dotnet publish "Company.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Api.dll"]