using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType {
    Name,
    Coordinates,
    Color,
    Luminosity,
    Spectrum
}

public class DataSlot : MonoBehaviour
{
    [SerializeField] private SlotType SlotType;

    [SerializeField] private Graphic DropHighlight;


    [SerializeField] private Graphic Graphic;
    [SerializeField] private TMP_Text Text;

}
