using System;
using NSubstitute;
using Xunit;
using FluentAssertions;
using NFluent;

// Import static methods from ArgCapture
using static Library.Test.ArgCapture;

namespace Library.Test {
    public class ValueTypeArguments {
        private readonly IFooService _fooService;

        private readonly Component _component;

        public ValueTypeArguments() {
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
                .DoStuff(Arg.Is<PlainValueArgument>(
                    e => e.X == 3 && e.Y == 7));

            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is<PlainValueArgument>(
                    e => e.X == 100 && e.Y == 200));
            // */
        }

        [Fact]
        public void Checking_stringable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Is<StringableValueArgument>(
                    e => e.X == 3 && e.Y == 7));

            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is<StringableValueArgument>(
                    e => e.X == 100 && e.Y == 200));
            // */
        }

        [Fact]
        public void Checking_equatable_argument_using_Arg_Is() {
            // Value types are equatable by default - yay!

            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            
            // passes
            _fooService.Received()
                .DoStuff(Arg.Is(new StringableValueArgument(3, 7)));
            
            /*
            // fails
            _fooService.Received()
                .DoStuff(Arg.Is(new StringableValueArgument(100, 200)));
            // */
        }

        [Fact]
        public void Catching_argument_and_checking_manually_with_fluent_assertions() {
            // Arrange
            PlainValueArgument arg = default;
            
            _fooService
                .DoStuff(Arg.Do<PlainValueArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

            // passes
            arg.Should()
                .BeEquivalentTo(new PlainValueArgument(3, 7));        

            /*
            // fails
            arg.Should()
                .BeEquivalentTo(new PlainValueArgument(100, 200));        
            // */
        }

        [Fact]
        public void Catching_argument_and_checking_manually_with_nfluent() {
            // Arrange
            PlainValueArgument arg = default;
            
            _fooService
                .DoStuff(Arg.Do<PlainValueArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

                
            // passes
            Check.That(arg).HasFieldsWithSameValues(
                new PlainValueArgument(3, 7));

            /*
            // fails
            Check.That(arg).HasFieldsWithSameValues(
                new PlainValueArgument(100, 200));        
            // */
        }
        
        [Fact]
        public void Catching_argument_and_checking_manually_with_nfluent_and_syntactic_sugar() {
            // Arrange
            _fooService
                .DoStuff(Capture(out Arg<PlainValueArgument> arg));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

            // passes
            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainValueArgument(3, 7));

            /*
            // fails
            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainValueArgument(100, 200));        
            // */
        }
    }
}
