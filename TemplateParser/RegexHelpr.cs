namespace StringTemplateParser
{
    public class RegexHelper
    {
        public const string VALID_TEMPLATE = @"(\[\w+\])|(\[\w+\.\w+\])+";
        public const string VALID_DOT_TEMPLATE = @"(\[\w+\])|(\[(\w+.)+\])";
        public const string VALID_WITH_CLAUSE_TEMPLATE = @"\[with \w+\](.+?)\[\/with\]$";
        public const string WITH_CLAUSE_TEMPLATE = @"\[with \w+\]";
        public const string WITH_SCOPE_TEMPLATE = @"\w+";
    }
}
