using UnityEngine;
using System.Collections;

public class Menu_armi_sound : MonoBehaviour 
{
	private AudioSource audioSource;
	public AudioClip BenvenutoPrimaVolta;
	public AudioClip Benvenuto;


	void Start () 
	{
		audioSource = GetComponent<AudioSource>();

		if (PlayerPrefs.HasKey ("MenuArmi"))
		{
			audioSource.PlayOneShot (Benvenuto);
		}	
		else
		{
			audioSource.PlayOneShot (BenvenutoPrimaVolta);
			PlayerPrefs.SetInt ("MenuArmi",1);
		}

	}

}
