using UnityEngine;

public class Barrote : MonoBehaviour
{

    public float rotationSpeed = 5.0f; // Velocidad de rotación en grados por segundo

    // Update is called once per frame
    void Update()
    {
        //newBarrote.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
    }
    private GameObject newBarrote;
    [SerializeField] private GameObject barrote_p;
    [SerializeField] private Material barroteMaterial;
    [SerializeField] private Material barroteMaterial2;
    private Vector3 initialScale = new Vector3(1f,0.5f, 1f);
    [SerializeField] private bool useGravity = true;

    void Start()
    {   
        if (barrote_p != null)
        {
            //CreateBarrote(new Vector3(-3, 0, 3));
        }
        else
        {
            Debug.LogError("Barrote no asignado");
        }
    }

    public GameObject CreateBarrote(Vector3 position)
    {
        Vector3 new_position = new Vector3(position.x + 0.3f, position.y, position.z - 0.5f);
        newBarrote = Instantiate(barrote_p, new_position, Quaternion.identity);
        newBarrote.transform.localScale = initialScale;

        // Añadir un BoxCollider sin habilitar el modo trigger
        BoxCollider boxColliderBarrote = newBarrote.AddComponent<BoxCollider>();
        boxColliderBarrote.isTrigger = true;


        // Configurar Rigidbody con detección de colisiones y gravedad
        Rigidbody rb = newBarrote.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        

        /*Renderer renderer = newBarrote.GetComponent<Renderer>();
        renderer.material = barroteMaterial;*/
        Transform part1 = newBarrote.transform.Find("O20 Part 1");
        Transform part2 = newBarrote.transform.Find("O20 Part 2");

        if (part1 != null && part2 != null)
        {
            Debug.Log("FINDDDDD");
            // Asigna el material a cada parte
            Renderer rendererPart1 = part1.GetComponent<Renderer>();
            Renderer rendererPart2 = part2.GetComponent<Renderer>();

            if (rendererPart1 != null)
            {
                rendererPart1.material = barroteMaterial; // Asigna el material a 020 Part1
            }

            if (rendererPart2 != null)
            {
                rendererPart2.material = barroteMaterial2; // Asigna el material a 020 Part2
            }
        }

        newBarrote.tag = "barrote";
        newBarrote.AddComponent<RotationObjects>();
        return newBarrote;
    }
}