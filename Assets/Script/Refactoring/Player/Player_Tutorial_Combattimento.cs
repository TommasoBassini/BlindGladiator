using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class Player_Tutorial_Combattimento : MonoBehaviour 
{
	private AudioSource audioSource;

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
	
	public float TempoDanno;													//Il tempo che hai per reagire
	public AudioClip SuoniDodgeLaterale;
	public AudioClip SuoniDodgeCentrale;
	public AudioClip[] Attacco;
	public AudioClip[] SuonoDanno;
	private int random;

	public bool ScudoAlzato = false;											//Bool per sapere se lo scudo e' alzato
	public AudioClip[] HitScudo;
	private int NParate = 0;
	
	private float CurrAction = 0;												//Float crescente, a 100 si puo' fare un azione
	public float SpeedAction;													//La velocita' con cui cresce l'action Bar
	public Text UiText;															// GameObject per UI
	public Image ActionBar; 													// GameObject per UI
	public AudioClip BarraFull;

	private int ColpiRicevuti = 0;

	public AudioClip AudioInizioTutorial;
	public AudioClip[] AudioFineTutorial;

	public bool PlayerMorto = false;
	private Enemy_Control_Tutorial enemy;

	private bool Tutorial_Iniziato = false;

	public AudioSource audioSource1;
	public AudioClip Affaticamento;
	private bool affaticato = false;

	public Image schivataSinistra;
	public Image schivataCentrale;
	public Image schivataDestra;


	public AudioClip PausaSkip;

	void Start () 
	{
        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            AudioFineTutorial = AudioGestor.AudioClipArrayLoading("Localization/English/Tutorial/Parte 2");
            AudioInizioTutorial = Resources.Load<AudioClip>("Localization/English/Tutorial/Tut 14 15 16");
        }
        else
        {
            AudioFineTutorial = AudioGestor.AudioClipArrayLoading("Localization/Italian/Tutorial/Parte 2");
            AudioInizioTutorial = Resources.Load<AudioClip>("Localization/Italian/Tutorial/Tut 14 15 16");
        }

        SpeedAction = PlayerPrefs.GetFloat ("Velocita");
		audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot (AudioInizioTutorial);
		Invoke ("InizioTutorial",AudioInizioTutorial.length + 0.5f);
		InvokeRepeating ("DecreaseActionBar",0.01f,0.01f);						//Invoca la funzione che fa crescere l'action Bar
		enemy = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<Enemy_Control_Tutorial>();
	}
	
	void Update () 
	{
		ActionBar.fillAmount = CurrAction / 100;								//Funzione per UI
		UiText.text = CurrAction.ToString ("0");								//Funzione per UI

		if (Tutorial_Iniziato)
		{
			if(Input.GetKey (KeyCode.UpArrow) )											//Momentaneo Pulsante di azione
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
			
			if(Input.GetKeyDown (KeyCode.Space))						//Momentaneo Pulsante di azione
			{
				if (CurrAction < 81)
				{ 
					StartCoroutine ("AttaccoVeloce");
					affaticato = false;
				}
				else 
				{
					if (!affaticato)
					{
						audioSource1.PlayOneShot (Affaticamento);
						affaticato = true;
					}
				}
			}
		}
	}

	void InizioTutorial()
	{
		Tutorial_Iniziato = true;
	}

	IEnumerator SchivataSinistra ()												//Coroutines per la schivata sinistra		
	{
		AzioneInCorso = true;
		SchivataSinistraWork = true;
		Color schivata_sinistra = schivataSinistra.color;
		schivata_sinistra.a = 255;
		schivataSinistra.color = schivata_sinistra;
		audioSource.PlayOneShot (SuoniDodgeLaterale);
		AttaccoSinistroSchivato = true;
		yield return new WaitForSeconds (1.5f);
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
		audioSource.PlayOneShot (SuoniDodgeCentrale);
		AttaccoCentraleSchivato = true;
		yield return new WaitForSeconds (1.5f);
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
		audioSource.PlayOneShot (SuoniDodgeLaterale);
		AttaccoDestroSchivato = true;
		yield return new WaitForSeconds (1.5f);
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
		if(AttaccoNemicoSinistro)												//Capisce se il nemico attacca da sinistra
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;
			}
			
			if (!ScudoAlzato && !AttaccoSinistroSchivato)
			{
				SuonoDanni();
				ColpiRicevuti++;
			}

			AttaccoNemicoSinistro = false;
		}
		
		if(AttaccoNemicoDestro)													//Capisce se il nemico attacca da destra
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;

			}
			
			if (!ScudoAlzato && !AttaccoDestroSchivato)
			{
				SuonoDanni();
				ColpiRicevuti++;
			}

			AttaccoNemicoDestro = false;
		}
		
		if(AttaccoNemicoCentrale)												//Capisce se il nemico attacca centralmente
		{
			if (ScudoAlzato)													//Se lo scudo e' alzato lava la percientuale del danno all' armatura se c'e' o senno' la vita
			{
				SuonoScudo();
				NParate++;
			}
			
			if (!ScudoAlzato && !AttaccoCentraleSchivato)
			{
				SuonoDanni();
				ColpiRicevuti++;
			}

			AttaccoNemicoCentrale = false;				
		}
	}
	
	void SuonoScudo()
	{
		int SuonoRandom = Random.Range (0,HitScudo.Length);
		audioSource.PlayOneShot (HitScudo[SuonoRandom]);
	}

	void SuonoAttacco()
	{
		int SuonoRandom = Random.Range (0,Attacco.Length);
		audioSource.PlayOneShot (Attacco[SuonoRandom]);
	}

	void SuonoDanni()
	{
		int SuonoRandom = Random.Range (0,SuonoDanno.Length);
		audioSource.PlayOneShot (SuonoDanno[SuonoRandom]);
	}

	void DecreaseActionBar ()													//Funzione incremento action bar
	{
		if (CurrAction > 0)
			CurrAction = CurrAction - SpeedAction;
		else 
			CurrAction = 0;
	}
	
	void ActionReady ()															//Funzione che viene chiamata quando CurrAction arriva a 100
	{
		CancelInvoke("IncreaseActionBar");										//cancella il precedente Invoke, setta CurrAction a 100
		audioSource.PlayOneShot (BarraFull);
		CurrAction = 100;
	}
	
	IEnumerator AttaccoVeloce()
	{
		SuonoAttacco();
		CurrAction += 20.0f;
		if (enemy.DannoPieno)
			enemy.EnemyLife -= 5;
		else
			enemy.EnemyLife -= 2;
		if(enemy.EnemyLife <= 0)
		{
			enemy.Morto();
			FineTutorial();
		}
		yield return new WaitForSeconds (0.15f);
		enemy.SuonoColpito();
	}

	void ChangeScene ()
	{
		PlayerPrefs.SetInt ("ScenaDaCaricare",5);
		SceneManager.LoadScene ("Torneo_Di_Segobrida_menu");
	
	}
		

	void FineTutorial()
	{
		Tutorial_Iniziato = false;
		if (ColpiRicevuti < 3)
		{
			audioSource.PlayOneShot (AudioFineTutorial[0]);
			Invoke ("ChangeScene",AudioFineTutorial[0].length + 0.5f);
		}

		if (ColpiRicevuti >= 3 && ColpiRicevuti < 10)
		{
			audioSource.PlayOneShot (AudioFineTutorial[1]);
			Invoke ("ChangeScene",AudioFineTutorial[1].length + 0.5f);
		}

		if (ColpiRicevuti >= 10)
		{
			audioSource.PlayOneShot (AudioFineTutorial[2]);
			Invoke ("ChangeScene",AudioFineTutorial[2].length + 0.5f);
		}
	}
}
