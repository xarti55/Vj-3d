using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField] int current_lvl;
    int current_stage;

    public TextMeshProUGUI instructions;


    private int max_l1, max_l2;
    private Map my_map;
    private Player_Script my_player;

    private GameObject playerCreated;
    public static Game Instance;
    GameObject Initialcube, Initialcube1, Initialcube2;
    private bool generating;
    public List<GameObject> characters = new List<GameObject>();


    private void Awake(){ 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }


    void Start(){
        //string selectedCharacterName = "Zombie";
        max_l1 = PlayerPrefs.GetInt("Percent1", 0);
        max_l2 = PlayerPrefs.GetInt("Percent2", 0);
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter");

        Debug.Log(selectedCharacterName);

        GameObject Prefab = null;

        foreach (GameObject character in characters){
            if (character.name == selectedCharacterName){
                Prefab = character;
                break;
            }
        }

        if (Prefab != null){
            playerCreated = Instantiate(Prefab);   
            playerCreated.transform.position = new Vector3(0, 1.05000002f, -1);
            Debug.Log("HOLA");
            CapsuleCollider capsuleCollider = playerCreated.AddComponent<CapsuleCollider>();                  
            Rigidbody rb = playerCreated.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            playerCreated.AddComponent<Player_Script>();
            playerCreated.tag="player";
           /* playerCreated.AddComponent<PlayerPinxo>();
            playerCreated.AddComponent<PlayerBarrote>();
            playerCreated.AddComponent<PlayerTrampolin>();
            playerCreated.AddComponent<coinCounter>();*/
        }
        current_stage = 0;
        my_map = Map.Instance; 
        my_player = playerCreated.GetComponent<Player_Script>();
        my_player.set_can_move(false);
        if (current_lvl == 1) creata_initial_cubes_l1();
        if (current_lvl == 2) creata_initial_cubes_l2();
        my_map.load_lvl_stage(current_lvl,0); 
        my_player.set_rotation(180);    
        generating = true;
        if(current_lvl == 1 && current_stage == 0){
            instructions.gameObject.SetActive(true);
        }
        my_player.setPosition(new Vector3(0,0,-1));
    }



    void Update(){
        if (generating){
            if (my_map.generating_map(current_lvl,current_stage)){
                my_player.set_can_move(true);
                generating = false;
            }
        }
        int percentatge = my_map.get_percentage();
        if (current_lvl == 1 && percentatge > max_l1){
            max_l1 = percentatge;
            PlayerPrefs.SetInt("Percent1", max_l1);
        }
        else if (current_lvl == 2 && percentatge > max_l2){
            max_l2 = percentatge;
            PlayerPrefs.SetInt("Percent2", max_l2);
        }

    }

    private void creata_initial_cubes_l1(){
        Texture cubeTexture_grass = Resources.Load<Texture>("Grass");
        Texture cubeTexture_dirt = Resources.Load<Texture>("Dirt");
        Material dynamicMaterial_grass = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_dirt = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_grass.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_dirt.mainTextureScale = new Vector2(0.1f, 0.1f);
        dynamicMaterial_grass.mainTexture = cubeTexture_grass;
        dynamicMaterial_dirt.mainTexture = cubeTexture_dirt;
        Initialcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube.transform.position = new Vector3(0, 0 , -1);
        Initialcube1.transform.position = new Vector3(0, -1 , -1);
        Initialcube2.transform.position = new Vector3(0, -2 , -1);
        Initialcube.GetComponent<Renderer>().material = dynamicMaterial_grass;
        Initialcube1.GetComponent<Renderer>().material = dynamicMaterial_dirt;
        Initialcube2.GetComponent<Renderer>().material = dynamicMaterial_dirt;
    }

    private void creata_initial_cubes_l2(){
        Texture cubeTexture_wood = Resources.Load<Texture>("wood");
        Texture cubeTexture_stone = Resources.Load<Texture>("stone");
        Material dynamicMaterial_wood = new Material(Shader.Find("HDRP/Lit"));
        Material dynamicMaterial_stone = new Material(Shader.Find("HDRP/Lit"));  
        dynamicMaterial_wood.mainTextureScale = new Vector2(0.02f, 0.02f);
        dynamicMaterial_stone.mainTextureScale = new Vector2(0.02f, 0.02f);
        dynamicMaterial_wood.mainTexture = cubeTexture_wood;
        dynamicMaterial_stone.mainTexture = cubeTexture_stone;
        Initialcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Initialcube.transform.position = new Vector3(0, 0 , -1);
        Initialcube1.transform.position = new Vector3(0, -1 , -1);
        Initialcube2.transform.position = new Vector3(0, -2 , -1);
        Initialcube.GetComponent<Renderer>().material = dynamicMaterial_wood;
        Initialcube1.GetComponent<Renderer>().material = dynamicMaterial_stone;
        Initialcube2.GetComponent<Renderer>().material = dynamicMaterial_stone;
    }

    public void stage_finished(){
        ++current_stage;
        if (current_stage == 10){
            if (current_lvl == 1 ){
                max_l1 = 100;
                PlayerPrefs.SetInt("Percent1", 100);
            }
            else if (current_lvl == 2 ){
                max_l2 = 100;
                PlayerPrefs.SetInt("Percent2", 100);
            }
            SceneManager.LoadScene("Menu");
        }
        instructions.gameObject.SetActive(false);
        my_map.load_lvl_stage(current_lvl,current_stage);
        my_player.setPosition(new Vector3(0,0,-1));
        my_player.set_can_move(false);
        my_player.set_rotation(180);
        generating = true;
        
        
        //else my_map.load_lvl_stage(current_lvl,current_stage);
    }

    public void death(int percentatge){
        if (current_lvl == 1 && percentatge > max_l1){
            max_l1 = percentatge;
            PlayerPrefs.SetInt("Percent1", max_l1);
        }
        else if (current_lvl == 2 && percentatge > max_l2){
            max_l2 = percentatge;
            PlayerPrefs.SetInt("Percent2", max_l2);
        }
        current_stage = -1;
        stage_finished();
    }

    private int get_percentage(){

        return max_l1;
    }

    private void set_percentage(int percentage){
        max_l1 = percentage;
    }
}
