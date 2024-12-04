using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public static ColorPalette Instance { get; private set; }

    public ColorDataDictionary m_colorDataDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public string MixedColorsOutput(string _reflectorColor, string _rayColor)
    {
        switch(_rayColor)
        {
            case "Red":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Red";
                    case "Blue":
                        return "Purple";
                    case "Yellow":
                        return "Orange";
                    case "Orange":
                        return "Red";
                    case "Purple":
                        return "Blue";
                    case "Green":
                        return "Pink";
                    case "Turquoise":
                        return "Green";
                    case "Pink":
                        return "Red";
                }
                break;
            case "Blue":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Purple";
                    case "Blue":
                        return "Blue";
                    case "Yellow":
                        return "Green";
                    case "Orange":
                        return "Purple";
                    case "Purple":
                        return "Blue";
                    case "Green":
                        return "Turquoise";
                    case "Turquoise":
                        return "Green";
                    case "Pink":
                        return "Purple";
                }
                break;
            case "Yellow":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Orange";
                    case "Blue":
                        return "Green";
                    case "Yellow":
                        return "Yellow";
                    case "Orange":
                        return "Pink";
                    case "Purple":
                        return "Green";
                    case "Green":
                        return "Yellow";
                    case "Turquoise":
                        return "Green";
                    case "Pink":
                        return "Orange";
                }
                break;
            case "Orange":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Red";
                    case "Blue":
                        return "Purple";
                    case "Yellow":
                        return "Pink";
                    case "Orange":
                        return "Orange";
                    case "Purple":
                        return "Red";
                    case "Green":
                        return "Yellow";
                    case "Turquoise":
                        return "Red";
                    case "Pink":
                        return "Pink";
                }
                break;
            case "Purple":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Blue";
                    case "Blue":
                        return "Blue";
                    case "Yellow":
                        return "Pink";
                    case "Orange":
                        return "Red";
                    case "Purple":
                        return "Purple";
                    case "Green":
                        return "Purple";
                    case "Turquoise":
                        return "Orange";
                    case "Pink":
                        return "Red";
                }
                break;
            case "Green":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Pink";
                    case "Blue":
                        return "Turquoise";
                    case "Yellow":
                        return "Turquoise";
                    case "Orange":
                        return "Orange";
                    case "Purple":
                        return "Green";
                    case "Green":
                        return "Green";
                    case "Turquoise":
                        return "Yellow";
                    case "Pink":
                        return "Orange";
                }
                break;
            case "Turquoise":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Green";
                    case "Blue":
                        return "Green";
                    case "Yellow":
                        return "Green";
                    case "Orange":
                        return "Red";
                    case "Purple":
                        return "Orange";
                    case "Green":
                        return "Yellow";
                    case "Turquoise":
                        return "Turquoise";
                    case "Pink":
                        return "Yellow";
                }
                break;
            case "Pink":
                switch (_reflectorColor)
                {
                    case "Red":
                        return "Red";
                    case "Blue":
                        return "Purple";
                    case "Yellow":
                        return "Orange";
                    case "Orange":
                        return "Pink";
                    case "Purple":
                        return "Red";
                    case "Green":
                        return "Orange";
                    case "Turquoise":
                        return "Yellow";
                    case "Pink":
                        return "Pink";
                }
                break;
        }
        return _rayColor;
    }

    //-------------------------------------------------------------------------
    // Getter & Setter

    public ColorDataDictionary GetColorData => m_colorDataDictionary;
}
