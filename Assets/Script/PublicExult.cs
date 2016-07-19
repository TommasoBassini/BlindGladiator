using UnityEngine;
using System.Collections;

public class PublicExult : MonoBehaviour 
{

	private Tornei torneo;
	public AudioClip EsultanzaPubblico;
	private AudioSource audioSource;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
        torneo = GameObject.Find ("GestioneTorneo").GetComponent<Tornei>();
		InvokeRepeating ("Esultanza_Pubblico", torneo.AudioIntroduzione[torneo.LivelloTorneo].length - 0.5f,Random.Range (15.0f, 25.0f));
	}
	
	void Esultanza_Pubblico()
	{
		audioSource.PlayOneShot(EsultanzaPubblico);
	}

}
