using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class NuovaPartita : MonoBehaviour 
{
	public Button[] ButtonMenu;
	public AudioClip[] AudioSpiegazioni;
	private AudioSource audioSource;
	public GameObject listButton;
	private bool esc_menu = false;
	
	public GameObject conferma;
	public GameObject annulla;
	private int numButton = -1;

	public AudioClip SeiSicuro;
	public AudioClip Destra;
	public AudioClip Sinistra;
    public GameObject Panel1;
    public GameObject Panel2;
    void Start () 
	{
		audioSource = GetComponent<AudioSource>();
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/MainMenu/ButtonAudio/Newgame");
            SeiSicuro = Resources.Load<AudioClip>("Localization/English/MainMenu/Button 18");
            Destra = Resources.Load<AudioClip>("Localization/English/MainMenu/Button 3");
            Sinistra = Resources.Load<AudioClip>("Localization/English/MainMenu/Button 4");
        }
        else
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/MainMenu/ButtonAudio/Newgame");
            SeiSicuro = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Button 18");
            Destra = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Button 3");
            Sinistra = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Button 4");
        }
    }
	
	void Update () 
	{
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

    public void SwipeDownMenu()
    {
        int n = numButton + 1;
        audioSource.Stop();
        if (numButton < (ButtonMenu.Length - 1))
        {
            numButton++;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
        else
        {
            numButton = 0;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
    }

    public void coDestraSinistra()
	{
		StartCoroutine ("DestraSinistra");
	}

	IEnumerator DestraSinistra ()
	{
		audioSource.PlayOneShot (Destra);
		audioSource.panStereo = 1;
		yield return new WaitForSeconds (Destra.length + 0.5f);
		audioSource.panStereo = -1;
		audioSource.PlayOneShot (Sinistra);
		yield return new WaitForSeconds (Sinistra.length + 0.5f);
        SceneManager.LoadScene("Manù_Scelta_Personaggio");
	}

	public void NewGame ()
	{
		if(PlayerPrefs.HasKey ("Gioco_Iniziato"))
		{
			audioSource.Stop ();
			audioSource.PlayOneShot (SeiSicuro);
			numButton = -1;
			esc_menu = true;
			conferma.SetActive (true);
			annulla.SetActive (true);
			listButton.SetActive (false);
            Panel1.SetActive(false);
            Panel2.SetActive(true);
        }
        else 
		{
			PlayerPrefs.SetInt ("Gioco_Iniziato",1);
			PlayerPrefs.SetInt ("Gruppo0",1);
			PlayerPrefs.SetInt ("ScenaDaCaricare",2);
			StartCoroutine ("DestraSinistra");
		}
	}

	public void Conferma ()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt ("Gioco_Iniziato",1);
		PlayerPrefs.SetInt ("ScenaDaCaricare",2);
		StartCoroutine ("DestraSinistra");
	}
	public void Annulla ()
	{
		conferma.SetActive (false);
		annulla.SetActive (false);
        Panel1.SetActive(true);
        Panel2.SetActive(false);
        listButton.SetActive (true);
		esc_menu = false;

	}

    public void Confirm()
    {
        ButtonMenu[numButton].onClick.Invoke();
    }
}
