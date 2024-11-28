using UnityEngine;

public class Player_Script : MonoBehaviour
{
   public float movement_speed = 4.0f;
   int points = 0;
   public UI my_ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualEfect;
        Label my_label = root.Q<Label>("Label_points");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal == 1) ++points;
        my_label.text = "aaa";
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0.0f, vertical) *Time.deltaTime * movement_speed);
    }
}
