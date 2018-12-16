using System;
using NSubstitute;
using Xunit;
using FluentAssertions;
using NFluent;

// Import static methods from ArgCapture
using static Library.Test.ArgCapture;

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

            // passes
            _fooService.Received()
                .DoStuff(Arg.Is<PlainArgument>(
                    e => e.Id == 7 &&
                         e.FirstName == "john" &&
                         e.LastName == "doe" &&
                         e.EmailAddress == "john.doe@gmail.com"
                         ));

            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is<PlainArgument>(
                    e => e.Id == 9 &&
                         e.FirstName == "jan" &&
                         e.LastName == "kowalski" &&
                         e.EmailAddress == "jan.kowalski@gmail.com"
                         ));
            // */
        }

        [Fact]
        public void Checking_stringable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert

            // passes
            _fooService.Received()
                .DoStuff(Arg.Is<StringableArgument>(
                    e => e.Id == 7 &&
                         e.FirstName == "john" &&
                         e.LastName == "doe" &&
                         e.EmailAddress == "john.doe@gmail.com"
                         ));

            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is<StringableArgument>(
                    e => e.Id == 9 &&
                         e.FirstName == "jan" &&
                         e.LastName == "kowalski" &&
                         e.EmailAddress == "jan.kowalski@gmail.com"
                         ));
            // */
        }

        [Fact]
        public void Checking_eqatable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            // passes
            _fooService.Received()
                .DoStuff(Arg.Is(new EquotableArgument(
                    id: 7, 
                    firstName: "john", 
                    lastName: "doe",
                    emailAddress: "john.doe@gmail.com")));

            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is(new EquotableArgument(
                    id: 9, 
                    firstName: "jan", 
                    lastName: "kowalski",
                    emailAddress: "jan.kowalski@gmail.com")));
            // */
        }

        [Fact]
        public void Catching_argument_and_checking_manually_with_fluent_assertions() {
            // Arrange
            PlainArgument arg = null;
            
            _fooService
                .DoStuff(Arg.Do<PlainArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            // passes
            arg.Should()
                .BeEquivalentTo(new PlainArgument(
                    id: 7, 
                    firstName: "john", 
                    lastName: "doe", 
                    emailAddress: "john.doe@gmail.com"));    

            /*
            // fails
            arg.Should()
                .BeEquivalentTo(new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
            // */
        }

        [Fact]
        public void Catching_argument_and_checking_manually_with_nfluent() {
            // Arrange
            PlainArgument arg = null;
            
            _fooService
                .DoStuff(Arg.Do<PlainArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            // passes
            Check.That(arg).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 7, 
                    firstName: "john", 
                    lastName: "doe", 
                    emailAddress: "john.doe@gmail.com"));        
            /*
            // fails
            Check.That(arg).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
            // */
        }
        
        [Fact]
        public void Catching_argument_and_checking_manually_with_nfluent_using_syntactic_sugar() {
            // Arrange
            _fooService
                .DoStuff(Capture(out Arg<PlainArgument> arg));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainArgument>());

            // passes
            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 7, 
                    firstName: "john", 
                    lastName: "doe", 
                    emailAddress: "john.doe@gmail.com"));        
            /*
            // fails
            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainArgument(
                    id: 11, 
                    firstName: "jan", 
                    lastName: "kowlaski", 
                    emailAddress: "jan.kowalski@gmail.com"));        
            // */
        }

        [Fact]
        public void Checking_argument_using_custom_NSubstitute_matcher() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            
            // passes
            var expected = new PlainArgument(
                id: 7, 
                firstName: "john", 
                lastName: "doe", 
                emailAddress: "john.doe@gmail.com");

            _fooService.Received()
                .DoStuff(WithArg.EquivalentTo(expected));

            /*
            // fails
            var expected2 = new PlainArgument(
                id: 11, 
                firstName: "jan", 
                lastName: "kowlaski", 
                emailAddress: "jan.kowalski@gmail.com");

            _fooService.Received()
                .DoStuff(WithArg.EquivalentTo(expected2));
            // */
        }
    }
}


/* TODO
    -structs
    -where is wooly feeling when looking for props with diff
                // Manuall checking to error prone and usually not worth it*/