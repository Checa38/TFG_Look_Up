using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordsManager : MonoBehaviour
{
        public static Vector3 EquatorialToCartesian(float azimuth, float altitude, float distance)
        {
            /*float escalado = distance; 
            //Debug.Log($"Azimuth {azimuth} y altitud {altitude}"); 
            // Convert RA and Dec from degrees to radians
            float azimuthRad = azimuth * Mathf.Deg2Rad;
            float altitudeRad = altitude * Mathf.Deg2Rad;
            //Debug.Log($"AzimuthRad {azimuthRad} altitudRad {altitudeRad}"); 

            // Calculate Cartesian coordinates
            float x = Mathf.Cos(altitudeRad) * Mathf.Sin(azimuthRad)  * escalado;
            float y = Mathf.Cos(altitudeRad) * Mathf.Cos(azimuthRad)  * escalado;
            float z = Mathf.Sin(altitudeRad)  * escalado;
            //Debug.Log($"X{x} Y{y} Z"); 

            return new Vector3(x, y, z);*/

            distance = 15f; 
            // Convert RA and Dec from degrees to radians
            float azimuthRad = azimuth * Mathf.Deg2Rad;
            float altitudeRad = altitude * Mathf.Deg2Rad;

            // Calculate Cartesian coordinates
            float x = Mathf.Cos(altitudeRad) * Mathf.Cos(azimuthRad) * distance;
            float y = Mathf.Cos(altitudeRad) * Mathf.Sin(azimuthRad) * distance;
            float z = Mathf.Sin(altitudeRad) * distance;

            return new Vector3(x, y, z);
        }
}
