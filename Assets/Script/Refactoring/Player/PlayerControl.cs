using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour 
{
	private Player_Script player_Script;
	public AudioSource audioSource1;
	public AudioSource audioSource2;
	private PlayerStat playerStat;

	//======================================================   GESTIONE BARRE   ===========================================================================

	private float CurrAction = 0;												//Float crescente, a 100 si puo' fare un azione
	public Text UiText;															// GameObject per UI
	public Image ActionBar; 													// GameObject per UI

	//======================================================   GESTIONE ATTACCO   =========================================================================

	private float SpeedAction;													//La velocita' con cui cresce l'action Bar
	private int AttaccoMin;
	private int AttaccoMax;
	public AudioClip[] Attacco;
	private bool enemyMorto = false;
	private float CostoAttacco;
	//======================================================   GESTIONE SCHIVATE   ========================================================================

	public float TempoDanno;													//Il tempo che hai per reagire

	private bool AzioneInCorso = false;
	public bool AttaccoNemicoDestro = false;									//Serie di Bool per attacchi nemico dalla destra
	private bool SchivataDestraWork = false;
	public bool AttaccoDestroSchivato = false;

	public bool AttaccoNemicoSinistro = false;									//Serie di Bool per attacchi nemico dalla sinistra
	public bool AttaccoSinistroSchivato = false;
	private bool SchivataSinistraWork = false;

	public bool AttaccoNemicoCentrale = false;									//Serie di Bool per attacchi nemico centrali
	public bool AttaccoCentraleSchivato = false;
	private bool SchivataCentraleWork = false;

	public AudioClip SuoniDodgeLaterale;
	public AudioClip SuoniDodgeCentrale;

	public Image schivataSinistra;
	public Image schivataCentrale;
	public Image schivataDestra;

	//======================================================   GESTIONE SCUDO   =============================================================================
	
	public bool ScudoAlzato = false;											//Bool per sapere se lo scudo e' alzato
	private int BlooccoScudo;													//Percentuale di blocco danno dello scudo
	public AudioClip[] HitScudo;

	public AudioClip Affaticamento;
	private bool affaticato = false;

	public float DannoNemico;													//Danni del nemico
	private int Pot_Dif = 0;

	public AudioClip Findivita;
	public bool PlayerMorto = false;

	//private EnemyControl enemyControl;
	public AudioClip[] SuonoDanno;
	
	private EnemyControl enemy;

	private Tornei gestione;
	public Gestione_audio_speaker AudioSpeaker;
	public bool CombattimentoIniziato = false;
	private Duelli duello;


	void Awake()
	{
        if (GameObject.FindGameObjectWithTag("Gestione"))
            gestione = GameObject.FindGameObjectWithTag("Gestione").GetComponent<Tornei>();

        if (GameObject.FindGameObjectWithTag("Duello"))
            duello = GameObject.FindGameObjectWithTag("Duello").GetComponent<Duelli>();

        player_Script = GetComponent<Player_Script>();

		InvokeRepeating ("DecreaseActionBar",0.01f,0.01f);						//Invoca la funzione che fa crescere l'action Bar
		Invoke ("CaricaStatistiche",0.1f);
	}

	void Update () 
	{
		ActionBar.fillAmount = CurrAction / 100;								//Funzione per UI
		UiText.text = CurrAction.ToString ("0");								//Funzione per UI

		if (CombattimentoIniziato)
		{
			if(Input.GetKeyDown (KeyCode.Space))						//Momentaneo Pulsante di azione
			{
			if (CurrAction < 81 && !affaticato)
			{ 
				StartCoroutine ("AttaccoVeloce");
			}
			else 
			{
				if (!affaticato)
				{
					StartCoroutine ("affaticamento");
				}
			}
		}
				
		if(Input.GetKey (KeyCode.UpArrow))											//Momentaneo Pulsante di azione
		{
			ScudoAlzato = true;
		}
		else 
			ScudoAlzato = false;

		if(Input.GetKey (KeyCode.RightArrow) && !SchivataSinistraWork && !AzioneInCorso)			//Comando Per schivata sinistra	
		{
			StartCoroutine ("SchivataSinistra");
		}

		if(Input.GetKey (KeyCode.LeftArrow) && !SchivataDestraWork && !AzioneInCorso)			//Comando per schivata destra
		{
			StartCoroutine ("SchivataDestra");
		}

		if(Input.GetKey (KeyCode.DownArrow) && !SchivataCentraleWork && !AzioneInCorso)			//Comando schivata centrale
		{
			StartCoroutine ("SchivataCentrale");
		}
		}
	}

	IEnumerator affaticamento()
	{
		audioSource1.PlayOneShot (Affaticamento);
		affaticato = true;
		yield return new WaitForSeconds (Affaticamento.length);
		affaticato = false;
	}

	void SuonoAttacco()
	{
		int SuonoRandom = Random.Range (0,Attacco.Length);
		audioSource1.PlayOneShot (Attacco[SuonoRandom]);
	}

	void DecreaseActionBar ()													//Funzione incremento action bar
	{
		if (CurrAction > 0)
			CurrAction = CurrAction - SpeedAction;
		else 
			CurrAction = 0;
	}


	IEnumerator AttaccoVeloce()
	{
		SuonoAttacco();
		CurrAction += CostoAttacco;
		if (enemy.DannoPieno)
		{
			AudioSpeaker.Attacco_pieno();
			enemy.EnemyLife -= Random.Range (AttaccoMin,AttaccoMax);
		}
		else
		{
			AudioSpeaker.Attacco_parato ();
			enemy.EnemyLife -= ((Random.Range (AttaccoMin,AttaccoMax) * 30) / 100 );
		}

		if(enemy.EnemyLife < ((enemy.VitaIniziale * 30 ) / 100))
			AudioSpeaker.StartCoroutine ("Enemy_Fin_di_vita");

		if(enemy.EnemyLife <= 0 && !enemyMorto)
		{
			enemy.Morto();
			PlayerPrefs.SetInt("Monete",PlayerPrefs.GetInt ("Monete") + enemy.monete);
			enemyMorto = true;
			//PlayerPrefs.SetInt ("Armatura",player_Script.Armor); 
			//PlayerPrefs.SetInt ("Vita",player_Script.PlayerLife); 
		}
		yield return new WaitForSeconds (0.15f);
		enemy.SuonoColpito();
	}
//======================================================================     DIFESA    ========================================================================================
	
	void SuonoScudo()
	{
		int SuonoRandom = Random.Range (0,HitScudo.Length);
		audioSource1.PlayOneShot (HitScudo[SuonoRandom]);
	}

	void SuonoDanni()
	{
		int SuonoRandom = Random.Range (0,SuonoDanno.Length);
		audioSource1.PlayOneShot (SuonoDanno[SuonoRandom]);
	}

	IEnumerator SchivataSinistra ()												//Coroutines per la schivata sinistra		
	{
		AzioneInCorso = true;
		SchivataSinistraWork = true;
		Color schivata_sinistra = schivataSinistra.color;
		schivata_sinistra.a = 255;
		schivataSinistra.color = schivata_sinistra;
		audioSource1.PlayOneShot (SuoniDodgeLaterale);
		AttaccoSinistroSchivato = true;
		yield return new WaitForSeconds (1.1f);
		Color schivata_sinistra1 = schivataSinistra.color;
		schivata_sinistra1.a = 0;
		schivataSinistra.color = schivata_sinistra1;
		SchivataSinistraWork = false;
		AttaccoSinistroSchivato = false;
		AzioneInCorso = false;
	}
	
	IEnumerator SchivataCentrale ()												//Coroutines per la schivata centrale
	{
		AzioneInCorso = true;
		SchivataCentraleWork = true;
		Color schivata_centrale = schivataCentrale.color;
		schivata_centrale.a = 255;
		schivataCentrale.color = schivata_centrale;
		audioSource1.PlayOneShot (SuoniDodgeCentrale);
		AttaccoCentraleSchivato = true;
		yield return new WaitForSeconds (1.1f);
		Color schivata_centrale1 = schivataCentrale.color;
		schivata_centrale1.a = 0;
		schivataCentrale.color = schivata_centrale1;
		SchivataCentraleWork = false;
		AttaccoCentraleSchivato = false;
		AzioneInCorso = false;
	}
	
	IEnumerator SchivataDestra ()												//Coroutines per la schivata destra
	{
		AzioneInCorso = true;
		SchivataDestraWork = true;
		Color schivata_destra = schivataDestra.color;
		schivata_destra.a = 255;
		schivataDestra.color = schivata_destra;
		audioSource1.PlayOneShot (SuoniDodgeLaterale);
		AttaccoDestroSchivato = true;
		yield return new WaitForSeconds (1.1f);
		Color schivata_destra1 = schivataDestra.color;
		schivata_destra1.a = 0;
		schivataDestra.color = schivata_destra1;
		SchivataDestraWork = false;
		AttaccoDestroSchivato = false;
		AzioneInCorso = false;
	}

	public void AttaccoNemico ()
	{
		Invoke ("TogliVita",TempoDanno);
	}

	void TogliVita ()
	{
//====================================================================   ATTACCO SINISTRO   ==============================================================================================

		if(AttaccoNemicoSinistro)												//Capisce se il nemico attacca da sinistra
		{
			if (!ScudoAlzato && !AttaccoSinistroSchivato)
			{
				Danni();
			}

			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				DannoNemico -= ((DannoNemico * BlooccoScudo)/100);
				if(player_Script.Armor > 0)
				{
					player_Script.Armor -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
					if (player_Script.Armor < 0)
					{
						player_Script.PlayerLife += player_Script.Armor;
						player_Script.Armor = 0;
					}
				}
				else
					player_Script.PlayerLife -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
			}
				
			if( player_Script.PlayerLife < enemy.DannoMax)
			{
				AudioSpeaker.StartCoroutine ("Un_colpo_morto");
			}

			AttaccoNemicoSinistro = false;
		}

//====================================================================   ATTACCO DESTRO   ==============================================================================================

		if(AttaccoNemicoDestro)													//Capisce se il nemico attacca da destra
		{
			if (!ScudoAlzato && !AttaccoDestroSchivato)
			{
				Danni();
			}

			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				DannoNemico -= ((DannoNemico * BlooccoScudo)/100);
				if(player_Script.Armor > 0)
				{
					player_Script.Armor -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
					if (player_Script.Armor < 0)
					{
						player_Script.PlayerLife += player_Script.Armor;
						player_Script.Armor = 0;
					}
				}
				else
					player_Script.PlayerLife -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
			}
	
			if( player_Script.PlayerLife < enemy.DannoMax)
			{
				AudioSpeaker.Un_colpo_morto ();
			}
			AttaccoNemicoDestro = false;
		}

