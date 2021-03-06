using Application.Contracts;
using Application.Services.Interests;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace HackerNewsCommentsFeed.Controllers;

public static class InterestsController
{
    public static IEndpointRouteBuilder MapInterestsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/interests/{id:int}", async (
            [FromRoute] string id, 
            [FromServices] IInterestsService interestsService) =>
        {
            var interest = await interestsService.GetByIdAsync(id);
    
            return Results.Ok(interest);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
            
        app.MapGet("/interests", async ([FromServices] IInterestsService interestsService) =>
        {
            var interests = await interestsService.GetAllAsync();

            return Results.Ok(interests);
    
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapPost("/interests", async (
            [FromBody] InterestDto interest, 
            [FromServices] IInterestsService interestsService) =>
        {
            var interestId = await interestsService.AddAsync(interest);
            
            return Results.Ok(interestId);
 
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        app.MapPut("/interests/{id}", async (
            [FromRoute] string id, 
            [FromBody] InterestDto interest, 
            [FromServices] IInterestsService interestsService) =>
        {
            var interestId = await interestsService.UpdateAsync(id, interest);

            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapDelete("/interests/{id}", async (
            [FromRoute] string id, 
            [FromServices] IInterestsService interestsService) =>
        {
            var interestId = await interestsService.DeleteAsync(id);

            return Results.Ok(interestId);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        return app;
    }
}