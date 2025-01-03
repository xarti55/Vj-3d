using TMPro;



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public float movement_speed = 4.0f;
    public float JumpForce = 8.0f;
    private bool can_move;
    private Map my_map;
    private TrailRenderer trail;
    private Gradient gradient;
    private Rigidbody rb;
    public static Player_Script Instance;

    private int next_dir;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI percentageText;
    private bool trampolin_actioned;
    public AudioClip coinSound, jumpSound, deathSound, trampolineSound;
    private AudioSource audioSource;
    private bool God_mode;
    private float since_last_jump;

    private void Awake(){ 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            
            Instance = this; 
            God_mode = false;
            rb = GetComponent<Rigidbody>();
            trail = gameObject.AddComponent<TrailRenderer>();
            trail.time = 1.0f; 
            trail.startWidth = 0.5f;
            trail.endWidth = 0.1f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            gradient = new Gradient();
            gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            trail.colorGradient = gradient;
            since_last_jump = 500;
        }
    }

    void Start()
    {
        my_map = Map.Instance; 
        rb = GetComponent<Rigidbody>();

        GameObject coinObject = GameObject.Find("Canvas/Coin");
        GameObject percentageObject = GameObject.Find("Canvas/Percentage");
        coinText = coinObject.GetComponent<TextMeshProUGUI>();
        percentageText = percentageObject.GetComponent<TextMeshProUGUI>();
        audioSource = gameObject.AddComponent<AudioSource>();
        trampolineSound = Resources.Load<AudioClip>("Sounds/funny-spring-229170");
        coinSound = Resources.Load<AudioClip>("Sounds/coin-recieved-230517");
        jumpSound = Resources.Load<AudioClip>("Sounds/cartoon-jump-6462");
        deathSound = Resources.Load<AudioClip>("Sounds/male-death-sound-128357");
    }

    public void set_can_move(bool new_can_move){
        trampolin_actioned = false;
        if (new_can_move == false) trail.time = 0.0f;
        else trail.time = 1.0f;
        can_move = new_can_move;
    }

    public void set_rotation(int rotation){
        transform.eulerAngles = new Vector3(-90, rotation, 0);
    }

    void Update()
    {
        // Basic movement logic
        if (Input.GetKeyDown(KeyCode.G)){
            God_mode = !God_mode;
            Debug.Log("GOD");
        } 
        if (!can_move) return;
        float x = 0; 
        float z = 0; 
        if (trampolin_actioned) return;
        next_dir = my_map.player_next_move(rb.position);
        if (next_dir == -1){ 
            x = -1;
            transform.eulerAngles = new Vector3(-90, 90, 0);
        }
        else if (next_dir == 1){
            x = 1;
            transform.eulerAngles = new Vector3(-90, 270, 0);
        }
        else if (next_dir == -2){
            z = -1;
            transform.eulerAngles = new Vector3(-90, 0 , 0);  
        }    
        else if (next_dir == 2){
            z = 1;
            transform.eulerAngles = new Vector3(-90, 180, 0);
        }
        
        Vector3 movement = new Vector3(x, 0 , z) * movement_speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
        since_last_jump += Time.deltaTime;
        if (God_mode){
            if(my_map.has_to_jump(rb.position) && IsGrounded()  && since_last_jump > 0.1){
                
                since_last_jump = 0;
                audioSource.PlayOneShot(jumpSound);
                rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            }
        }
        else {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && IsGrounded())
            {
                audioSource.PlayOneShot(jumpSound);
                rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            }
        }

        percentageText.text = "% " + my_map.get_percentage().ToString();        
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f);
    }

    public void setPosition(Vector3 x){
        trampolin_actioned = false;
        since_last_jump = 500;
        rb.linearVelocity  = Vector3.zero;
        rb.MovePosition(x);
    }



    private int Coin = 0;
    

    private void OnTriggerEnter(Collider other){
        if(other.transform.tag == "coin")
        {
            Coin++;
            coinText.text = "Coin: " + Coin.ToString();
            audioSource.PlayOneShot(coinSound);
            Destroy(other.gameObject);
        }
        if(other.transform.tag == "pinxo")
        {
            Coin = 0;
            audioSource.PlayOneShot(deathSound);
            Game.Instance.death(my_map.get_percentage());
            percentageText.text = "% " + my_map.get_percentage().ToString();
        }
        if(other.transform.tag == "barrote")
        {
            trampolin_actioned = true;
            rb.AddForce(new Vector3(-4, 2, 0), ForceMode.Impulse);
            StartCoroutine(TranslateAfterDelay());
        }
        if(other.transform.tag == "trampolin")
        {
            audioSource.PlayOneShot(trampolineSound);
            trampolin_actioned = true;
            if (next_dir == 1) rb.AddForce(new Vector3(5, 13, 0), ForceMode.Impulse);
            else if (next_dir == -1) rb.AddForce(new Vector3(-5, 13, 0), ForceMode.Impulse);
            else if (next_dir == 2) rb.AddForce(new Vector3(0, 13, 5), ForceMode.Impulse);
            else if (next_dir == -2) rb.AddForce(new Vector3(0, 13, -5), ForceMode.Impulse);
            StartCoroutine(TranslateAfterDelay());
        }
    }
    private IEnumerator TranslateAfterDelay()
    {
        // Espera 3 segundos
        yield return new WaitForSeconds(3f);

        audioSource.PlayOneShot(deathSound);
        Game.Instance.death(my_map.get_percentage());
    }    
}
