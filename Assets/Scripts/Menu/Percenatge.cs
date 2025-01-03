using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Percenatge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI percentL1Text;
    public TextMeshProUGUI percentL2Text;


    void Start()
    {
        
        //POR SI QUIERES BORRAR LA MEMORIA DEL JUEGO, LO EJECUTAS UNA VEZ Y VUELVES A COMENTARLO
        
       /* PlayerPrefs.DeleteKey("Percent1");
        PlayerPrefs.DeleteKey("Percent2");*/

        int percentL1 = PlayerPrefs.GetInt("Percent1", 0);
        Debug.Log(percentL1);
        percentL1Text.text = "Max Percent: " + percentL1.ToString() + "%";

        int percentL2 = PlayerPrefs.GetInt("Percent2", 0);
        percentL2Text.text = "Max Percent: " + percentL2.ToString() + "%";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
