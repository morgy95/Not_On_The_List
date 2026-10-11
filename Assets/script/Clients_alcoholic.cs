using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Clients_alcoholic : Clients
{
    private Timer timer;

    public string status = "normal";

    private bool dansLeBar = false;

    void Awake()
    {
        comportement = "alcoholic";
    }

    protected override void Start()
    {
        base.Start();
        timer = new Timer();
    }

    protected override void Update()
    {
        base.Update();

        if (dansLeBar)
            timer.MettreAJour(Time.deltaTime);
        if (dansLeBar && status == "normal" && timer.Minute >= 4)
            status = "pissed";
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.gameObject.CompareTag("Bar"))
        {
            dansLeBar = true;
            timer.Reset();
            timer.Demarrer();
            timer.GetTime();
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (other.gameObject.CompareTag("Bar"))
        {
            dansLeBar = false;
            timer.Stop();
        }
    }
}