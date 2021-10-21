using AutoMapper;

namespace StudentsFirst.Api.Monolithic.Tests
{
    public class MapperProvider : IMapperProvider
    {
        public MapperProvider()
        {
            MapperConfiguration configuration = new MapperConfiguration(c => c.AddMaps(typeof(Startup)));
            Mapper = new Mapper(configuration);
        }

        public IMapper Mapper { get; }
    }
}
