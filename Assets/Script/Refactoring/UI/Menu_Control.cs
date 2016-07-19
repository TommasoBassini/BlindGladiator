using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Menu_Control : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private int numButton = -1;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip Intro;
	public AudioClip Comandi;
	private bool TornaIndietro = false;
	public AudioClip tornaIndietro;
	private bool stop = false;

	void Awake () 
	{
		audioSource = GetComponent<AudioSource>();
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/Intro/Intro");
            Intro = Resources.Load<AudioClip>("Localization/English/Intro/Intro 1 2 3");
            Comandi = Resources.Load<AudioClip>("Localization/English/Intro/Intro 4");
        }
        else
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/Intro/Intro");
            Intro = Resources.Load<AudioClip>("Localization/Italian/Intro/Intro 1 2 3");
            Comandi = Resources.Load<AudioClip>("Localization/Italian/Intro/Intro 4");
        }
        StartCoroutine ("Audio");
	}
	

	void Update () 
	{
		if(TornaIndietro && Input.GetKey (KeyCode.LeftShift))
		{
			audioSource.Stop ();
			audioSource.PlayOneShot (Intro);
			Invoke ("Tornaindietro", 0.1f);
		}

		if(TornaIndietro && Input.GetKeyDown (KeyCode.Tab))
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",1);
			SceneManager.LoadScene ("Menu_Principale");
		}

		if(!TornaIndietro && Input.GetKeyDown (KeyCode.Tab))
		{
			stop = true;
			audioSource.Stop ();
			if (numButton < (ButtonMenu.Length - 1))
			{
				numButton++;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			else 
			{
				numButton = 0;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
		}

		if(!TornaIndietro && Input.GetKeyDown (KeyCode.LeftShift))
		{
			stop = true;
			audioSource.Stop ();
			if (numButton > 0)
			{
				numButton--;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				
			}
			else 
			{
				numButton = (ButtonMenu.Length - 1);
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
		}
	}

	void Tornaindietro()
	{
		TornaIndietro = false;
	}

	IEnumerator Audio()
	{
		audioSource.PlayOneShot (Intro);
		yield return new WaitForSeconds (Intro.length + 0.5f);
		if (!stop)
			audioSource.PlayOneShot (Comandi);
	}

    public void SwipeDownMenu()
    {
        stop = true;
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

    public void SwipeUpMenu()
    {

        stop = true;
        audioSource.Stop();
        if (numButton > 0)
        {
            numButton--;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);

        }
        else
        {
            numButton = (ButtonMenu.Length - 1);
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
    }

    public void ConfirmButton()
    {
        ButtonMenu[numButton].onClick.Invoke();
    }

    public void Eque()
	{
		PlayerPrefs.SetInt("Gioco_Iniziato",1);
		PlayerPrefs.SetInt("Vita",100);
		PlayerPrefs.SetInt("Armatura",10);
		PlayerPrefs.SetInt("ArmaturaMax",10);
		PlayerPrefs.SetInt("BloccoScudo",20);
		PlayerPrefs.SetFloat("PesoArmatura",3.0f);
		PlayerPrefs.SetFloat("PesoScudo",3.0f);
		PlayerPrefs.SetFloat("PesoSpada",3.5f);
		PlayerPrefs.SetInt("AttaccoMin",5);
		PlayerPrefs.SetInt("AttaccoMax",11);
		PlayerPrefs.SetInt("ScenaDaCaricare",3);
		PlayerPrefs.Save ();
        SceneManager.LoadScene("Tutorial");
	}

	public void Mirmillone()
	{
		PlayerPrefs.SetInt("Gioco_Iniziato",1);
		PlayerPrefs.SetInt("Vita",100);
		PlayerPrefs.SetInt("Armatura",10);
		PlayerPrefs.SetInt("ArmaturaMax",10);
		PlayerPrefs.SetInt("BloccoScudo",45);
		PlayerPrefs.SetFloat("PesoArmatura",3.0f);
		PlayerPrefs.SetFloat("PesoScudo",12.0f);
		PlayerPrefs.SetFloat("PesoSpada",1.5f);
		PlayerPrefs.SetInt("AttaccoMin",6);
		PlayerPrefs.SetInt("AttaccoMax",10);
		PlayerPrefs.SetInt("ScenaDaCaricare",3);
		PlayerPrefs.Save ();
        SceneManager.LoadScene("Tutorial");
	}

	public void Reziario()
	{
		PlayerPrefs.SetInt("Gioco_Iniziato",1);
		PlayerPrefs.SetInt("Vita",100);
		PlayerPrefs.SetInt("Armatura",10);
		PlayerPrefs.SetInt("ArmaturaMax",10);
		PlayerPrefs.SetInt("BloccoScudo",30);
		PlayerPrefs.SetFloat("PesoArmatura",3.0f);
		PlayerPrefs.SetFloat("PesoScudo",6.0f);
		PlayerPrefs.SetFloat("PesoSpada",1.0f);
		PlayerPrefs.SetInt("AttaccoMin",5);
		PlayerPrefs.SetInt("AttaccoMax",9);
		PlayerPrefs.SetInt("ScenaDaCaricare",3);
		PlayerPrefs.Save ();
        SceneManager.LoadScene("Tutorial");
	}

	public void Trace()
	{
		PlayerPrefs.SetInt("Gioco_Iniziato",1);
		PlayerPrefs.SetInt("Vita",100);
		PlayerPrefs.SetInt("Armatura",15);
		PlayerPrefs.SetInt("ArmaturaMax",15);
		PlayerPrefs.SetInt("BloccoScudo",35);
		PlayerPrefs.SetFloat("PesoArmatura",5.0f);
		PlayerPrefs.SetFloat("PesoScudo",6.5f);
		PlayerPrefs.SetFloat("PesoSpada",1.5f);
		PlayerPrefs.SetInt("AttaccoMin",6);
		PlayerPrefs.SetInt("AttaccoMax",11);
		PlayerPrefs.SetInt("ScenaDaCaricare",3);
		PlayerPrefs.Save ();
        SceneManager.LoadScene("Tutorial");
	}

	public void Spada ()
	{
        SceneManager.LoadScene(2);
	}

	public void Armatura ()
	{
        SceneManager.LoadScene(3);
	}

	public void Scudo ()
	{
        SceneManager.LoadScene(4);
	}
}
