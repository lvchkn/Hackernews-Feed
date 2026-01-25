using Application.Interests;
using Application.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace Api.Controllers;

public static class UsersController
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user/{id}/interests", async (
            [FromRoute] int id, 
            [FromServices] IUsersService usersService) =>
        {
            var userInterests = await usersService.GetInterestsAsync(id);

            return Results.Ok(userInterests);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapPost("/api/user/{id}/interests", async (
            [FromRoute] int id,
            [FromBody] InterestDto interest, 
            [FromServices] IUsersService usersService) =>
        {
            await usersService.AddInterestAsync(id, interest);

            return Results.Created();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapDelete("/api/user/{userId}/interests/{interestId}", async (
            [FromRoute] int userId, 
            [FromRoute] int interestId,
            [FromServices] IUsersService usersService) =>
        {
            await usersService.DeleteInterestAsync(userId, interestId);

            return Results.NoContent();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersInterests);

        app.MapGet("/api/user/{id}/favourites", async (
            [FromRoute] int id,
            [FromServices] IUsersService usersService) =>
        {
            var stories = await usersService.GetFavouriteStoriesAsync(id);

            return Results.Ok(stories);

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersFavourites);

        app.MapPost("/api/user/{id}/favourites/{storyId}", async (
            [FromRoute] int id, 
            [FromRoute] int storyId,
            [FromServices] IUsersService usersService) => 
        {
            await usersService.AddFavouriteStoryAsync(id, storyId);

            return Results.Created();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersFavourites);

        app.MapDelete("/api/user/{id}/favourites/{storyId}", async (
            [FromRoute] int id,
            [FromRoute] int storyId,
            [FromServices] IUsersService usersService) =>
        {
            await usersService.RemoveFavouriteStoryAsync(id, storyId);

            return Results.NoContent();

        }).RequireAuthorization().WithTags(EndpointGroupTags.UsersFavourites);

        return app;
    }
}