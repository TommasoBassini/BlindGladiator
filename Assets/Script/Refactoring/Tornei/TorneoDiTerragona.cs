using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class TorneoDiTerragona : Tornei 
{
	
	void Awake () 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTerragona/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTerragona/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTerragona/Lose");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTerragona/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTerragona/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTerragona/Lose");
        }

        playercontrol = GameObject.Find ("Player").GetComponent<PlayerControl>();
		PlayerScript = GameObject.Find ("Player").GetComponent<Player_Script>();
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey ("LivelloTerragona"))
			LivelloTorneo = PlayerPrefs.GetInt ("LivelloTerragona");
		else
		{
			PlayerPrefs.SetInt ("LivelloTerragona",0);
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
		Debug.Log(LivelloTorneo);
		
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
				PlayerPrefs.SetInt("ScudoPiccolo",1);
			}
			if (LivelloTorneo == 2)
			{
				PlayerPrefs.SetInt("FerroRiforgiato",1);
			}
			if (LivelloTorneo == 3)
			{
				PlayerPrefs.SetInt("SpadaLunga",1);
				PlayerPrefs.SetInt("ScudoGrande",1);
			}
			if (LivelloTorneo == 4)
			{
				PlayerPrefs.SetInt("Scintillante",1);
			}
			PlayerPrefs.SetInt ("LivelloTerragona",LivelloTorneoPiu);
			AudioSpeaker.Stop ();
			audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
			Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);
		}	
	}
	
	public override void Lose()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		LivelloTorneo = PlayerPrefs.GetInt ("LivelloTerragona");
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
            SceneManager.LoadScene("Torneo_Di_Terragona_menu");
		}
		else
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",11);
            SceneManager.LoadScene("Torneo_Di_Treviri_menu");
		}
	}
	void ChangeSceneLose ()
	{
        SceneManager.LoadScene("Torneo_Di_Terragona_menu");
	}
}
