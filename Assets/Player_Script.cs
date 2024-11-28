using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public float movement_speed = 4.0f;

    private Map my_map;

    void Start()
    {
        my_map = Map.Instance; 
    }

    void Update()
    {
        // Basic movement logic
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * movement_speed);
        Debug.Log(transform.position);
    }
}
