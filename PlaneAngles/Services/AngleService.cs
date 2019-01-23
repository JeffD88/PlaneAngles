using System;

using Mastercam.Math;

using PlaneAngles.DataTypes;


namespace PlaneAngles.Services
{
    public class AngleService : IAngleService
    {
        public RotationAngle CalculateAngles(Matrix3D planeMatrix, RotarySetup rotarySetup)
        {
            var zAxisVector = planeMatrix.Row3;
            var wcsZVector = new Point3D (0, 0, 1);

            var primaryAngle = 0.0;
            var secondaryAngle = 0.0;

            switch (rotarySetup)
            {
                case RotarySetup.AC:
                    if (zAxisVector != wcsZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(zAxisVector.x, zAxisVector.y));
                    }
                    else
                    {
                        var xAxisVector = planeMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(xAxisVector.y, xAxisVector.x));
                    }

                    secondaryAngle = ConvertRadiansToDegrees(Math.Acos(zAxisVector.z));
                    break;

                case RotarySetup.BC:
                    if (zAxisVector != wcsZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(zAxisVector.y, zAxisVector.x));
                    }
                    else
                    {
                        var xAxisVector = planeMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(xAxisVector.y, xAxisVector.x));
                    }
                    
                    secondaryAngle = ConvertRadiansToDegrees(Math.Acos(zAxisVector.z));
                    break;

                default:
                    break;
            }

            return new RotationAngle { PrimaryAngle = primaryAngle, SecondaryAngle = secondaryAngle };
        }

        private double ConvertRadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}
