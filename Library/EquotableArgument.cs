using System;

namespace Library {
    public class EquotableArgument : IEquatable<EquotableArgument> {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string EmailAddress { get; }

        public EquotableArgument(int id, string firstName, string lastName, string emailAddress) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }

        public override string ToString()
            => $"{nameof(StringableArgument)}(id: {Id}, firstName: \"{FirstName}\", " +
                $"lastName: \"{LastName}\", emailAddres: \"{EmailAddress}\")";

        public bool Equals(EquotableArgument other) {
            if (other is null) return false;

            return ToTuple(this) == ToTuple(other);
        }

        public override bool Equals(object obj) {
            if (obj is EquotableArgument other) {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
            => ToTuple(this).GetHashCode();

        private static (int, string, string, string) ToTuple(EquotableArgument arg) {
            return (arg.Id, arg.FirstName, arg.LastName, arg.EmailAddress);
        }
    }
}
