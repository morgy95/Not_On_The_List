using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    
    [Header("Réglages de la Caméra")]
    [SerializeField] private float smoothness = 5f; 
    
    [SerializeField] private Vector2 offset = new Vector2(0f, 0f); 

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            float cibleX = player.transform.position.x + offset.x;
            float cibleY = player.transform.position.y + offset.y;

            Vector3 positionCible = new Vector3(cibleX, cibleY, transform.position.z);
            
            transform.position = Vector3.Lerp(transform.position, positionCible, smoothness * Time.deltaTime);
        }
    }
}
