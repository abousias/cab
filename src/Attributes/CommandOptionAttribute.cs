using System;
using System.Reflection;

namespace ConsoleAppBase.Attributes
{
    /// <summary>
    /// Property attribute to define a command option.
    /// </summary>
    public class CommandOptionAttribute : SimpleTypeParameterAttribute
    {
        /// <summary>
        /// Gets or sets the templated identifier(s) of the command option.
        /// </summary>
        /// <example>-s|--server</example>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the description of the command option (e.g. shown in usage messages).
        /// </summary>
        public string Description { get; set; }

        public override bool ValidateValue(PropertyInfo property, string value)
        {
            throw new NotImplementedException();
        }
    }
}
