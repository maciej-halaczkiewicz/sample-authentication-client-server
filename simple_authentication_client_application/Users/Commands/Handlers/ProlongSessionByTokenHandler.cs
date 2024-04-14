using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_application.Users.Commands.Dto;
using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Users.Commands.Handlers;

public class ProlongSessionByTokenHandler : IRequestHandler<ProlongSessionByToken, SessionResponse>
{
    private readonly IRepository<UserSession> _userSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProlongSessionByTokenHandler(IUnitOfWork unitOfWork, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
        _userSessionRepository = _unitOfWork.GetRepository<UserSession>();
    }

    public async Task<SessionResponse> Handle(ProlongSessionByToken request, CancellationToken cancellationToken)
    {
        var session = await _userSessionRepository.GetAll().Include(x => x.User)
            .Where(x => x.Token == request.Token && x.Expires >= _dateTimeProvider.Now)
            .FirstOrDefaultAsync(cancellationToken);

        // if session exists, prolong it
        if (session != null)
        {
            session.Expires = _dateTimeProvider.Now.AddMinutes(10);
            await _unitOfWork.CommitAsync();
        }

        return _mapper.Map<SessionResponse>(session);
    }
};