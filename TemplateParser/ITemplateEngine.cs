using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringTemplateParser {
    /// <summary>
    /// Interface describing a template engine that takes a string source with tokens, and uses
    /// a datasource to replace tokens in the template to produce a new string as output.
    /// </summary>
    public interface ITemplateEngine {
        /// <summary>
        /// Applies the specified datasource to a string template, and returns a result string
        /// with substituted values.
        /// </summary>
        string Apply(string template, object dataSource);
    }
}
