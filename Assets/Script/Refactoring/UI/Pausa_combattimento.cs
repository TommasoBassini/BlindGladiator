using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Pausa_combattimento : MonoBehaviour 
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
	public AudioSource audio1;
	public AudioSource audio2;
	public AudioSource audio3;

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
			audio1.Pause();
			audio2.Pause();
			audio3.Pause();
			numButton = -1;
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
	
	public void Esci() 
	{
		Application.LoadLevel (PlayerPrefs.GetInt("ScenaDaCaricare"));
	}
	
	public void Continua() 
	{
		esc_menu = false;
		continua.SetActive (false);
		indietro.SetActive (false);
		MenuPrincipale.SetActive (false);
		Time.timeScale = 1;
		audio1.UnPause ();
		audio2.UnPause ();
		audio3.UnPause ();
	}
	public void Menuprincipale() 
	{
		Application.LoadLevel (1);
	}


}
