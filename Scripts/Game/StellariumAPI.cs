using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

//Clase en la que se almacenará el proceso del jugador
[Serializable]
public class PlayerProgress
{
    public List<string> discovered_constellations = new List<string>();
}

public class CoordinateConverter
{
    
    public static Vector3 EquatorialToCartesian(float ra, float dec)
    {
        float distance = 15f; 
        // Convert RA and Dec from degrees to radians
        float raRad = ra * Mathf.Deg2Rad;
        float decRad = dec * Mathf.Deg2Rad;

        // Calculate Cartesian coordinates
        float x = Mathf.Cos(decRad) * Mathf.Cos(raRad) * distance;
        float y = Mathf.Cos(decRad) * Mathf.Sin(raRad) * distance;
        float z = Mathf.Sin(decRad) * distance;

        return new Vector3(x, y, z);
    }
}
public class CelestialObjectInfo
{
    public string name;
    public string objectType;
    public float dec;
    public float ra;
    public Vector3 unityPosition;
    public bool discovered;
}

public class StellariumAPI : MonoBehaviour
{
    private List<string> constellationNames = new List<string>
    {
        "Andromeda", "Antlia", "Apus", "Aquarius", "Aquila", "Ara", "Aries", "Auriga", "Bootes", "Caelum",
        "Camelopardalis", "Cancer", "Canes Venatici", "Canis Major", "Canis Minor", "Capricornus", "Carina",
        "Cassiopeia", "Centaurus", "Cepheus", "Cetus", "Chamaeleon", "Circinus", "Columba", "Coma Berenices",
        "Corona Australis", "Corona Borealis", "Corvus", "Crater", "Crux", "Cygnus", "Delphinus", "Dorado",
        "Draco", "Equuleus", "Eridanus", "Fornax", "Gemini", "Grus", "Hercules", "Horologium", "Hydra",
        "Hydrus", "Indus", "Lacerta", "Leo", "Leo Minor", "Lepus", "Libra", "Lupus", "Lynx", "Lyra",
        "Mensa", "Microscopium", "Monoceros", "Musca", "Norma", "Octans", "Ophiuchus", "Orion", "Pavo",
        "Pegasus", "Perseus", "Phoenix", "Pictor", "Pisces", "Piscis Austrinus", "Puppis", "Pyxis",
        "Reticulum", "Sagitta", "Sagittarius", "Scorpius", "Sculptor", "Scutum", "Serpens", "Sextans",
        "Taurus", "Telescopium", "Triangulum", "Triangulum Australe", "Tucana", "Ursa Major", "Ursa Minor",
        "Vela", "Virgo", "Volans", "Vulpecula"
    };

    private string[] planetNames = { "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto" };
    private List<CelestialObjectInfo> celestialObjects = new List<CelestialObjectInfo>();
    private string progressFilePath;
    void Start()
    {
        
    }

    public IEnumerator Inicio()
    {
        progressFilePath = Application.persistentDataPath + "/playerProgression.json";
        yield return StartCoroutine(FetchCelestialData());
    }

    IEnumerator FetchCelestialData()
    {
    PlayerProgress playerProgress = LoadPlayerProgress();
    // Fetch constellation data
    foreach (var constellationName in constellationNames)
    {
        string url = $"https://2bcb-37-11-182-149.ngrok-free.app/api/objects/info?name={constellationName}&format=json";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var jsonResponse = www.downloadHandler.text;
                CelestialData data = JsonUtility.FromJson<CelestialData>(jsonResponse);

                if (data.found)
                {
                    CelestialObjectInfo info = new CelestialObjectInfo
                    {
                        name = data.name,
                        objectType = "constelacion",
                        dec = data.dec,
                        ra = data.ra,
                        unityPosition = CoordinateConverter.EquatorialToCartesian(data.ra, data.dec),
                        discovered = playerProgress.discovered_constellations.Contains(data.name)
                    };

                    celestialObjects.Add(info);

                    // Debug output for each constellation
                    //Debug.Log($"Constellation: {info.name}, Type: {info.objectType}, DEC: {info.dec}, RA: {info.ra}, unityPosition: {info.unityPosition}, discovered: {info.discovered}");
                }
            }
        }
    }

    // Fetch planet data
    foreach (var planetName in planetNames)
    {
        string url = $"http://localhost:8090/api/objects/info?name={planetName}&format=json";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                var jsonResponse = www.downloadHandler.text;
                CelestialData data = JsonUtility.FromJson<CelestialData>(jsonResponse);

                if (data.found)
                {
                    CelestialObjectInfo info = new CelestialObjectInfo
                    {
                        name = data.name,
                        objectType = "planeta",
                        dec = data.dec,
                        ra = data.ra,
                        unityPosition = CoordinateConverter.EquatorialToCartesian(data.ra, data.dec),
                        discovered = playerProgress.discovered_constellations.Contains(data.name)
                    };

                    celestialObjects.Add(info);

                    // Debug output for each planet
                    //Debug.Log($"Planet: {info.name}, Type: {info.objectType}, DEC: {info.dec}, RA: {info.ra}, unityPosition: {info.unityPosition}, discovered: {info.discovered}");
                }
            }
        }
    }

    Debug.Log("Fetched all celestial object data.");
    CreatePanels();
    CreateObjetosCielo();
}


