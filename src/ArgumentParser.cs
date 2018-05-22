﻿using ConsoleAppBase.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleAppBase
{
    internal class ArgumentParser
    {
        /// <summary>
        /// Parses the given arguments for the related command and fills all arguments and options.
        /// </summary>
        /// <param name="args">The arguments to execute the command or subcommand with.</param>
        /// <param name="command">The command or subcommand on which the parsing is applied.</param>
        public void Parse(string[] args, Command command)
        {
            if (args == null || !args.Any()) return;

            var optionOnlyValues = FillAllArguments(args, command);

            FillAllOptions(optionOnlyValues, command);
        }

        /// <summary>
        /// Check if the arguments contain the custom help template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom help template, else false.</returns>
        internal bool IsHelpOption(IEnumerable<string> args, Command command)
        {
            return IsAnyMatchingTemplate(args, command.HelpOptionTemplate);
        }

        /// <summary>
        /// Check if the arguments contain the custom version template.
        /// </summary>
        /// <param name="args">The arguments passed in from commandline.</param>
        /// <param name="command">The command on which the parsing is applied.</param>
        /// <returns>true if the arguments contain the custom version template, else false.</returns>
        internal bool IsVersionOption(IEnumerable<string> args, Command command)
        {
            return IsAnyMatchingTemplate(args, command.VersionOptionTemplate);
        }

        private bool IsAnyMatchingTemplate(IEnumerable<string> args, string template)
        {
            return args.Any(a => OptionTemplateContains(template, a));
        }

        /// <summary>
        /// Parses passed in arguments and fills all properties with the <see cref="CommandArgumentAttribute"/> Attribute.
        /// </summary>
        /// <param name="args">passed in arguments.</param>
        /// <param name="command"></param>
        /// <returns>remaining arguments which were not filled in properties.</returns>
        private IEnumerable<string> FillAllArguments(IEnumerable<string> args, Command command)
        {
            var arguments = Command.GetArgumentProperties(command.GetType());
            if (!arguments.Any())
            {
                if (IsOption(args.FirstOrDefault(), command))
                {
                    return args;
                }
                throw new NoArgumentsExistException();
            }

            var counter = 0;

            foreach (var value in args)
            {
                if (IsOption(value, command))
                {
                    break;
                }

                var argument = arguments.ElementAt(counter);
                argument.Key.SetValue(command, argument.Value, value);
                counter++;
            }

            var nextRequiredArgument = command.GetRequiredArguments().Skip(counter).FirstOrDefault();
            if (nextRequiredArgument != null)
            {
                throw new RequiredArgumentException(nextRequiredArgument.Attribute.Name);
            }

            return args.Skip(counter);
        }
        
        private void FillAllOptions(IEnumerable<string> args, Command command)
        {
            CommandOptionAttribute option = null;
            foreach (var arg in args)
            {
                var matchingOption = FindOptionProperty(arg, command);

                if (matchingOption.Key != null)
                {
                    if (matchingOption.Value.PropertyType == typeof(bool))
                    {
                        matchingOption.Key.SetValue(command, matchingOption.Value, true.ToString());
                    }
                    else
                    {
                        option = matchingOption.Key;
                    }
                }
                else
                {
                    
                    option.SetValue(command, matchingOption.Value, arg);
                }
            }
        }

        

        private bool IsOption(string arg, Command command)
        {
            if (string.IsNullOrEmpty(arg)) return false;

            return FindOptionProperty(arg, command).Key != null;
        }

        private KeyValuePair<CommandOptionAttribute, PropertyInfo> FindOptionProperty(string arg, Command command)
        {
            var options = Command.GetOptionProperties(command.GetType());
            return options.FirstOrDefault(o => OptionTemplateContains(o.Key.Template, arg));
        }

        private bool OptionTemplateContains(string template, string arg)
        {
            return template.Split('|').Contains(arg);
        }

        
    }
}
