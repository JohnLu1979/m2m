using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.Relation.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTempProject.Relation
{
   public interface IRelationAppService: IApplicationService
    {

        CDataResults<CRelationListDto> GetRelations(CRelationInput input);
        CDataResults<CRelationListDto> AddRelations(CRelationInput input);
    }
}