//Función que carga el progreso del jugador
    PlayerProgress LoadPlayerProgress()
    {
        Debug.Log(progressFilePath);
        if (File.Exists(progressFilePath))
        {
            //Debug.Log("SÍ existe el archivo de progreso");
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

    public GameObject panelPrefab; // Prefab del Panel
    public Transform parentElement; // Elemento padre bajo el cual se crearán los paneles
    void CreatePanels()
    {
        //Debug.Log("Crear panales");
        foreach (CelestialObjectInfo objetoCielo in celestialObjects)
        {
            //Debug.Log(objetoCielo.discovered);
            if (!objetoCielo.discovered)
                continue;

            GameObject panel = Instantiate(panelPrefab, parentElement);
            // Aquí puedes añadir lógica adicional para configurar el panel si es necesario
            //Debug.Log("Siguiente:");
            //Debug.Log(panel.GetComponentInChildren<Text>().text);
            //Debug.Log(objetoCielo.name);
            panel.GetComponentInChildren<Text>().text = objetoCielo.name;

            // Asignar imagen a la RawImage
            var texture = Resources.Load<Texture2D>("Cielo_ilustraciones/" + objetoCielo.name);
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
                    Debug.LogWarning($"No se pudo cargar la imagen para {objetoCielo.name} ");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente RawImage en el panel prefab.");
            }
        }
    }

    public GameObject constelacionPrefab; // Prefab de las constelaciones
    public Transform parentElementConstelacion; // Elemento padre bajo el cual se crearán las constelaciones
    public GameObject PlanetaPrefab; // Elemento padre bajo el cual se crearán los planetas
    void CreateObjetosCielo()
    {

        foreach (CelestialObjectInfo objetoCielo in celestialObjects)
        {
            if (objetoCielo.objectType != "planeta")
            {
                // Instanciar el prefab del 2D Object
                GameObject spriteObject = Instantiate(constelacionPrefab, parentElementConstelacion);

                // Configurar el nombre del objeto
                spriteObject.name = objetoCielo.name;

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
                var sprite = Resources.Load<Sprite>("Constelaciones_cielo/" + objetoCielo.name);
                if (sprite != null)
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar el sprite para {objetoCielo.name} ");
                }

                // Configurar las dimensiones del SpriteRenderer si es necesario
                // spriteRenderer.transform.localScale = GetConstellationPositionByName(constellation.Name); // Ajustar según el tamaño deseado del sprite
                spriteRenderer.transform.localPosition = objetoCielo.unityPosition;

                if (objetoCielo.unityPosition.y < 2)
                {
                    Debug.Log("Menor que 5");
                    Debug.Log(objetoCielo.name);
                    Debug.Log(spriteRenderer.transform.localScale);
                    spriteRenderer.transform.localScale = new Vector3(0, 0, 0); 
                    Debug.Log(spriteRenderer.transform.localScale);
                }
                

                // Asegurar que la imagen siempre mire hacia la cámara
                spriteRenderer.transform.LookAt(Camera.main.transform);
                //spriteRenderer.transform.Rotate(0, 180, 0); // Ajustar rotación si es necesario

                // Configurar el texto del objeto hijo "Title"
                Transform titleTransform = spriteObject.transform.Find("AR Annotation/Title");
                if (titleTransform != null)
                {
                    TextMeshPro titleText = titleTransform.GetComponent<TextMeshPro>();
                    if (titleText != null)
                    {
                        titleText.text = objetoCielo.name;
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


                //Debug.Log($"Sprite creado para la constelación: {objetoCielo.name}");

            }
            else
            {
                //Debug.Log("Es un planeta!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                GameObject object3D  = Instantiate(PlanetaPrefab, parentElementConstelacion);
                object3D.name = objetoCielo.name;
                MeshRenderer meshRenderer = object3D.GetComponent<MeshRenderer>();

                if (meshRenderer == null)
                {
                    Debug.LogWarning("El prefab no contiene un componente SpriteRenderer.");
                    continue;
                }

                // Asignar sprite al SpriteRenderer
                //string spritePath = Path.Combine("Assets/Imagenes/Constelaciones_ilustraciones", constellation.Name + ".png");
                //Sprite sprite = LoadSprite(spritePath);
                // Verificar que el componente MeshRenderer existe
                if (meshRenderer != null)
                {
                    // Cargar el material desde la carpeta Resources
                    Material material = Resources.Load<Material>($"Materiales/{objetoCielo.name}");

                    if (material != null)
                    {
                        // Asignar el material al MeshRenderer
                        meshRenderer.material = material;
                    }
                    else
                    {
                        Debug.LogWarning($"No se pudo cargar el material para {objetoCielo.name}");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró un componente MeshRenderer en el prefab instanciado.");
                }

                // Configurar las dimensiones del SpriteRenderer si es necesario
                // spriteRenderer.transform.localScale = GetConstellationPositionByName(constellation.Name); // Ajustar según el tamaño deseado del sprite
                object3D.transform.localPosition = objetoCielo.unityPosition;

                if (objetoCielo.unityPosition.y < 2)
                {
                    object3D.transform.localScale = new Vector3(0, 0, 0); 
                }

                // Asegurar que la imagen siempre mire hacia la cámara
                object3D.transform.LookAt(Camera.main.transform);
                object3D.transform.Rotate(0, 180, 0); // Ajustar rotación si es necesario

                // Configurar el texto del objeto hijo "Title"
                Transform titleTransform = object3D.transform.Find("AR Annotation/Title");
                if (titleTransform != null)
                {
                    TextMeshPro titleText = titleTransform.GetComponent<TextMeshPro>();
                    if (titleText != null)
                    {
                        titleText.text = objetoCielo.name;
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


                //Debug.Log($"Sprite creado para la constelación: {objetoCielo.name}");
            }

            
        }

    }

}

[System.Serializable]
public class CelestialData
{
    public bool found;
    public string name;
    public string objectType;
    public float dec;
    public float ra;
}

