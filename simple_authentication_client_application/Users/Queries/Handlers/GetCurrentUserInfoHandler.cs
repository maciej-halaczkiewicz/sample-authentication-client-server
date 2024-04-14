using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_application.Common;
using simple_authentication_client_application.Users.Queries.Dto;

namespace simple_authentication_client_application.Users.Queries.Handlers;

public class GetCurrentUserInfoHandler : IRequestHandler<GetCurrentUserInfo, ResponseWrapper<CurrentUserInfoResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetCurrentUserInfoHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
    }

    public async Task<ResponseWrapper<CurrentUserInfoResponse>> Handle(GetCurrentUserInfo request, CancellationToken cancellationToken)
    {
        var userName = _userContext.UserName;
        var user = await _userRepository.GetAll().Where(x => x.UserName == userName)
            .ProjectTo<CurrentUserInfoResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        if (user != null)
        {
            return ResponseWrapper<CurrentUserInfoResponse>.Ok(user);
        }
        else
        {
            return ResponseWrapper<CurrentUserInfoResponse>.Error("User not found");
        }
    }
};