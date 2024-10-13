using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct Range
{
    public float min;
    public float max;
}

public class UIBarController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _barForeground;
    [SerializeField] private UnityEngine.UI.Image _barBackground; // Imagen de fondo de la barra
    [SerializeField] private TextMeshProUGUI _barCounter;
    [SerializeField] private TextMeshProUGUI _barTitle;
    [SerializeField] private string title;
    
    [SerializeField] private float lerpSpeed = 5f; // Velocidad de la interpolación
    [SerializeField] private Color32 barColorFull; // Color de la barra cuando está llena
    [SerializeField] private Color32 barColorMid; // Color de la barra cuando está a la mitad
    [SerializeField] private Color32 barColorLow; // Color de la barra cuando está baja
    [SerializeField] private Color32 barColorEmpty; // Color de la barra cuando está vacía
    [SerializeField] private Range fullRangePercentage = new Range { min = 75f, max = 100f }; // Rango de porcentaje para el color de la barra llena
    [SerializeField] private Range midRangePercentage = new Range { min = 25f, max = 75f }; // Rango de porcentaje para el color de la barra a la mitad
    [SerializeField] private Range lowRangePercentage = new Range { min = 1f, max = 25f }; // Rango de porcentaje para el color de la barra baja

    
    

    // Guardamos el valor actual del fill amount y lo interpolamos al nuevo valor

    void Start() {
        ChangeColor(barColorFull);
        _barTitle.text = title;
    }

    void Update() {
        float fillAmount = _barForeground.fillAmount * 100;
        if (fillAmount >= fullRangePercentage.min && fillAmount <= fullRangePercentage.max) {
            ChangeColor(barColorFull);
        } else if (fillAmount >= midRangePercentage.min && fillAmount <= midRangePercentage.max) {
            ChangeColor(barColorMid);
        } else if (fillAmount >= lowRangePercentage.min && fillAmount <= lowRangePercentage.max) {
            ChangeColor(barColorLow);
        } else {
            ChangeColor(barColorEmpty);
        }
    }

    void ChangeColor(Color32 color) {
        Color32 backgroundColor = color;
        backgroundColor.a = 100;
        _barBackground.color = backgroundColor;
        // Color de la barra
        _barForeground.color = color;
        // Color del texto
        _barCounter.color = color;
        _barTitle.color = color;
    }

    public void SetFillAmount(float fillAmount, float maxValue) {
        float targetFillAmount = fillAmount / maxValue;
        _barForeground.fillAmount = Mathf.Lerp(_barForeground.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
    }

    // Actualizamos el texto del contador
    public void SetCounterText(float value) {
        int intValue = Mathf.RoundToInt(value);
        _barCounter.text = intValue.ToString();
    }
}

