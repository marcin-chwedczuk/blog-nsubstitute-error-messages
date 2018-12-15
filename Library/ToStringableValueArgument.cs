namespace Library {
    public readonly struct StringableValueArgument {
        public readonly int X;
        public readonly int Y;

        public StringableValueArgument(int x, int y) {
            X = x;
            Y = y;
        }

        // Equals for free because we do not use reference types here.

        public override string ToString() 
            => $"{nameof(StringableValueArgument)}({X}, {Y})";
    }
}