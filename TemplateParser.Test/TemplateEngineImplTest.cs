using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringTemplateParser.Test
{
    [TestClass]
    public class TemplateEngineImplTest
    {

        private TemplateEngine CreateEngine()
        {
            return new TemplateEngine();
        }

        /// <summary>
        /// Test engine substitution of a local datasource string property.
        /// </summary>
        [TestMethod]
        public void Test_basic_local_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply("Hello [Name]", dataSource);
            Assert.AreEqual("Hello John", output);
        }

        /// <summary>
        /// Test engine substitution of spanned ([object.property]) datasource string properties.
        /// </summary>
        [TestMethod]
        public void Test_spanned_local_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith"
                }
            };
            string output = engine.Apply("Hello [Contact.FirstName] [Contact.LastName]", dataSource);
            Assert.AreEqual("Hello John Smith", output);
        }

        /// <summary>
        /// Test engine substitution of scoped ([with]) datasource string properties.
        /// </summary>
        [TestMethod]
        public void Test_scoped_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland"
                    }
                }
            };
            string output = engine.Apply(@"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with]", dataSource);
            Assert.AreEqual("Hello John from Acme Ltd in Auckland", output);
        }

        /// <summary>
        /// Test engine substitution of invalid datasource string properties.
        /// </summary>
        [TestMethod]
        public void Test_invalid_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply(@"Hello [InvalidProperty1] [InvalidProperty2] [Name]", dataSource);
            Assert.AreEqual("Hello   John", output);
        }

        /// <summary>
        /// Test engine substitution of a template without any tokens.
        /// </summary>
        [TestMethod]
        public void Test_no_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply("Hello there", dataSource);
            Assert.AreEqual("Hello there", output);
        }

        /// <summary>
        /// Test engine substitution of a template with formatted tokens.
        /// </summary>
        [TestMethod]
        public void Test_formatted_date_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Today = new DateTimeOffset(1990, 12, 1, 0, 0, 0, 0, TimeSpan.Zero)
            };
            string output = engine.Apply("The current date is [Today \"d MMMM yyyy\"]", dataSource);
            Assert.AreEqual("The current date is 1 December 1990", output);
        }

        /// <summary>
        /// Test engine substitution of a template with tokens containing format arguments that should be ignored.
        /// </summary>
        [TestMethod]
        public void Test_unformattable_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply("Hello [Name \"d MMMM yyyy\"]", dataSource);
            Assert.AreEqual("Hello John", output);
        }
    }
}
