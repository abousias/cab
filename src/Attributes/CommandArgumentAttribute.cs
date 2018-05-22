﻿using System;
using System.Reflection;

namespace ConsoleAppBase.Attributes
{
    /// <summary>
    /// Property attribute to define a command argument.
    /// </summary>
    public class CommandArgumentAttribute : SimpleTypeParameterAttribute
    {
        /// <summary>
        /// Gets the position of the command argument in the argument list.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets or sets the name of the command argument (e.g. shown in usage messages).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the command argument (e.g. shown in usage messages).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the command argument is required when calling the command.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandArgumentAttribute"/> class.
        /// </summary>
        /// <param name="position"></param>
        public CommandArgumentAttribute(int position)
        {
            Position = position;
        }

        public override bool ValidateValue(PropertyInfo property, string value)
        {
            throw new NotImplementedException();
        }
    }
}
