﻿using MediatR;
using simple_authentication_client_application.Common;
using simple_authentication_client_application.Users.Queries.Dto;

namespace simple_authentication_client_application.Users.Queries
{
    public class GetAllUsers : PageableRequest, IRequest<ResponseWrapper<PageableList<UserDto>>>
    {
    }
}
