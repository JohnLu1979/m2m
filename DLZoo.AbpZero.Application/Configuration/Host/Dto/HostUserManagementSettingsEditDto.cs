using Abp.Runtime.Validation;

namespace MyTempProject.Configuration.Host.Dto
{
    public class HostUserManagementSettingsEditDto : IValidate
    {
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
    }
}