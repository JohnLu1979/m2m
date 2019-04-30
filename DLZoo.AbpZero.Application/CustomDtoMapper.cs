using AutoMapper;
//using MyTempProject.Authorization.Users;
//using MyTempProject.Authorization.Users.Dto;
//using MyTempProject.Dictionary;

namespace MyTempProject
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings()
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal();

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal()
        {
            //Mapper.CreateMap<User, UserEditDto>()
            //    .ForMember(dto => dto.Password, options => options.Ignore())
            //    .ReverseMap()
            //    .ForMember(user => user.Password, options => options.Ignore());

            //◊‘∂®“Â”≥…‰
            //var codeDtoMapper = Mapper.CreateMap<Entities.Dictionary.Code, CodeDto>();
            //codeDtoMapper.ForMember(dto => dto.TypeName, map => map.MapFrom(m => m.Type.Name));
        }
    }
}