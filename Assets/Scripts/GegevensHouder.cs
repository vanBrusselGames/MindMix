using System;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GegevensHouder : MonoBehaviour
{
    public static GegevensHouder Instance;
    bool _testBuild;
    [HideInInspector] public bool startNewGame;
    public Texture2D zwart;
    private bool _isPaused;
    private bool _wasPaused;
    public WaitForSecondsRealtime wachtHonderdste = new(0.01f);
    public List<Sprite> achtergronden;
    public Sprite spriteWit;
    private readonly List<int> _backgroundSudoku = new() { 0, -2 };
    private readonly List<int> _backgroundMenu = new() { 0, -2 };
    private readonly List<int> _background2048 = new() { 0, -2 };
    private readonly List<int> _backgroundMinesweeper = new() { 0, -2 };
    private readonly List<int> _backgroundSolitaire = new() { 0, -2 };
    private readonly List<int> _backgroundColorSort = new() { 0, -2 };
    private Achtergrond _bgScript;
    private SaveScript _saveScript;
    [HideInInspector] public bool isLoginWarned;
    [HideInInspector] public int currentSelectedGameWheelIndex;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        if (Application.platform == RuntimePlatform.WindowsEditor || true)
        {
            _testBuild = true;
        }

        _bgScript = GetComponent<Achtergrond>();
        _saveScript = GetComponent<SaveScript>();
        Application.targetFrameRate = Mathf.Min(30, Screen.currentResolution.refreshRate);
    }

    private void Start()
    {
        if (!_testBuild)
        {
            Debug.unityLogger.logEnabled = false;
        }

        SetData();
        _saveScript.LongDict["laatsteLogin"] = DateTime.UtcNow.Ticks;
    }

    private bool _changedColor;

    // Update is called once per frame
    private void Update()
    {
        if (!_isPaused && _wasPaused) SetData();
        if (_changedColor)
        {
            _changedColor = false;
            _bgScript.isChangedColor = true;
        }

        _wasPaused = _isPaused;
    }

    public List<int> GetBackgroundList(string sceneName = "noscene")
    {
        if (sceneName.Equals("noscene"))
        {
            sceneName = SceneManager.GetActiveScene().name.ToLower();
        }

        List<int> list = new() { -1000, -1000 };
        switch (sceneName)
        {
            case "sudoku":
                list = _backgroundSudoku;
                break;
            case "spellenoverzicht":
                list = _backgroundMenu;
                break;
            case "2048":
                list = _background2048;
                break;
            case "minesweeper":
                list = _backgroundMinesweeper;
                break;
            case "solitaire":
                list = _backgroundSolitaire;
                break;
            case "colorsort":
                list = _backgroundColorSort;
                break;
            case "inlogenvoorplaatapp":
                list = new List<int> { 1, -1 };
                break;
            case "instellingen":
                list = _backgroundMenu;
                break;
            case "shop":
                list = _backgroundMenu;
                break;
        }

        return list;
    }

    public void ChangeSavedBackground(string sceneName, int backgroundType, int value)
    {
        _changedColor = true;
        switch (sceneName)
        {
            case "sudoku":
                _backgroundSudoku[0] = backgroundType;
                _backgroundSudoku[1] = value;
                break;
            case "menu":
                _backgroundMenu[0] = backgroundType;
                _backgroundMenu[1] = value;
                break;
            case "2048":
                _background2048[0] = backgroundType;
                _background2048[1] = value;
                break;
            case "minesweeper":
                _backgroundMinesweeper[0] = backgroundType;
                _backgroundMinesweeper[1] = value;
                break;
            case "solitaire":
                _backgroundSolitaire[0] = backgroundType;
                _backgroundSolitaire[1] = value;
                break;
            case "colorsort":
                _backgroundColorSort[0] = backgroundType;
                _backgroundColorSort[1] = value;
                break;
        }
    }

    public void ChangeLanguage(int languageValue)
    {
        if (languageValue == -1)
        {
            return;
        }

        _saveScript.StringDict["taal"] = LocalizationSettings.AvailableLocales.Locales[languageValue].LocaleName;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageValue];
        FirebaseAuth.DefaultInstance.LanguageCode = LocalizationSettings.SelectedLocale.Identifier.Code;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        _isPaused = pauseStatus;
    }

    private void SetData()
    {
        zwart = new Texture2D(1, 1);
        zwart.SetPixel(0, 0, Color.black);
        zwart.Apply();
        SetLanguage();
    }

    public void SetLanguage()
    {
        if (_saveScript.StringDict["taal"] != "")
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (locale.LocaleName.Equals(_saveScript.StringDict["taal"]))
                {
                    LocalizationSettings.SelectedLocale = locale;
                    break;
                }
            }
        }
        else
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (!locale.LocaleName.Equals(LocalizationSettings.SelectedLocale.LocaleName)) continue;
                _saveScript.StringDict["taal"] = locale.LocaleName;
                break;
            }
        }
    }

    public void SetBackground()
    {
        List<string> sceneNames = new() { "All", "Sudoku", "Solitaire", "2048", "Minesweeper", "Menu", "ColorSort" };
        foreach (string sceneName in sceneNames)
        {
            ChangeSavedBackground(sceneName.ToLower(), _saveScript.IntDict["bgSoort" + sceneName],
                _saveScript.IntDict["bgWaarde" + sceneName]);
        }
    }
}