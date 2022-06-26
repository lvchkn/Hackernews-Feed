using HackerNewsCommentsFeed.Domain;
using HackerNewsCommentsFeed.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsCommentsFeed.Controllers;

public static class InterestsController
{
    public static IEndpointRouteBuilder MapInterestsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:int}", async ([FromRoute] string id, [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interest = await interestsRepository.GetByIdAsync(id);
    
            return Results.Ok(interest);
            
        }).RequireAuthorization().WithTags("Interests");
            
        app.MapGet("/", async ([FromServices] IInterestsRepository interestsRepository) =>
        {
            var interests = await interestsRepository.GetAllAsync();

            return Results.Ok(interests);
    
        }).RequireAuthorization().WithTags("Comments");
        
        app.MapPost("/", async ([FromBody] Interest interest, [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interestId = await interestsRepository.AddAsync(interest);
            
            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags("Interests");

        app.MapPut("/{id:string}", async ([FromRoute] string id, [FromBody] Interest interest, [FromServices] IInterestsRepository interestsRepository) =>
        {
            var interestId = await interestsRepository.UpdateAsync(id, interest);

            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags("Interests");
        
        app.MapDelete("/{id:string}", async ([FromRoute] string id, [FromServices] IInterestsRepository interestsRepository) =>
        {
            await interestsRepository.DeleteAsync(id);

            return Results.NoContent();
            
        }).RequireAuthorization().WithTags("Interests");

        return app;
    }
}