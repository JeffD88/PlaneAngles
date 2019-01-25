using System;

using Mastercam.Math;
using Mastercam.Support;
using Mastercam.IO.Types;

using PlaneAngles.DataTypes;


namespace PlaneAngles.Services
{
    public class AngleService : IAngleService
    {
        public RotationAngle CalculatePositiveTilt(Matrix3D viewMatrix, RotarySetup rotarySetup)
        {
            var zAxisVector = viewMatrix.Row3;
            var topZVector = SearchManager.GetSystemView(SystemPlaneType.Top).ViewMatrix.Row3;

            var primaryAngle = 0.0;
            var secondaryAngle = 0.0;

            switch (rotarySetup)
            {
                case RotarySetup.AC:
                    if (zAxisVector != topZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(zAxisVector.x, zAxisVector.y));
                    }
                    else
                    {
                        var xAxisVector = viewMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(xAxisVector.y, xAxisVector.x));
                    }

                    secondaryAngle = ConvertRadiansToDegrees(Math.Acos(zAxisVector.z));
                    break;

                case RotarySetup.BC:
                    if (zAxisVector != topZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(zAxisVector.y, zAxisVector.x));
                    }
                    else
                    {
                        var xAxisVector = viewMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(Math.Atan2(xAxisVector.y, xAxisVector.x));
                    }
                    
                    secondaryAngle = ConvertRadiansToDegrees(Math.Acos(zAxisVector.z));
                    break;

                default:
                    break;
            }

            return new RotationAngle { PrimaryAngle = primaryAngle, SecondaryAngle = secondaryAngle, Solution = TiltSolution.Positive };
        }

        public RotationAngle CalculateNegativeTilt(Matrix3D viewMatrix, RotarySetup rotarySetup)
        {
            var zAxisVector = viewMatrix.Row3;
            var topZVector = SearchManager.GetSystemView(SystemPlaneType.Top).ViewMatrix.Row3;

            var primaryAngle = 0.0;
            var secondaryAngle = 0.0;

            switch (rotarySetup)
            {
                case RotarySetup.AC:
                    if (zAxisVector != topZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(-Math.Atan2(zAxisVector.x, -zAxisVector.y));
                    }
                    else
                    {
                        var xAxisVector = viewMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(-Math.Atan2(xAxisVector.y, -xAxisVector.x));
                    }

                    secondaryAngle = ConvertRadiansToDegrees(-Math.Acos(zAxisVector.z));
                    break;

                case RotarySetup.BC:
                    if (zAxisVector != topZVector)
                    {
                        primaryAngle = ConvertRadiansToDegrees(-Math.Atan2(zAxisVector.y, -zAxisVector.x));
                    }
                    else
                    {
                        var xAxisVector = viewMatrix.Row1;
                        primaryAngle = ConvertRadiansToDegrees(-Math.Atan2(xAxisVector.y, -xAxisVector.x));
                    }

                    secondaryAngle = ConvertRadiansToDegrees(-Math.Acos(zAxisVector.z));
                    break;

                default:
                    break;
            }

            return new RotationAngle { PrimaryAngle = primaryAngle, SecondaryAngle = secondaryAngle, Solution = TiltSolution.Negative };
        }

        private double ConvertRadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}
