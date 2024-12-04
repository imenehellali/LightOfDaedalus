using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base class for all obstacles that have to interact with the ray on hit
/// </summary>
public abstract class IRayInteractable : MonoBehaviour
{
    public enum InteractableColor
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple,
        Orange,
        Turquoise,
        Pink,
        None,
    }

    [SerializeField]
    protected InteractableColor m_color = InteractableColor.None;
    [SerializeField]
    protected float m_interactionTime = 2f;
    protected bool m_interacting = false;
    protected bool m_interactionFinished = false;

    /// <summary> Call this function when the ray hits this object </summary>
    public abstract void OnHit(string colorCode, RaycastHit hit);
    
    /// <summary> Called when a IRayInteractable has been missed in a frame following one where it was hit. </summary>
    public abstract void OnMiss();

    protected bool CheckIncomingColor(string colorCode)
    {
        return (InteractableColor.None == m_color || colorCode == m_color.ToString());
    }

    // changes the RayInteractables material to show the selected m_color properly
    abstract protected void SetColor();

    // reset rayInteractable, when the interaction was canceled before the required interactionTime
    abstract protected void ResetRayInteractable();
}
