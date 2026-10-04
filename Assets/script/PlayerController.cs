using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Timer timer;
    private bool dansLeBar;
    [Header("Paramètres de déplacement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = new Timer();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX += 1f;
            if (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX -= 1f;

            if (Keyboard.current.zKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY -= 1f;
        }
        movementInput = new Vector2(moveX, moveY).normalized;
        if (dansLeBar)
            timer.MettreAJour(Time.deltaTime);
        if (dansLeBar && timer.Seconde >= 2)
            Debug.Log("le temp es passer LOL");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            Debug.Log("player entrer dans le Bar !!!");
            dansLeBar = true;
            timer.Reset();
            timer.Demarrer();
            timer.GetTime();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            timer.Stop();
        }
    }
}