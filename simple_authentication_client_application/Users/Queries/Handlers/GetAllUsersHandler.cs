using AutoMapper;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_domain.Entities;
using MediatR;
using simple_authentication_client_application.Common;
using simple_authentication_client_application.Users.Queries.Dto;

namespace simple_authentication_client_application.Users.Queries.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsers, ResponseWrapper<PageableList<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
    }

    public async Task<ResponseWrapper<PageableList<UserDto>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
    {
        var users = _userRepository.GetAll();
        var userList = await users.PaginatedListAsync<User, UserDto>(request.PageSize, request.PageNumber, _mapper.ConfigurationProvider);
        return ResponseWrapper<PageableList<UserDto>>.Ok(userList);
    }
};