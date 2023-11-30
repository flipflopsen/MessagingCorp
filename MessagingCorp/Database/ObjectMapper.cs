using AutoMapper;
using MessagingCorp.Database.DAO;
using MessagingCorp.EntityManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Database
{
    public class ObjectMapper
    {
        public ObjectMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserRecordDao>());
            var mapper = config.CreateMapper();
        }
    }
}
