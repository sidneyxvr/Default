using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Default.Api.Extensions
{
    public class CustomValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly ResourceManager resourceManager; 
        private readonly Type resourceType;

        public CustomValidationMetadataProvider(Type type)
        {
            resourceType = type;
            resourceManager = new ResourceManager(type);
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata)
            {
                ValidationAttribute tAttr = attribute as ValidationAttribute;
                if (tAttr != null && tAttr.ErrorMessage == null && tAttr.ErrorMessageResourceName == null)
                {
                    var name = tAttr.GetType().Name;
                    if (resourceManager.GetString(name) != null)
                    {
                        tAttr.ErrorMessageResourceType = resourceType;
                        tAttr.ErrorMessageResourceName = name;
                        tAttr.ErrorMessage = null;
                    }
                }
            }
        }
    }
}
