using UnityEngine;
using System.Collections;

public class SoundEnea : MonoBehaviour
{
    public AudioClip[] suoniEnea;
    public AudioSource audioSource;
    public AudioSource audioSourcePrincipale;
    private Gestione_audio_speaker AudioSpeaker;

    void Start ()
    {
        InvokeRepeating("PlaySound", 30, 10);
        AudioSpeaker = GameObject.Find("Gestione audio Speaker").GetComponent<Gestione_audio_speaker>();
        AudioSpeaker.isEnea = true;
	}
	
	void PlaySound ()
    {
        int random = Random.Range(0, suoniEnea.Length);
        audioSource.panStereo = audioSourcePrincipale.panStereo;
        audioSource.PlayOneShot(suoniEnea[random]);
    }
}
