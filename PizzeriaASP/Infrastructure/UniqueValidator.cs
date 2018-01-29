using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

namespace PizzeriaASP.Models
{
    //public class MustBeUniqueAttribute : Attribute, IModelValidator
    //{
    //    public bool IsRequired => true;
    //    public string ErrorMessage { get; set; } = "This value must be unique";

    //    public IEnumerable<ModelValidationResult> Validate(TomasosContext context, ModelValidationContext model)
    //    {
    //        bool? value = context.Kund.SingleOrDefault(p=>p.AnvandarNamn == )

    //        if (value == null || value == false)
    //        {
    //            return new List<ModelValidationResult> {new ModelValidationResult("", ErrorMessage)};
    //        }
    //        else
    //        {
    //            return Enumerable.Empty<ModelValidationResult>();
    //        }
    //    }
    //}
}
