using Application.Contracts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Comments;

internal class CommentsService : ICommentsService
{
    private readonly IMapper _mapper;
    private readonly ICommentsRepository _commentsRepository;
    
    public CommentsService(IMapper mapper, ICommentsRepository commentsRepository)
    {
        _mapper = mapper;
        _commentsRepository = commentsRepository;
    }

    public Task AddAsync(CommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        return _commentsRepository.AddAsync(comment);
    }

    public async Task<List<CommentDto>> GetAllAsync()
    {
        var comments = await _commentsRepository.GetAllAsync();
        return _mapper.Map<List<CommentDto>>(comments);
    }
}
