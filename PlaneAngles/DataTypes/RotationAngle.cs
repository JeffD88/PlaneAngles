namespace PlaneAngles.DataTypes
{
    public enum TiltSolution
    {
        Positive,
        Negative
    }

    public class RotationAngle
    {
        public double PrimaryAngle { get; set; }
        public double SecondaryAngle { get; set; }
        public TiltSolution Solution { get; set; }
    }
}
