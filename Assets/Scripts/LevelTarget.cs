using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTarget : IRayInteractable
{
    /// <summary> This variable represents the remaining required interaction time in seconds /// </summary>
    private float m_remainingTime;
    private SceneTransitions m_sceneTransitions;
    [SerializeField] private AudioClip _breakClip;
    [SerializeField] private GameObject _brokenWall;
    [SerializeField] private AudioSource _audioSource;
    private GameObject door;
    public bool _testLevelCleared = false;

    private void Awake()
    {
        SetColor();
        m_remainingTime = m_interactionTime;
        m_sceneTransitions = FindObjectOfType<SceneTransitions>();
    }

    private void Start()
    {
        door = transform.Find("door").gameObject;
        door.GetComponent<Renderer>().material.SetColor("_EmissionColor", ColorPalette.Instance.GetColorData.GetValueOrDefault(m_color.ToString()).GetColor()); // Set the visual color according to the one set in the inspector
    }

    private void LateUpdate()
    {
        m_interacting = false;

        if (_testLevelCleared)
        {
            StartCoroutine(EndLevel());
            _testLevelCleared = false;
        }
    }

    protected override void SetColor()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnHit(string colorCode, RaycastHit hit)
    {
        if (!CheckIncomingColor(colorCode))
        {
            return;
        }

        m_interacting = true;
        m_remainingTime -= Time.deltaTime;

        if(m_remainingTime < 0 && !m_interactionFinished)
        {
            m_interactionFinished = true;
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator EndLevel()
    {
        var replacement = Instantiate(_brokenWall, transform.position, transform.rotation);
        replacement.transform.localScale = transform.localScale;
        _audioSource.PlayOneShot(_breakClip);

        if(SceneManager.GetActiveScene().buildIndex==2)
        {
            Debug.Log("we are in L2");
            LTwoGoalHitBehavior _L2 = GetComponent<LTwoGoalHitBehavior>();
            _L2.Trigger();
        }

        door.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(3);
        SceneTransitions.Instance.LevelCleared();
    }

    protected override void ResetRayInteractable()
    {
        throw new System.NotImplementedException();
    }

    public override void OnMiss()
    {
    }

}
