using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
public class StellariumManager : MonoBehaviour
{
    public IEnumerator FetchCelestialData(List<CelestialObjectInfo> celestialObjects)
    {
        Debug.Log("Obteniendo datos de astros...");
        
        string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/DatosEstrellas.php";
        Debug.Log(path);

        WWWForm form = new WWWForm();
        Debug.Log(DatosEntreEscenas.correo);
        form.AddField("loginUser", DatosEntreEscenas.correo);
        form.AddField("longitud", DatosEntreEscenas.longitud.ToString());
        form.AddField("latitud", DatosEntreEscenas.latitud.ToString());
        Debug.Log("Realizando conexion");

        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            Debug.Log("Post realizado. Esperando respuesta...");
            yield return www.SendWebRequest();
            Debug.Log("Respuesta recibida.");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Respuesta recibida correctamente.");
                // Assuming the PHP script returns a JSON array of celestial objects
                string jsonResponse = "{\"constellations\":" + www.downloadHandler.text + "}";
                try
                {
                    ConstellationDataList celestialDataList = JsonUtility.FromJson<ConstellationDataList>("{\"constellations\":" + www.downloadHandler.text + "}");
                    Debug.Log("Iterando sobre objetos...");
                    foreach (var data in celestialDataList.constellations)
                    {
                       // Debug.Log("");
                       // Debug.Log($"{data.name}");
                        CelestialObjectInfo info = new CelestialObjectInfo
                        {
                            name = data.name,
                            objectType = data.objectType, // Assuming objectType is provided in the JSON
                            altitude = data.altitude,
                            azimuth = data.azimuth,
                            distance = data.distance,
                            unityPosition = CoordsManager.EquatorialToCartesian(data.azimuth, data.altitude, data.distance),
                            discovered = DatosEntreEscenas.playerProgress.discovered_constellations.Any(c => c.name == data.name),
                        };

                        celestialObjects.Add(info);

                        // Debug output for each celestial object
                        //Debug.Log($"Object: {info.name}, Type: {info.objectType}, Altitude: {info.altitude}, Azimuth: {info.azimuth}, Distancia_ {info.distance} UnityPosition: {info.unityPosition}, Discovered: {info.discovered}");
                        //Debug.Log($"Descripcion: {data.descripcion}");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error reading JSON file: " + e.Message);
                }

                Debug.Log("Fetched all celestial object data.");
            }
        } 
        yield return null;
    }
}
