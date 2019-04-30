using System.Collections.Generic;
using Abp.Application.Services.Dto;
//using MyTempProject.Editions.Dto;

namespace MyTempProject.Web.Areas.Mpa.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        //List<FlatFeatureDto> Features { get; set; }
    }
}