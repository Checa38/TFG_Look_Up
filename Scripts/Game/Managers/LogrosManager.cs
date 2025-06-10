using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using System;

public class LogrosManager : MonoBehaviour
{
    public GameObject panelPrefabBiblioteca; // Prefab del Panel
    public Transform parentElementBiblioteca; // Elemento padre bajo el cual se crearán los paneles

    public IEnumerator CreatePanelsLogros()
    {
        foreach (infoLogros logroObj in DatosEntreEscenas.playerLogros.listaLogros)
        {
            if (!logroObj.conseguido)
                continue;

            GameObject panel = Instantiate(panelPrefabBiblioteca, parentElementBiblioteca);

            // Obtener los componentes LocalizeStringEvent en el panel
            LocalizeStringEvent[] localizeStringEvents = panel.GetComponentsInChildren<LocalizeStringEvent>();

            if (localizeStringEvents.Length >= 2)
            {
                // Configurar la referencia de la tabla y la clave de localización para el nombre del logro
                localizeStringEvents[0].StringReference.TableReference = "logros"; // Nombre de la tabla de localización
                localizeStringEvents[0].StringReference.TableEntryReference = "nom" + logroObj.name; // Clave de la tabla

                // Configurar la referencia de la tabla y la clave de localización para más información del logro
                localizeStringEvents[1].StringReference.TableReference = "logros"; // Nombre de la tabla de localización
                localizeStringEvents[1].StringReference.TableEntryReference = logroObj.name; // Clave de la tabla
            }
            else
            {
                Debug.LogError("No se encontraron suficientes componentes LocalizeStringEvent en los hijos.");
            }

            // Configurar la imagen
            var texture = Resources.Load<Texture2D>("Logros/" + logroObj.name);
            RawImage rawImage = panel.GetComponentInChildren<RawImage>();
            if (rawImage != null)
            {
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar la imagen para {logroObj.name} ");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente RawImage en el panel prefab.");
            }

            Button buttonComponent = panel.GetComponent<Button>();
            if (buttonComponent != null)
            {
                Destroy(buttonComponent);
            }

            
        }
        yield return null;
    }
}
