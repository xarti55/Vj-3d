using UnityEngine;

public class Player_Script : MonoBehaviour
{
    public float movement_speed = 0.01f;

    private Map my_map;
    Rigidbody rb;

    void Start()
    {
        my_map = Map.Instance; 
        my_map.load_lvl_stage(1,0);
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.MovePosition(new Vector3(0,1,0));
    }

    void Update()
    {
        // Basic movement logic
        float horizontal = 0; 
        float vertical = 0; 
        int next_dir = my_map.player_next_move(rb.position);
        if (next_dir == -1) horizontal = -1;
        else if (next_dir == 1) horizontal = 1;
        else if (next_dir == -2) vertical = -1;
        else if (next_dir == 2) vertical = 1;
        Vector3 movement = new Vector3(horizontal, 0 , vertical ) * movement_speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
