using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReVersion.Models;
using ReVersion.Models.Settings;

namespace ReVersion.Utilities.Extensions
{
    public static class StringExtensions
    {
        #region Splitting

        public static string[] Split(this string text, string splitTerm)
        {
            return text.Split(new[] {splitTerm}, StringSplitOptions.None);
        }

        #endregion

        #region String formatting

        public static string ToConventionCase(this string value, SvnNamingConvention convention)
        {
            switch (convention)
            {
                case SvnNamingConvention.LowerCamelCase:
                    return value.ToCamelCase();
                case SvnNamingConvention.UpperCamelCase:
                    return value.ToPascalCase();

                case SvnNamingConvention.LowerHyphenCase:
                    return value.ToHyphenCase();
                case SvnNamingConvention.UpperHyphenCase:
                    return value.ToHyphenCase(true);

                case SvnNamingConvention.LowerUnderscoreCase:
                    return value.ToUnderscoreCase();
                case SvnNamingConvention.UpperUnderscoreCase:
                    return value.ToUnderscoreCase(true);

                case SvnNamingConvention.PreserveOriginal:
                default:
                    return value;
            }
        }

        /// <summary>
        ///     Capitalises the first letter of a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NormalizeCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return Capitalise(value);
        }

        /// <summary>
        ///     Convert string to Camel hump format e.g. 'Some string here' => 'someStringHere' || 'Some-String-here' =>
        ///     'someStringHere'
        /// </summary>
        /// <param name="str">Text to Camel hump format</param>
        /// <returns>Camel hump formatted string</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            var words = CaseConvertSplitter(value);

