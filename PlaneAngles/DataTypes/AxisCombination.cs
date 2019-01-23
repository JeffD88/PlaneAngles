namespace PlaneAngles.DataTypes
{
    public enum RotarySetup
    {
        AC,
        BC
    }

    public class AxisCombination
    {
        public RotarySetup Setup { get; set; }

        public string CombinationName { get; set; }

        public string SecondaryAxisLabel { get; set; }

        public string PrimaryAxisLabel { get; set; }
    }
}
