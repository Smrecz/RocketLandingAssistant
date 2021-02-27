namespace RocketLandingAssistant.Model
{
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Position: (X: {X}, Y: {Y})";
        }
    }
}
