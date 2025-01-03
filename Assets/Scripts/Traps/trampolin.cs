using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private GameObject trampolin_p;
    [SerializeField] private Material trampolinMaterial;
    private Vector3 initialScale = new Vector3(2, 4, 2);
    [SerializeField] private bool useGravity = true;

    void Start()
    {   
        if (trampolin_p != null)
        {
            //CreateTrampolin(new Vector3(0, 0.25f, 3));
        }
        else
        {
            Debug.LogError("Trampolin no asignado");
        }
    }

    public GameObject CreateTrampolin(Vector3 position)
    {

        Vector3 new_position = new Vector3(position.x, position.y, position.z);
        GameObject newTrampolin = Instantiate(trampolin_p, new_position, Quaternion.identity);
        newTrampolin.transform.localScale = initialScale;

        // Añadir un BoxCollider sin habilitar el modo trigger
        BoxCollider boxColliderTrampolin = newTrampolin.AddComponent<BoxCollider>();
        boxColliderTrampolin.isTrigger = true;


        // Configurar Rigidbody con detección de colisiones y gravedad
        Rigidbody rb = newTrampolin.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        

        Renderer renderer = newTrampolin.GetComponent<Renderer>();
        renderer.material = trampolinMaterial;

        newTrampolin.tag = "trampolin";
        return newTrampolin;
    }
}
