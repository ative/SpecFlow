﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Should;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.RuntimeTests.AssistTests.ExampleEntities;

namespace TechTalk.SpecFlow.RuntimeTests.AssistTests
{
    [TestFixture]
    public class RowExtensionMethodTests
    {

        [Test]
        public void GetString_should_return_the_string_value_from_the_row()
        {
            var table = new Table("Name");
            table.AddRow("John Galt");
            table.Rows.First()
                .GetString("Name").ShouldEqual("John Galt");
        }

        [Test]
        public void GetString_should_return_null_if_the_value_is_not_defined()
        {
            var table = new Table("Name");
            table.AddRow("John Galt");
            table.Rows.First()
                .GetString("SomethingThatDoesNotExist").ShouldEqual(null);
        }

        [Test]
        public void GetInt_should_return_the_int_from_the_row()
        {
            var table = new Table("Count");
            table.AddRow("3");
            table.Rows.First()
                .GetInt32("Count").ShouldEqual(3);
        }

        [Test]
        public void GetInt_should_return_MinValue_when_the_value_is_not_defined()
        {
            var table = new Table("Count");
            table.AddRow("4");
            table.Rows.First()
                .GetInt32("SomethingThatDoesNotExist").ShouldEqual(int.MinValue);
        }

        [Test]
        public void GetInt_should_return_MinValue_when_the_value_is_empty()
        {
            var table = new Table("Count");
            table.AddRow("");
            table.Rows.First()
                .GetInt32("Count").ShouldEqual(int.MinValue);
        }

        [Test]
        public void GetDecimal_should_return_the_decimal_from_the_row()
        {
            var table = new Table("Amount");
            table.AddRow(4.01M.ToString());
            table.Rows.First()
                .GetDecimal("Amount").ShouldEqual(4.01M);
        }

        [Test]
        public void GetDecimal_should_return_MinValue_when_the_value_is_not_defined()
        {
            var table = new Table("Amount");
            table.AddRow("4.01");
            table.Rows.First()
                .GetDecimal("SomethingThatDoesNotExist").ShouldEqual(decimal.MinValue);
        }

        [Test]
        public void GetDecimal_should_return_MinValue_when_the_value_is_empty()
        {
            var table = new Table("Amount");
            table.AddRow("");
            table.Rows.First()
                .GetDecimal("Amount").ShouldEqual(decimal.MinValue);
        }

        [Test]
        public void GetDateTime_should_return_the_datetime_from_the_row()
        {
            var table = new Table("Birthdate");
            table.AddRow("4/28/2009 21:02:03");
            table.Rows.First()
                .GetDateTime("Birthdate").ShouldEqual(new DateTime(2009, 4, 28, 21, 2, 3));
        }

        [Test]
        public void GetDateTime_should_return_MinValue_when_the_value_is_not_defined()
        {
            var table = new Table("Birthdate");
            table.AddRow("4/28/2009 21:02:03");
            table.Rows.First()
                .GetDateTime("SomethingThatDoesNotExist").ShouldEqual(DateTime.MinValue);
        }

        [Test]
        public void GetDateTime_should_return_MinValue_when_the_value_is_empty()
        {
            var table = new Table("Birthdate");
            table.AddRow("");
            table.Rows.First()
                .GetDateTime("Birthdate").ShouldEqual(DateTime.MinValue);
        }

        [Test]
        public void GetBool_returns_true_when_the_value_is_true()
        {
            var table = new Table("IsNeat");
            table.AddRow("true");
            table.Rows.First()
                .GetBoolean("IsNeat").ShouldBeTrue();
        }

        [Test]
        public void GetBool_returns_false_when_the_value_is_false()
        {
            var table = new Table("IsNeat");
            table.AddRow("false");
            table.Rows.First()
                .GetBoolean("IsNeat").ShouldBeFalse();
        }

        [Test]
        public void GetBool_throws_an_exception_when_the_value_is_not_true_or_false()
        {
            var table = new Table("IsNeat");
            table.AddRow("is not true nor false");
            var exceptionThrown = false;
            try
            {
                table.Rows.First()
                    .GetBoolean("IsNeat");
            }
            catch (InvalidCastException exception)
            {
                if (exception.Message == "You must use 'true' or 'false' when setting bools for IsNeat")
                    exceptionThrown = true;
            }
            exceptionThrown.ShouldBeTrue();
        }

