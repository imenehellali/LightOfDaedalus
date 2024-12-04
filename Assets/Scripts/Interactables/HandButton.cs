using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script handles the visual interaction as well as triggering events for a VR button. 
/// </summary>
public class HandButton : XRBaseInteractable
{
    [SerializeField] private UnityEvent m_onPressed = new UnityEvent(); // The event being triggered when pressing the button
    [SerializeField] private float m_activationThreshold = 0.01f; // The height threshold over the minHeight for the button to activate
    [SerializeField]private bool m_previousPress = false; // Whether the button was pressed in the previous frame
    [SerializeField]private float m_minHeight; // The minimum height the button can go
    [SerializeField]private float m_maxHeight;// The maximum height the button can go
    [SerializeField]private float m_previousHandHeight = 0f; // The height of the player's hand in the previous frame
    [SerializeField]private IXRHoverInteractor m_hoverInteractor = null; // The current IXRHoverInteractor interacting with this 

    private void Start()
    {
        SetMinMax();
    }

    //////////////////////////////////////////////////////////

    #region events

    /// <summary> Triggered when the BaseInteractable is being hovered. </summary>
    /// <param name="args"> The parameters of that event. </param>
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        ((XRDirectInteractor) args.interactorObject).hideControllerOnSelect = false;
        base.OnHoverEntered(args);
        StartPress(args.interactorObject);
    }

    /// <summary> Triggered when the BaseInteractable is no longer being hovered. </summary>
    /// <param name="args"> The parameters of that event. </param>
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        EndPress(args.interactorObject);
    }

    /// <summary> Checks every frame whether the button is being pressed, then  </summary>
    /// <param name="updatePhase"></param>
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        //base.ProcessInteractable(updatePhase);

        if (m_hoverInteractor != null)
        {
            float newHandHeight = GetLocalYPosition(m_hoverInteractor.transform.position);
            float handDifference = m_previousHandHeight - newHandHeight;
            m_previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);
            
            CheckPress();
        }
    }
    
    /// <summary> The behavior to be executed when starting to press: Set the current interactor, save the height of the hand.  </summary>
    /// <param name="interactor"> The Interactor that is pressing the button. </param>
    private void StartPress(IXRHoverInteractor interactor)
    {
        m_hoverInteractor = interactor;
        m_previousHandHeight = GetLocalYPosition(interactor.transform.position);
    }

    /// <summary> Reset the values.  </summary>
    /// <param name="interactor"> The Interactor that is no longer pressing the button. </param>
    private void EndPress(IXRHoverInteractor interactor)
    {
        m_hoverInteractor = null;
        m_previousHandHeight = 0f;
        m_previousPress = false;
        SetYPosition(m_maxHeight);
    }

    #endregion

    /// <summary> Get the minimal and maximal height the button will be able to move inbetween from its position and 0.4 times the height of its collider. </summary>
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        m_minHeight = transform.localPosition.y - (collider.bounds.size.y * 0.85f);
        m_maxHeight = transform.localPosition.y;
    }

    /// <summary> Gets the local y position of a world position in relation to the root transform of this transform. </summary>
    /// <param name="position"> The world position to be transformed. </param>
    /// <returns></returns>
    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    /// <summary> Make sure the position of the button stays in between the predefined bounds. </summary>
    /// <param name="y"></param>
    private void SetYPosition(float y)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(y, m_minHeight, m_maxHeight);
        transform.localPosition = newPosition;
    }

    /// <summary> Saves whether the button has been pressed for the next frame and throws an event. </summary>
    private void CheckPress()
    {
        if (InPosition() && !m_previousPress)
        {
            m_previousPress = true;
            m_onPressed.Invoke();
        }
    }

    /// <summary> Checks whether the button is within an activation threshold. </summary>
    /// <returns></returns>
    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, m_minHeight, m_minHeight + m_activationThreshold);
        return transform.localPosition.y == inRange;
    }
}