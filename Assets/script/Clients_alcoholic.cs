using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Clients_alcoholic : Clients
{
    [SerializeField] private Timer timer;
    [SerializeField] private Transform cible;

    [Header("Mouvement normal")]
    [SerializeField] private float vitesseNormale = 1.5f;
    [SerializeField] private float rayonNormal = 4f;
    [SerializeField] private float attenteNormaleMin = 1f;
    [SerializeField] private float attenteNormaleMax = 3f;

    [Header("Mouvement énervé")]
    [SerializeField] private float vitesseEnervee = 4f;
    [SerializeField] private float rayonEnervee = 6f;
    [SerializeField] private float attenteEnerveeMax = 0.5f;

    public string status = "normal";

    private Rigidbody2D rb;
    private Vector2 centre;
    private Vector2 destination;
    private float attente = 0f;
    private bool dansLeBar = false;

    void Awake()
    {
        comportement = "alcoholic";
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        centre = rb.position;
        ChoisirDestination(rayonNormal);
    }

    void Update()
    {
        if (dansLeBar && status == "normal" && timer.Minute >= 4)
            status = "pissed";
    }

    void FixedUpdate()
    {
        Action();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bar"))
        {
            Debug.Log("entrer dans le Bar !!!");
            dansLeBar = true;
            timer.Reset();
            timer.Demarrer();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bar"))
        {
            dansLeBar = false;
            timer.Stop();
        }
    }

    void Action()
    {
        if (status == "pissed")
            pissedMove();
        else if (status == "normal")
            normalMove();
    }

    void normalMove()
    {
        Deplacer(vitesseNormale, rayonNormal, attenteNormaleMin, attenteNormaleMax);
    }

    void pissedMove()
    {
        if (cible != null)
        {
            Vector2 pos = Vector2.MoveTowards(rb.position, cible.position, vitesseEnervee * Time.fixedDeltaTime);
            rb.MovePosition(pos);
            return;
        }

        Deplacer(vitesseEnervee, rayonEnervee, 0f, attenteEnerveeMax);
    }

    void Deplacer(float vitesse, float rayon, float attenteMin, float attenteMax)
    {
        if (attente > 0f)
        {
            attente -= Time.fixedDeltaTime;
            return;
        }

        Vector2 pos = Vector2.MoveTowards(rb.position, destination, vitesse * Time.fixedDeltaTime);
        rb.MovePosition(pos);

        if (Vector2.Distance(rb.position, destination) < 0.1f)
        {
            attente = Random.Range(attenteMin, attenteMax);
            ChoisirDestination(rayon);
        }
    }

    void ChoisirDestination(float rayon)
    {
        destination = centre + Random.insideUnitCircle * rayon;
    }
}