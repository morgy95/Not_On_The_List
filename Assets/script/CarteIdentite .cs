using TMPro;
using UnityEngine;

public class CarteIdentite : MonoBehaviour
{
    [SerializeField] private TMP_Text nom;
    [SerializeField] private TMP_Text prenom;
    [SerializeField] private TMP_Text age;
    [SerializeField] private TMP_Text metier;

    void Start()
    {
        nom.text = "";
        prenom.text = "";
        age.text = "";
        metier.text = "";
    }

    public void MiseAJour(string[] str)
    {
        if (prenom == null) Debug.LogError("prenom non assigné dans l'Inspector", this);
        if (nom == null) Debug.LogError("nom non assigné dans l'Inspector", this);
        if (age == null) Debug.LogError("age non assigné dans l'Inspector", this);
        if (metier == null) Debug.LogError("metier non assigné dans l'Inspector", this);

        if (str == null || str.Length < 4)
        {
            Debug.LogWarning("MiseAJour : tableau invalide");
            return;
        }

        Debug.Log("MiseAJour appelée");

        prenom.text = "Prénom : " + str[0];
        nom.text = "Nom : " + str[1];
        age.text = "Âge : " + str[2];
        metier.text = "Métier : " + str[3];
    }
}