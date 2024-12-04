using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandKnob : XRBaseInteractable
{
    [Serializable]
    public class RotationEvent : UnityEvent<Quaternion>
    {
    }

    [SerializeField] private RotationEvent m_onRotation = new RotationEvent(); // The event being triggered when rotating the knob
    private Quaternion m_initialKnobRotation; // The very initial rotation of the knob GameObject (to compute an offset)
    private Quaternion m_initialHandRotation; // The hand's initial rotation (to compute an offset)
    private Quaternion m_lastGrabRotation; // The rotation of the knob after it has last been released from a grab
    [SerializeField] private Quaternion m_lastRotationPosition = Quaternion.Euler(0, 0, 0); // The most recent rotation the button snapped to (gets compared with snappedRotation, e.g. 0°, 45°, etc.)
    private Quaternion m_targetRotation;
    [SerializeField] private float m_rotationRate = 360f;
    private IXRSelectInteractor m_hoverInteractor = null; // The current IXRSelectInteractor interacting with this 

    private void Start()
    {
        m_initialKnobRotation = transform.localRotation;
        m_lastGrabRotation = m_initialKnobRotation;
        m_targetRotation = m_lastGrabRotation;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, m_targetRotation, m_rotationRate * Time.deltaTime);
    }

    //////////////////////////////////////////////////////////

    #region events

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        ((XRDirectInteractor) args.interactorObject).hideControllerOnSelect = false;
        base.OnSelectEntered(args);
        StartGrab(args.interactorObject);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        EndGrab(args.interactorObject);
    }

    /// <summary> Checks every frame whether the button is being pressed, then  </summary>
    /// <param name="updatePhase"></param>
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (m_hoverInteractor != null)
        {
            Quaternion snappedRotation = GetSnappedRotation();
            m_targetRotation = m_lastGrabRotation * snappedRotation;
            CheckRotation(snappedRotation);
        }
    }

    /// <summary> The behavior to be executed when starting to press: Set the current interactor, save the height of the hand.  </summary>
    /// <param name="interactor"> The Interactor that is pressing the button. </param>
    private void StartGrab(IXRSelectInteractor interactor)
    {
        m_hoverInteractor = interactor;
        m_initialHandRotation = interactor.transform.localRotation;
    }

    /// <summary> Reset the values.  </summary>
    /// <param name="interactor"> The Interactor that is no longer pressing the button. </param>
    private void EndGrab(IXRSelectInteractor interactor)
    {
        m_hoverInteractor = null;
        m_lastGrabRotation = m_targetRotation;
        m_initialHandRotation = quaternion.Euler(0, 0, 0);
    }

    #endregion

    /// <summary> Computes the z (forward) rotation of the hand relative to the button's initial rotation and the hand's rotation in the moment of grabbing the button. </summary>
    /// <returns></returns>
    private Quaternion GetSnappedRotation()
    {
        Quaternion relativeRotation =
            Quaternion.Inverse(m_initialKnobRotation) * Quaternion.Inverse(m_initialHandRotation) * 
            m_hoverInteractor.transform.localRotation; // Rotation respecting the rotation with which the knob was grabbed 
        Vector3 relativeRotationEuler = relativeRotation.eulerAngles;

        switch (relativeRotationEuler.z)
        {
            case float n when n >= 22.5 && n < 22.5 + 45: // snap to 45°
                return Quaternion.Euler(0, -45, 0);
            case float n when n >= 22.5 + 45 && n < 22.5 + 45 * 2: // snap to 90°
                return Quaternion.Euler(0, -90, 0);
            case float n when n >= 22.5 + 45 * 2 && n < 22.5 + 45 * 3: // snap to 135°
                return Quaternion.Euler(0, -135, 0);
            case float n when n >= 22.5 + 45 * 3 && n < 22.5 + 45 * 4: // snap to 180°
                return Quaternion.Euler(0, -180, 0);
            case float n when n >= 22.5 + 45 * 4 && n < 22.5 + 45 * 5: // snap to 225°
                return Quaternion.Euler(0, -225, 0);
            case float n when n >= 22.5 + 45 * 5 && n < 22.5 + 45 * 6: // snap to 270°
                return Quaternion.Euler(0, -270, 0);
            case float n when n >= 22.5 + 45 * 6 && n < 22.5 + 45 * 7: // snap to 315°
                return Quaternion.Euler(0, -315, 0);
            default: // snap to 0°
                return Quaternion.Euler(0, 0, 0);
        }
    }

    /// <summary> Saves whether the button has been pressed for the next frame and throws an event. </summary>
    private void CheckRotation(Quaternion snappedRotation)
    {
        if (snappedRotation != m_lastRotationPosition)
        {
            m_onRotation.Invoke(m_lastGrabRotation * snappedRotation);
        }
        m_lastRotationPosition = snappedRotation;
    }
}