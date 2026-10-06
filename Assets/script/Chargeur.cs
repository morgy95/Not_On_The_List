using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ListeNoms
{
    public string[] prenoms;
    public string[] noms;
    public string[] sexes;
    public string[] metiers;
    public string[] adresses;
    public string[] comportements;
    public string[] descriptions;
    public int ageMin;
    public int ageMax;
}

public class Chargeur : MonoBehaviour
{
    private ListeNoms liste;

    void Awake()
    {
        TextAsset fichier = Resources.Load<TextAsset>("jsonListe"); 
        if (fichier == null)
        {
            Debug.LogError("jsonListe.json introuvable dans Assets/Resources");
            return;
        }
        liste = JsonUtility.FromJson<ListeNoms>(fichier.text);
    }

    public string[] CreateListe(int maxNb)
    {
        string[] str = new string[maxNb];
        int i = 0;
        while (i < maxNb)
        {
            str[i] = liste.noms[Random.Range(0, liste.noms.Length)];
            i++;
        }
        return (str);
    }

    public string PrenomAleatoire()       => liste.prenoms[Random.Range(0, liste.prenoms.Length)];
    public string NomAleatoire()          => liste.noms[Random.Range(0, liste.noms.Length)];
    public string SexeAleatoire()         => liste.sexes[Random.Range(0, liste.sexes.Length)];
    public string MetierAleatoire()       => liste.metiers[Random.Range(0, liste.metiers.Length)];
    public string AdressesAleatoire()     => liste.adresses[Random.Range(0, liste.adresses.Length)];
    public string ComportementAleatoire() => liste.comportements[Random.Range(0, liste.comportements.Length)];
    public string DescriptionsAleatoire() => liste.descriptions[Random.Range(0, liste.descriptions.Length)];
    public int AgeAleatoire()             => Random.Range(liste.ageMin, liste.ageMax + 1);
}
