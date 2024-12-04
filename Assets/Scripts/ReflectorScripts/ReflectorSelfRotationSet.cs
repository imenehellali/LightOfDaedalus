using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorSelfRotationSet : MonoBehaviour
{
    public enum RotOrFix
    {
        Rotating,
        Fix,
    }

    [SerializeField] private RotOrFix _rotOrFix;
    [SerializeField] private GameObject _stand;
    [SerializeField] private GameObject _mirror;
    [SerializeField] private float m_rotationRate = 360f;
    private Vector3 _rotation;
    private Quaternion m_offset;
    private Quaternion m_targetRotation;

    private void Awake()
    {
        _stand = transform.Find("Stand").gameObject;
        m_offset = _stand.transform.localRotation;
        m_targetRotation = m_offset;
    }

    private void Update()
    {
        // Rotate smoothly towards the target rotation
        _stand.transform.localRotation = Quaternion.RotateTowards(_stand.transform.localRotation, m_targetRotation, m_rotationRate * Time.deltaTime);
    }

    /// <summary>
    /// Sets the target rotation of the stand on the statue by a Quaternion q while taking into effect the initial offset rotation of the statue.
    /// </summary>
    /// <param name="q"></param>
    public void Rotate(Quaternion q)
    {
        m_targetRotation = Quaternion.Inverse(m_offset) * q;
    }

    public void SetRotationAngleOnReflector(Vector3 _rot)
    {
        if (this._rotOrFix.Equals(RotOrFix.Rotating))
        {
            this._rotation = _rot;

            //we rotate the stand if rotation on the Y axis

            //we rotate the mirror if rotation on the X axis

            //Code to set fixed rotation angles with joystick 
        }
    }
}