using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitions : MonoBehaviour
{
    public static SceneTransitions Instance { get; private set; }

    public bool _switchLevels = false;

    private MainMenuScenManager _mainMenueSceneManager;
    private AmbientAudioPlayer _ambientAudioPlayer;
    [SerializeField]
    private Animator _transition;
    [SerializeField]
    private float _transitionDuration = 1f;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        _mainMenueSceneManager=FindObjectOfType<MainMenuScenManager>();
        _ambientAudioPlayer = FindObjectOfType<AmbientAudioPlayer>();
        LoadPlayerData();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Only for testing the script will remove afterwards
    private void Update()
    {
        if (_switchLevels)
        {
            SwitchBetweenMainMenuAndCurrentLevel();
            _switchLevels = false;
        }

        _mainMenueSceneManager.Instance.UpdateUnlockedObstacles(PlayerDataManager.GetUnlockedObstacles);
    }
    //Probably won't need this as in loading if palyer doesn exist it will create
    //a new one and initialize it with first time playing variables
    public void InitPlayerData()
    {
        PlayerDataManager.CreateDataAsset();
    }
    
    public void LoadPlayerData()
    {
        Debug.Log("current level calling from awake");
        PlayerData _pData= PlayerDataManager.LoadPlayerData();

        //Here we will Set LightDevice ColorBand with a function like this:
        //LightDevice.Instance.UpdateUnlockedColors(_playerData.GetUnlockedColors);

        //Setting Main Menu unlocked Obstacles
        _mainMenueSceneManager.Instance.UpdateUnlockedObstacles(PlayerDataManager.GetUnlockedObstacles);
    }

    //Level Cleared will update and save 
    public void LevelCleared()
    {
        int _activeSceneInt = SceneManager.GetActiveScene().buildIndex;

        //CurrLevel will be the next level so when we load the data the correct level will be displayed after clearing this one
        PlayerDataManager.UpdateCurrentLevel(_activeSceneInt+1);


        string _toUnlockColor = GetLevelClearedNewColorUnlocked(_activeSceneInt);
        string _toUnlockObstacle = GetLevelClearedNewObstacleUnlocked(_activeSceneInt);

        if (!_toUnlockColor.Equals(""))
        {

            PlayerDataManager._unlockedColors.Add(_toUnlockColor);
            Debug.Log("NewUnlocked:         " + _toUnlockObstacle);
            PlayerDataManager.UpdateUnlockedColors(_toUnlockColor);
        }
        if(!_toUnlockObstacle.Equals(""))
        {
            PlayerDataManager._unlockedObstacles.Add(_toUnlockObstacle);
            Debug.Log("NewUnlocked:         " + _toUnlockObstacle);
            PlayerDataManager.UpdateUnlockedObstacles(_toUnlockObstacle);
        }

        //Here we will Set LightDevice ColorBand with a function like this:
        //LightDevice.Instance.UpdateUnlockedColors(_playerData.GetUnlockedColors);

        Debug.Log("Setting Main Menu unlocked Obstacles");
        _mainMenueSceneManager.Instance.UpdateUnlockedObstacles(PlayerDataManager.GetUnlockedObstacles);

        //SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        StartCoroutine(LoadLevel((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings));

        //StartCoroutine(WaitAfterAnimation());

    }
    //IEnumerator WaitAfterAnimation()
    //{
    //    if(_animations.Length>0)
    //        yield return _animations[SceneManager.GetActiveScene().rootCount].averageDuration;
    //    yield return new WaitForSecondsRealtime(5f);
    //    SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().rootCount + 1) % SceneManager.sceneCountInBuildSettings);
    //}

    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("current level:        " + PlayerDataManager.GetCurrentLevel);
        _transition.SetTrigger("Start");


        yield return new WaitForSeconds(_transitionDuration);

        SceneManager.LoadSceneAsync(levelIndex);

        _transition.SetTrigger("End");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            _ambientAudioPlayer.ToggleSoundScape(AmbientAudioPlayer.Soundscape.MainMenu);
        }
        else if (scene.buildIndex==1)
        {
            _ambientAudioPlayer.ToggleSoundScape(AmbientAudioPlayer.Soundscape.Outside);
        }
        else
        {
            _ambientAudioPlayer.ToggleSoundScape(AmbientAudioPlayer.Soundscape.Dungeon);
        }
        //Debug.Log("<color=red> SceneLoaded: </color>" + scene.name);
        //_transition.SetTrigger("End");
        Debug.Log("NewUnlocked Colors: " + PlayerDataManager.GetUnlockedColors.Count);
        PlayerDataManager.GetUnlockedColors.ForEach(a => Debug.Log("NewUnlocked Color: " + a));
        Debug.Log("NewUnlocked Obstacles: " + PlayerDataManager.GetUnlockedObstacles.Count);
        PlayerDataManager.GetUnlockedObstacles.ForEach(a => Debug.Log("NewUnlocked Obstacle: " + a));
    }

    //The color is unlocked after hitting the goal--> we take currLevelInt
    private string GetLevelClearedNewColorUnlocked(int _levelInt)
    {
        if(_levelInt== 4)
        {

            Debug.Log("unlocked Red");
            return "Red";
        }
        Debug.Log("No Color Unlocking this Level");
        return "";
    }
   //The obstacle is unlocked in Main Menu after playing a round with it --> we take currLevelInt
    private string GetLevelClearedNewObstacleUnlocked(int _levelInt)
    {
        switch (_levelInt)
        {
            case 1:
                Debug.Log("unlocked ReflectiveSurface");
                return "ReflectiveSurface";
            case 3:
                Debug.Log("unlocked Breakable");
                return "Breakable";
            case 4:
                Debug.Log("unlocked Cloaked");
                return "Cloaked";
        }
        Debug.Log("no obstacle unlocking this Level");
        return "";
    }

    //Call this when button triggered from main menu to currectly to-unlock aka nnot passed level
    
    public void SwitchBetweenMainMenuAndCurrentLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 && !PlayerDataManager.GetCurrentLevel.Equals(SceneManager.GetActiveScene().buildIndex))
        {
            Debug.Log("current level load with switch button:          " + PlayerDataManager.GetCurrentLevel);
            //SceneManager.LoadSceneAsync(PlayerDataManager.GetCurrentLevel);
            StartCoroutine(LoadLevel(PlayerDataManager.GetCurrentLevel));
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.Log("current level load with switch button:          " + PlayerDataManager.GetCurrentLevel);
            Debug.Log("current obstacles unlcoked:      " + PlayerDataManager.GetUnlockedObstacles.Count);
            Debug.Log("current colors unlcoked:      " + PlayerDataManager.GetUnlockedColors.Count);
            //SceneManager.LoadSceneAsync(0);
            StartCoroutine(LoadLevel(0));
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
