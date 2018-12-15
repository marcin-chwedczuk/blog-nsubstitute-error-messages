using System;
using NSubstitute;
using Xunit;
using FluentAssertions;
using NFluent;

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
            _fooService.Received()
                .DoStuff(Arg.Is<PlainValueArgument>(
                    e => e.X == 100 && e.Y == 200));
        }

        [Fact]
        public void Checking_stringable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Is<StringableValueArgument>(
                    e => e.X == 100 && e.Y == 200));
        }

        [Fact]
        public void Checking_eqatable_argument_using_Arg_Is() {
            // Arrange

            // Act
            _component.DoStuff();

            // Assert

            // Value types are equatable by default - yay!

            _fooService.Received()
                .DoStuff(Arg.Is(new StringableValueArgument(100, 200)));
        }

        [Fact]
        public void Catch_argument_and_check_manually_with_fluent_assertions() {
            // Arrange
            PlainValueArgument arg = default;
            
            _fooService
                .DoStuff(Arg.Do<PlainValueArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

            // Manuall checking to error prone and usually not worth it
            arg.Should()
                .BeEquivalentTo(new PlainValueArgument(100, 200));        
        }

        [Fact]
        public void Catch_argument_and_check_manually_with_nfluent() {
            // Arrange
            PlainValueArgument arg = default;
            
            _fooService
                .DoStuff(Arg.Do<PlainValueArgument>(x => arg = x));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

            Check.That(arg).HasFieldsWithSameValues(
                new PlainValueArgument(100, 200));        
        }
        
        [Fact]
        public void Catch_argument_and_check_manually_with_nfluent_and_syntactic_sugar() {
            // Arrange
            _fooService
                .DoStuff(Capture(out Arg<PlainValueArgument> arg));

            // Act
            _component.DoStuff();

            // Assert
            _fooService.Received()
                .DoStuff(Arg.Any<PlainValueArgument>());

            Check.That(arg.Value).HasFieldsWithSameValues(
                new PlainValueArgument(100, 200));        
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
    }
}