//====================================================================   ATTACCO CENTRALE   ==============================================================================================

		if(AttaccoNemicoCentrale)												//Capisce se il nemico attacca centralmente
		{
			if (!ScudoAlzato && !AttaccoCentraleSchivato)
			{
				Danni();
			}

			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				DannoNemico -= ((DannoNemico * (BlooccoScudo))/100);
				if(player_Script.Armor > 0)
				{
					player_Script.Armor -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
					if (player_Script.Armor < 0)
					{
						player_Script.PlayerLife += player_Script.Armor;
						player_Script.Armor = 0;
					}
				}
				else
					player_Script.PlayerLife -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
			}
		
			AttaccoNemicoCentrale = false;	

			if( player_Script.PlayerLife < enemy.DannoMax)
			{
				AudioSpeaker.Un_colpo_morto ();
			}
		}

		if (player_Script.PlayerLife <= 35)
		{
			audioSource2.Play ();
			AudioSpeaker.StartCoroutine ("Fin_di_vita");
		}

		if (player_Script.PlayerLife <= 0)
		{
			if(GameObject.FindGameObjectWithTag("Gestione"))
				gestione.Lose ();
			if(GameObject.FindGameObjectWithTag("Duello"))
				duello.Lose ();
			player_Script.PlayerLife = 0;
			PlayerMorto = true;
			audioSource2.Stop ();
		}

	}

	void Danni()
	{
		if(player_Script.Armor > 0)
		{
			player_Script.Armor -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);	//Diminuisce l'armatura se c'e' altrimenti toglie la vita
			SuonoDanni();
			AudioSpeaker.Colpito ();
			if (player_Script.Armor < 0)
			{
				player_Script.PlayerLife += player_Script.Armor;
				player_Script.Armor = 0;
				SuonoDanni();
				AudioSpeaker.Colpito ();
			}
		}
		else
		{
			player_Script.PlayerLife -= Mathf.RoundToInt ((DannoNemico * (100 - Pot_Dif))/100);
			SuonoDanni();
			AudioSpeaker.Colpito ();
		}	
	}

