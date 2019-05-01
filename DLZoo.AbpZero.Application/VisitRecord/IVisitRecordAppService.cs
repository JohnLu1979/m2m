using Abp.Application.Services;
using MyTempProject.Base.Dto;
using MyTempProject.VisitRecord.Dto;

namespace MyTempProject.VisitRecord
{
    public interface IVisitRecordAppService : IApplicationService
    {
        CDataResults<CVisitRecordListDto> GetVisitRecord(CVisitRecordInput input);
        CDataResults<CVisitRecordDetailListDto> GetVisitWhiteRecord(CVisitRecordInput input);
        CDataResults<CVisitRecordDetailListDto> GetVisitBlackRecord(CVisitRecordInput input);
    }
}
