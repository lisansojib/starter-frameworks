using EPYSLACSCustomer.Core.Statics;
using FluentValidation;
using System.Linq;

namespace Presentation.Models.Validation
{
    public class UkAndCeLabellingMasterBindingModelValidator : AbstractValidator<UkAndCeLabellingMasterBindingModel>
    {
        public UkAndCeLabellingMasterBindingModelValidator(string orderFor)
        {
            RuleFor(x => x.OrderForId).NotEmpty();
            RuleFor(x => x.OrderFor).Must(x => x.Contains("THL") || x.Contains("TCL")).NotEmpty();

            When(x => x.Childs.Any(), () =>
            {
                RuleForEach(x => x.Childs).SetValidator(new UkAndCeLabellingChildBindingModelValidator(orderFor));
            });
        }
    }

    public class UkAndCeLabellingChildBindingModelValidator : AbstractValidator<UkAndCeLabellingChildBindingModel>
    {
        public UkAndCeLabellingChildBindingModelValidator(string orderFor)
        {
            RuleFor(x => x.PackType).NotEmpty();
            RuleFor(x => x.Season).NotEmpty();
            RuleFor(x => x.TPND).NotEmpty().MaximumLength(13);
            RuleFor(x => x.PONo).NotEmpty().MaximumLength(100);
            RuleFor(x => x.UKStyleRef).NotEmpty();
            RuleFor(x => x.CEStyleRef).NotEmpty();
            RuleFor(x => x.ShortDesc).NotEmpty();
            RuleFor(x => x.EQOSCode).NotEmpty();
            RuleFor(x => x.Supplier).NotEmpty();
            RuleFor(x => x.PackagingSupplier).NotEmpty();
            RuleFor(x => x.Dept).NotEmpty();
            RuleFor(x => x.Brand).NotEmpty();
            RuleFor(x => x.Qty).NotEmpty();
            RuleFor(x => x.ManufactureDate).NotEmpty();

            if (orderFor == UKAndCELabellings.TCL1 || orderFor == UKAndCELabellings.TCL3 || orderFor == UKAndCELabellings.THL1 || orderFor == UKAndCELabellings.THL2)
            {
                RuleFor(x => x.BarcodeNo).NotEmpty().Length(14);
            } 
            
            if(orderFor == UKAndCELabellings.THL3)
            {
                RuleFor(x => x.BarcodeNo).NotEmpty().Length(13);
            }
        }
    }
}