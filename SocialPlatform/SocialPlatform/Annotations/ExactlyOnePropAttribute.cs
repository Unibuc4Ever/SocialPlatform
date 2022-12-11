using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SocialPlatform.Annotations
{
    public class ExactlyOnePropAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        public ExactlyOnePropAttribute(params string[] propertyList)
        {
            this.PropertyList = propertyList;
        }

        //See http://stackoverflow.com/a/1365669
        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        public override bool IsValid(object value)
        {
            PropertyInfo propertyInfo;
            int howMany = 0;
            foreach (string propertyName in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    howMany += 1;
                }
            }

            return howMany == 1;
        }
    }
}