            return words.FirstOrDefault() + string.Join("", words.Skip(1).Select(Capitalise));
        }

        /// <summary>
        ///     Convert string to Pascal format e.g. 'Some string here' => 'SomeStringHere' || 'Some-String-here' =>
        ///     'SomeStringHere'
        /// </summary>
        /// <param name="str">Text to Pascal format</param>
        /// <returns>Pascal formatted string</returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return string.Join("", CaseConvertSplitter(value).Select(Capitalise));
        }

        /// <summary>
        ///     Formats a string to a generic hyphen case e.g. "Some Random_Text" => "some-random-text"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHyphenCase(this string value, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            var words = CaseConvertSplitter(value);

            if (upperCase)
                words = words.Select(Capitalise).ToList();

            return string.Join("-", words);
        }

        /// <summary>
        ///     Formats a string to a generic hyphen case e.g. "Some Random-Text" => "some_random_text"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUnderscoreCase(this string value, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            var words = CaseConvertSplitter(value);

            if (upperCase)
                words = words.Select(Capitalise).ToList();

            return string.Join("_", words);
        }

        /// <summary>
        ///     Proxy to the standard `string.Format`
        /// </summary>
        /// <param name="format">string format</param>
        /// <param name="args">format parameters</param>
        /// <returns>Formatted string</returns>
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        #endregion

        #region Remove Starting/ending text

        /// <summary>
        ///     Remove Character from the start of a string
        /// </summary>
        /// <param name="unwantedChar">string to remove from the start of the string (if present)</param>
        /// <returns>Text with starting term removed</returns>
        public static string RemoveStarting(this string text, char unwantedChar)
        {
            return text.RemoveStarting(unwantedChar.ToString());
        }

        /// <summary>
        ///     Remove string from the start of a string
        /// </summary>
        /// <param name="unwantedString">string to remove from the start of the string (if present)</param>
        /// <returns>Text with starting term removed</returns>
        public static string RemoveStarting(this string text, string unwantedString)
        {
            var result = text;
            if (text.StartsWith(unwantedString))
                result = text.Substring(unwantedString.Length, text.Length - unwantedString.Length);
            return result;
        }

        /// <summary>
        ///     Remove Character from the end of a string
        /// </summary>
        /// <param name="unwantedChar">string to remove from the start of the string (if present)</param>
        /// <returns>Text with ending term removed</returns>
        public static string RemoveTrailing(this string text, char unwantedChar)
        {
            return text.RemoveTrailing(unwantedChar.ToString());
        }

        /// <summary>
        ///     Remove string from the end of a string
        /// </summary>
        /// <param name="unwantedString">string to remove from the start of the string (if present)</param>
        /// <returns>Text with ending term removed</returns>
        public static string RemoveTrailing(this string text, string unwantedString)
        {
            var result = text;
            if (text.EndsWith(unwantedString))
                result = text.Substring(0, text.Length - unwantedString.Length);
            return result;
        }

        #endregion

        #region Ensure Starts/Ends with

        /// <summary>
        ///     Ensures the source string starts with the 'startingChar' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'startingChar' parameter is prepended to</param>
        /// <param name="startingChar">string to ensure is prepended to the source</param>
        /// <returns>Source string with the 'startingChar' paremeter prepended</returns>
        public static string EnsureStartsWith(this string source, char startingChar)
        {
            return source.EnsureStartsWith(startingChar.ToString());
        }

        /// <summary>
        ///     Ensures the source string starts with the 'startingString' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'startingString' parameter is prepended to</param>
        /// <param name="startingString">string to ensure is prepended to the source</param>
        /// <returns>Source string with the 'startingString' paremeter prepended</returns>
        public static string EnsureStartsWith(this string source, string startingString)
        {
            if (string.IsNullOrEmpty(source))
                return startingString;

            if (source.StartsWith(startingString))
                return source;
            return startingString + source;
        }

        /// <summary>
        ///     Ensures the source string ends with the 'endingChar' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'endingChar' parameter is appended to</param>
        /// <param name="endingChar">string to ensure is appended to the source</param>
        /// <returns>Source string with the 'endingChar' paremeter appended</returns>
        public static string EnsureEndsWith(this string source, char endingChar)
        {
            return source.EnsureEndsWith(endingChar.ToString());
        }

        /// <summary>
        ///     Ensures the source string ends with the 'endingString' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'endingString' parameter is appended to</param>
        /// <param name="endingString">string to ensure is appended to the source</param>
        /// <returns>Source string with the 'endingString' paremeter appended</returns>
        public static string EnsureEndsWith(this string source, string endingString)
        {
            if (string.IsNullOrEmpty(source))
                return endingString;

            if (source.EndsWith(endingString))
                return source;
            return source + endingString;
        }

        #region Not Start/Ending with

        /// <summary>
        ///     Ensures the source string does not start with the 'startingChar' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'startingChar' parameter does not start with</param>
        /// <param name="startingChar">string to ensure the source does not start with</param>
        /// <returns>Source string that does not start with the 'startingChar'</returns>
        public static string EnsureDoesNotStartWith(this string source, char startingChar)
        {
            return source.EnsureDoesNotStartWith(startingChar.ToString());
        }

        /// <summary>
        ///     Ensures the source string does not start with the 'startingString' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'startingString' parameter does not start with</param>
        /// <param name="startingString">string to ensure the source does not start with</param>
        /// <returns>Source string that does not start with the 'startingChar'</returns>
        public static string EnsureDoesNotStartWith(this string source, string startingString)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (source.StartsWith(startingString))
                return source.Substring(startingString.Length, source.Length - startingString.Length);
            return source;
        }

        /// <summary>
        ///     Ensures the source string does not end with the 'endingChar' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'endingChar' parameter does not end with</param>
        /// <param name="endingChar">string to ensure the source does not end with</param>
        /// <returns>Source string that does not end with the 'endingChar'</returns>
        public static string EnsureDoesNotEndWith(this string source, char endingChar)
        {
            return source.EnsureDoesNotEndWith(endingChar.ToString());
        }

        /// <summary>
        ///     Ensures the source string does not end with the 'endingString' parameter
        /// </summary>
        /// <param name="source">String to ensure the 'endingString' parameter does not end with</param>
        /// <param name="endingString">string to ensure the source does not end with</param>
        /// <returns>Source string that does not end with the 'endingString'</returns>
        public static string EnsureDoesNotEndWith(this string source, string endingString)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (source.EndsWith(endingString))
                return source.Substring(0, source.Length - endingString.Length);
            return source;
        }

        #endregion

        #endregion

        #region NullOrEmpty

        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotBlank(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        #endregion

        #region private helper methods

        private static string Capitalise(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;
            if (word.Length == 1)
                return word.ToUpper();
            return word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1).ToLower();
        }

        private static List<string> CaseConvertSplitter(string source)
        {
            //ignore apostrophes
            source = source.Replace("'", "");

            //Matches any none alphabetical character before a alphabetical character and lowercase before an uppercase
            var splitRegex = new Regex("((?<=[a-z])([A-Z]))|((?<=\\W)([a-z]|[A-Z]))|((?<=_)([a-z]|[A-Z]))");
            var replaceRegex = new Regex("\\W|_");

            var separatedString = splitRegex.Replace(source, " $0");

            return replaceRegex.Replace(separatedString, " ")
                .Split(' ')
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        #endregion
    }
}