using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class MainManager : MonoBehaviour
{
    [SerializeField] private BibliotecaManager bibliotecaManager; 
    [SerializeField] private UbicacionManager ubicacionManager; 
    [SerializeField] private LogrosManager logrosManager; 
    [SerializeField] private CieloManager cieloManager; 
    [SerializeField] private StellariumManager stellariumManager; 
    private List<CelestialObjectInfo> celestialObjects = new List<CelestialObjectInfo>();
    [SerializeField] private GameObject tutorial;

    private bool tutorialActive = false;

    void Start()
    {
        DatosEntreEscenas.notificacionLogros =  GameObject.Find("GONotificacionLogro");
        //El objeto debe estar activo para poder encontrarlo. Por eso lo desactivo a continuación:
        DatosEntreEscenas.notificacionLogros.SetActive(false);
        DatosEntreEscenas.playerProgress = Miscelanea.LoadPlayerProgress();
        DatosEntreEscenas.playerLogros = Miscelanea.LoadPlayerLogros();
        //DatosEntreEscenas.logrosProgreso = Miscelanea.LoadProgresoLogros();
        Debug.Log("Combined:" + Miscelanea.CombinePlayerData());

        if (PlayerPrefs.GetInt("tutorial", 1) == 1)
        {
            tutorial.SetActive(true);
            PlayerPrefs.SetInt("tutorial", 0);
            PlayerPrefs.Save();
            tutorialActive = true;
        }
        else
        {
            tutorial.SetActive(false);
        }

    }

    public IEnumerator Inicio()
    {
        DatosEntreEscenas.horaDescargaDatos = DateTime.Now;
        celestialObjects = new List<CelestialObjectInfo>();
        yield return StartCoroutine(stellariumManager.FetchCelestialData(celestialObjects));
        Debug.Log("Creando paneles biblioteca");
        if (!DatosEntreEscenas.cambiandoUbicacion)
        {
            yield return StartCoroutine(bibliotecaManager.CreatePanelsBiblioteca());
            Debug.Log("Creando paneles logros");
            yield return StartCoroutine(logrosManager.CreatePanelsLogros());
            Debug.Log("Creando paneles ciudades");
            yield return StartCoroutine(ubicacionManager.CreatePanelsCiudades());
            Debug.Log("Creando objetos del cielo");
            DatosEntreEscenas.cambiandoUbicacion = false;
        }
        yield return StartCoroutine(cieloManager.CreateObjetosCielo(celestialObjects, bibliotecaManager));
        Debug.Log("Finalizando, ocultando logros");
        
        if (!tutorialActive)
        {
            DatosEntreEscenas.logros.SetActive(false);
        }

        if (DatosEntreEscenas.cambiandoUbicacion)
        {
            DatosEntreEscenas.logros.SetActive(false);
        }

        DatosEntreEscenas.cambiandoUbicacion = false;
        
    }

}

