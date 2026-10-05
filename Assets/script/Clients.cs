using UnityEngine;

public class Clients : MonoBehaviour
{
    public int    life = 5;
    public string comportement;

    public string prenom;
    public string nom;
    public int    age;
    public string metier;
    public string adresse;
    public string description;
    public string nomClient;

    [SerializeField] private Chargeur chargeur;

    protected virtual void Start()
    {
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
    }

    protected void WastedLife()
    {
        life--;
    }

    
}