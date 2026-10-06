using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Clients_alcoholic : Clients
{
     private Timer timer;
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
    }

    void Start()
    {
        base.Start();
        timer = new Timer();
    }

    void Update()
    {
        Queu();
        if (dansLeBar)
            timer.MettreAJour(Time.deltaTime);
        if (dansLeBar && status == "normal" && timer.Minute >= 4)
            status = "pissed";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            Debug.Log("entrer dans le Bar !!!");
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