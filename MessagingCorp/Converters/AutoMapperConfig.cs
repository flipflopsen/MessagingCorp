using AutoMapper;
using MessagingCorp.EntityManagement.BO;

namespace MessagingCorp.Converters
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<User.UserProfile>();
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
