using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] videurSprites;
    
    [Header("Animation")]
    [SerializeField] private float frameDuration = 0.15f; // Vitesse de changement de frame
    private float animationTimer = 0f;
    private int currentFrame = 0;
    
    [Header("Paramètres de déplacement")]
    public float moveSpeed = 5f;

    private Timer timer;
    private bool dansLeBar;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = new Timer();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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

        MettreAJourSprite(movementInput);

        if (dansLeBar)
            timer.MettreAJour(Time.deltaTime);
            
        if (dansLeBar && timer.Seconde >= 2)
            Debug.Log("le temp es passer LOL");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void MettreAJourSprite(Vector2 direction)
    {
        if (videurSprites == null || videurSprites.Length == 0 || spriteRenderer == null) return;

        if (direction == Vector2.zero)
        {
            currentFrame = 0;
            animationTimer = 0f;
        }
        else
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= frameDuration)
            {
                animationTimer = 0f;
                currentFrame = (currentFrame + 1) % 4;
            }
        }

        if (direction.y > 0)
        {
            spriteRenderer.sprite = videurSprites[16 + currentFrame];
            spriteRenderer.flipX = false;
        }
        else if (direction.y < 0)
        {
            spriteRenderer.sprite = videurSprites[1 + currentFrame];
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.sprite = videurSprites[11 + currentFrame];
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.sprite = videurSprites[6 + currentFrame];
            spriteRenderer.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            Debug.Log("player entrer danstokenizer Bar !!!");
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
            dansLeBar = false;
            timer.Stop();
        }
    }
}