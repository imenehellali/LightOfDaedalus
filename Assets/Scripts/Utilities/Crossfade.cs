using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crossfade : MonoBehaviour
{
    [SerializeField]
    private Animator m_fadeAnimator;
    [SerializeField]
    private float m_fadeDuration = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(transform);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        m_fadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(m_fadeDuration);

        SceneManager.LoadScene(levelIndex);
    }
}
