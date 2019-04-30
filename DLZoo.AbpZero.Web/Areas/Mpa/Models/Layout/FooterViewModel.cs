//using MyTempProject.Sessions.Dto;

namespace MyTempProject.Web.Areas.Mpa.Models.Layout
{
    public class FooterViewModel
    {
        //public GetCurrentLoginInformationsOutput LoginInformations { get; set; }

        public string GetProductNameWithEdition()
        {
            var productName = "DaLian Forest Zoo";

            //if (LoginInformations.Tenant != null && LoginInformations.Tenant.EditionDisplayName != null)
            //{
            //    productName += " " + LoginInformations.Tenant.EditionDisplayName;
            //}

            return productName;
        }
    }
}