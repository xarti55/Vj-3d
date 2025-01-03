using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LevelLoader : MonoBehaviour
{
    public void LoadNewLevel(){
        SceneManager.LoadScene("Menu");
    }
}
