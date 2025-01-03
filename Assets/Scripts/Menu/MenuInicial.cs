using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MenuInicial : MonoBehaviour
{

    public void Level1(){
        PlayerPrefs.SetString("SelectedLevel", "1");
        SceneManager.LoadScene("Level 1");
    }
    public void Level2(){
        PlayerPrefs.SetString("SelectedLevel", "2");
        SceneManager.LoadScene("Level 2");
    }

    public void Credits(){
        SceneManager.LoadScene("Credits");
    }

    public void ChangeCharacter(){
        SceneManager.LoadScene("SelectionPlayer");
    }

}
