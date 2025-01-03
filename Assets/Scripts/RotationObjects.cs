using UnityEngine;

public class RotationObjects : MonoBehaviour
{


    public float rotationSpeed = 120.0f; // Velocidad de rotación en grados por segundo


    void Update()
    {
        if (this.tag == "barrote")
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        else
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}