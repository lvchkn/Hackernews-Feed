FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5245
ENV ASPNETCORE_ENVIRONMENT="Development"
EXPOSE 5245

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HackerNewsCommentsFeed/HackerNewsCommentsFeed.csproj", "HackerNewsCommentsFeed/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Tests/Tests.csproj", "Tests/"]
RUN dotnet restore "HackerNewsCommentsFeed/HackerNewsCommentsFeed.csproj"
COPY . .
WORKDIR "/src/HackerNewsCommentsFeed"
RUN dotnet build "HackerNewsCommentsFeed.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackerNewsCommentsFeed.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackerNewsCommentsFeed.dll"]
