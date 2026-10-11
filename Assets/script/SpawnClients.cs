using UnityEngine;
using UnityEngine.Rendering;

public class SpawnClients : MonoBehaviour
{
    public GameObject clients;
    private Timer timer;
    void Start()
    {
        if (clients == null)
        {
            Debug.Log("Aucun client n'a été assigné ou trouvé dans la scène !", this);
            return;
        }
        timer = new Timer();
    }

    void Update()
    {
        timer.Demarrer();
        timer.MettreAJour(Time.deltaTime);
        if (timer.Seconde == Random.Range(2, 5))
        {
            timer.Stop();
            timer.Reset();
            GameObject newObject = Instantiate(clients, transform.position, Quaternion.identity);
        }
    }
}
