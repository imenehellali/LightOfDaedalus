using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ReflectiveRay : MonoBehaviour
{
    [SerializeField] private int m_reflections;
    [SerializeField] private float m_maxLength;

    [SerializeField] private GameObject m_beamImpuls;
    [SerializeField] private Material m_impulsMaterial;
    [SerializeField] private float m_impulsIntensity = 9f;

    [SerializeField] private Material m_beamMaterial;
    [SerializeField] private float m_beamIntensity = 5f;


    [SerializeField] private GameObject m_lightBeamPrefab;
    private LineRenderer m_lineRenderer;
    private Ray m_ray;
    private RaycastHit m_hit;

    private List<GameObject> m_lightRays = new List<GameObject>();
    private string m_rayColor = "Yellow";
    private string m_initialRayColor = "Yellow";

    /// <summary> The lst of IRayInteractables that were hit within the last frame. </summary>
    public List<IRayInteractable> m_hitLastFrame { get; private set; } = new List<IRayInteractable>();

    private void Awake()
    {
        // calculate the actual intensity value necessary internally
        // (color intensity is not just  multiplication with the corresponding value. Displayed value is an binary exponent)
        m_impulsIntensity = Mathf.Pow(2, m_impulsIntensity);
        m_beamIntensity = Mathf.Pow(2, m_beamIntensity);
    }

    private void Update()
    {
        List<IRayInteractable> m_hitThisFrame = new List<IRayInteractable>(); // Collects the IRayInteractables that will be hit during this frame.

        // Destroy all rays from previous frame and initialize starting ray + lineRenderer
        DestroyRays();
        m_lightRays.Add(GameObject.Instantiate(m_lightBeamPrefab));
        m_lightRays[0].transform.position = transform.position;
        InitializeLineRenderer(m_lightRays[0].GetComponentInChildren<LineRenderer>(), transform.position);

        m_ray = new Ray(transform.position, transform.forward);

        for (int i = 0; i < m_reflections; ++i)
        {
            if (Physics.Raycast(m_ray.origin, m_ray.direction, out m_hit))
            {
                m_lineRenderer.positionCount += 1;
                m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, m_hit.point);

                if (m_hit.collider.tag != "ReflectiveSurface")
                {
                    if (m_hit.collider.tag == "RayInteractable")
                    {
                        IRayInteractable m_rayInteractable = m_hit.transform.gameObject.GetComponent<IRayInteractable>();
                        if(m_rayInteractable!=null) m_rayInteractable.OnHit(m_rayColor, m_hit);
                        m_hitThisFrame.Add(m_rayInteractable); // Save the hit IRayInteractable.
                    }

                    break;
                }

                GameObject reflectionBeam = GameObject.Instantiate(m_lightBeamPrefab);
                m_lightRays.Add(reflectionBeam);
                reflectionBeam.transform.position = m_hit.point;
                InitializeLineRenderer(reflectionBeam.GetComponentInChildren<LineRenderer>(), m_hit.point);
                m_lineRenderer.material.name = i.ToString();

                // change color according to the ingoing ray color and the reflector color
                Color segmentColor = m_hit.collider.transform.gameObject.GetComponentInParent<ReflectorColorBehaviorIO>().GetOutputRayColor(ref m_rayColor);
                segmentColor = new Color(segmentColor.r * m_beamIntensity, segmentColor.g * m_beamIntensity, segmentColor.b * m_beamIntensity, 1);
                //m_lineRenderer.material.color = segmentColor;
                m_lineRenderer.material.SetColor("_Color", segmentColor);

                m_ray = new Ray(m_hit.point, Vector3.Reflect(m_ray.direction, m_hit.normal));
            }
            else
            {
                m_lineRenderer.positionCount += 1;
                m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, m_ray.origin + m_ray.direction * m_maxLength);
            }
        }

        m_rayColor = m_initialRayColor;

        m_hitLastFrame.Except(m_hitThisFrame).ToList().ForEach(x => {if(x != null) x.OnMiss();}); // Call OnMiss() for every IRayInteractable that was hit last frame but not during this one.
        m_hitLastFrame = m_hitThisFrame; // Save the IRayInteractable that have been hit during this frame as reference for the next frame.
    }

    //-------------------------------------------------------------------------
    // Event Handler

    private void OnDisable()
    {
        m_beamImpuls.SetActive(false);
        DestroyRays();
    }

    private void OnEnable()
    {
        m_beamImpuls.SetActive(true);
    }

    //-------------------------------------------------------------------------
    // Utilities

    private void DestroyRays()
    {
        for (int i = 0; i < m_lightRays.Count; ++i)
        {
            Destroy(m_lightRays[i]);
        }

        m_lightRays.Clear();
    }

    private void InitializeLineRenderer(LineRenderer lineRenderer, Vector3 startPosition)
    {
        m_lineRenderer = lineRenderer;
        m_lineRenderer.material = Material.Instantiate(lineRenderer.material);
        m_lineRenderer.positionCount = 1;
        m_lineRenderer.SetPosition(0, startPosition);
    }

    //-------------------------------------------------------------------------
    // Getter & Setter

    public void SetColor(string colorKey)
    {
        //Here we fetch the color and then do your thing X) 
        m_initialRayColor = colorKey;
        m_rayColor = colorKey;
        Color color = ColorPalette.Instance.GetColorData.GetValueOrDefault(m_rayColor).GetColor();

        Color intensifiedColor = new Color(color.r * m_impulsIntensity, color.g * m_impulsIntensity, color.b * m_impulsIntensity, 1);
        m_impulsMaterial.color = intensifiedColor;

        intensifiedColor = new Color(color.r * m_beamIntensity, color.g * m_beamIntensity, color.b * m_beamIntensity, 1);
        m_beamMaterial.color = intensifiedColor;
    }
}