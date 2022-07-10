using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using HackerNewsCommentsFeed.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsCommentsFeed.Controllers;

public static class InterestsController
{
    public static IEndpointRouteBuilder MapInterestsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/interests/{id:int}", async (
            [FromRoute] string id, 
            [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interest = await interestsRepository.GetByIdAsync(id);
    
            return Results.Ok(interest);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
            
        app.MapGet("/interests", async ([FromServices] IInterestsRepository interestsRepository) =>
        {
            var interests = await interestsRepository.GetAllAsync();

            return Results.Ok(interests);
    
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapPost("/interests", async (
            [FromBody] Interest interest, 
            [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interestId = await interestsRepository.AddAsync(interest);
            
            return Results.Ok(interestId);
 
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        app.MapPut("/interests/{id}", async (
            [FromRoute] string id, 
            [FromBody] Interest interest, 
            [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interestId = await interestsRepository.UpdateAsync(id, interest);

            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapDelete("/interests/{id}", async (
            [FromRoute] string id, 
            [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interestId = await interestsRepository.DeleteAsync(id);

            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        return app;
    }
}