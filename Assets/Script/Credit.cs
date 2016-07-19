using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour 
{

	private AudioSource audioSource;
	public AudioClip Crediti;
	public AudioClip FraseFine;
	public AudioClip TornaAlMenuPrincipale;
	private string nomeScena;
	private bool skip = false;

	void Start()
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            Crediti = Resources.Load<AudioClip>("Localization/English/Credit/Credit");
            FraseFine = Resources.Load<AudioClip>("Localization/English/Credit/Congratulation");
            TornaAlMenuPrincipale = Resources.Load<AudioClip>("Localization/English/Credit/Esc for main menu");
        }
        else
        {
            Crediti = Resources.Load<AudioClip>("Localization/Italian/Credit/Credit");
            FraseFine = Resources.Load<AudioClip>("Localization/Italian/Credit/Congratulation");
            TornaAlMenuPrincipale = Resources.Load<AudioClip>("Localization/Italian/Credit/Esc for main menu");
        }

        audioSource = GetComponent<AudioSource>();
		nomeScena = Application.loadedLevelName;
		if(nomeScena == "FineGiocoCredit")
		{
			audioSource.PlayOneShot(FraseFine);
			Invoke("AudioCredit",FraseFine.length + 1.0f);
			Invoke("Skip",FraseFine.length + Crediti.length + 1.0f);
		}
		else
		{
			skip = true;
			audioSource.PlayOneShot(TornaAlMenuPrincipale);
			Invoke("AudioCredit",TornaAlMenuPrincipale.length + 1.0f);
		}
	}
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Escape) && skip == true)
		{
            SceneManager.LoadScene("Menu_Principale");
		}
	}

	void AudioCredit()
	{
		audioSource.PlayOneShot(Crediti);
	}

	void Skip()
	{
		audioSource.PlayOneShot(TornaAlMenuPrincipale);
		skip = true;
	}
}
