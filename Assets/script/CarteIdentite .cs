using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarteIdentite : MonoBehaviour
{
    [SerializeField] private TMP_Text nom;
    [SerializeField] private TMP_Text prenom;
    [SerializeField] private TMP_Text age;
    [SerializeField] private TMP_Text metier;
    [SerializeField] private Liste list;

    [Header("Button GUI")]
    [SerializeField] private Image imageTargetAccept;
    [SerializeField] private Image imageTargetRefuse;
    [SerializeField] private Sprite[] buttonSprite;
    [SerializeField] private Button buttonRefuse;
    [SerializeField] private Button buttonAccepter;

    public Button buttonList;
    public GameObject testNameList;

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

        if (nom != null) nom.text = "";
        if (prenom != null) prenom.text = "";
        if (age != null) age.text = "";
        if (metier != null) metier.text = "";
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

    public void AccepteEvent() 
    { 
        if (imageTargetAccept == null) return;

        if (buttonSprite != null && buttonSprite.Length > 1 && buttonSprite[1] != null)
        {
            imageTargetAccept.sprite = buttonSprite[1];
            Debug.Log("Client accepted");
        }
    }

    public void RefuseEvent()
    {
        if (imageTargetRefuse == null) return;

        if (buttonSprite != null && buttonSprite.Length > 3 && buttonSprite[3] != null)
        {
            imageTargetRefuse.sprite = buttonSprite[3];
            Debug.Log("Client refused");
        }
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