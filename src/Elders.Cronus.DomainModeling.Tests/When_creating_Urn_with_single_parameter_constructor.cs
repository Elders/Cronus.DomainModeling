using Machine.Specifications;
using System;

namespace Elders.Cronus
{
    public class When_creating_Urn_with_single_parameter_constructor
    {
        static Exception exception;

        class When_the_input_is_not_encoded
        {
            Because of = () => exception = Catch.Exception(() => new Urn("urn:tenant:something³"));

            It should_have_thrown_an_exception = () => exception.ShouldNotBeNull();

            It and_should_be_an_ArgumentException = () => exception.ShouldBeOfExactType<ArgumentException>();

            It and_the_exception_message_should_be = () => exception.Message.ShouldEqual("String is not a valid URN! (Parameter 'urnSpan')");
        }

        class When_the_input_is_encoded
        {
            static Urn urn;

            Because of = () => urn = new Urn("urn:tenant:something%C2%B3");

            It should_be_valid_urn = () => urn.Value.ShouldEqual("urn:tenant:something%c2%b3");
        }

        class When_the_input_is_not_valid_encoded
        {
            Because of = () => exception = Catch.Exception(() => new Urn("urn:tenant:something%Ct%B3"));

            It should_have_thrown_an_exception = () => exception.ShouldNotBeNull();

            It and_should_be_an_ArgumentException = () => exception.ShouldBeOfExactType<ArgumentException>();

            It and_the_exception_message_should_be = () => exception.Message.ShouldEqual("String is not a valid URN! (Parameter 'urnSpan')");
        }
    }

    public class When_creating_Urn_with_the_multi_paramete_constructor
    {
        static Exception exception;

        class When_the_input_is_not_encoded
        {
            Because of = () => exception = Catch.Exception(() => new Urn("tenant", "something³"));

            It should_have_thrown_an_exception = () => exception.ShouldNotBeNull();

            It and_should_be_an_ArgumentException = () => exception.ShouldBeOfExactType<ArgumentException>();

            It and_the_exception_message_should_be = () => exception.Message.ShouldEqual("NSS is not valid something³ (Parameter 'nss')");
        }

        class When_the_input_is_encoded
        {
            static Urn urn;

            Because of = () => urn = new Urn("tenant", "something%C2%B3");

            It should_be_valid_urn = () => urn.Value.ShouldEqual("urn:tenant:something%c2%b3");
        }

        class When_the_input_is_not_valid_encoded
        {
            Because of = () => exception = Catch.Exception(() => new Urn("tenant", "something%Ct%B3"));

            It should_have_thrown_an_exception = () => exception.ShouldNotBeNull();

            It and_should_be_an_ArgumentException = () => exception.ShouldBeOfExactType<FormatException>();

            It and_the_exception_message_should_be = () => exception.Message.ShouldEqual("Invalid URN format.");
        }
    }
}
