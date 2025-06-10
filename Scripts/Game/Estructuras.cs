using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*[Serializable]
public class infoLogrosProgreso
{
    public string name;
    public bool conseguido;
    public int progreso;
}

[Serializable]
public class LogrosProgreso
{
    public List<infoLogrosProgreso> progresoLogros;
}*/

[Serializable]
public class infoLogros
{
    public string name;
    public bool conseguido;
    public int progreso;
}

[Serializable]
public class PlayerLogros
{
    public List<infoLogros> listaLogros;
}

[Serializable]
public class infoCuerpoProgreso
{
    public string name;
    public float altitude;
    public float azimuth;
    public float distance;
    public string location;
    public string date;
}

[Serializable]
public class PlayerProgress
{
    public List<infoCuerpoProgreso> discovered_constellations;
}

[Serializable]
public class ConstellationDataList
{
    public List<CelestialData> constellations;
}

public class CelestialObjectInfo
{
    public string name;
    public string objectType;
    public float altitude;
    public float azimuth;
    public float distance;
    public Vector3 unityPosition;
    public bool discovered;
}

[System.Serializable]
public class CelestialData
{
    public bool found;
    public string name;
    public string objectType;
    public float azimuth;
    public float altitude;
    public float distance;
    public string descripcion;
}

[System.Serializable]
public class CiudadData
{
    public string name;
    public string country;
    public float lat;
    public float lng;
}
