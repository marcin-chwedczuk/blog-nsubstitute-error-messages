using System;

namespace Library {
    public class Component {
        private readonly IFooService _fooService;

        public Component(IFooService fooService) {
            _fooService = fooService 
                ?? throw new ArgumentNullException(nameof(fooService));
        }

        public void DoStuff() {
            // for reference types:

            _fooService.DoStuff(
                new PlainArgument(id: 7, firstName: "john", lastName: "doe", emailAddress: "john.doe@gmail.com"));

            _fooService.DoStuff(
                new StringableArgument(id: 7, firstName: "john", lastName: "doe", emailAddress: "john.doe@gmail.com"));

            _fooService.DoStuff(
                new EquotableArgument(id: 7, firstName: "john", lastName: "doe", emailAddress: "john.doe@gmail.com"));

            // for value types:

            _fooService.DoStuff(
                new PlainValueArgument(3, 7));

            _fooService.DoStuff(
                new StringableValueArgument(3, 7));
        }
    }
}
