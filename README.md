[![.NET](https://github.com/lvchkn/Hackernews-Feed/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/lvchkn/Hackernews-Feed/actions/workflows/build-and-test.yml)

# Hackernews-Feed

This API acts as a kind of proxy backend for my custom [HN client](https://github.com/lvchkn/hn-client) and provides RESTful endpoints that allow for sorting, filtering and paging stories from Hackernews.

This is only partially implemented and still is under development, but the idea is that users may authenticate to the custom client page via Github, specify their topics of interest, and get a personal stories feed based on their preferences.

## How to run locally

1. Create a `.env` file in the project's root directory and fill it in according to the `.env.example` file.
1. Start the compose stack

   ```bash
   docker compose -f docker-compose.local up
   ```

1. Run the following command from the root's directory:

   ```bash
    dotnet run --project HackerNewsCommentsFeed/HackerNewsCommentsFeed.csproj
   ```

1. Navigate to `http://localhost/index.html` or `https://localhost:7245` to see the Swagger OpenAPI definition.

## How to run the tests

Just run the `dotnet test` command in the project's root directory.
Note that the entries in the `.env` file related to the authentication must be prepopulated before running the tests to avoid errors during the test server's startup.
