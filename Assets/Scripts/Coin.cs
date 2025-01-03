using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private Vector3 initialScale = new Vector3(10, 10, 10);
    [SerializeField] private Vector3 initialRotation = new Vector3(90, 0, 0);
    [SerializeField] private bool useGravity = false;

    public GameObject CreateCoin(Vector3 position)
    {
        if (coin == null)
        {
            Debug.LogError("Coin prefab no asignado en el Inspector.");
            return null;
        }
        GameObject newCoin = Instantiate(coin, position, Quaternion.Euler(initialRotation));
        newCoin.transform.localScale = initialScale;

        BoxCollider boxColliderCoin = newCoin.AddComponent<BoxCollider>();
        boxColliderCoin.isTrigger = true;

        Rigidbody rb = newCoin.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
        newCoin.AddComponent<RotationObjects>();


        newCoin.tag = "coin";
        return newCoin;
    }
}


