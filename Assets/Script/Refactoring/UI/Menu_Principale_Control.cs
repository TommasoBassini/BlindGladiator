using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Menu_Principale_Control : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private int numButton = -1;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip benvenuto;
	public AudioClip comandi;
	public AudioClip IniziaLaPartita;
	private bool stop = false;

	void Awake () 
	{
        audioSource = GetComponent<AudioSource>();
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/MainMenu/ButtonAudio");
            benvenuto = Resources.Load<AudioClip>("Localization/English/MainMenu/Button 1");
            comandi = Resources.Load<AudioClip>("Localization/English/MainMenu/Comandi");
            IniziaLaPartita = Resources.Load<AudioClip>("Localization/English/MainMenu/Start New Game");
        }
        else
        {
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/MainMenu/ButtonAudio");
            benvenuto = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Button 1");
            comandi = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Comandi");
            IniziaLaPartita = Resources.Load<AudioClip>("Localization/Italian/MainMenu/Start New Game");
        }
        StartCoroutine ("Benvenuto");
    }
	
	
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			stop = true;
			audioSource.Stop ();
			if (numButton < 3 )
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            Confirm();
        }

            if (Input.GetKeyDown (KeyCode.LeftShift))
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
				numButton = 3;
				ButtonMenu[numButton].Select ();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
		}
	}

    public void SwipeDownMenu()
    {
        
            stop = true;
            audioSource.Stop();
            if (numButton < 3)
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
            numButton = 3;
            ButtonMenu[numButton].Select();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
    }

    public void LoadGame ()
	{
		if(!(PlayerPrefs.HasKey ("Gioco_Iniziato")))
		{
			audioSource.PlayOneShot (IniziaLaPartita);
		}
		else
		{
			int n = PlayerPrefs.GetInt ("ScenaDaCaricare");
			Application.LoadLevel (n);
		}
	}
	public void Credit ()
	{
        SceneManager.LoadScene("Crediti");
	}

	public void Exit ()
	{
		Application.Quit();
	}

	IEnumerator Benvenuto()
	{
		audioSource.PlayOneShot (benvenuto);
		yield return new WaitForSeconds (benvenuto.length + 0.5f);
		if (!stop)
			audioSource.PlayOneShot (comandi);
	}

    public void Confirm()
    {
         ButtonMenu[numButton].onClick.Invoke();
    }
}
