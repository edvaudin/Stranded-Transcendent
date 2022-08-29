using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class TileOverlay : MonoBehaviour
{
    [SerializeField] TMP_Text overlayText;
    [SerializeField] Image overlayImage;
    [SerializeField] Color wallOverlay;
    [SerializeField] Color groundOverlay;

    public void SetText(string text)
    {
        overlayText.text = text;
    }

    public void SetWall()
    {
        overlayImage.color = wallOverlay;
    }

    public void SetGround()
    {
        overlayImage.color = groundOverlay;
    }
}
