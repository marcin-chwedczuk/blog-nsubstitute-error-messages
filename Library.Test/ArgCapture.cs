using System;
using NSubstitute;

namespace Library.Test {
    // Better to not use this in production code ;)

    public static class ArgCapture {    
        public static T Capture<T>(out Arg<T> tmp) {
            var valueHolder = new ArgImpl<T>();

            T value = Arg.Do<T>(x => valueHolder.Value = x);
            
            tmp = valueHolder;
            return value;
        }

        private class ArgImpl<T> : Arg<T> {
            public bool IsSet { get; private set; }

            private T _value;
            public T Value { 
                get {
                    if (!IsSet) throw new InvalidOperationException("Value is not yet set.");
                    return _value;
                }
                set { 
                    if (IsSet) throw new InvalidOperationException("Value is already set.");
                    _value = value;
                    IsSet = true;
                }
            }

            public ArgImpl() {
                _value = default;
                IsSet = false;
            }
        }
    }

    public interface Arg<T> {
        T Value { get; }
    }

}