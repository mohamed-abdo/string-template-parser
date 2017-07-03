using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringTemplateParser.Test {
    class Program {
        public static int Main() {

            
            //F5 Run this if you can't get the integrated Test Explorer to work
            TemplateEngineImplTest test = new TemplateEngineImplTest();
            test.Test_basic_local_property_substitute();
            test.Test_scoped_property_substitute();
            test.Test_spanned_local_property_substitute();
            test.Test_invalid_property_substitute();
            test.Test_no_property_substitute();
            test.Test_formatted_date_property_substitute();
            test.Test_unformattable_property_substitute();

            Console.WriteLine("All tests passed successfully!");
            Console.ReadKey();

            return 0;
        }
    }
}
