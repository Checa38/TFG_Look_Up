using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DatosEntreEscenas
{
    public static string correo { get; set; } = "admin";
    public static string contrasena { get; set; } = "admin";
    public static string pathDatosUsuario { get; set; } = Application.persistentDataPath + "/playerProgression.json";
    public static string urlToLocalHost { get; set; } = "https://rested-fox-accurately.ngrok-free.app";
    public static string ubicacion { get; set; } = "Córdoba";
    public static float longitud { get; set; } = -4.7727500f;
    public static float latitud { get; set; } = 37.8915500f;
    public static DateTime horaDescargaDatos { get; set; } = DateTime.Now;
    public static bool logged { get; set; } = false;

    public static bool cambiandoUbicacion { get; set; } = false;
    
    public static int biblioLogro { get; set; } = 0;
    public static int constDescLogro { get; set; } = 0;
    public static int generalLogro { get; set; } = 0;
    public static int planetLogro { get; set; } = 0;
    public static int ubiLogro { get; set; } = 0;

    public static PlayerProgress playerProgress;
    public static PlayerLogros playerLogros;

    public static GameObject logros;
    public static GameObject notificacionLogros;
    public static bool conseguido = false;

    public static float latitudCheck { get; set; } = 0f;
   // public static LogrosProgreso logrosProgreso;

}