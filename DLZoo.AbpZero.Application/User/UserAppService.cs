using Abp.AutoMapper;
using Abp.Domain.Repositories;
using MyTempProject.Configuration;
using MyTempProject.EntityFramework;
using MyTempProject.Temps.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyTempProject.Base;
using MyTempProject.Base.Dto;
using MyTempProject.User.Dto;
using MyTempProject.Entities;

namespace MyTempProject.User
{
    public class UserAppService : IUserAppService
    {
        public readonly IRepository<Entities.CUser, int> _userRepository;
        public UserAppService(
            IRepository<Entities.CUser, int> userRepository)
        {
            this._userRepository = userRepository;
        }

        public CDataResults<CUserOutputDto> login(CUserInput input)
        {
            //this._userRepository.GetAll().Where(r=>r.)

            var query = from u in _userRepository.GetAll().Where(r => r.username == input.username && r.password == input.password)
                        select new CUserOutputDto
                        {
                            username = u.username,
                            address = u.password,
                            tel = u.tel
                        };

            var ouList = query.ToList();
            if (ouList.Count > 0)
            {
                return new CDataResults<CUserOutputDto>
                {
                    IsSuccess = true,
                    ErrorMessage = null,
                    Data = ouList
                };
            }

            return new CDataResults<CUserOutputDto>
            {
                IsSuccess = true,
                ErrorMessage = "Invalid username or password",
                Data = ouList
            };
        }
    }
}
