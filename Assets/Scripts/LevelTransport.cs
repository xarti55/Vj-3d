using UnityEngine.SceneManagement;

using UnityEngine;

public class leveltransport : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            PlayerPrefs.SetString("SelectedLevel", "1");
            SceneManager.LoadScene("Level 1");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            PlayerPrefs.SetString("SelectedLevel", "2");
            SceneManager.LoadScene("Level 2");
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Menu");
        }
    }
}
