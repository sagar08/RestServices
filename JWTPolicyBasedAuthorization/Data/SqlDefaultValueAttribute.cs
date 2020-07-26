using System;

namespace JWTPolicyBasedAuthorization.Data
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    public class SqlDefaultValueAttribute:Attribute
    {
        public dynamic DefaultValue { get; set; }
    }
}