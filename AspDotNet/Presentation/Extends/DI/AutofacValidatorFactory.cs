using Autofac;
using FluentValidation;
using System;

namespace Presentation.Extends.DI
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (_context.TryResolve(validatorType, out object instance))
            {
                var validator = instance as IValidator;
                return validator;
            }

            return null;
        }
    }
}