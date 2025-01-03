using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadBird : MonoBehaviour
{
    public GameObject birdPrefabON;
    public GameObject birdPrefabOFF;

    public RuntimeAnimatorController birdAnimatorControllerON;
    public RuntimeAnimatorController birdAnimatorControllerOFF;

    public Material birdMaterial;

    private Transform player;
    public float activationDistance = 3f;

    private List<GameObject> birdInstancesON = new List<GameObject>();
    private List<GameObject> birdInstancesOFF = new List<GameObject>();

    private bool ready, active;

    void Start()
    {
        ready = false;
        active = false;
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        while (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                ready = true;
            }
            yield return null;
        }


    }

void Update()
{
    for (int i = 0; i < birdInstancesOFF.Count; i++)
    {
        // Verificar que las referencias no sean nulas
        if (birdInstancesOFF[i] != null && birdInstancesON[i] != null)
        {
            // Calcular la distancia entre el jugador y el pájaro OFF
            float distance = Vector3.Distance(player.position, birdInstancesOFF[i].transform.position);

            // Si está dentro del rango de activación, activar el pájaro ON
            if (distance < activationDistance && !birdInstancesON[i].activeSelf)
            {
                birdInstancesOFF[i].SetActive(false);
                birdInstancesON[i].SetActive(true);

                // Iniciar la animación del pájaro ON
                Animator birdAnimator = birdInstancesON[i].GetComponent<Animator>();
                if (birdAnimator != null)
                {
                    birdAnimator.Play("FlyAnimation"); // Cambiar por el nombre correcto de la animación
                }
            }
        }
    }

    // Mover hacia adelante todos los pájaros activos (ON)
    foreach (var birdON in birdInstancesON)
    {
        if (birdON != null && birdON.activeSelf)
        {
            MoveBirdForward(birdON);
        }
    }
}

    void MoveBirdForward(GameObject bird)
    {
        float moveSpeed = 5f; // Velocidad de movimiento
        bird.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void LoadBirdModels(Vector3 position, string direction)
    {
        GameObject birdON = Instantiate(birdPrefabON, position, Quaternion.identity);
        GameObject birdOFF = Instantiate(birdPrefabOFF, position, Quaternion.identity);

        if (direction == "Right")
        {
            birdON.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == "Left")
        {
            birdON.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == "Back")
        {
            birdON.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        Animator animatorOFF = birdOFF.AddComponent<Animator>();
        animatorOFF.runtimeAnimatorController = birdAnimatorControllerOFF;

        // Configurar materiales para el pájaro ON
        Transform part1 = birdON.transform.Find("Bird_3D_On_Wing");
        Transform part2 = birdON.transform.Find("Bird_3D_On_Wing 1");
        Transform part3 = birdON.transform.Find("Body");

        if (part1 != null) part1.GetComponent<Renderer>().material = birdMaterial;
        if (part2 != null) part2.GetComponent<Renderer>().material = birdMaterial;
        if (part3 != null) part3.GetComponent<Renderer>().material = birdMaterial;

        // Configurar material para el pájaro OFF
        Renderer renderer2 = birdOFF.GetComponent<Renderer>();
        if (renderer2 != null) renderer2.material = birdMaterial;

        birdON.transform.localScale = new Vector3(2, 2, 2);
        birdOFF.transform.localScale = new Vector3(2, 2, 2);

        birdON.SetActive(false);

        birdInstancesON.Add(birdON);
        birdInstancesOFF.Add(birdOFF);
    }


    // Función para borrar un pájaro en una posición específica
    public void RemoveBirdAt(int index)
    {
        if (index >= 0 && index < birdInstancesON.Count)
        {
            if (birdInstancesON[index] != null)
            {
                Destroy(birdInstancesON[index]);
            }
            birdInstancesON.RemoveAt(index);
        }

        if (index >= 0 && index < birdInstancesOFF.Count)
        {
            if (birdInstancesOFF[index] != null)
            {
                Destroy(birdInstancesOFF[index]);
            }
            birdInstancesOFF.RemoveAt(index);
        }
    }

    // Función para obtener la lista de pájaros ON
    public List<GameObject> GetBirdsON()
    {
        return new List<GameObject>(birdInstancesON);
    }

    // Función para obtener la lista de pájaros OFF
    public List<GameObject> GetBirdsOFF()
    {
        return new List<GameObject>(birdInstancesOFF);
    }
}