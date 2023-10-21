using Application.Interests;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils;

namespace Api.Controllers;

public static class InterestsController
{
    public static IEndpointRouteBuilder MapInterestsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/interests/{id}", async (
            [FromRoute] int id, 
            [FromServices] IInterestsService interestsService) =>
        {
            var interest = await interestsService.GetByIdAsync(id);
    
            return interest is null
                ? Results.NotFound()
                : Results.Ok(interest);
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
            
        app.MapGet("/api/interests", async ([FromServices] IInterestsService interestsService) =>
        {
            var interests = await interestsService.GetAllAsync();

            return Results.Ok(interests);
    
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapPost("/api/interests", async (
            [FromBody] InterestDto interest, 
            [FromServices] IInterestsService interestsService) =>
        {
            var id = await interestsService.AddAsync(interest);
            
            return Results.Ok(id);
 
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        app.MapPut("/api/interests/{id}", async (
            [FromRoute] int id, 
            [FromBody] InterestDto interest, 
            [FromServices] IInterestsService interestsService) =>
        {
            await interestsService.UpdateAsync(id, interest);

            return Results.NoContent();
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);
        
        app.MapDelete("/api/interests/{id}", async (
            [FromRoute] int id, 
            [FromServices] IInterestsService interestsService) =>
        {
            await interestsService.DeleteAsync(id);

            return Results.NoContent();
            
        }).RequireAuthorization().WithTags(EndpointGroupTags.Interests);

        return app;
    }
}