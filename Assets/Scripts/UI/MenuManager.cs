using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _settingsCamera;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private TMP_Dropdown _qualityDropDown;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;


    private void Start()
    {
        LoadSettings();
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

    public void Settings()
    {
        _mainCamera.SetActive(false);
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _settingsCamera.SetActive(true);
    }
    
    public void Back()
    {
        _mainCamera.SetActive(true);
        _menuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _settingsCamera.SetActive(false);
        SaveSettings();
    }
    
    

    public void SetSensetivity(float desiredSensetivity)
    {
        PlayerPrefs.SetFloat("Sensetivity", desiredSensetivity);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettings", _qualityDropDown.value);
        PlayerPrefs.SetInt("Fullscreen", System.Convert.ToInt32(Screen.fullScreen));
        
        float volume;
        _audioMixer.GetFloat("MasterVolume", out volume);
        PlayerPrefs.SetFloat("Volume", volume);
        
    }

    public void SetVolume(float desiredVolume)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(desiredVolume) * 20);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("QualitySettings"))
            _qualityDropDown.value = PlayerPrefs.GetInt("QualitySettings");
        else
            _qualityDropDown.value = 3;
        SetQuality(_qualityDropDown.value);
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
        }
        else
            Screen.fullScreen = false;

        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume;
            volume = PlayerPrefs.GetFloat("Volume");
            _audioMixer.SetFloat("MasterVolume", volume);
        }
        else
        {
            _audioMixer.SetFloat("MasterVolume", 0);
        }

        if (PlayerPrefs.HasKey("Sensetivity"))
        {
            _volumeSlider.value = PlayerPrefs.GetFloat("Sensetivity");
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
