using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Back_to_menu : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private AudioSource audioSource;
	public GameObject listButton;
	private bool esc_menu = false;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip AudioPausa;

	public GameObject esci;
	public GameObject continua;
	public GameObject MenuPrincipale;
	public GameObject Panel;
	private int numButton = -1;


	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			numButton = -1;
			esc_menu = true;
			esci.SetActive (true);
			continua.SetActive (true);
			MenuPrincipale.SetActive (true);
			listButton.SetActive (false);
			Panel.SetActive (false);
			audioSource.PlayOneShot (AudioPausa);
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
        SceneManager.LoadScene(PlayerPrefs.GetInt("ScenaDaCaricare"));
	}
	

	public void Continua() 
	{
		esc_menu = false;
		esci.SetActive (false);
		continua.SetActive (false);
		MenuPrincipale.SetActive (false);
		listButton.SetActive (true);
		Panel.SetActive (true);
	}
	public void Menuprincipale() 
	{
        SceneManager.LoadScene (1);
	}

}
