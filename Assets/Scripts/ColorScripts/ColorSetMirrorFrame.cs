using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetMirrorFrame : MonoBehaviour
{
    private Color color; 
    [SerializeField] private ReflectorSelfColorSet.ReflectorColor _color;
    [SerializeField] private List<MeshRenderer> _frameRenders=new List<MeshRenderer>();
    private void OnEnable()
    {
        Color color = ColorPalette.Instance.GetColorData.GetValueOrDefault(_color.ToString()).GetColor();

        foreach (MeshRenderer rend in _frameRenders)
        {
            Color intensifiedColor = new Color(color.r * Mathf.Pow(2, 4f), color.g * Mathf.Pow(2, 4f), color.b * Mathf.Pow(2, 4f), 1);
            rend.material.color = intensifiedColor;

            
        }
    }

}
