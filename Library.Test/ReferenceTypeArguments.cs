using System;
using NSubstitute;
using Xunit;
using FluentAssertions;
using NFluent;

namespace Library.Test {
    public class ReferenceTypeArguments {
        private readonly IFooService _fooService;

        private readonly Component _component;

        public ReferenceTypeArguments() {
            _fooService = Substitute.For<IFooService>();

            _component = new Component(_fooService);
        }

        [Fact]
        public void Checking_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Is<PlainArgument>(
                    e => e.Id == 9 &&
                         e.FirstName == "jan" &&
                         e.LastName == "kowalski" &&
                         e.EmailAddress == "jan.kowalski@gmail.com"
                         ));
        }

        [Fact]
        public void Checking_stringable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Is<StringableArgument>(
                    e => e.Id == 9 &&
                         e.FirstName == "jan" &&
                         e.LastName == "kowalski" &&
                         e.EmailAddress == "jan.kowalski@gmail.com"
                         ));
        }

        [Fact]
        public void Checking_eqatable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Is(new EquotableArgument(
                    id: 9, 
                    firstName: "jan", 
                    lastName: "kowalski",
                    emailAddress: "jan.kowalski@gmail.com")));
        }

        [Fact]
        public void Catch_argument_and_check_manually_with_fluent_assertions() {
            // Arrange
            PlainArgument arg = null;
            
            _fooService
                .DoStuff(Arg.Do<PlainArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            // Manuall checking to error prone and usually not worth it
            arg.Should()
                .BeEquivalentTo(new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
        }

        [Fact]
        public void Catch_argument_and_check_manually_with_nfluent() {
            // Arrange
            PlainArgument arg = null;
            
            _fooService
                .DoStuff(Arg.Do<PlainArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            Check.That(arg).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
        }
        
        [Fact]
        public void Catch_argument_and_check_manually_with_nfluent_and_syntactic_sugar() {
            // Arrange
            _fooService
                .DoStuff(Capture(out Arg<PlainArgument> arg));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
        }

        // Better to not do this in production code ;)
        private static T Capture<T>(out Arg<T> tmp) {
            var valueHolder = new Arg<T>();

            T value = Arg.Do<T>(x => valueHolder.Value = x);
            
            tmp = valueHolder;
            return value;
        }

        private class Arg<T> {
            public T Value = default;
        }

        
        [Fact]
        public void Checking_argument_using_custom_NSubstitute_matcher() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            var expected = new PlainArgument(
                id: 11, 
                firstName: "jan", 
                lastName: "kowlaski", 
                emailAddress: "jan.kowalski@gmail.com");

            _fooService.Received()
                .DoStuff(ArgEx.Equivalent(expected));
        }
    }
}


/* TODO
    -structs
    -where is wooly feeling when looking for props with diff*/