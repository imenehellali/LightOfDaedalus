using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorColorBehaviorIO : MonoBehaviour
{
    private string _reflectorColor;
    private void Start()
    {
        _reflectorColor = gameObject.GetComponent<ReflectorSelfColorSet>().reflectorColor.ToString();
    }
    public Color GetOutputRayColor(ref string rayColor)
    {
        ColorPalette _palett = ColorPalette.Instance;

        if (_reflectorColor.Equals("None"))
            return _palett.GetColorData.GetValueOrDefault(rayColor).GetColor();

        rayColor = _palett.MixedColorsOutput(_reflectorColor, rayColor);
        return _palett.GetColorData.GetValueOrDefault(rayColor).GetColor();
    }

}
