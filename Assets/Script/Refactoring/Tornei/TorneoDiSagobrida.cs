using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class TorneoDiSagobrida : Tornei 
{
	private EnemyControl bossControl;

	public GameObject boss;

	public AudioClip Fine4Scontro;


	void Start()
	{

        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoSagobriga/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoSagobriga/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/English/TorneoSagobriga/Lose");
        }
        else
        {
            AudioIntroduzione = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoSagobriga/Intro");
            AudioVittoria = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoSagobriga/Win");
            AudioSconfitta = AudioGestor.AudioClipArrayLoading("Localization/Italian/TorneoSagobriga/Lose");
        }

        playercontrol = GameObject.Find ("Player").GetComponent<PlayerControl>();
		PlayerScript = GameObject.Find ("Player").GetComponent<Player_Script>();
		audioSource = GetComponent<AudioSource>();
		if (PlayerPrefs.HasKey ("LivelloSagobrida"))
			LivelloTorneo = PlayerPrefs.GetInt ("LivelloSagobrida");
		else
		{
			PlayerPrefs.SetInt ("LivelloSagobrida",0);
			LivelloTorneo = 0;
		}

		if (LivelloTorneo < 4)
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
		if (LivelloTorneo < 3)
		{
			if (LivelloTorneo == 2)
			{
				PlayerPrefs.SetInt("MazzaChiodata",1);
			}
			PlayerPrefs.SetInt ("LivelloSagobrida",LivelloTorneoPiu);
            AudioSpeaker.volume = 0;
            AudioSpeaker.Stop ();
			audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
			Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);

		}
		else if (LivelloTorneo == 3)
		{
			LivelloTorneo += 1;
			PlayerPrefs.SetInt("Orientale",1);
			PlayerPrefs.SetInt("Katana",1);
			StartCoroutine ("ArrivoBoss");
			Destroy (enemy);
		}
		else if (LivelloTorneo == 4)
			{
				audioSource.PlayOneShot (AudioVittoria[LivelloTorneo]);
				Invoke ("ChangeScene", AudioVittoria[LivelloTorneo].length + 1);
				LivelloTorneo += 1;
			}
	}

	public override void Lose()
	{
        AudioSpeaker.volume = 0;
        AudioSpeaker.Stop ();
		LivelloTorneo = PlayerPrefs.GetInt ("LivelloSagobrida");
		audioSource.PlayOneShot (AudioSconfitta[LivelloTorneo]);
		PlayerPrefs.SetInt("Vita",50);
		PlayerPrefs.SetInt("Armatura",(PlayerPrefs.GetInt ("ArmaturaMax")/2));
		Invoke ("ChangeSceneLose", AudioSconfitta[LivelloTorneo].length + 1);
	}

	void ChangeScene ()
	{
		PlayerPrefs.SetInt("Vita",PlayerScript.PlayerLife);
		PlayerPrefs.SetInt("Armatura",PlayerScript.Armor);
		if (LivelloTorneo < 4)
		{
			SceneManager.LoadScene ("Torneo_Di_Segobrida_menu");
		}
		else
		{
			PlayerPrefs.SetInt("ScenaDaCaricare",8);
            SceneManager.LoadScene("Torneo_Di_Terragona_menu");
		}
	}

	void ChangeSceneLose ()
	{
        SceneManager.LoadScene("Torneo_Di_Segobrida_menu");
	}

	IEnumerator ArrivoBoss ()
	{
		boss.SetActive (true);
		bossControl = boss.GetComponent<EnemyControl>();
		audioSource.PlayOneShot (AudioVittoria[3]);
        AudioSpeaker.volume = 0;
		yield return new WaitForSeconds(AudioVittoria[3].length);
		playercontrol.CercaNemico ();
        AudioSpeaker.volume = 0.5f;
        bossControl.IniziaBattaglia (2);
	}
}