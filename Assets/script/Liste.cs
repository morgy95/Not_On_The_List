using UnityEngine;
using TMPro;

// Renommé TextListManager car "Liste" peut entrer en conflit avec des collections C#
public class Liste : MonoBehaviour
{
    [SerializeField] private GameObject listePanel;
    public GameObject panelContent;
    public GameObject textList;
    public Chargeur chargeur;


    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 positionOuverte;
    [SerializeField] private Vector2 positionFermee = new Vector2(0, -800f);

    private bool estOuvert = false;
    private void Awake()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = positionFermee;
        estOuvert = false;
    }
    void Start()
    {
        if (chargeur == null)
            chargeur = FindFirstObjectByType<Chargeur>();

        if (chargeur == null)
        {
            Debug.LogError("Aucun Chargeur n'a été assigné ou trouvé dans la scène !", this);
            return;
        }

        if (textList == null)
        {
            Debug.LogError("Le prefab textList n'a pas été assigné dans l'Inspector !", this);
            return;
        }

        string[] nameList = chargeur.CreateListe(10);
        Debug.Log(string.Join(", ", nameList));

        for (int i = 0; i < nameList.Length; i++)
        {
            Vector2 position = new Vector2(19.7f, 117.9561f - i * 34f);
            GameObject nouvelObjet = CreerObjet(position);

            if (nouvelObjet == null)
                continue;

            TextMeshProUGUI texteTMP = nouvelObjet.GetComponent<TextMeshProUGUI>();
            if (texteTMP != null)
            {
                texteTMP.text =  nameList[i];
            }
            else
            {
                Debug.LogWarning("Le composant TextMeshProUGUI n'a pas été trouvé sur le prefab !", nouvelObjet);
            }
        }
    }

    GameObject CreerObjet(Vector2 position)
    {
        Transform parentCible = panelContent != null ? panelContent.transform : transform;

        GameObject nouvelObjet = Instantiate(textList, position, Quaternion.identity, parentCible);
        
        RectTransform rectTransform = nouvelObjet.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }

        nouvelObjet.name = "ElementList_" + position;
        return nouvelObjet;
    }

    public void OpenCloseListe()
    {
        estOuvert = !estOuvert;

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = estOuvert ? positionOuverte : positionFermee;
        }
    }
}