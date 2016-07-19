using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class TorneoDiPola : Tornei 
{
	void Awake() 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoPola/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoPola/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoPola/Lose");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoPola/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoPola/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoPola/Lose");
        }

        playercontrol = GameObject.Find ("Player").GetComponent<PlayerControl>();
		PlayerScript = GameObject.Find ("Player").GetComponent<Player_Script>();
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey ("LivelloPola"))
			LivelloTorneo = PlayerPrefs.GetInt ("LivelloPola");
		else
		{
			PlayerPrefs.SetInt ("LivelloPola",0);
			LivelloTorneo = 0;
		}
		
		if (LivelloTorneo < 5)
		{
			Instantiate (TipiGladiatori[LivelloTorneo]);
			enemy = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<EnemyControl>();
		}
		
		audioSource.PlayOneShot (AudioIntroduzione[LivelloTorneo]);
		enemy.IniziaBattaglia (AudioIntroduzione[LivelloTorneo].length + 1.0f);
		Invoke ("combattimentoIniziato",AudioIntroduzione[LivelloTorneo].length);
		
	}
	
	void combattimentoIniziato ()
	{
		playercontrol.CombattimentoIniziato = true;
	}
	
	public override void Win()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		int LivelloTorneoPiu = LivelloTorneo + 1;
		if (LivelloTorneo < 5)
		{
			if (LivelloTorneo == 0)
			{
				PlayerPrefs.SetInt("Accetta",1);
			}
			if (LivelloTorneo == 1)
			{
				PlayerPrefs.SetInt("Placche",1);
				PlayerPrefs.SetInt("ScudoFerroRiforgiato",1);
			}
			if (LivelloTorneo == 3)
			{
				PlayerPrefs.SetInt("ArmaturaPesante",1);
				PlayerPrefs.SetInt("ScudoAcciaio",1);
				PlayerPrefs.SetInt("Tridente",1);
			}
			if (LivelloTorneo == 4)
			{
				PlayerPrefs.SetInt("ArmaturaDiBande",1);
			}
			PlayerPrefs.SetInt("Monete",PlayerPrefs.GetInt("Monete") + (LivelloTorneo + 1));
			PlayerPrefs.SetInt ("LivelloPola",LivelloTorneoPiu);
			AudioSpeaker.Stop ();
			audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
			Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);
		}	
	}
	
	public override void Lose()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		LivelloTorneo = PlayerPrefs.GetInt ("LivelloPola");
		audioSource.PlayOneShot (AudioSconfitta[LivelloTorneo]);
		PlayerPrefs.SetInt("Vita",50);
		PlayerPrefs.SetInt("Armatura",(PlayerPrefs.GetInt ("ArmaturaMax")/2));
		Invoke ("ChangeSceneLose", AudioSconfitta[LivelloTorneo].length + 1);
	}
	
	void ChangeScene ()
	{

		PlayerPrefs.SetInt("Vita",PlayerScript.PlayerLife);
		PlayerPrefs.SetInt("Armatura",PlayerScript.Armor);
		LivelloTorneo ++;
		if (LivelloTorneo < 6)
		{
            SceneManager.LoadScene("Torneo_Di_Pola_menu");
		}
		else
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",17);
            SceneManager.LoadScene("Torneo_Di_Roma_menu");
		}
	}
	void ChangeSceneLose ()
	{
        SceneManager.LoadScene("Torneo_Di_Pola_menu");
	}
}