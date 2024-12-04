using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorSelfColorSet : MonoBehaviour
{
    public enum ReflectorColor
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

    [SerializeField] private ReflectorColor _color;
    [SerializeField] private GameObject _indicator;

    public ReflectorColor reflectorColor => _color;
    private void Start()
    {
        ChangeReflectorColor();
        Debug.Log("self reflector color:           " + _color.ToString());
    }

    private void ChangeReflectorColor()
    {
        if(_color.Equals(ReflectorColor.None))
        {

            //_indicator.GetComponent<MeshRenderer>().material.DisableKeyword("_Emission");
            //_indicator.GetComponent<MeshRenderer>().material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            //_indicator.GetComponent<MeshRenderer>().material.color = Color.black;
            //_indicator.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);

            Color intensifiedColor = new Color(Color.black.r * Mathf.Pow(2, 9f), Color.black.g * Mathf.Pow(2, 9f), Color.black.b * Mathf.Pow(2, 9f), 1);
            _indicator.GetComponent<MeshRenderer>().material.color = intensifiedColor;
        }
        else
        {
            Color color = ColorPalette.Instance.GetColorData.GetValueOrDefault(_color.ToString()).GetColor();
            //Texture texture = ColorPalette.Instance.GetColorData.GetValueOrDefault(_color.ToString()).GetTexture();

            //_indicator.GetComponent<MeshRenderer>().material.mainTexture = texture;
            ////_indicator.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap",texture);

            //_indicator.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color);

            Color intensifiedColor = new Color(color.r * Mathf.Pow(2, 6f), color.g * Mathf.Pow(2, 6f), color.b * Mathf.Pow(2, 6f), 1);
            _indicator.GetComponent<MeshRenderer>().material.color = intensifiedColor;



        }
    }
}
