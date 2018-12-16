using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace Library {
    // from: https://github.com/nsubstitute/NSubstitute/issues/160#issuecomment-53013051

    public class EquivalentArgumentMatcher<T> : IArgumentMatcher, IDescribeNonMatches
    {
        private static readonly ArgumentFormatter DefaultArgumentFormatter = new ArgumentFormatter();
        private readonly object _expected;
        private readonly Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> _options;

        public EquivalentArgumentMatcher(object expected)
            : this(expected, x => x.IncludingAllDeclaredProperties())
        {
        }

        public EquivalentArgumentMatcher(object expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options)
        {
            _expected = expected;
            _options = options;
        }

        public override string ToString()
        {
            return DefaultArgumentFormatter.Format(_expected, false);
        }

        public string DescribeFor(object argument)
        {
            try
            {
                ((T)argument).Should().BeEquivalentTo((T)_expected, _options);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool IsSatisfiedBy(object argument)
        {
            try
            {
                ((T)argument).Should().BeEquivalentTo((T)_expected, _options);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class WithArg
    {
        public static T EquivalentTo<T>(T obj)
        {
            SubstitutionContext.Current.EnqueueArgumentSpecification(
                new ArgumentSpecification(typeof(T), new EquivalentArgumentMatcher<T>(obj)));
            return default(T);
        }

        public static T Equivalent<T>(T obj, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options)
        {
            SubstitutionContext.Current.EnqueueArgumentSpecification(new ArgumentSpecification(typeof(T), new EquivalentArgumentMatcher<T>(obj, options)));
            return default(T);
        }
    }
}