        [Test]
        public void GetBool_returns_false_when_the_value_is_empty()
        {
            var table = new Table("IsNeat");
            table.AddRow("");
            table.Rows.First()
                .GetBoolean("IsNeat").ShouldBeFalse();
        }

        [Test]
        public void GetBool_throws_an_exception_when_the_id_is_not_defined()
        {
            var table = new Table("IsNeat");
            table.AddRow("is not true nor false");
            var exceptionThrown = false;
            try
            {
                table.Rows.First()
                    .GetBoolean("SomethingThatDoesNotExist");
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message == "SomethingThatDoesNotExist could not be found in the row.")
                    exceptionThrown = true;
            }
            exceptionThrown.ShouldBeTrue();
        }

        [Test]
        public void GetDouble_should_return_the_double_from_the_row()
        {
            var table = new Table("Amount");
            table.AddRow(4.01M.ToString());
            table.Rows.First()
                .GetDouble("Amount").ShouldEqual(4.01);
        }

        [Test]
        public void GetDouble_should_return_MinValue_when_the_value_is_not_defined()
        {
            var table = new Table("Amount");
            table.AddRow("4.01");
            table.Rows.First()
                .GetDouble("SomethingThatDoesNotExist").ShouldEqual(double.MinValue);
        }

        [Test]
        public void GetDouble_should_return_MinValue_when_the_value_is_empty()
        {
            var table = new Table("Amount");
            table.AddRow("");
            table.Rows.First()
                .GetDouble("Amount").ShouldEqual(double.MinValue);
        }

        [Test]
        public void GetEnum_returns_enum_value_when_the_value_is_defined_in_the_type()
        {
            var table = new Table("Sex");
            table.AddRow("NotDefinied");

            var exceptionThrown = false;
            try
            {
                var e = table.Rows.First().GetEnum<Person>("Sex");
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message == "No enum with value NotDefinied found in type Person")
                    exceptionThrown = true;
            }
            exceptionThrown.ShouldBeTrue();
        }

        [Test]
        public void GetEnum_should_return_the_enum_value_from_the_row()
        {
            var table = new Table("Sex");
            table.AddRow("Male");

            table.Rows.First()
                .GetEnum<Person>("Sex").ShouldEqual(Sex.Male);

        }

        [Test]
        public void GetEnum_should_return_the_enum_value_from_the_row_even_when_i_use_spaces()
        {
            var table = new Table("Sex");
            table.AddRow("Unknown Sex");

            table.Rows.First()
                .GetEnum<Person>("Sex").ShouldEqual(Sex.UnknownSex);

        }

        [Test]
        public void GetEnum_should_return_the_enum_value_from_the_row_even_when_i_mess_up_the_casing()
        {
            var table = new Table("Sex");
            table.AddRow("feMale");

            table.Rows.First()
                .GetEnum<Person>("Sex").ShouldEqual(Sex.Female);

        }

        [Test]
        public void GetEnum_should_return_the_enum_value_from_the_row_even_when_i_mess_up_the_casing_and_use_spaces()
        {
            var table = new Table("Sex");
            table.AddRow("unknown sex");

            table.Rows.First()
                .GetEnum<Person>("Sex").ShouldEqual(Sex.UnknownSex);

        }

        [Test]
        public void GetEnum_throws_exception_when_the_value_is_not_defined_in_any_Enum_in_the_type()
        {
            var table = new Table("Sex");
            table.AddRow("NotDefinied");

            var exceptionThrown = false;
            try
            {
                var e = table.Rows.First().GetEnum<Person>("Sex");
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message == "No enum with value NotDefinied found in type Person")
                    exceptionThrown = true;
            }
            exceptionThrown.ShouldBeTrue();
        }

        [Test]
        public void GetEnum_throws_exception_when_the_value_is_not_defined_in_more_than_one_Enum_in_the_type()
        {
            var table = new Table("Sex");
            table.AddRow("Male");

            var exceptionThrown = false;
            try
            {
                var e = table.Rows.First().GetEnum<PersonWithStyle>("Sex");
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message == "Found sevral enums with the value Male in type PersonWithStyle")
                    exceptionThrown = true;
            }
            exceptionThrown.ShouldBeTrue();
        }


    }
}
