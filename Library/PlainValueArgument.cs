namespace Library {
    public readonly struct PlainValueArgument {
        public readonly int X;
        public readonly int Y;

        public PlainValueArgument(int x, int y) {
            X = x;
            Y = y;
        }

        // Equals for free because we do not use reference types here.
    }
}