/*using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
[Serializable]
public class Constellation
{
    public string Name;
    public float RightAscension;
    public float Declination;
    public string PrincipalStar;
    public Vector3 UnityPosition; // Añadir posición en Unity
    public bool Descubierta; // Nuevo parámetro

    public Constellation(string name, float ra, float dec, string principalStar, bool descubierta = false)
    {
        Name = name;
        RightAscension = ra;
        Declination = dec;
        PrincipalStar = principalStar;
        Descubierta = descubierta;
    }
}

[Serializable]
public class ConstellationData
{
    public string season_saison;
    public string iau_code;
    public string latin_name_nom_latin;
    public string french_name_nom_francais;
    public string english_name_nom_en_anglais;
    public float constellation_area_in_degrees_etendue_de_la_constellation_en_degres_2;
    public string dec_declinaison;
    public string test;  // RA (Right Ascension)
    public string principal_star_etoile_principale;
    // Otros campos no utilizados en este ejemplo
}

[Serializable]
public class ConstellationDataList
{
    public List<ConstellationData> constellations;
}

[Serializable]
public class PlayerProgress
{
    public List<string> discovered_constellations = new List<string>();
}

public class ConstellationManager : MonoBehaviour
{
    public TextAsset jsonFile; // Asigna el archivo JSON desde el inspector 
    public float observerLatitude = 0f;
    public float observerLongitude = 0f;
    public List<Constellation> constellations;

    public string progressFilePath;

    void Start()
    {
        progressFilePath = Application.persistentDataPath + "/playerProgression.json";
        if (jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned!");
            return;
        }

        Debug.Log("JSON File Content:\n" + jsonFile.text);

        LoadConstellations();
        CalculateCurrentPositions();
        Debug.Log("Creando paneles de biblioteca...");
        CreatePanels();
        CreateConstelaciones();
        Debug.Log("Creando planetas...");
        ColocarPlanetas();
    }

    void LoadConstellations()
    {
        constellations = new List<Constellation>();
        PlayerProgress playerProgress = LoadPlayerProgress();

        try
        {
            ConstellationDataList dataList = JsonUtility.FromJson<ConstellationDataList>("{\"constellations\":" + jsonFile.text + "}");
            
            foreach (var data in dataList.constellations)
            {
                if (string.IsNullOrEmpty(data.test) || string.IsNullOrEmpty(data.dec_declinaison))
                {
                    Debug.LogWarning($"Skipping constellation due to missing RA or Dec. Data: {JsonUtility.ToJson(data)}");
                    continue;
                }

                float ra = ParseRA(data.test);
                float dec = ParseDec(data.dec_declinaison);
                bool descubierta = playerProgress.discovered_constellations.Contains(data.latin_name_nom_latin);

                Constellation constellation = new Constellation(data.latin_name_nom_latin, ra, dec, data.principal_star_etoile_principale, descubierta);
                constellations.Add(constellation);

                //Debug.Log($"Loaded constellation: {data.latin_name_nom_latin}, RA: {ra}, Dec: {dec}, Principal Star: {data.principal_star_etoile_principale}, Descubierta: {descubierta}");
            }

            //Debug.Log($"Loaded {constellations.Count} constellations.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading JSON file: " + e.Message);
        }
    }

    PlayerProgress LoadPlayerProgress()
    {
        Debug.Log(progressFilePath);
        if (File.Exists(progressFilePath))
        {
            Debug.Log("SÍ existe el archivo de progreso");
            string json = File.ReadAllText(progressFilePath);
            print(json);
            return JsonUtility.FromJson<PlayerProgress>(json);
        }
        else
        {
            Debug.Log("NO existe el archivo de progreso");
            return new PlayerProgress();
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        // Cargar la imagen desde el disco
        byte[] fileData;
        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
            {
                return texture; // Retorna la textura cargada
            }
        }
        return null; // Retorna null si la carga falla
    }

    float ParseRA(string raStr)
    {
        try
        {
            string[] parts = raStr.Split(':', ',');
            float hours = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float minutes = parts.Length > 1 ? float.Parse(parts[1], CultureInfo.InvariantCulture) : 0;
            return hours + minutes / 60f;
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing RA: " + raStr + " - " + e.Message);
            return 0f;
        }
    }

    float ParseDec(string decStr)
    {
        try
        {
            string[] parts = decStr.Split(new char[] { '°', '\'', ',' }, StringSplitOptions.RemoveEmptyEntries);
            float degrees = float.Parse(parts[0].Replace("+", "").Replace(",", "."), CultureInfo.InvariantCulture);
            float minutes = parts.Length > 1 ? float.Parse(parts[1].Replace(",", "."), CultureInfo.InvariantCulture) : 0;
            return degrees < 0 ? degrees - minutes / 60f : degrees + minutes / 60f;
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing Dec: " + decStr + " - " + e.Message);
            return 0f;
        }
    }

    public GameObject panelPrefab; // Prefab del Panel
    public Transform parentElement; // Elemento padre bajo el cual se crearán los paneles

    public GameObject constelacionPrefab; // Prefab del Panel
    public Transform parentElementConstelacion; // Elemento padre bajo el cual se crearán los paneles

    void CreatePanels()
    {
        foreach (Constellation constellation in constellations)
        {
            if (!constellation.Descubierta)
                continue;
            GameObject panel = Instantiate(panelPrefab, parentElement);
            // Aquí puedes añadir lógica adicional para configurar el panel si es necesario
            Debug.Log("Siguiente:");
            Debug.Log(panel.GetComponentInChildren<Text>().text);
            Debug.Log(constellation.Name);
            panel.GetComponentInChildren<Text>().text = constellation.Name;

            // Asignar imagen a la RawImage
            var texture = Resources.Load<Texture2D>("Constelaciones_ilustraciones/" + constellation.Name);
            //string imagePath = Path.Combine("Assets/Imagenes/Constelaciones_ilustraciones", constellation.Name + ".png");
            RawImage rawImage = panel.GetComponentInChildren<RawImage>();
            if (rawImage != null)
            {
                //Texture2D texture = LoadTexture(imagePath);
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar la imagen para {constellation.Name} ");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente RawImage en el panel prefab.");
            }
        }
    }

    void CreateConstelaciones()
    {

        foreach (Constellation constellation in constellations)
        {
            //if (!constellation.Descubierta)
            //    continue;

            // Instanciar el prefab del 2D Object
            GameObject spriteObject = Instantiate(constelacionPrefab, parentElementConstelacion);

            // Configurar el nombre del objeto
            spriteObject.name = constellation.Name;

            // Obtener el componente SpriteRenderer del prefab instanciado
            SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogWarning("El prefab no contiene un componente SpriteRenderer.");
                continue;
            }

            // Asignar sprite al SpriteRenderer
            //string spritePath = Path.Combine("Assets/Imagenes/Constelaciones_ilustraciones", constellation.Name + ".png");
            //Sprite sprite = LoadSprite(spritePath);
            var sprite = Resources.Load<Sprite>("Constelaciones_cielo/" + constellation.Name);
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"No se pudo cargar el sprite para {constellation.Name} ");
            }

            // Configurar las dimensiones del SpriteRenderer si es necesario
           // spriteRenderer.transform.localScale = GetConstellationPositionByName(constellation.Name); // Ajustar según el tamaño deseado del sprite
            spriteRenderer.transform.localPosition = GetConstellationPositionByName(constellation.Name);

            // Asegurar que la imagen siempre mire hacia la cámara
            spriteRenderer.transform.LookAt(Camera.main.transform);
            spriteRenderer.transform.Rotate(0, 180, 0); // Ajustar rotación si es necesario

              // Configurar el texto del objeto hijo "Title"
            Transform titleTransform = spriteObject.transform.Find("AR Annotation/Title");
            if (titleTransform != null)
            {
                TextMeshPro titleText = titleTransform.GetComponent<TextMeshPro>();
                if (titleText != null)
                {
                    titleText.text = constellation.Name;
                }
                else
                {
                    Debug.LogWarning("No se encontró el componente Text en el objeto hijo 'Title'.");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el objeto hijo 'AR Annotation/Title'.");
            }


            Debug.Log($"Sprite creado para la constelación: {constellation.Name}");
        }

    }

    public Vector3 GetConstellationPositionByName(string constellationName)
    {
        foreach (Constellation constellation in constellations)
        {
            if (constellation.Name.Equals(constellationName, StringComparison.OrdinalIgnoreCase))
            {
                return constellation.UnityPosition;
            }
        }
        Debug.LogWarning($"Constellation '{constellationName}' not found.");
        return Vector3.zero; // Devuelve Vector3.zero si no se encuentra la constelación
    }

    void CalculateCurrentPositions()
    {
        DateTime utcNow = DateTime.UtcNow;
        double julianDate = GetJulianDate(utcNow);
        double gst = GetGST(julianDate);
        double lst = gst + observerLongitude / 15.0;

        foreach (var constellation in constellations)
        {
            double ha = lst - constellation.RightAscension;
            if (ha < 0) ha += 24;
            if (ha > 24) ha -= 24;

            double haRad = ha * (Math.PI / 12.0);
            double decRad = constellation.Declination * (Math.PI / 180.0);
            double latRad = observerLatitude * (Math.PI / 180.0);

            double sinAlt = Math.Sin(decRad) * Math.Sin(latRad) + Math.Cos(decRad) * Math.Cos(latRad) * Math.Cos(haRad);
            double alt = Math.Asin(sinAlt) * (180.0 / Math.PI);

            double cosAz = (Math.Sin(decRad) - Math.Sin(alt * (Math.PI / 180.0)) * Math.Sin(latRad)) / (Math.Cos(alt * (Math.PI / 180.0)) * Math.Cos(latRad));
            double az = Math.Acos(cosAz) * (180.0 / Math.PI);

            if (Math.Sin(haRad) > 0)
                az = 360 - az;

            // Convertir acimut y altitud a coordenadas de Unity
            float x = Mathf.Cos((float)alt * Mathf.Deg2Rad) * Mathf.Sin((float)az * Mathf.Deg2Rad);
            float y = Mathf.Sin((float)alt * Mathf.Deg2Rad);
            float z = Mathf.Cos((float)alt * Mathf.Deg2Rad) * Mathf.Cos((float)az * Mathf.Deg2Rad);

            // Alejar las constelaciones de la cámara (punto 0,0,0)
            float distance = 15f; // Puedes ajustar esta distancia según sea necesario
            x *= distance;
            y *= distance;
            z *= distance;

            constellation.UnityPosition = new Vector3(x, y, z);

            //Debug.Log($"Constellation: {constellation.Name}, Azimuth: {az}, Altitude: {alt}, Unity Position: {constellation.UnityPosition}, Principal Star: {constellation.PrincipalStar}");
        }
    }

    double GetJulianDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        if (month <= 2)
        {
            year -= 1;
            month += 12;
        }
        int day = date.Day;
        double hour = date.Hour + date.Minute / 60.0 + date.Second / 3600.0;

        int A = year / 100;
        int B = 2 - A + A / 4;
        double jd = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + hour / 24.0 - 1524.5 + B;
        return jd;
    }

    double GetGST(double julianDate)
    {
        double jd0 = Math.Floor(julianDate - 0.5) + 0.5;
        double H = (julianDate - jd0) * 24.0;
        double D = julianDate - 2451545.0;
        double D0 = jd0 - 2451545.0;
        double T = D / 36525.0;
        double gst = 6.697374558 + 0.06570982441908 * D0 + 1.00273790935 * H + 0.000026 * (T * T);
        gst = gst % 24;
        if (gst < 0) gst += 24;
        return gst;
    }

    void ColocarPlanetas()
    {
        Dictionary<string, Vector3> planetPositions = new Dictionary<string, Vector3>
        {
            { "Sol", new Vector3(0, 0, 0) },
            { "Mercurio", new Vector3(0.4f, 0, 0) },
            { "Venus", new Vector3(0.7f, 0, 0) },
            { "Tierra", new Vector3(1, 0, 0) },
            { "Marte", new Vector3(1.5f, 0, 0) },
            { "Jupiter", new Vector3(5.2f, 0, 0) },
            { "Saturno", new Vector3(9.5f, 0, 0) },
            { "Urano", new Vector3(19.8f, 0, 0) },
            { "Neptuno", new Vector3(30.1f, 0, 0) }
        };

        foreach (var planet in planetPositions)
        {
            GameObject planetObject = GameObject.Find(planet.Key);
            if (planetObject != null)
            {
                planetObject.transform.localPosition = planet.Value;
            }
            else
            {
                Debug.LogWarning($"No se encontró el objeto de juego para el planeta '{planet.Key}'.");
            }
        }
    }
}
*/