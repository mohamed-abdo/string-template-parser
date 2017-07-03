using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringTemplateParser
{
    public class TemplateRegexMatcher
    {
        readonly string _pattern; readonly char[] _placeHolders;
        public TemplateRegexMatcher(string pattern, char[] placeHolders)
        {
            _pattern = pattern;
            _placeHolders = placeHolders;
        }
        public IEnumerable<string> IterateTemplateFields(string template)
        {
            var matches = new Regex(_pattern).Matches(template);
            foreach (Match item in matches)
            {
                yield return item.Value;
            }
        }

        public string ApplyTransformars(string template, params Func<string, string>[] transformars)
        {
            var tansformedTemplate = template;
            foreach (var item in transformars)
            {
                tansformedTemplate = item(tansformedTemplate);
            }
            return tansformedTemplate;
        }
        public string FlattenNormalizeScope(string template, string pattern)
        {
            var normalizedTemplate = NormalizeScope(template, pattern);
            var group = Regex.Match(normalizedTemplate, pattern).Groups.Cast<Group>().Where(g => Regex.Match(g.Value, pattern).Success).LastOrDefault();
            if (group == null || !group.Success)
                return normalizedTemplate;
            return FlattenNormalizeScope(normalizedTemplate, pattern);
        }
        public string NormalizeScope(string template, string pattern)
        {
            var outerMatch = Regex.Match(template, pattern);
            var group = outerMatch.Groups.Cast<Group>().Where(g => Regex.Match(g.Value, pattern).Success).LastOrDefault();
            if (group == null || !group.Success)
                return template;
            if (!string.IsNullOrEmpty(group.Value))
            {
                var outer = new Regex(pattern).Replace(group.Value, match =>
                {
                    //get scope
                    var scope = new Regex(pattern).Match(match.Value)?.Value;
                    var withScopeValue = Regex.Match(scope, RegexHelper.WITH_CLAUSE_TEMPLATE)?.Value;
                    var scopeValue = Regex.Matches(withScopeValue, RegexHelper.WITH_SCOPE_TEMPLATE).Cast<Match>().LastOrDefault()?.Value;
                    //add scope to fields
                    var inner = new Regex(RegexHelper.VALID_TEMPLATE).Replace(scope, innerMatch =>
                         {
                             var value = innerMatch.Value.Trim(_placeHolders);
                             return $"[{scopeValue}.{value}]";
                         });
                    var innerValue = Regex.Match(inner, RegexHelper.VALID_WITH_CLAUSE_TEMPLATE).Groups.Cast<Group>().LastOrDefault()?.Value;
                    return innerValue;
                });
                var ajustetedMatch = outerMatch.Value.Replace(group.Value, outer);
                return ajustetedMatch;
            }
            return template;
        }
        public string MatchReplace(string template, IDictionary<string, object> dictorany)
        {
            return new Regex(_pattern).Replace(template, match =>
            {
                var value = match.Value.Trim(_placeHolders);
                return dictorany.ContainsKey(value) ? match.Result(dictorany[value].ToString()) : string.Empty;
            });
        }

    }
}
