﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;
using NzbDrone.Api.ClientSchema;
using System.Linq;

namespace NzbDrone.Api.REST
{
    public class ResourceValidator<TResource> : AbstractValidator<TResource>
    {
        public IRuleBuilderInitial<TResource, TProperty> RuleForField<TProperty>(Expression<Func<TResource, IEnumerable<Field>>> fieldListAccessor, string filedName)
        {
            var rule = new PropertyRule(fieldListAccessor.GetMember(), c => GetValue(c, fieldListAccessor.Compile(), filedName), null, () => { return CascadeMode.Continue; }, typeof(TProperty), typeof(TResource));
            rule.PropertyName += "." + filedName;

            AddRule(rule);
            return new RuleBuilder<TResource, TProperty>(rule);
        }

        private static object GetValue(object container, Func<TResource, IEnumerable<Field>> fieldListAccessor, string fieldName)
        {

            var resource = fieldListAccessor((TResource)container).SingleOrDefault(c => c.Name == fieldName);

            if (resource == null)
            {
                return null;
            }

            return resource.Value;
        }
    }



}