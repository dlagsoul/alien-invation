using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public int fpsLimit = 60;
    public bool showFPS = true;

    private void Start()
    {
        // Inicializamos el texto con un valor por defecto
        fpsText.text = "FPS: 0";
    }

    private void Update()
    {
        // Calculamos los FPS dividiendo 1 por el tiempo que tarda en renderizar el último frame
        float currentFPS = 1.0f / Time.deltaTime;
        
        // Mostramos los FPS en el texto (opcional: puedes formatear para reducir decimales)
        fpsText.gameObject.SetActive(showFPS);
        fpsText.text = "FPS: " + Mathf.RoundToInt(currentFPS).ToString();
        Application.targetFrameRate = fpsLimit;
    }
}
