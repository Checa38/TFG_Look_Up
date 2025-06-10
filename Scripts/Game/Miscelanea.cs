using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
public class Miscelanea : MonoBehaviour
{
    
    public static PlayerProgress LoadPlayerProgress()
    {
        string progressFilePath = DatosEntreEscenas.pathDatosUsuario;

        Debug.Log(progressFilePath);
        if (File.Exists(progressFilePath))
        {
            string json = File.ReadAllText(progressFilePath);  
            return JsonUtility.FromJson<PlayerProgress>(json);
        }
        else
        {
            Debug.Log("NO existe el archivo de progreso");
            return new PlayerProgress();
        }
    }

    public static PlayerLogros LoadPlayerLogros()
    {
        string progressFilePath = DatosEntreEscenas.pathDatosUsuario;

        Debug.Log(progressFilePath);
        if (File.Exists(progressFilePath))
        {
            string json = File.ReadAllText(progressFilePath);  
            return JsonUtility.FromJson<PlayerLogros>(json);
        }
        else
        {
            Debug.Log("NO existe el archivo de progreso");
            return new PlayerLogros();
        }
    }
    
    public static bool UpdatePlayerProgress(infoCuerpoProgreso newConstellation)
    {
        string progressFilePath = DatosEntreEscenas.pathDatosUsuario;

        bool constellationExists = DatosEntreEscenas.playerProgress.discovered_constellations.Exists(c => c.name == newConstellation.name);

        if (!constellationExists)
        {
            DatosEntreEscenas.playerProgress.discovered_constellations.Add(newConstellation);
            
            // Combinar ambos objetos en un único JSON
            string updatedJson = CombinePlayerData();

            File.WriteAllText(progressFilePath, updatedJson);

            Debug.Log("Archivo de progreso actualizado: " + updatedJson);
            return true;
        }
        else
        {
            Debug.Log("La constelación ya está descubierta. No actualizar local.");
            return false;
        }
    }

    public static string CombinePlayerData()
    {
        var combinedData = new
        {
            discovered_constellations = DatosEntreEscenas.playerProgress?.discovered_constellations,
            listaLogros = DatosEntreEscenas.playerLogros?.listaLogros
        };

        return JsonConvert.SerializeObject(combinedData, Formatting.Indented);
    }

    public static bool UpdateLogroProgreso(string nombreLogro)
    {
        // Verificar si el logro existe en la lista de logros del jugador.
        infoLogros logro = DatosEntreEscenas.playerLogros.listaLogros.Find(l => l.name == nombreLogro);

        if (logro != null)
        {
            // Verificar si el logro ya ha sido conseguido.
            if (!logro.conseguido)
            {
                // Sumar 1 al progreso del logro.
                logro.progreso += 1;
                
                // Verificar si el progreso ha alcanzado el valor requerido para conseguir el logro.
                if (ProgresoLogrosManager.valoresParaConseguirLogro.ContainsKey(nombreLogro))
                {
                    int valorParaConseguir = ProgresoLogrosManager.valoresParaConseguirLogro[nombreLogro];

                    if (logro.progreso >= valorParaConseguir)
                    {
                        logro.conseguido = true; // Marcar el logro como conseguido.
                        Debug.Log("Logro conseguido: " + nombreLogro);
                        DatosEntreEscenas.conseguido = true;

                        // Actualizar el archivo JSON con los datos combinados.
                        string updatedJson = CombinePlayerData();
                        File.WriteAllText(DatosEntreEscenas.pathDatosUsuario, updatedJson);

                        return true; // Retornar true indicando que el logro ha sido conseguido.
                    }
                    else
                    {
                        // Actualizar el archivo JSON con los datos combinados.
                        string updatedJson = CombinePlayerData();
                        File.WriteAllText(DatosEntreEscenas.pathDatosUsuario, updatedJson);

                        return false; // Retornar false indicando que el logro aún no ha sido conseguido.
                    }
                }
                else
                {
                    Debug.LogWarning("El logro " + nombreLogro + " no tiene un valor definido para ser conseguido.");
                }
            }
            else
            {
                Debug.Log("El logro ya ha sido conseguido previamente. " + logro.progreso + " " + logro.conseguido);
            }
            
        }
        else
        {
            Debug.LogWarning("Logro no encontrado: " + nombreLogro);
        }

        return false; // En caso de error o de que no se cumplan las condiciones.
    }


}