using UnityEngine;

public class Musique : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        // source.Play();
    }
}