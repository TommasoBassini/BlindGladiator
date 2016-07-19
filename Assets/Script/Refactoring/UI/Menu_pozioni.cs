using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu_pozioni : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private int numButton = -1;
	private PlayerStat playerStat;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip[] nomeOggetti;
	public AudioClip[] PocheMonete;

	public AudioClip[] VitaAlMax;
	public AudioClip Usato;

	public AudioClip[] ArmaturaAlMax;
	public AudioClip[] GiaPotenziato;

	
	private Vector3 posIni;
	private Vector3 posFin;
	
	private Descr_Pozioni descr_Pozioni;
	private Descr_Potenziamenti descr_potenziamenti;

	public AudioClip BenvenutoPrimaVolta;
	public AudioClip Benvenuto;
	
	void Start () 
	{
		posFin = new Vector3 (transform.localPosition.x,65.0f * ButtonMenu.Length,transform.localPosition.z);
		posIni = transform.localPosition;
		//playerStat = GameObject.Find ("GameControl").GetComponent<PlayerStat>();
		audioSource = GetComponent<AudioSource>();

        PlayerStat ps = FindObjectOfType<PlayerStat>();
        // Controllo se e' in inglese
        if (ps.isEnglish)
        {
            PocheMonete = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/FrasiPoz/NoMoney");
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/Poz");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/SoloNomi");

            VitaAlMax = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/FrasiPoz/VitaMax");
            ArmaturaAlMax = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/FrasiPoz/ArmourMax");
            GiaPotenziato = AudioGestor.AudioClipArrayLoading("Localization/English/Inn/FrasiPoz/GiaPotenz");

            Usato = Resources.Load<AudioClip>("Localization/English/Inn/FrasiPoz/Var14");
        }
        else
        {
            PocheMonete = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/FrasiPoz/NoMoney");
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/Poz");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/SoloNomi");

            VitaAlMax = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/FrasiPoz/VitaMax");
            ArmaturaAlMax = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/FrasiPoz/ArmourMax");
            GiaPotenziato = AudioGestor.AudioClipArrayLoading("Localization/Italian/Inn/FrasiPoz/GiaPotenz");

            Usato = Resources.Load<AudioClip>("Localization/Italian/Inn/FrasiPoz/Var14");
        }
    }
	
	
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			audioSource.Stop ();
			int n = numButton + 1;
			if (ButtonMenu[n].interactable == true)
			{
				if (numButton > 0)
				{
					transform.localPosition += new Vector3 (0.0f,65.0f,0.0f);
				}
				if (numButton < (ButtonMenu.Length - 1))
				{
					numButton++;
					ButtonMenu[numButton].Select();
					if(ButtonMenu[numButton].tag == "Pozioni")
					{
						descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
						descr_Pozioni.Descrizione();
					}
					else
					{
						descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
						descr_potenziamenti.Descrizione();
					}
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
					
				}
				else 
				{
					numButton = 0;
					transform.localPosition = posIni;
					ButtonMenu[numButton].Select ();
					if(ButtonMenu[numButton].tag == "Pozioni")
					{
						descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
						descr_Pozioni.Descrizione();
					}
					else
					{
						descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
						descr_potenziamenti.Descrizione();
					}
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
			}
			else
			{
				numButton = 0;
				transform.localPosition = posIni;
				ButtonMenu[numButton].Select ();
				if(ButtonMenu[numButton].tag == "Pozioni")
				{
					descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
					descr_Pozioni.Descrizione();
				}
				else
				{
					descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
					descr_potenziamenti.Descrizione();
				}
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			
		}

		if(Input.GetKeyDown (KeyCode.LeftShift))
		{
			audioSource.Stop ();
			int n = ButtonMenu.Length - 1;
			bool trovato = false;
			
			if (numButton > 0)
			{
				transform.localPosition -= new Vector3 (0.0f,65.0f,0.0f);
			}
			
			if (numButton > 0)
			{
				numButton--;
				ButtonMenu[numButton].Select ();
				if(ButtonMenu[numButton].tag == "Pozioni")
				{
					descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
					descr_Pozioni.Descrizione();
				}
				else
				{
					descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
					descr_potenziamenti.Descrizione();
				}
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			else 
			{
				if (ButtonMenu[n].interactable == true)
				{
					numButton = (ButtonMenu.Length - 1);
					transform.localPosition = posFin;
					ButtonMenu[numButton].Select ();
					if(ButtonMenu[numButton].tag == "Pozioni")
					{
						descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
						descr_Pozioni.Descrizione();
					}
					else
					{
						descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
						descr_potenziamenti.Descrizione();
					}
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
				else
				{
					Vector3 posbutton = posFin;
					while (!trovato)
					{
						n--;
						posbutton -= new Vector3 (0.0f,65.0f,0.0f);
						if (ButtonMenu[n].interactable == true)
						{
							trovato = true;
							numButton = n;
							ButtonMenu[numButton].Select ();
							transform.localPosition = posbutton;
							if(ButtonMenu[numButton].tag == "Pozioni")
							{
								descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
								descr_Pozioni.Descrizione();
							}
							else
							{
								descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
								descr_potenziamenti.Descrizione();
							}
							audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
						}
					}
				}
			}
		}
	}

    public void SwipeDownMenu()
    {
        Menu_Equipaggiamenti_sound sound = FindObjectOfType<Menu_Equipaggiamenti_sound>();
        sound.gameObject.GetComponent<AudioSource>().Stop();
        sound.stop = true;

        audioSource.Stop();
        int n = numButton + 1;
        if (ButtonMenu[n].interactable == true)
        {
            if (numButton > 0)
            {
                transform.localPosition += new Vector3(0.0f, 65.0f, 0.0f);
            }
            if (numButton < (ButtonMenu.Length - 1))
            {
                numButton++;
                ButtonMenu[numButton].Select();
                if (ButtonMenu[numButton].tag == "Pozioni")
                {
                    descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                    descr_Pozioni.Descrizione();
                }
                else
                {
                    descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                    descr_potenziamenti.Descrizione();
                }
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);

            }
            else
            {
                numButton = 0;
                transform.localPosition = posIni;
                ButtonMenu[numButton].Select();
                if (ButtonMenu[numButton].tag == "Pozioni")
                {
                    descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                    descr_Pozioni.Descrizione();
                }
                else
                {
                    descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                    descr_potenziamenti.Descrizione();
                }
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
        }
        else
        {
            numButton = 0;
            transform.localPosition = posIni;
            ButtonMenu[numButton].Select();
            if (ButtonMenu[numButton].tag == "Pozioni")
            {
                descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                descr_Pozioni.Descrizione();
            }
            else
            {
                descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                descr_potenziamenti.Descrizione();
            }
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }

    }

    public void ConfirmButton()
    {
        ButtonMenu[numButton].onClick.Invoke();
    }

    public void SwipeUpMenu()
    {
        Menu_Equipaggiamenti_sound sound = FindObjectOfType<Menu_Equipaggiamenti_sound>();
        sound.gameObject.GetComponent<AudioSource>().Stop();
        sound.stop = true;

        audioSource.Stop();
        int n = ButtonMenu.Length - 1;
        bool trovato = false;

        if (numButton > 0)
        {
            transform.localPosition -= new Vector3(0.0f, 65.0f, 0.0f);
        }

        if (numButton > 0)
        {
            numButton--;
            ButtonMenu[numButton].Select();
            if (ButtonMenu[numButton].tag == "Pozioni")
            {
                descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                descr_Pozioni.Descrizione();
            }
            else
            {
                descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                descr_potenziamenti.Descrizione();
            }
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
        else
        {
            if (ButtonMenu[n].interactable == true)
            {
                numButton = (ButtonMenu.Length - 1);
                transform.localPosition = posFin;
                ButtonMenu[numButton].Select();
                if (ButtonMenu[numButton].tag == "Pozioni")
                {
                    descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                    descr_Pozioni.Descrizione();
                }
                else
                {
                    descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                    descr_potenziamenti.Descrizione();
                }
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
            else
            {
                Vector3 posbutton = posFin;
                while (!trovato)
                {
                    n--;
                    posbutton -= new Vector3(0.0f, 65.0f, 0.0f);
                    if (ButtonMenu[n].interactable == true)
                    {
                        trovato = true;
                        numButton = n;
                        ButtonMenu[numButton].Select();
                        transform.localPosition = posbutton;
                        if (ButtonMenu[numButton].tag == "Pozioni")
                        {
                            descr_Pozioni = ButtonMenu[numButton].GetComponent<Descr_Pozioni>();
                            descr_Pozioni.Descrizione();
                        }
                        else
                        {
                            descr_potenziamenti = ButtonMenu[numButton].GetComponent<Descr_Potenziamenti>();
                            descr_potenziamenti.Descrizione();
                        }
                        audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
                    }
                }
            }
        }

    }

    public void PozioneVita ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_Pozioni.Costo)
		{
			if (PlayerPrefs.GetInt ("Vita") < 100)
			{
				audioSource.PlayOneShot (Usato);
				Invoke ("NomeOggetto",Usato.length + 0.3f);
				PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_Pozioni.Costo);
				PlayerPrefs.SetInt ("Vita",PlayerPrefs.GetInt ("Vita") + descr_Pozioni.Ripristino_Vita);

				if (PlayerPrefs.GetInt ("Vita") > 100)
				{
					PlayerPrefs.SetInt ("Vita",100);
				}
			}
			else 
			{
				int i = Random.Range (0, VitaAlMax.Length);
				audioSource.PlayOneShot (VitaAlMax[i]);
			}
		}
		else if (PlayerPrefs.GetInt ("Monete") < descr_Pozioni.Costo)
		{
			int y = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[y]);
		}
	}

	public void Riforgiatura ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_Pozioni.Costo)
		{
			if (PlayerPrefs.GetInt ("Armatura") < PlayerPrefs.GetInt ("ArmaturaMax"))
			{
				audioSource.PlayOneShot (Usato);
				Invoke ("NomeOggetto",Usato.length + 0.5f);
				PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_Pozioni.Costo);
				PlayerPrefs.SetInt ("Armatura",PlayerPrefs.GetInt ("Armatura") + ((PlayerPrefs.GetInt ("ArmaturaMax") * descr_Pozioni.Ripristino_Vita)/ 100));
				if(PlayerPrefs.GetInt ("Armatura") > PlayerPrefs.GetInt ("ArmaturaMax"))
					PlayerPrefs.SetInt ("Armatura", PlayerPrefs.GetInt ("ArmaturaMax"));
			}
			else 
			{
				int i = Random.Range (0, ArmaturaAlMax.Length);
				audioSource.PlayOneShot (ArmaturaAlMax[i]);
			}
		}
		else if (PlayerPrefs.GetInt ("Monete") < descr_Pozioni.Costo)
		{
			int y = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[y]);
		}
	}

	public void PotenziaVelocita ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_potenziamenti.Costo)
		{
			if(PlayerPrefs.GetInt ("PotenziamentoVelocita") == 0)
			{
				audioSource.PlayOneShot (Usato);
				Invoke ("NomeOggetto",Usato.length + 0.5f);
				PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_potenziamenti.Costo);
				PlayerPrefs.SetInt ("Perc_pot_Velocita",descr_potenziamenti.Aumento_Statistiche);
				PlayerPrefs.SetInt ("PotenziamentoVelocita",1);
			}
			// hai gia preso un potenziamento
			else
			{
				int i = Random.Range (0,GiaPotenziato.Length);
				audioSource.PlayOneShot (GiaPotenziato[i]);	
			}
		}
		//non hai i soldi
		else if (PlayerPrefs.GetInt ("Monete") < descr_Pozioni.Costo)
		{
			int y = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[y]);
		}
	}

	public void PotenziaDifesa ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_potenziamenti.Costo)
		{
			if(PlayerPrefs.GetInt ("PotenziamentoVelocita") == 0)
			{
				audioSource.PlayOneShot (Usato);
				Invoke ("NomeOggetto",Usato.length + 0.5f);
				PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_potenziamenti.Costo);
				PlayerPrefs.SetInt ("Perc_pot_Difesa",descr_potenziamenti.Aumento_Statistiche);
				PlayerPrefs.SetInt ("PotenziamentoDifesa",1);
			}
			else
			{
				int i = Random.Range (0,GiaPotenziato.Length);
				audioSource.PlayOneShot (GiaPotenziato[i]);	
			}
		}
		//non hai i soldi
		else if (PlayerPrefs.GetInt ("Monete") < descr_Pozioni.Costo)
		{
			int y = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[y]);
		}
	}

	void NomeOggetto ()
	{
		audioSource.PlayOneShot (nomeOggetti[numButton]);
	}
}
