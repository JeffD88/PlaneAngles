using Mastercam.Math;

using PlaneAngles.DataTypes;

namespace PlaneAngles.Services
{
    public interface IAngleService
    {
        RotationAngle CalculateAngles(Matrix3D planeMatrix, RotarySetup rotarySetup);
    }
}
