using EPYSLACSCustomer.Core.Statics;
using FluentValidation;
using System.Linq;

namespace Presentation.Models.Validation
{
    public class TSLMasterBindingModelValidator : AbstractValidator<TSLLabellingMasterBindingModel>
    {
        public TSLMasterBindingModelValidator(string orderFor)
        {
            RuleFor(x => x.OrderForId).NotEmpty();
            RuleFor(x => x.OrderFor).NotEmpty().MaximumLength(10);

            When(x => x.Childs.Any(), () => {
                RuleForEach(x => x.Childs).SetValidator(new TSLChildBindingModelValidator(orderFor));
            });
        }
    }

    public class TSLChildBindingModelValidator : AbstractValidator<TSLLabellingChildBindingModel>
    {
        public TSLChildBindingModelValidator(string orderFor)
        {
            RuleFor(x => x.ShortDesc).NotEmpty().MinimumLength(15).MaximumLength(22);

            if (orderFor.Contains("CE"))
                RuleFor(x => x.TPND).NotEmpty().Length(13);
            else
                RuleFor(x => x.TPND).NotEmpty().Length(9);

            if (orderFor == UKLabellings.TSL3 || orderFor == CELabellings.TSL3_CE)
                RuleFor(x => x.BarcodeNo).NotEmpty().Length(13);
            else
                RuleFor(x => x.BarcodeNo).NotEmpty().Length(14);
        }
    }
}