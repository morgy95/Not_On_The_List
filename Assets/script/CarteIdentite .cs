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

    public Button buttonList;
    public GameObject testNameList;

    void Start()
    {
        if (buttonList != null)
        {
            buttonList.onClick.AddListener(OnButtonClicked);
        }

        // Reinitialisation des champs texte
        if (nom != null) nom.text = "";
        if (prenom != null) prenom.text = "";
        if (age != null) age.text = "";
        if (metier != null) metier.text = "";
    }

    public void MiseAJour(string[] str)
    {
        if (prenom == null) Debug.LogError("prenom non assigne dans l'Inspector", this);
        if (nom == null) Debug.LogError("nom non assigne dans l'Inspector", this);
        if (age == null) Debug.LogError("age non assigne dans l'Inspector", this);
        if (metier == null) Debug.LogError("metier non assigne dans l'Inspector", this);

        if (str == null || str.Length < 4)
        {
            Debug.LogWarning("MiseAJour : tableau invalide ou incomplet (minimum 4 elements attendus)");
            return;
        }

        prenom.text = "Prenom : " + str[0];
        nom.text = "Nom : " + str[1];
        age.text = "Age : " + str[2];
        metier.text = "Metier : " + str[3];
    }

    public void OnButtonClicked()
    {
        if (list != null)
        {
            list.OpenCloseListe();
        }
        else
        {
            Debug.LogError("La variable 'list' n'est pas assignee dans CarteIdentite !", this);
        }
    }

    private void OnDestroy()
    {
        if (buttonList != null)
        {
            buttonList.onClick.RemoveListener(OnButtonClicked);
        }
    }
}