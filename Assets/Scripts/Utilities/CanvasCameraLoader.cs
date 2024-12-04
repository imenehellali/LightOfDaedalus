using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class CanvasCameraLoader : MonoBehaviour
{
    private Canvas m_canvas;

    private void Awake()
    {
        m_canvas = this.GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetCanvasRenderCamera;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetCanvasRenderCamera;
    }

    private void SetCanvasRenderCamera(Scene scene, LoadSceneMode mode)
    {
        m_canvas.worldCamera = Camera.main;
    }
}
