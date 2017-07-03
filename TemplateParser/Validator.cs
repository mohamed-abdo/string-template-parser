using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StringTemplateParser
{
    public static class Validator
    {
        public static Func<string, bool> isValidTemplate = (tempate) =>
        {
            return new Regex(RegexHelper.VALID_TEMPLATE).IsMatch(tempate) || new Regex(RegexHelper.WITH_SCOPE_TEMPLATE).IsMatch(tempate);
        };

        public static Func<object, bool> isValidDataSource = (source) =>
        {
            return source != null;
        };


    }
}
