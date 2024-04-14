using AutoMapper;

namespace simple_authentication_client_application.Abstractions;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}