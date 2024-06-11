using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroLab
{
    public static class CoordinateUtility
    {
        /// <summary>
        /// Converts right ascension and declination into cartesian coordinates on a unit
        /// sphere (r = 1)
        /// </summary>
        /// <param name="rAsc">in degrees</param>
        /// <param name="decl"></param>
        /// <returns></returns>
        public static Vector3 RAscDeclDegreesToCartesianCoordinates(float rAsc, float decl)
        {
            double raRad = DegreeToRadian(rAsc);
            double decRad = DegreeToRadian(decl);

            return RadiansToCartesianCoordinates((float)raRad, (float)decRad);
        }

        public static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180.0;
        }

        /// <summary>
        /// Converts right ascension and declination into cartesian coordinates on a unit
        /// sphere (r = 1)
        /// </summary>
        /// <param name="rAsc">in degrees</param>
        /// <param name="decl"></param>
        /// <returns></returns>
        public static Vector3 RadiansToCartesianCoordinates(float raRad, float decRad)
        {
            // Note: Skybox map is rotated -90 degrees in the z from expected calculations
            
            // Expected
            double x = Math.Cos(raRad) * Math.Cos(decRad);
            double y = Math.Sin(raRad) * Math.Cos(decRad);
            double z = Math.Sin(decRad);

            // Corrected
            double correctedX = -y;
            double correctedY = z;
            double correctedZ = x;

            return new Vector3((float)correctedX, (float)correctedY, (float)correctedZ);
        }

        /// <summary>
        /// Converts Right Ascension hours, minutes, and seconds into degrees
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double RAToDegrees(int hours, int minutes, double seconds)
        {
            // Convert hours, minutes, and seconds to degrees
            double totalHours = hours + (minutes / 60.0) + (seconds / 3600.0);
            double degrees = totalHours * 15; // 1 hour of RA equals 15 degrees
            return degrees;
        }


        /// <summary>
        /// Converts Right Ascension hours, minutes, and seconds into degrees
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double DeclensionToDecimalDegrees(int hours, int minutes, double seconds)
        {
            // Convert hours, minutes, and seconds to degrees
            double totalHours = hours + (minutes / 60.0) + (seconds / 3600.0);
            double degrees = totalHours;
            return degrees;
        }

        /// <summary>
        /// Converts degree hours, minutes, and seconds into degrees
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double DegreesToDecimalDegrees(int hours, int minutes, double seconds)
        {
            // Convert hours, minutes, and seconds to degrees
            double totalHours = hours + (minutes / 60.0) + (seconds / 3600.0);
            double degrees = totalHours;
            return degrees;
        }

        /// <summary>
        /// Converts right ascension and declination into cartesian coordinates on a unit
        /// sphere (r = 1)
        /// </summary>
        /// <param name="rAsc">in degrees</param>
        /// <param name="decl"></param>
        /// <returns></returns>
        public static Vector3 LatLongToCartesianCoordinates(float lat, float longitude)
        {
            double latRad = DegreeToRadian(lat);
            double longRad = DegreeToRadian(longitude);

            return RadiansToCartesianCoordinates((float)latRad, (float)longRad);
        }

        public static Vector2 CartesianToPolar(Vector3 cartPoint)
        {
            Vector2 polarPoint;

            //calc longitude
            polarPoint.y = Mathf.Atan2(cartPoint.x, cartPoint.z);

            //this is easier to write and read than sqrt(pow(x,2), pow(y,2))!
            var xzLen = new Vector2(cartPoint.x, cartPoint.z).magnitude;

            //atan2 does the magic
            polarPoint.x = Mathf.Atan2(-cartPoint.y,xzLen);

            //convert to deg
            polarPoint *= Mathf.Rad2Deg;

            return polarPoint;
        }


        public static Vector3 PolarToCartesian(Vector2 polarPoint)
        {

            //an origin vector, representing lat,lon of 0,0. 
            var origin = new Vector3(0, 0, 1);
             
            // build a quaternion using euler angles for lat,lon
            var rotation = Quaternion.Euler(polarPoint.x, polarPoint.y, 0);

            //transform our reference vector by the rotation. Easy-peasy!
            Vector3 cartPoint = rotation * origin;

            return cartPoint;
        }

        public static Vector3 RAscDeclToSphericalCoordinates(float rAsc, float decl)
        {
            double raRad = Math.PI * rAsc / 180.0;
            double decRad = Math.PI * decl / 180.0;

            double x = Math.Cos(raRad) * Math.Cos(decRad);
            double y = Math.Sin(raRad) * Math.Cos(decRad);
            double z = Math.Sin(decRad);

            double r = Math.Sqrt(x * x + y * y + z * z);
            double inclination = Math.Acos(z / r) * 180.0 / Math.PI;
            double azimuth = Math.Atan2(y, x) * 180.0 / Math.PI;

            return new Vector3((float)r, (float)inclination, (float)azimuth);
        }

        public static Vector3 SphericalToCartesianCoordinates(float radius, float inclination, float azimuth)
        {
            double inclinationRad = Math.PI * inclination / 180.0;
            double azimuthRad = Math.PI * azimuth / 180.0;

            double x = radius * Math.Sin(inclinationRad) * Math.Cos(azimuthRad);
            double y = radius * Math.Sin(inclinationRad) * Math.Sin(azimuthRad);
            double z = radius * Math.Cos(inclinationRad);

            return new Vector3((float)x, (float)y, (float)z);
        }
    }

}