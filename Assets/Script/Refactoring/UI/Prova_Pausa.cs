using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Prova_Pausa : MonoBehaviour 
{
	
	public Button[] ButtonMenu;
	public AudioClip AudioPausa;
	public AudioClip[] AudioSpiegazioni;
	private AudioSource audioSource;
	private bool esc_menu = false;
	
	public GameObject continua;
	public GameObject MenuPrincipale;
	public GameObject indietro;
	private int numButton = -1;

	void Start ()
	{
		AudioListener.pause = false;
		audioSource = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if(! esc_menu)
				audioSource.PlayOneShot(AudioPausa);
			Time.timeScale = 0;
			numButton = -1;
			AudioListener.pause = true;
			audioSource.ignoreListenerPause = true;
			esc_menu = true;
			continua.SetActive (true);
			MenuPrincipale.SetActive (true);
			indietro.SetActive (true);
		}
		
		if(Input.GetKeyDown (KeyCode.Tab) && esc_menu == true)
		{
			audioSource.Stop ();
			if (numButton < (ButtonMenu.Length - 1))
			{
				numButton++;
				ButtonMenu[numButton].Select();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			else 
			{
				numButton = 0;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
		}
	}
	
	public void Esci(string i) 
	{
		AudioListener.pause = false;
		Time.timeScale = 1;
		Application.LoadLevel (i);
	}
	
	public void Continua() 
	{
		audioSource.Stop ();
		esc_menu = false;
		continua.SetActive (false);
		indietro.SetActive (false);
		MenuPrincipale.SetActive (false);
		Time.timeScale = 1;
		AudioListener.pause = false;
	}
	public void Menuprincipale() 
	{
		Time.timeScale = 1;
		AudioListener.pause = false;
		Application.LoadLevel (0);
	}
	
	
}
