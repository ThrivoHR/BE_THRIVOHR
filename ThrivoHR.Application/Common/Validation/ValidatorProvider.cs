﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ThrivoHR.Application.Common.Validation
{
    public class ValidatorProvider(IServiceProvider serviceProvider) : IValidatorProvider
    {
        public IValidator<T> GetValidator<T>()
        {
            return serviceProvider.GetService<IValidator<T>>()!;
        }
    }
}
