using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorData
{
    [SerializeField]
    private Texture m_texture;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color m_color;

    public ColorData(Texture texture, Color color)
    {
        m_texture = texture;
        m_color = color;
    }

    //-------------------------------------------------------------------------
    // Getter & Setter

    public Texture GetTexture()
    {
        return m_texture;
    }

    public Color GetColor()
    {
        return m_color;
    }
}
