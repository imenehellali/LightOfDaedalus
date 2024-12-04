using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MirrorCamera : MonoBehaviour
{
    [SerializeField]
    private Camera m_playerCamera;
    private Camera m_mirrorCamera;

    private Vector3 m_reflectionVector;
    private Vector3 m_reflectionPos;
    [SerializeField]
    private Vector3 m_wallNormal;

    // Start is called before the first frame update
    void Start()
    {
        m_mirrorCamera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        m_reflectionVector = Vector3.Reflect(m_mirrorCamera.transform.position - m_playerCamera.transform.position, m_wallNormal);
        m_reflectionPos = m_mirrorCamera.transform.position + m_reflectionVector;

        m_mirrorCamera.transform.LookAt(m_reflectionPos);
    }
}
