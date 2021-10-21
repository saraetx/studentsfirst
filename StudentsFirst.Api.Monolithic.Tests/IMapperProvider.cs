using AutoMapper;

namespace StudentsFirst.Api.Monolithic.Tests
{
    public interface IMapperProvider
    {
        public IMapper Mapper { get; }
    }
}
