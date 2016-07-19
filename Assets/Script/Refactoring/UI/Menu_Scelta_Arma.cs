using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Menu_Scelta_Arma : MonoBehaviour 
{
	public Button[] ButtonMenu;
	private int numButton = -1;
	private PlayerStat playerStat;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip[] nomeOggetti;

	public AudioClip[] PocheMonete;
	public AudioClip equipaggiato;
	public AudioClip comprato_equipaggiato;

	private Vector3 posIni;
	private Vector3 posFin;

	private Descr_ogg descr_ogg;
	

	public AudioClip Equip;
	public AudioClip Buy;

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
            equipaggiato = Resources.Load<AudioClip>("Localization/English/Armour/FrasiArmature/Var 9");
            comprato_equipaggiato = Resources.Load<AudioClip>("Localization/English/Armour/FrasiArmature/Var 10");
            PocheMonete = AudioGestor.AudioClipArrayLoading("Localization/English/Armour/FrasiArmature/NoMoney");
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/Weapon/Armi");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/English/Weapon/SoloNomi");
        }
        else
        {
            equipaggiato = Resources.Load<AudioClip>("Localization/Italian/Weapon/FrasiArmi/Var 9");
            comprato_equipaggiato = Resources.Load<AudioClip>("Localization/Italian/Weapon/FrasiArmi/Var 10");
            PocheMonete = AudioGestor.AudioClipArrayLoading("Localization/Italian/Weapon/FrasiArmi/NoMoney");
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/Weapon/Armi");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/Italian/Weapon/SoloNomi");
        }
    }
	
	
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Tab))
		{
			int n = numButton + 1;
			if (ButtonMenu[n].interactable == true)
			{
				if (numButton > 0)
				{
					transform.localPosition += new Vector3 (0.0f,65.0f,0.0f);
				}
				audioSource.Stop ();
				if (numButton < (ButtonMenu.Length - 1))
				{
					numButton++;
					ButtonMenu[numButton].Select();
					descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
					descr_ogg.Descrizione();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
					
				}
				else 
				{
					numButton = 0;
					transform.localPosition = posIni;
					ButtonMenu[numButton].Select ();
					descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
					descr_ogg.Descrizione();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
			}
			else
			{
				numButton = 0;
				transform.localPosition = posIni;
				ButtonMenu[numButton].Select ();
				descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
				descr_ogg.Descrizione();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}

		}
		
		if(Input.GetKeyDown (KeyCode.LeftShift))
		{
			int n = ButtonMenu.Length - 1;
			bool trovato = false;

			if (numButton > 0)
			{
				transform.localPosition -= new Vector3 (0.0f,65.0f,0.0f);
			}

			audioSource.Stop ();
			if (numButton > 0)
			{
				numButton--;
				ButtonMenu[numButton].Select ();
				descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
				descr_ogg.Descrizione();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			else 
			{
				if (ButtonMenu[n].interactable == true)
				{
					numButton = (ButtonMenu.Length - 1);
					transform.localPosition = posFin;
					ButtonMenu[numButton].Select ();
					descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
					descr_ogg.Descrizione();
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
							descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
							descr_ogg.Descrizione();
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

        int n = numButton + 1;
        if (ButtonMenu[n].interactable == true)
        {
            if (numButton > 0)
            {
                transform.localPosition += new Vector3(0.0f, 65.0f, 0.0f);
            }
            audioSource.Stop();
            if (numButton < (ButtonMenu.Length - 1))
            {
                numButton++;
                ButtonMenu[numButton].Select();
                descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
                descr_ogg.Descrizione();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);

            }
            else
            {
                numButton = 0;
                transform.localPosition = posIni;
                ButtonMenu[numButton].Select();
                descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
                descr_ogg.Descrizione();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
        }
        else
        {
            numButton = 0;
            transform.localPosition = posIni;
            ButtonMenu[numButton].Select();
            descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
            descr_ogg.Descrizione();
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

        int n = ButtonMenu.Length - 1;
        bool trovato = false;

        if (numButton > 0)
        {
            transform.localPosition -= new Vector3(0.0f, 65.0f, 0.0f);
        }

        audioSource.Stop();
        if (numButton > 0)
        {
            numButton--;
            ButtonMenu[numButton].Select();
            descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
            descr_ogg.Descrizione();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
        else
        {
            if (ButtonMenu[n].interactable == true)
            {
                numButton = (ButtonMenu.Length - 1);
                transform.localPosition = posFin;
                ButtonMenu[numButton].Select();
                descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
                descr_ogg.Descrizione();
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
                        descr_ogg = ButtonMenu[numButton].GetComponent<Descr_ogg>();
                        descr_ogg.Descrizione();
                        audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
                    }
                }
            }
        }
    }

    public void EquipaggiaArma ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_ogg.Costo && ! PlayerPrefs.HasKey (descr_ogg.playerprefs))
		{

			PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_ogg.Costo);
			PlayerPrefs.SetInt (descr_ogg.playerprefs,1);
			PlayerPrefs.SetFloat("PesoArma", descr_ogg.Peso);
			PlayerPrefs.SetInt("AttaccoMin", descr_ogg.AttaccoMin);
			PlayerPrefs.SetInt("AttaccoMax", descr_ogg.AttaccoMax);
			StartCoroutine ("BuyEquip");
		}

		else if (PlayerPrefs.HasKey (descr_ogg.playerprefs))
		{
			PlayerPrefs.SetFloat("PesoArma", descr_ogg.Peso);
			PlayerPrefs.SetInt("AttaccoMin", descr_ogg.AttaccoMin);
			PlayerPrefs.SetInt("AttaccoMax", descr_ogg.AttaccoMax);
			StartCoroutine ("Equipp");

		}
		else if (PlayerPrefs.GetInt ("Monete") < descr_ogg.Costo)
		{
			int i = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[i]);
		}
	}

	public IEnumerator BuyEquip()
	{
		audioSource.PlayOneShot (Buy);
		yield return new WaitForSeconds(Buy.length);
		audioSource.PlayOneShot (comprato_equipaggiato);
		yield return new WaitForSeconds(comprato_equipaggiato.length + 0.2f);
		audioSource.PlayOneShot (nomeOggetti[numButton]);
	}
	public IEnumerator Equipp()
	{
		audioSource.PlayOneShot (Equip);
		yield return new WaitForSeconds(Equip.length);
		audioSource.PlayOneShot (equipaggiato);
		yield return new WaitForSeconds(equipaggiato.length + 0.2f);
		audioSource.PlayOneShot (nomeOggetti[numButton]);
	}
}

