using Application.Interests;
using Domain.Entities;

namespace Application.Mappers;

public static class InterestMapper
{
    public static Interest ToInterest(this InterestDto interestDto)
    {
        return new Interest
        {
            Id = interestDto.Id,
            Text = interestDto.Text,
        };
    }
    
    public static InterestDto ToInterestDto(this Interest interest)
    {
        return new InterestDto
        {
            Id = interest.Id,
            Text = interest.Text,
        };
    }
}