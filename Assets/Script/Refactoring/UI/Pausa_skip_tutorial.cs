using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Pausa_skip_tutorial : MonoBehaviour 
{
	public Button[] ButtonMenu;
	public AudioClip AudioPausa;
	public AudioClip[] AudioSpiegazioni;
	private AudioSource audioSource;
	private bool esc_menu = false;
	
	public GameObject continua;
	public GameObject MenuPrincipale;
	public GameObject skipTutorial;
	private int numButton = -1;
	public AudioSource audioTutorial;
	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Time.timeScale = 0;
			audioSource.PlayOneShot (AudioPausa);
			audioTutorial.Pause();
			numButton = -1;
			esc_menu = true;
			continua.SetActive (true);
			skipTutorial.SetActive (true);
			MenuPrincipale.SetActive (true);
		}
		
		if(Input.GetKeyDown (KeyCode.Tab) && esc_menu == true)
		{
			int n = numButton + 1;
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

	public void Esci() 
	{
		Application.LoadLevel (4);
	}
	
	public void Continua() 
	{
		esc_menu = false;
		continua.SetActive (false);
		skipTutorial.SetActive (false);
		MenuPrincipale.SetActive (false);
		Time.timeScale = 1;
		audioTutorial.UnPause ();
	}
	public void Menuprincipale() 
	{
		Time.timeScale = 1;
		Application.LoadLevel (1);
	}

	public void SkipTutorial ()
	{
		PlayerPrefs.SetInt ("ScenaDaCaricare",5);
		Time.timeScale = 1;
		Application.LoadLevel ("Torneo_Di_Segobrida_menu");
	}

}
