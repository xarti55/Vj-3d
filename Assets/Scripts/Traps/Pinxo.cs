using UnityEngine;

public class Pinxo : MonoBehaviour
{
    [SerializeField] private GameObject pinxo_p;
    [SerializeField] private Material pinxoMaterial;
    private Vector3 initialScale = new Vector3(1.8f,2,1.8f);
    [SerializeField] private bool useGravity = true;

    private Vector3 front = new Vector3(-90, 0, 0);
    private Vector3 back = new Vector3(0, 0, -180);
    private Vector3 left = new Vector3(0, 0, 90);
    private Vector3 right = new Vector3(0, 0, -90);


    void Start()
    {   
        if (pinxo_p != null)
        {
            //CreatePinxo(new Vector3(3, 0.25f, 3), false, "LEFT");
        }
        else
        {
            Debug.LogError("Pinxo no asignado");
        }
    }

    public GameObject CreatePinxo(Vector3 position, bool vertical, string direccion)
    {
        Vector3 aux_pos;
        if (vertical) aux_pos = new Vector3(position.x + 0.2f, position.y, position.z -0.3f);
        else aux_pos = new Vector3(position.x + 0.4f, position.y, position.z -0.4f);
        GameObject newPinxo = Instantiate(pinxo_p, aux_pos, Quaternion.identity);
        newPinxo.transform.localScale = initialScale;

        // Añadir un BoxCollider sin habilitar el modo trigger
        BoxCollider boxColliderPinxo = newPinxo.AddComponent<BoxCollider>();
        boxColliderPinxo.isTrigger = true;


        // Configurar Rigidbody con detección de colisiones y gravedad
        Rigidbody rb = newPinxo.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        

        Renderer renderer = newPinxo.GetComponent<Renderer>();
        renderer.material = pinxoMaterial;

        newPinxo.tag = "pinxo";

        if(vertical){
            newPinxo.transform.Rotate(front);
            Vector3 currentPosition = newPinxo.transform.position;
            newPinxo.transform.position = new Vector3(currentPosition.x, currentPosition.y + 0.25f, currentPosition.z);
            if(direccion == "LEFT"){
                newPinxo.transform.Rotate(left);
                Debug.Log("Izquierdaa");

            }
            else if (direccion == "RIGHT"){
                newPinxo.transform.Rotate(right);
                Debug.Log("Derechaaa");


            }
            else if ( direccion == "BACK"){
                newPinxo.transform.Rotate(back);
                Debug.Log("Baccck");
            }
        }
        return newPinxo;
    }
}