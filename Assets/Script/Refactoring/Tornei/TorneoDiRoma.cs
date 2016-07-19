using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TorneoDiRoma : Tornei 
{
	
	void Awake() 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoRoma/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoRoma/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoRoma/Lose");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoRoma/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoRoma/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoRoma/Lose");
        }

        playercontrol = GameObject.Find ("Player").GetComponent<PlayerControl>();
		PlayerScript = GameObject.Find ("Player").GetComponent<Player_Script>();
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey ("LivelloRoma"))
			LivelloTorneo = PlayerPrefs.GetInt ("LivelloRoma");
		else
		{
			PlayerPrefs.SetInt ("LivelloRoma",0);
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
				PlayerPrefs.SetInt("SpadaCorta",1);
			}
			if (LivelloTorneo == 1)
			{
				PlayerPrefs.SetInt("ScudoConSpuntone",1);
			}
			if (LivelloTorneo == 2)
			{
				PlayerPrefs.SetInt("ArmaturaApache",1);
			}
			if (LivelloTorneo == 3)
			{
				PlayerPrefs.SetInt("ArmaturaCavaliere",1);
				PlayerPrefs.SetInt("ScudoBorchiato",1);
				PlayerPrefs.SetInt("Tridente",1);
			}
			PlayerPrefs.SetInt("Monete",PlayerPrefs.GetInt("Monete") + (LivelloTorneo + 1));
			PlayerPrefs.SetInt ("LivelloRoma",LivelloTorneoPiu);
			AudioSpeaker.Stop ();
			audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
			Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);
		}	
	}
	
	public override void Lose()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		LivelloTorneo = PlayerPrefs.GetInt ("LivelloRoma");
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
            SceneManager.LoadScene("Torneo_Di_Roma_menu");
		}
		else
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",17);
            SceneManager.LoadScene("FineGiocoCredit");
		}
	}
	void ChangeSceneLose ()
	{
        SceneManager.LoadScene("Torneo_Di_Roma_menu");
	}
}