using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool active = false;

    void Start()
    {
        int savedLocaleID = PlayerPrefs.GetInt("idioma", 0);
        ChangeLocale(savedLocaleID);
    }

    public void ChangeLocale(int localeID)
    {
        if (active)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
        PlayerPrefs.SetInt("idioma", localeID);
        PlayerPrefs.Save();
    }

    private IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
    }

}