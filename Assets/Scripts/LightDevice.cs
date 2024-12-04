using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class LightDevice : MonoBehaviour
{
    [SerializeField] private ReflectiveRay m_reflectiveRay;

    [ColorUsage(true, true)] [SerializeField]
    private Color[] m_rayColors;

    [SerializeField] private int m_currentColorIndex = 0;

    [SerializeField] private Material m_deviceBeamMaterial;
    [SerializeField] private float m_deviceBeamIntensity = 5f;

    private ColorPalette m_palette;
    private SceneTransitions _sceneTransitions;

    [SerializeField] private GameObject m_colorRing;
    [SerializeField] private GameObject m_cog1;
    [SerializeField] private GameObject m_cog2;
    [SerializeField] private Quaternion m_initialRotationColorRing; // The very initial rotation of the ring GameObject (as the default rotation reference)
    [SerializeField] private Quaternion m_initialRotationCog1;
    [SerializeField] private Quaternion m_initialRotationCog2;
    [SerializeField] private Quaternion m_targetRotation; // The rotation to lerp towards
    [SerializeField] private Quaternion m_targetRotationCog1;
    [SerializeField] private Quaternion m_targetRotationCog2;
    private Light m_lightSource;
    private Light m_lightBeam;
    [SerializeField] private float m_rotationRate = 360f;

    private void Awake()
    {
        m_initialRotationColorRing = m_colorRing.transform.localRotation;
        m_initialRotationCog1 = m_cog1.transform.localRotation;
        m_initialRotationCog2 = m_cog2.transform.localRotation;
        m_targetRotation = m_initialRotationColorRing;
        m_targetRotationCog1 = m_initialRotationCog1;
        m_targetRotationCog2 = m_initialRotationCog2;
        m_lightSource = transform.Find("light_source").GetComponent<Light>();
        m_lightBeam = transform.Find("light_beam").GetComponent<Light>();
        m_lightSource.enabled = true;
        m_lightBeam.enabled = false;
    }

    private void Start()
    {
        m_reflectiveRay.enabled = false;
        m_palette = ColorPalette.Instance;
        _sceneTransitions = FindObjectOfType<SceneTransitions>();

        m_currentColorIndex = FindNextUnlockedColor(0); // Get the first unlocked color
        if (m_currentColorIndex < 0) Debug.LogError($"No colors unlocked!");
        Debug.Log($"First free color: {m_currentColorIndex}");
        Debug.Log($"Unlocked colors: {PlayerDataManager.GetUnlockedColors.Count}");
        ComputeTargetRotation();
        SetRayColor();
    }

    private void Update()
    {
        m_colorRing.transform.localRotation = Quaternion.RotateTowards(m_colorRing.transform.localRotation, m_targetRotation, m_rotationRate * Time.deltaTime);
        m_cog1.transform.localRotation = Quaternion.RotateTowards(m_cog1.transform.localRotation, m_targetRotationCog1, 4 * m_rotationRate * Time.deltaTime);
        m_cog2.transform.localRotation = Quaternion.RotateTowards(m_cog2.transform.localRotation, m_targetRotationCog2, 4 * m_rotationRate * Time.deltaTime);

        // Rotate only clockwise, not working yet
        //m_colorRing.transform.localRotation = Quaternion.Euler(m_initialRotationColorRing.x, m_initialRotationColorRing.y, Mathf.LerpAngle(m_colorRing.transform.localRotation.eulerAngles.z, m_targetRotation.eulerAngles.z, m_rotationRate * Time.deltaTime));
        //m_cog1.transform.localRotation = Quaternion.Euler(m_initialRotationCog1.x, m_initialRotationCog1.y, Mathf.LerpAngle(m_cog1.transform.localRotation.eulerAngles.z, m_targetRotationCog1.eulerAngles.z, 4 * m_rotationRate * Time.deltaTime));
        //m_cog2.transform.localRotation = Quaternion.Euler(m_initialRotationCog2.x, m_initialRotationCog2.y, Mathf.LerpAngle(m_cog2.transform.localRotation.eulerAngles.z, m_targetRotationCog2.eulerAngles.z, 4 * m_rotationRate * Time.deltaTime));
    }

    public void OnRayShooting(InputValue value)
    {
        bool m_deviceEnabled = Convert.ToBoolean(value.Get<float>());
        m_reflectiveRay.enabled = m_deviceEnabled;
        m_lightBeam.enabled = m_deviceEnabled;

        if (m_deviceEnabled)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            m_reflectiveRay.m_hitLastFrame.ForEach(x =>
            {
                if (x != null) x.OnMiss();
            }); // Update each IRayInteractable that was hit during the last frame that is was missed this frame.
            GetComponent<AudioSource>().Stop();
        }
    }

    public void OnColorChange(InputValue value)
    {
        // change the modulo to unlocked colors
        int numOfColors = m_palette.GetColorData.Count;
        int currentIndex;

        if (value.Get<Vector2>().x > 0)
        {
            currentIndex = (m_currentColorIndex + 1) % numOfColors;
        }
        else
        {
            currentIndex = ((m_currentColorIndex - 1) % numOfColors + numOfColors) % numOfColors;
        }


        if (PlayerDataManager.GetUnlockedColors.Contains(item: DetermineColor(currentIndex)))
        {
            Debug.Log("fetching the color:    " + PlayerDataManager.GetUnlockedColors.Contains(item:DetermineColor(currentIndex)));

            m_currentColorIndex = currentIndex;
            ComputeTargetRotation();
            SetRayColor();
        }

        //else
        //{
        //    FindNextUnlockedColor(currentIndex);
        //}
    }

    private string DetermineColor(int currentIndex)
    {
        switch(currentIndex)
        {
            case 0:
                return "Red";
            case 1:
                return "Blue";
            case 2:
                return "Yellow";
            case 3:
                return "Orange";
            case 4:
                return "Purple";
            case 5:
                return "Green";
            case 6:
                return "Turquoise";
            case 7:
                return "Pink";
        }
        return "";
    }

    private int FindNextUnlockedColor(int currentIndex)
    {
        for (int i = currentIndex; i < 8 + currentIndex; i++)
        {
            if (PlayerDataManager.GetUnlockedColors.Contains(item:DetermineColor(i % 8))) 
                return i % 8;
        }

        return -1;
    }

    /// <summary> Computes the z rotations depending on individual offsets. </summary>
    /// <returns></returns>
    private void ComputeTargetRotation()
    {
        m_targetRotation = m_initialRotationColorRing * Quaternion.Euler(0, 0, 45f * m_currentColorIndex);
        m_targetRotationCog1 = m_initialRotationCog1 * Quaternion.Euler(0, 0, 4 * 45f * m_currentColorIndex);
        m_targetRotationCog2 = m_initialRotationCog2 * Quaternion.Euler(0, 0, -4 * 45f * m_currentColorIndex);
    }

    public void OnMenuReturn()
    {
        _sceneTransitions.SwitchBetweenMainMenuAndCurrentLevel();
    }

    public void OnExitGame()
    {
        _sceneTransitions.ExitGame();
    }

    private void SetRayColor()
    {
        //we don't fetch colors from colorpalette but rather from unlocked colors

        if(m_currentColorIndex>-1)
        {

            String colorCode = m_palette.GetColorData.ElementAt(m_currentColorIndex).Key;
            Color color = m_palette.GetColorData.ElementAt(m_currentColorIndex).Value.GetColor();
            SetDeviceRayColor(colorCode);
            m_reflectiveRay.SetColor(colorCode);
            m_lightSource.color = color;
            m_lightBeam.color = color;
        }
    }

    private void SetDeviceRayColor(string colorKey)
    {
        Color color = ColorPalette.Instance.GetColorData.GetValueOrDefault(colorKey).GetColor();
        color = new Color(color.r * m_deviceBeamIntensity, color.g * m_deviceBeamIntensity, color.b * m_deviceBeamIntensity, 1);
        m_deviceBeamMaterial.color = color;
    }
}