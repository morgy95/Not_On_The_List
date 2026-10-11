using UnityEngine;

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
    public string[] list;
}

public class Chargeur : MonoBehaviour
{
    [System.Serializable]
    public struct Sprites
    {
        public Sprite[] ligneSprite;
    }
    
    public Sprites[] sprite;
    private ListeNoms liste;

    void Awake()
    {
        TextAsset fichier = Resources.Load<TextAsset>("jsonListe"); 
        if (fichier == null)
        {
            Debug.LogError("jsonListe.json introuvable dans Assets/Resources/ ou fichier mal nommé.");
            return;
        }
        liste = JsonUtility.FromJson<ListeNoms>(fichier.text);
    }

    public string[] CreateListe(int maxNb)
    {
        if (liste == null || liste.noms == null || liste.noms.Length == 0) return new string[0];

        string[] str = new string[maxNb];
        for (int i = 0; i < maxNb; i++)
        {
            str[i] = liste.noms[Random.Range(0, liste.noms.Length)];
        }
        liste.list = str;
        return str;
    }

    public string[] getList()
    {
        return liste.list;
    }

    public string PrenomAleatoire()       => liste.prenoms[Random.Range(0, liste.prenoms.Length)];
    public string NomAleatoire()          => liste.noms[Random.Range(0, liste.noms.Length)];
    public string SexeAleatoire()         => liste.sexes[Random.Range(0, liste.sexes.Length)];
    public string MetierAleatoire()       => liste.metiers[Random.Range(0, liste.metiers.Length)];
    public string AdressesAleatoire()     => liste.adresses[Random.Range(0, liste.adresses.Length)];
    public string ComportementAleatoire() => liste.comportements[Random.Range(0, liste.comportements.Length)];
    public string DescriptionsAleatoire() => liste.descriptions[Random.Range(0, liste.descriptions.Length)];
    public int AgeAleatoire()             => Random.Range(liste.ageMin, liste.ageMax + 1);

    public Sprite[] SpritesAleatoire(string sexe)
    {
        if (sprite == null || sprite.Length == 0) return null;

        if (sexe == "Male")
        {
            int index = Random.Range(0, Mathf.Min(4, sprite.Length));
            return sprite[index].ligneSprite;
        }
        else
        {
            int index = Random.Range(4, Mathf.Min(6, sprite.Length));
            return sprite[index].ligneSprite;
        }
    }
}
