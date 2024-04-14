using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_application.Common;
using simple_authentication_client_application.Users.Commands.Dto;
using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Users.Commands.Handlers;

public class AuthenticateHandler : IRequestHandler<Authenticate, ResponseWrapper<AuthenticationResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<UserSession> _userSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AuthenticateHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
        _userSessionRepository = _unitOfWork.GetRepository<UserSession>();
    }

    public async Task<ResponseWrapper<AuthenticationResponse>> Handle(Authenticate request, CancellationToken cancellationToken)
    {
        var result = ResponseWrapper<AuthenticationResponse>.Error("Login failed");
        // todo validate request
        // username is not empty, password not empty etc.

        var userName = request.UserName.Trim().ToLower();
        var passwordHash = request.Password.CalculateMD5();
        var user = await _userRepository.GetAll().Where(x => x.UserName == userName && x.Password == passwordHash)
            .Select(x => new { x.Id })
            .FirstOrDefaultAsync(cancellationToken);

        if (user != null)
        {
            // check if session already exists 
            var currentUserSession = await _userSessionRepository.GetAll()
                .Where(x => x.UserId == user.Id && x.Expires >= _dateTimeProvider.Now)
                .Select(x => new { x.Token })
                .FirstOrDefaultAsync(cancellationToken);

            if (currentUserSession != null)
            {
                // return current session
                result = ResponseWrapper<AuthenticationResponse>.Ok(new AuthenticationResponse { Token = currentUserSession.Token });
            }
            else
            {
                // create new session
                var newUserSession = new UserSession
                {
                    UserId = user.Id,
                    Token = Guid.NewGuid(),
                    Expires = _dateTimeProvider.Now.AddMinutes(10),
                };
                _userSessionRepository.Add(newUserSession);
                result = ResponseWrapper<AuthenticationResponse>.Ok(new AuthenticationResponse { Token = newUserSession.Token });
                // todo use snapshot isolation here
                await _unitOfWork.CommitAsync();
            }
        }
        return result;
    }
};