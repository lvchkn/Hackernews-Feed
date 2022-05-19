FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/HackerNewsCommentsFeed/HackerNewsCommentsFeed.csproj", "HackerNewsCommentsFeed/"]
RUN dotnet restore "src/HackerNewsCommentsFeed/HackerNewsCommentsFeed.csproj"
COPY . .
WORKDIR "/src/HackerNewsCommentsFeed"
RUN dotnet build "HackerNewsCommentsFeed.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackerNewsCommentsFeed.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackerNewsCommentsFeed.dll"]
