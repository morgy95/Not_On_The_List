using UnityEngine;

public class Timer
{
    private float secondeAccumulee = 0f;
    private int seconde = 0;
    private int minute = 0;
    private int heure = 0;
    private int jour = 0;
    private int mois = 0;
    private int annee = 0;

    private bool enMarche = false;
    
    public int Minute => minute;
    public int Seconde => seconde;

    public void Demarrer()
    {
        enMarche = true;
    }

    public void MettreAJour(float deltaTime)
    {
        if (!enMarche)
            return;

        secondeAccumulee += deltaTime;

        while (secondeAccumulee >= 1f)
        {
            secondeAccumulee -= 1f;
            seconde++;
        }

        if (seconde >= 60) { seconde = 0; minute++; }
        if (minute >= 60)  { minute = 0;  heure++;  }
        if (heure >= 24)   { heure = 0;   jour++;   }
        if (jour >= 30)    { jour = 0;    mois++;   }
        if (mois >= 12)    { mois = 0;    annee++;  }
    }

    public void Reset()
    {
        secondeAccumulee = 0f;
        seconde = 0;
        minute = 0;
        heure = 0;
        jour = 0;
        mois = 0;
        annee = 0;
    }

    public void Stop()
    {
        enMarche = false;
    }

    public int[] GetTime()
    {
        int[] timer = { seconde, minute, heure, jour, mois, annee };
        Debug.Log($"{annee}a {mois}m {jour}j {heure}h {minute}min {seconde}s");
        return timer;
    }
}
