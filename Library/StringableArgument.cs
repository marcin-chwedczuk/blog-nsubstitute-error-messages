namespace Library {
    public class StringableArgument {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string EmailAddress { get; }

        public StringableArgument(int id, string firstName, string lastName, string emailAddress) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }

        public override string ToString()
            => $"{nameof(StringableArgument)}(id: {Id}, firstName: \"{FirstName}\", " +
                $"lastName: \"{LastName}\", emailAddres: \"{EmailAddress}\")";
    }
}