using Mastercam.Math;

using PlaneAngles.DataTypes;

namespace PlaneAngles.Services
{
    public interface IAngleService
    {
        RotationAngle CalculatePositiveTilt(Matrix3D planeMatrix, RotarySetup rotarySetup);

        RotationAngle CalculateNegativeTilt(Matrix3D planeMatrix, RotarySetup rotarySetup);        
    }
}
