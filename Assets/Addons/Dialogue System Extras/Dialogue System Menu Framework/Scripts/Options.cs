// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PixelCrushers.DialogueSystem.MenuSystem
{

    /// <summary>
    /// This script manages the OptionsPanel.
    /// </summary>
    public class Options : MonoBehaviour
    {

        [Header("Video")]
        public UnityEngine.UI.Toggle fullScreenToggle;
        public string fullScreenPrefsKey = "options.fullScreen";
        public UnityEngine.UI.Dropdown resolutionDropdown;
        public string resolutionPrefsKey = "options.resolution";
        public UnityEngine.UI.Dropdown graphicsQualityDropdown;
        public string graphicsQualityPrefsKey = "options.quality";

        [Header("Audio")]
        public AudioMixer mainMixer;
        public string musicVolumeMixerParameter = "musicVol";
        public string musicVolumePrefsKey = "options.musicVol";
        public UnityEngine.UI.Slider musicVolumeSlider;
        public string sfxVolumeMixerParameter = "sfxVol";
        public string sfxVolumePrefsKey = "options.sfxVol";
        public UnityEngine.UI.Slider sfxVolumeSlider;

        [Header("Subtitles")]
        public UnityEngine.UI.Toggle subtitles;
        public bool setNPCSubtitlesDuringLine = true;
        public bool setNPCSubtitlesWithResponseMenu = true;
        public bool setPCSubtitlesDuringLine = false;
        public string subtitlesPrefsKey = "options.subtitles";

        [Header("Languages")]
        public UnityEngine.UI.Dropdown languages;

        protected bool m_started = false;
        protected List<string> resolutionDropdownItems = new List<string>();

        protected virtual void Start()
        {
            m_started = true;
            RefreshMenuElements();
        }

        protected virtual void OnEnable()
        {
            if (m_started) RefreshMenuElements();
        }

        protected virtual void OnDisable()
        {
            m_started = false;
        }

        public virtual void RefreshMenuElements()
        {
            RefreshResolutionDropdown();
            RefreshFullscreenToggle();
            RefreshGraphicsQualityDropdown();
            RefreshMusicVolumeSlider();
            RefreshSfxVolumeSlider();
            RefreshSubtitlesToggle();
            RefreshLanguagesDropdown();
        }

        protected virtual void RefreshFullscreenToggle()
        {
            fullScreenToggle.isOn = GetFullScreen();
        }

        protected virtual bool GetFullScreen()
        {
            return PlayerPrefs.HasKey(fullScreenPrefsKey) ? (PlayerPrefs.GetInt(fullScreenPrefsKey) == 1) : Screen.fullScreen;
        }

        public virtual void SetFullScreen(bool on)
        {
            Screen.fullScreen = on;
            PlayerPrefs.SetInt(fullScreenPrefsKey, on ? 1 : 0);
            SetResolutionIndex(GetResolutionIndex());
            SelectNextFrame(fullScreenToggle);
        }

        protected virtual string GetResolutionString(Resolution resolution)
        {
            return (resolution.refreshRate > 0) ? (resolution.width + "x" + resolution.height + " " + resolution.refreshRate + "Hz")
                : (resolution.width + "x" + resolution.height);
        }

        protected virtual void RefreshResolutionDropdownItems()
        {
            resolutionDropdownItems.Clear();
            var uniqueResolutions = Screen.resolutions.Distinct();
            foreach (var resolution in uniqueResolutions)
            {
                resolutionDropdownItems.Add(GetResolutionString(resolution));
            }
        }

        protected virtual int GetCurrentResolutionDropdownIndex()
        {
            var currentString = GetResolutionString(Screen.currentResolution);
            for (int i = 0; i < resolutionDropdownItems.Count; i++)
            {
                if (string.Equals(resolutionDropdownItems[i], currentString)) return i;
            }
            return 0;
        }

        protected virtual int ResolutionDropdownIndexToScreenResolutionsIndex(int dropdownIndex)
        {
            if (0 <= dropdownIndex && dropdownIndex < resolutionDropdownItems.Count)
            {
                var dropdownString = resolutionDropdownItems[dropdownIndex];
                for (int i = 0; i < Screen.resolutions.Length; i++)
                {
                    if (string.Equals(GetResolutionString(Screen.resolutions[i]), dropdownString)) return i;
                }
            }
            // If we don't find a match, return current resolution's index:
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Equals(Screen.resolutions[i], Screen.currentResolution)) return i;
            }
            return 0;
        }

        protected virtual void RefreshResolutionDropdown()
        {
            if (PlayerPrefs.HasKey(resolutionPrefsKey)) SetResolutionIndex(PlayerPrefs.GetInt(resolutionPrefsKey));
            RefreshResolutionDropdownItems();
            if (resolutionDropdown != null)
            {
                resolutionDropdown.ClearOptions();
                resolutionDropdown.AddOptions(resolutionDropdownItems);
                var dropdownIndex = Mathf.Clamp(GetResolutionIndex(), 0, resolutionDropdownItems.Count - 1);
                resolutionDropdown.value = dropdownIndex;
                if (resolutionDropdown.captionText != null)
                {
                    resolutionDropdown.captionText.text = resolutionDropdownItems[dropdownIndex];
                }
            }
        }

        protected virtual int GetResolutionIndex() // Returns dropdown list index.
        {
            return PlayerPrefs.HasKey(resolutionPrefsKey) ? PlayerPrefs.GetInt(resolutionPrefsKey) : GetCurrentResolutionDropdownIndex();
        }

        public virtual void SetResolutionIndex(int dropdownIndex) // Dropdown list index.
        {
            if (0 <= dropdownIndex && dropdownIndex < resolutionDropdownItems.Count)
            {
                var resolutionsIndex = ResolutionDropdownIndexToScreenResolutionsIndex(dropdownIndex);
                if (0 <= resolutionsIndex && resolutionsIndex < Screen.resolutions.Length)
                {
                    var resolution = Screen.resolutions[resolutionsIndex];
                    if (InputDeviceManager.instance != null) InputDeviceManager.instance.BrieflyIgnoreMouseMovement(); // Mouse "moves" (resets position) when resolution changes.
                    Screen.SetResolution(resolution.width, resolution.height, GetFullScreen());
                    PlayerPrefs.SetInt(resolutionPrefsKey, dropdownIndex);
                }
            }
            SelectNextFrame(resolutionDropdown);
        }

        protected virtual void RefreshGraphicsQualityDropdown()
        {
            if (PlayerPrefs.HasKey(graphicsQualityPrefsKey)) SetGraphicsQualityIndex(PlayerPrefs.GetInt(graphicsQualityPrefsKey));
            if (graphicsQualityDropdown != null)
            {
                var list = new List<string>(QualitySettings.names);
                graphicsQualityDropdown.ClearOptions();
                graphicsQualityDropdown.AddOptions(list);
                var index = GetGraphicsQualityIndex();
                graphicsQualityDropdown.value = index;
                if (graphicsQualityDropdown.captionText != null)
                {
                    graphicsQualityDropdown.captionText.text = list[index];
                }
                SelectNextFrame(graphicsQualityDropdown);
            }
        }

        protected virtual int GetGraphicsQualityIndex()
        {
            return PlayerPrefs.HasKey(graphicsQualityPrefsKey) ? PlayerPrefs.GetInt(graphicsQualityPrefsKey) : QualitySettings.GetQualityLevel();
        }

        public virtual void SetGraphicsQualityIndex(int index)
        {
            QualitySettings.SetQualityLevel(index);
            PlayerPrefs.SetInt(graphicsQualityPrefsKey, index);
            SelectNextFrame(graphicsQualityDropdown);
        }

        protected virtual void SelectNextFrame(UnityEngine.UI.Selectable selectable)
        {
            if (InputDeviceManager.autoFocus && selectable != null && selectable.gameObject.activeInHierarchy)
            {
                StopAllCoroutines();
                StartCoroutine(SelectNextFrameCoroutine(selectable));
            }
        }

        protected virtual IEnumerator SelectNextFrameCoroutine(UnityEngine.UI.Selectable selectable)
        {
            yield return null;
            UITools.Select(selectable);
        }

        protected virtual void RefreshMusicVolumeSlider()
        {
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumePrefsKey, 0);
            }
        }

        public virtual void SetMusicLevel(float musicLevel)
        {
            if (!m_started) return;
            if (mainMixer != null)
            {
                mainMixer.SetFloat(musicVolumeMixerParameter, musicLevel);
            }
            PlayerPrefs.SetFloat(musicVolumePrefsKey, musicLevel);
        }

        protected virtual void RefreshSfxVolumeSlider()
        {
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolumeMixerParameter, 0);
            }
        }

        public virtual void SetSfxLevel(float sfxLevel)
        {
            if (!m_started) return;
            if (mainMixer != null)
            {
                mainMixer.SetFloat(sfxVolumeMixerParameter, sfxLevel);
            }
            PlayerPrefs.SetFloat(sfxVolumePrefsKey, sfxLevel);
        }

        protected virtual void RefreshSubtitlesToggle()
        {
            if (subtitles != null)
            {
                subtitles.isOn = PlayerPrefs.GetInt(subtitlesPrefsKey, GetDefaultSubtitlesSetting() ? 1 : 0) == 1;
            }
        }

        public virtual void OnSubtitlesToggleChanged()
        {
            if (!m_started) return;
            SetSubtitles(subtitles.isOn);
        }

        public virtual void SetSubtitles(bool on)
        {
            var subtitleSettings = DialogueManager.DisplaySettings.subtitleSettings;
            subtitleSettings.showNPCSubtitlesDuringLine = subtitles.isOn && setNPCSubtitlesDuringLine;
            subtitleSettings.showNPCSubtitlesWithResponses = subtitles.isOn && setNPCSubtitlesWithResponseMenu;
            subtitleSettings.showPCSubtitlesDuringLine = subtitles.isOn && setPCSubtitlesDuringLine;
            PlayerPrefs.SetInt(subtitlesPrefsKey, on ? 1 : 0);
        }

        protected virtual bool GetDefaultSubtitlesSetting()
        {
            var subtitleSettings = DialogueManager.displaySettings.subtitleSettings;
            return subtitleSettings.showNPCSubtitlesDuringLine || subtitleSettings.showNPCSubtitlesWithResponses || subtitleSettings.showPCSubtitlesDuringLine;
        }

        protected virtual void RefreshLanguagesDropdown()
        {
            if (languages == null || DialogueManager.DisplaySettings.localizationSettings.textTable == null) return;
            var language = PlayerPrefs.GetString("Language");
            var languageList = new List<string>(DialogueManager.DisplaySettings.localizationSettings.textTable.languages.Keys);
            languages.ClearOptions();
            languages.AddOptions(languageList);
            for (int i = 0; i < languageList.Count; i++)
            {
                if (languageList[i] == language)
                {
                    languages.value = i;
                    return;
                }
            }
        }

        public virtual void SetLanguageByIndex(int index)
        {
            if (DialogueManager.DisplaySettings.localizationSettings.textTable == null) return;
            var language = string.Empty;
            var languageList = new List<string>(DialogueManager.DisplaySettings.localizationSettings.textTable.languages.Keys);
            if (0 <= index && index < languageList.Count)
            {
                language = languageList[index];
            }
            var uiLocalizationManager = FindObjectOfType<UILocalizationManager>();
            if (uiLocalizationManager == null) uiLocalizationManager = gameObject.AddComponent<UILocalizationManager>();
            uiLocalizationManager.currentLanguage = language;
            Localization.language = language;
            DialogueManager.SetLanguage(language);
        }

    }
}