//===========================================================================   GESTIONE STATISTICHE   ===========================================================================

	void CoCaricaStatistiche()
	{
		Invoke ("CaricaStatistiche",0.1f);
	}

	void CaricaStatistiche ()
	{
		if (GameObject.FindGameObjectWithTag ("Enemy"))
		{
			enemy = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<EnemyControl>();
		}
		CostoAttacco = 10 + (PlayerPrefs.GetFloat ("PesoTotale")/1.5f);
		SpeedAction = PlayerPrefs.GetFloat ("Velocita");
		AttaccoMin = PlayerPrefs.GetInt ("AttaccoMin");
		AttaccoMax = PlayerPrefs.GetInt ("AttaccoMax");
		BlooccoScudo = PlayerPrefs.GetInt ("BloccoScudo");


		if (PlayerPrefs.GetInt ("PotenziamentoVelocita") == 1)
		{
			PiuVelocita ();
		}

		if (PlayerPrefs.GetInt ("PotenziamentoDifesa") == 1)
		{
			Pot_Dif = PlayerPrefs.GetInt ("Perc_pot_Difesa");
			PlayerPrefs.SetInt ("PotenziamentoDifesa",0);
		}
		else
			Pot_Dif = 0;
	}

	public void CercaNemico ()
	{
		enemy = GameObject.Find ("Boss Tigre").GetComponent<EnemyControl>();
		enemyMorto = false;
	}

	void PiuVelocita ()
	{
		Debug.Log ("pot velocita : " +PlayerPrefs.GetInt("Perc_pot_Velocita"));
		CostoAttacco = CostoAttacco - ((CostoAttacco * PlayerPrefs.GetInt("Perc_pot_Velocita")) / 100);
		SpeedAction =  SpeedAction + ((SpeedAction * PlayerPrefs.GetInt("Perc_pot_Velocita")) / 100);
		PlayerPrefs.SetInt ("PotenziamentoVelocita",0);
		Debug.Log("Velocita potenziata : " + SpeedAction ); 
		Debug.Log ( "pot vel : " +PlayerPrefs.GetInt ("PotenziamentoVelocita"));
	}
}
