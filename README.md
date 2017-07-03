# string-template-parser
C# string template parser, gets a string as a first input parameter , and the second parameter receive an object (anonymous), contains the [field] listed on first parameter string template.

# User as an email template:
A simple use case for this tempate parser, is build email template, from string, and object contains fields values.

# Test cases
# 1- simple match fields
  TemplateEngine engine = this.CreateEngine();
  var dataSource = new
            {
                Name = "Mohamed"
            };
            string output = engine.Apply("Hello [Name]", dataSource);
   Assert.AreEqual("Hello Mohamed", output);
   
 # 2- complex fields (field as a class contains other field)
   TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "Mohamed",
                    LastName = "Abdo"
                }
            };
       string output = engine.Apply("Hello [Contact.FirstName] [Contact.LastName]", dataSource);
       Assert.AreEqual("Hello Mohamed Abdo", output);
   
   # 3- scoped template 
    TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "Mohamed",
                    LastName = "Abdo",
                    Organisation = new
                    {
                        Name = "Soft-Ideas Ltd",
                        City = "Cairo"
                    }
                }
            };
            string output = engine.Apply(@"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with]",     dataSource);
            Assert.AreEqual("Hello Mohamed from Soft-Ideas Ltd in Cairo", output);
            
            
            
            
