using System;
using TMPro;
using Unity.Tutorials.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CarteIdentite : MonoBehaviour
{
    [SerializeField] private TMP_Text nom;
    [SerializeField] private TMP_Text prenom;
    [SerializeField] private TMP_Text age;
    [SerializeField] private TMP_Text metier;
    [SerializeField] private Liste list;
    [SerializeField] private TMP_Text MonneyText; 
    [SerializeField] private TMP_Text date; 

    [Header("Button GUI")]
    [SerializeField] private Image imageTargetAccept;
    [SerializeField] private Image imageTargetRefuse;
    [SerializeField] private Sprite[] buttonSprite;
    [SerializeField] private Button buttonRefuse;
    [SerializeField] private Button buttonAccepter;

    public Button buttonList;
    public GameObject testNameList;

    private Clients clientSelectionne;

    private Chargeur chargeur;
    public double monney = 0;
    private int reputaion = 0;
    private int nombreMois = 0;
    [SerializeField] private Timer timer;        
    private void Start()
    {
        if (buttonList != null)
        {
            buttonList.onClick.AddListener(OnButtonClicked);
        }

        if (buttonAccepter != null)
        {
            buttonAccepter.onClick.AddListener(AccepteEvent);
        }

        if (buttonRefuse != null)
        {
            buttonRefuse.onClick.AddListener(RefuseEvent);
        }

        timer = new Timer();
        timer.Demarrer();

        if (nom != null) nom.text = "";
        if (prenom != null) prenom.text = "";
        if (age != null) age.text = "";
        if (metier != null) metier.text = "";
        MettreBoutonsActifs(false);
    }

    public void Update()
    {
        timer.MettreAJourGame(Time.deltaTime);
        if (timer.Mois % 1 == 0 && timer.Mois - 1 == nombreMois)
        {
            monney -= 200 * timer.Mois;
            nombreMois = timer.Mois;
        }
        UpdateTime();
        if (MonneyText != null)
        {
            MonneyText.text = $"monney : {monney}";
        }
    }

    public void UpdateTime()
    {
        int[] tim= timer.GetTime();
        if (date != null)
        {
            date.text = $"{tim[5]}year {tim[4]}month {tim[3]}day {tim[2]}hour";
        }
    }

    public void MiseAJour(string[] str)
    {
        if (str == null || str.Length < 4)
        {
            Debug.LogWarning("MiseAJour : tableau invalide ou incomplet (minimum 4 éléments attendus)", this);
            return;
        }

        if (prenom != null) prenom.text = "Prénom : " + str[0];
        else Debug.LogError("prenom non assigné dans l'Inspector", this);

        if (nom != null) nom.text = "Nom : " + str[1];
        else Debug.LogError("nom non assigné dans l'Inspector", this);

        if (age != null) age.text = "Âge : " + str[2];
        else Debug.LogError("age non assigné dans l'Inspector", this);

        if (metier != null) metier.text = "Métier : " + str[3];
        else Debug.LogError("metier non assigné dans l'Inspector", this);
    }

    public void OnButtonClicked()
    {
        if (list != null)
        {
            list.OpenCloseListe();
        }
        else
        {
            Debug.LogError("La variable 'list' n'est pas assignée dans CarteIdentite !", this);
        }
    }

    public void SelectionnerClient(Clients client, string[] informations)
    {
        if (client == null)
            return;

        clientSelectionne = client;
        MiseAJour(informations);
        MettreBoutonsActifs(true);
    }

    public void DeselectionnerClient(Clients client)
    {
        if (clientSelectionne != client)
            return;

        clientSelectionne = null;
        MettreBoutonsActifs(false);
    }

    public void AccepteEvent()
    {
        int i = 0;
        bool verif = false;
        if (clientSelectionne == null)
        {
            Debug.LogWarning("Sélectionnez un client avant de l'accepter.", this);
            return;
        }

        Clients client = clientSelectionne;
        clientSelectionne = null;
        MettreBoutonsActifs(false);
        while (i < list.nameList.Length)
        {
            if (client.nom == list.nameList[i])
            {
                verif = true;;
            }
            i++;
        }
        if (verif)
        {
            monney += 30;
            reputaion += 1;
        }
        else
        {
            monney -= 50;
            reputaion -= 4;
        }
        client.Accepter();

        if (buttonSprite != null && buttonSprite.Length > 1 && buttonSprite[1] != null)
        {
            if (imageTargetAccept != null)
                imageTargetAccept.sprite = buttonSprite[1];
            Debug.Log("Client accepted");
        }
    }

    public void RefuseEvent()
    {
        if (clientSelectionne == null)
        {
            Debug.LogWarning("Sélectionnez un client avant de le refuser.", this);
            return;
        }

        Clients client = clientSelectionne;
        clientSelectionne = null;
        MettreBoutonsActifs(false);
        client.Refuser();

        if (buttonSprite != null && buttonSprite.Length > 3 && buttonSprite[3] != null)
        {
            if (imageTargetRefuse != null)
                imageTargetRefuse.sprite = buttonSprite[3];
            Debug.Log("Client refused");
        }
    }

    private void MettreBoutonsActifs(bool actif)
    {
        if (buttonAccepter != null)
            buttonAccepter.interactable = actif;

        if (buttonRefuse != null)
            buttonRefuse.interactable = actif;
    }

    private void OnDestroy()
    {
        if (buttonList != null)
        {
            buttonList.onClick.RemoveListener(OnButtonClicked);
        }

        if (buttonAccepter != null)
        {
            buttonAccepter.onClick.RemoveListener(AccepteEvent);
        }

        if (buttonRefuse != null)
        {
            buttonRefuse.onClick.RemoveListener(RefuseEvent);
        }
    }
}