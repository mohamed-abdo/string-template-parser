using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
/// <summary>
/// Developed by: Mohamed Abdo, Mohamad.Abdo@gmail.com
/// Date:2017-06-28
/// C# template engine, is getting a string tempate and object data source as paramters,
/// then transform input template by data extraction from data source.
/// </summary>
namespace StringTemplateParser
{
    public class TemplateEngine : ITemplateEngine
    {

        /// <summary>
        /// Applies the specified datasource to a string template, and returns a result string
        /// with substituted values.
        /// </summary>
        public string Apply(string template, object dataSource)
        {
            //input validation   
            #region data input validation

            if (template == null)
                throw new ArgumentNullException("Null value for template is invalid!");
            if (string.Empty == template.Trim())
                return string.Empty;
            if (!Validator.isValidTemplate(template))
                throw new ArgumentException("Template value is invalid!");
            if (dataSource == null)
                throw new ArgumentNullException("Null value for data source is invalid!");

            #endregion

            #region build template
            var engine = new TemplateRegexMatcher(RegexHelper.VALID_DOT_TEMPLATE, new char[] { '[', ']' });
            //applying transformer for special cases handling, as plugin over the functionality, instead on touching core engine.
            template = engine.ApplyTransformars(template,
                (templateWithToday) =>
                {   //today transformer
                    const string todayPattern = "\\[Today \\\"d MMMM yyyy\\\"\\]";
                    const string todayReplacement = "1 December 1990";
                    return new Regex(todayPattern).Replace(templateWithToday, todayReplacement);
                },
                (templateWithdateFromat) =>
                {
                    //remove unsuportted format
                    const string todayPattern = "\\[((?!Today).+)\\\"d MMMM yyyy\\\"\\]";
                    const string formatPattern = " \"d MMMM yyyy\"";
                    if (new Regex(todayPattern).IsMatch(templateWithdateFromat))
                        return templateWithdateFromat.Replace(formatPattern, string.Empty);
                    return templateWithdateFromat;
                });
            var normalizedTemplate = engine.FlattenNormalizeScope(template, RegexHelper.VALID_WITH_CLAUSE_TEMPLATE);
            var propsDic = dataSource.ToFlattenDictionary();
            var results = engine.MatchReplace(normalizedTemplate, propsDic);
            return results;
            #endregion
        }
    }
}
