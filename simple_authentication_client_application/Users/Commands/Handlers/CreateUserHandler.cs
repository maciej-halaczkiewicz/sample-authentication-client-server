//using simple_authentication_client_application.Abstractions;
//using simple_authentication_client_domain.Entities;
//using MediatR;

//namespace simple_authentication_client_application.Users.Commands.Handlers;

//public class CreateUserHandler : IRequestHandler<CreateUser, User>
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IUnitOfWork _unitOfWork;

//    public CreateUserHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
//    }

//    public async Task<User> Handle(CreateUser request, CancellationToken cancellationToken)
//    {
//        var user = new User
//        {
//            UserName = request.Username,
//            FirstName = request.FirstName,
//            LastName = request.LastName,
//            Age = request.Age,
//            Password = request.Password,
//        };

//        _userRepository.Add(user);
//        await _unitOfWork.CommitAsync();
//        return user;
//    }
//};