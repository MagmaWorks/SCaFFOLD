namespace CalcCore
{
    public class CalcCorePoint2D
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        private CalcCorePoint2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static CalcCorePoint2D Point2DByCoordinates(double x, double y)
        {
            return new CalcCorePoint2D(x, y);
        }
    }
}