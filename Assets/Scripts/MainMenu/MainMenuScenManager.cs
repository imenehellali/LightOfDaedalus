using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScenManager : MonoBehaviour
{
    public MainMenuScenManager Instance { get; private set; }

    [SerializeField] private GameObject[] _lockedContainers = null;

    [SerializeField] private GameObject[] _reflectiveSurfaceObjects = null;
    [SerializeField] private GameObject _breakableObjects = null;
    [SerializeField] private GameObject[] _CloakedObjects = null;

    private List<string> _unlockedObstaclesList = new List<string>();
    private void Awake()
    {
        Debug.Log("init Main Menu scene Manager");
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        UpdateUnlockedObstacles(PlayerDataManager.GetUnlockedObstacles);

        foreach(GameObject gameObject in _lockedContainers)
            gameObject.SetActive(true);
        foreach(GameObject gameObject in _reflectiveSurfaceObjects)
            gameObject.SetActive(false);
        _breakableObjects.SetActive(false);
        foreach (GameObject gameObject in _CloakedObjects)
            gameObject.SetActive(false);


        _unlockedObstaclesList.ForEach(a => AdjustAssetContainer(a));
    }
    //Only testing purpose
        private void Update()
    {
    }
    private void AdjustAssetContainer(string _ob)
    {
        switch(_ob)
        {
            case "ReflectiveSurface":
                _lockedContainers[0].SetActive(false);
                _reflectiveSurfaceObjects[0].gameObject.SetActive(true);
                _lockedContainers[1].gameObject.SetActive(false);
                _reflectiveSurfaceObjects[1].gameObject.SetActive(true);
                break;
            case "Breakable":
                _lockedContainers[2].SetActive(false);
                _breakableObjects.gameObject.SetActive(true);
                break;
            case "Cloaked":
                _lockedContainers[3].SetActive(false);
                //First is mirror other is the non showing object itself
                _CloakedObjects[0].gameObject.SetActive(true);
                _CloakedObjects[1].gameObject.SetActive(true);
                break;
        }
    }
    public void UpdateUnlockedObstacles(List<string> _unlockedObstacles)
    {
        Debug.Log("Updating from scene load");
        _unlockedObstaclesList = _unlockedObstacles;
    }

}
