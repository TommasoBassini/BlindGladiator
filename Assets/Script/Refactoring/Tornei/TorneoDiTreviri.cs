using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class TorneoDiTreviri : Tornei 
{
	
	void Awake() 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTreviri/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTreviri/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoTreviri/Lose");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTreviri/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTreviri/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoTreviri/Lose");
        }

        playercontrol = GameObject.Find ("Player").GetComponent<PlayerControl>();
		PlayerScript = GameObject.Find ("Player").GetComponent<Player_Script>();
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey ("LivelloTreviri"))
			LivelloTorneo = PlayerPrefs.GetInt ("LivelloTreviri");
		else
		{
			PlayerPrefs.SetInt ("LivelloTreviri",0);
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
				PlayerPrefs.SetInt("Lancia",1);
			}
			if (LivelloTorneo == 1)
			{
				PlayerPrefs.SetInt("Scimitarra",1);
			}
			if (LivelloTorneo == 2)
			{
				PlayerPrefs.SetInt("Leone",1);
			}
			if (LivelloTorneo == 3)
			{
				PlayerPrefs.SetInt("Gladio",1);
				PlayerPrefs.SetInt("CottaDiMaglia",1);
				PlayerPrefs.SetInt("ScudoTorre",1);
			}
			if (LivelloTorneo == 4)
			{
				PlayerPrefs.SetInt("AsciaBipenne",1);
			}
			PlayerPrefs.SetInt ("LivelloTreviri",LivelloTorneoPiu);
			AudioSpeaker.Stop ();
			audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
			Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);
		}	
	}
	
	public override void Lose()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		LivelloTorneo = PlayerPrefs.GetInt ("LivelloTreviri");
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
			SceneManager.LoadScene ("Torneo_Di_Treviri_menu");
		}
		else
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",14);
            SceneManager.LoadScene("Torneo_Di_Pola_menu");
		}
	}
	void ChangeSceneLose ()
	{
        SceneManager.LoadScene("Torneo_Di_Treviri_menu");
	}
}