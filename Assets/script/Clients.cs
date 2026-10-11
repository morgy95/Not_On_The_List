using System.Collections.Generic;
using Unity.Hierarchy.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class Clients : MonoBehaviour
{
    private const float espacementFile = 0.6f;
    private static readonly List<Clients> clientsEnFile = new List<Clients>();

    [SerializeField] private GameObject objetHover;
    [SerializeField] private Chargeur chargeur;
    [SerializeField] private CarteIdentite carteIdentite;

    [Header("Infos client")]
    public string comportement;
    public string prenom;
    public string nom;
    public int age;
    public string sexe;
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
    [SerializeField] private float vitesseDansBoite = 1.5f;
    [SerializeField] private float attenteDansBoiteMin = 1f;
    [SerializeField] private float attenteDansBoiteMax = 3f;

    private bool choixFait;
    private bool enFile = false;
    private int positionDansFile = -1;
    private Vector3? ciblePosition;
    private TypeCible typeCible = TypeCible.AvantFile;
    private Collider2D zoneBoite;
    private float attenteDansBoite;

    [Header("Hover")]
    [SerializeField] private SpriteRenderer hoverRenderer;
    [SerializeField] private Color couleurHover = new Color(1f, 0.9f, 0.3f, 0.9f);
    [SerializeField] private float echelleHover = 1.12f;
    private bool survole = false;

    [Header("Animation")]
    [SerializeField] private float frameDuration = 0.15f;
    private float animationTimer = 0f;
    private int currentFrame = 0;
    private Vector3 dernierePosition;
    private Vector2 derniereDirection = Vector2.down;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private enum TypeCible
    {
        AvantFile,
        EnFile,
        Sortie,
        EntreeBoite,
        DansBoite
    }

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

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (hoverRenderer != null)
        {
            hoverRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
            hoverRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            hoverRenderer.color = couleurHover;
            hoverRenderer.transform.localScale = Vector3.one * echelleHover;
            hoverRenderer.enabled = false;
        }

        prenom = chargeur.PrenomAleatoire();
        nom = chargeur.NomAleatoire();
        age = chargeur.AgeAleatoire();
        sexe = chargeur.SexeAleatoire();
        metier = chargeur.MetierAleatoire();
        adresse = chargeur.AdressesAleatoire();
        description = chargeur.DescriptionsAleatoire();
        nomClient = prenom + " " + nom;
        sprites = chargeur.SpritesAleatoire(sexe);
        Debug.Log("prenom = " + prenom + " nom = " + nom);

        if (waitZone == null)       waitZone       = GameObject.Find("WaitZone");
        if (beforWaitZone == null)  beforWaitZone  = GameObject.Find("BeforeWaitZone");
        if (endZone == null)        endZone        = GameObject.Find("endZone");
        GameObject bar = GameObject.FindGameObjectWithTag("Bar");
        if (bar != null)
        {
            zoneBoite = bar.GetComponent<Collider2D>();
            if (zoneBoite == null)
                Debug.LogError("L'objet tagué 'Bar' doit avoir un Collider2D trigger.", bar);
            else if (!zoneBoite.isTrigger)
                Debug.LogError("Le Collider2D de l'objet 'Bar' doit être configuré en trigger.", bar);
        }
        else
        {
            Debug.LogError("Aucun objet avec le tag 'Bar' n'a été trouvé.", this);
        }

        Queu();
        if (carteIdentite == null)
            carteIdentite = FindFirstObjectByType<CarteIdentite>();

        if (carteIdentite == null)
            Debug.LogError("Aucune CarteIdentite trouvée dans la scène !", this);
    }

    void OnMouseEnter()
    {
        survole = true;
        if (hoverRenderer != null)
            hoverRenderer.enabled = true;
    }

    void OnMouseExit()
    {
        survole = false;
        if (hoverRenderer != null)
            hoverRenderer.enabled = false;
    }

    void OnMouseDown()
    {
        string[] str = { prenom, nom, age.ToString(), metier, adresse, description };

        Debug.Log("prenom = " + prenom + " nom = " + nom + " age = " + age +
                  " metier = " + metier + " adresse = " + adresse +
                  " description = " + description);

        if (carteIdentite != null)
            carteIdentite.SelectionnerClient(this, str);
    }

    public void Queu()
    {
        if (waitZone == null || beforWaitZone == null || endZone == null)
        {
            Debug.LogError("Une des zones (wait / beforWait / end) n'est pas assignée !");
            return;
        }

        choixFait = false;
        enFile = false;
        positionDansFile = -1;
        typeCible = TypeCible.AvantFile;
        ciblePosition = beforWaitZone.transform.position;
    }

    protected virtual void Update()
    {
        Vector2 delta = transform.position - dernierePosition;
        dernierePosition = transform.position;

        MettreAJourSprite(delta);

        if (typeCible == TypeCible.DansBoite)
        {
            MettreAJourDeplacementDansBoite();
            return;
        }

        if (ciblePosition.HasValue)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                ciblePosition.Value,
                vitesse * Time.deltaTime);

            if (Vector2.Distance(transform.position, ciblePosition.Value) > distanceArrivee)
                return;

            ciblePosition = null;
        }

        if (typeCible == TypeCible.AvantFile)
        {
            JoinQueu();
        }
        else if (typeCible == TypeCible.EnFile)
        {
            return;
        }
        else if (typeCible == TypeCible.Sortie)
        {
            Despawn();
        }
        else if (typeCible == TypeCible.EntreeBoite && zoneBoite != null)
        {
            if (zoneBoite.OverlapPoint(transform.position))
                CommencerDeplacementDansBoite();
            else
                ciblePosition = zoneBoite.bounds.center;
        }
    }

    private void JoinQueu()
    {
        if (enFile)
            return;

        enFile = true;
        typeCible = TypeCible.EnFile;
        clientsEnFile.Add(this);
        positionDansFile = clientsEnFile.Count - 1;
        ciblePosition = GetQueuePosition(positionDansFile);
        RefreshQueuePositions();
        Debug.Log("A rejoint la queue");
    }

    private Vector3 GetQueuePosition(int index)
    {
        if (waitZone == null)
            return transform.position;

        Vector3 direction = waitZone.transform.right;
        return waitZone.transform.position + direction * (index * espacementFile);
    }

    private static void RefreshQueuePositions()
    {
        for (int i = 0; i < clientsEnFile.Count; i++)
        {
            Clients client = clientsEnFile[i];
            client.positionDansFile = i;

            if (client.enFile && client.typeCible == TypeCible.EnFile)
            {
                client.ciblePosition = client.GetQueuePosition(i);
            }
        }
    }

    public void QuitterFile()
    {
        if (!enFile) return;

        RetirerDeFile();
        typeCible = TypeCible.Sortie;
        ciblePosition = endZone.transform.position;
        choixFait = true;

        RefreshQueuePositions();
    }

    public void Accepter()
    {
        if (choixFait)
            return;

        if (zoneBoite == null)
        {
            Debug.LogError("Impossible d'admettre le client : le trigger 'Bar' n'est pas configuré.", this);
            return;
        }

        choixFait = true;
        RetirerDeFile();
        typeCible = TypeCible.EntreeBoite;

        if (zoneBoite.OverlapPoint(transform.position))
            CommencerDeplacementDansBoite();
        else
            ciblePosition = zoneBoite.bounds.center;
    }

    public void Refuser()
    {
        if (choixFait)
            return;

        if (endZone == null)
        {
            Debug.LogError("Impossible de refuser le client : la zone de sortie n'est pas configurée.", this);
            return;
        }

        choixFait = true;
        RetirerDeFile();
        typeCible = TypeCible.Sortie;
        ciblePosition = endZone.transform.position;
    }

    private void RetirerDeFile()
    {
        if (!enFile)
            return;

        clientsEnFile.Remove(this);
        enFile = false;
        positionDansFile = -1;
        RefreshQueuePositions();
    }

    public static void ServirPremier()
    {
        if (clientsEnFile.Count > 0)
            clientsEnFile[0].QuitterFile();
    }
    public void Despawn()
    {
        RetirerDeFile();
        if (carteIdentite != null)
            carteIdentite.DeselectionnerClient(this);

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar"))
        {
            zoneBoite = other;
            if (choixFait && typeCible == TypeCible.EntreeBoite)
                CommencerDeplacementDansBoite();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bar") && typeCible == TypeCible.DansBoite)
        {
            typeCible = TypeCible.EntreeBoite;
            ciblePosition = zoneBoite != null ? zoneBoite.bounds.center : transform.position;
        }
    }

    private void CommencerDeplacementDansBoite()
    {
        if (!choixFait || zoneBoite == null)
            return;

        typeCible = TypeCible.DansBoite;
        attenteDansBoite = 0f;
        ciblePosition = ChoisirDestinationDansBoite();
    }

    private void MettreAJourDeplacementDansBoite()
    {
        if (attenteDansBoite > 0f)
        {
            attenteDansBoite -= Time.deltaTime;
            return;
        }

        if (!ciblePosition.HasValue ||
            Vector2.Distance(transform.position, ciblePosition.Value) <= distanceArrivee)
        {
            attenteDansBoite = Random.Range(attenteDansBoiteMin, attenteDansBoiteMax);
            ciblePosition = ChoisirDestinationDansBoite();
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            ciblePosition.Value,
            vitesseDansBoite * Time.deltaTime);
    }

    private Vector2 ChoisirDestinationDansBoite()
    {
        Bounds bounds = zoneBoite.bounds;
        for (int i = 0; i < 20; i++)
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y));

            if (zoneBoite.OverlapPoint(point))
                return point;
        }

        return zoneBoite.ClosestPoint(bounds.center);
    }

    protected virtual void OnDestroy()
    {
        RetirerDeFile();
        if (carteIdentite != null)
            carteIdentite.DeselectionnerClient(this);
    }

    private void MettreAJourSprite(Vector2 delta)
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null) return;

        bool enMouvement = delta.sqrMagnitude > 0.00001f;

        if (enMouvement)
        {
            derniereDirection = delta.normalized;

            animationTimer += Time.deltaTime;
            if (animationTimer >= frameDuration)
            {
                animationTimer = 0f;
                currentFrame = (currentFrame + 1) % 4;
            }
        }
        else
        {
            currentFrame = 0;
            animationTimer = 0f;
        }

        AppliquerSprite(derniereDirection, currentFrame);
        if (survole && hoverRenderer != null)
        {
            hoverRenderer.sprite = spriteRenderer.sprite;
            hoverRenderer.flipX = spriteRenderer.flipX;
            hoverRenderer.flipY = spriteRenderer.flipY;
        }
    }

    private void AppliquerSprite(Vector2 direction, int frame)
    {
        int debut;

        if (Mathf.Abs(direction.y) >= Mathf.Abs(direction.x))
            debut = direction.y > 0 ? 16 : 1;
        else
            debut = direction.x > 0 ? 11 : 6;

        int index = debut + frame;
        if (index < sprites.Length)
            spriteRenderer.sprite = sprites[index];
    }
}