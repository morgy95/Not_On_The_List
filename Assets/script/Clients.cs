using UnityEngine;

public class Clients : MonoBehaviour
{
    [SerializeField] private GameObject objetHover;
    [SerializeField] private Chargeur chargeur;
    [SerializeField] private CarteIdentite carteIdentite;

    [Header("Infos client")]
    public string comportement;
    public string prenom;
    public string nom;
    public int age;
    public string metier;
    public string adresse;
    public string description;
    public string nomClient;

    [Header("Zones")]
    public GameObject waitZone;
    public GameObject beforWaitZone;
    public GameObject endZone;

    [Header("Déplacement")]
    [SerializeField] private float vitesse = 3f;
    [SerializeField] private float distanceArrivee = 0.05f;

    private bool choixFait = false;
    private bool aAtteintDebutQueue = false;
    private Transform cible;
    private Timer timer;

    protected virtual void Start()
    {
        if (objetHover != null)
        {
            SpriteRenderer srHover = objetHover.GetComponent<SpriteRenderer>();
            if (srHover != null)
                srHover.sortingOrder = -1;
            objetHover.SetActive(false);
        }

        if (chargeur == null)
            chargeur = FindFirstObjectByType<Chargeur>();

        if (chargeur == null)
        {
            Debug.LogError("Aucun Chargeur dans la scène !");
            return;
        }

        prenom = chargeur.PrenomAleatoire();
        nom = chargeur.NomAleatoire();
        age = chargeur.AgeAleatoire();
        metier = chargeur.MetierAleatoire();
        adresse = chargeur.AdressesAleatoire();
        description = chargeur.DescriptionsAleatoire();
        nomClient = prenom + " " + nom;
        Debug.Log("prenom = " + prenom + " nom = " + nom);

        timer = new Timer();
        if (waitZone == null)       waitZone       = GameObject.Find("WaitZone");
        if (beforWaitZone == null)  beforWaitZone  = GameObject.Find("BeforeWaitZone");
        if (endZone == null)        endZone        = GameObject.Find("endZone");
        Queu();
        if (carteIdentite == null)
            carteIdentite = FindFirstObjectByType<CarteIdentite>();

        if (carteIdentite == null)
            Debug.LogError("Aucune CarteIdentite trouvée dans la scène !", this);
    }

    void OnMouseEnter()
    {
        if (objetHover != null)
            objetHover.SetActive(true);
    }

    void OnMouseExit()
    {
        if (objetHover != null)
            objetHover.SetActive(false);
    }

    void OnMouseDown()
    {
        string[] str = { prenom, nom, age.ToString(), metier, adresse, description };

        Debug.Log("prenom = " + prenom + " nom = " + nom + " age = " + age +
                  " metier = " + metier + " adresse = " + adresse +
                  " description = " + description);

        carteIdentite.MiseAJour(str);
    }

    public void Queu()
    {
        if (waitZone == null || beforWaitZone == null || endZone == null)
        {
            Debug.LogError("Une des zones (wait / beforWait / end) n'est pas assignée !");
            return;
        }

        choixFait = false;
        aAtteintDebutQueue = false;
        cible = beforWaitZone.transform;
    }

    void Update()
    {
        if (cible == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            cible.position,
            vitesse * Time.deltaTime);

        if (Vector2.Distance(transform.position, cible.position) > distanceArrivee)
            return;

        if (cible == beforWaitZone.transform && !choixFait)
        {
            aAtteintDebutQueue = true;
            Debug.Log("A atteint le début de la queue");
            choixFait = true;

            int random = Random.Range(0, 2);
            Debug.Log("Le nombre = " + random);

            if (random == 1)
            {
                JoinQueu();
            }
            else
            {
                cible = endZone.transform;
            }
        }
        else
        {
            cible = null;
        }
        if (Vector2.Distance(endZone.transform.position, transform.position) <= 0.1f)
        {
            Despawn();
            Debug.Log("A rejoint les cieux");
        }
    }

    private void JoinQueu()
    {
        cible = waitZone.transform;
        Debug.Log("A rejoint la queue");
    }

    public void Despawn()
    {
        Debug.Log("Fin de VIe");
        Destroy(gameObject);
    }
}