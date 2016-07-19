using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Menu_Scelta_Armature : MonoBehaviour 
{
	public Button[] ButtonMenu;
	public int numButton = -1;
	private PlayerStat playerStat;
	private AudioSource audioSource;
	public AudioClip[] AudioSpiegazioni;
	public AudioClip[] nomeOggetti;

	public AudioClip[] PocheMonete;
	public AudioClip equipaggiato;
	public AudioClip comprato_equipaggiato;

	private Vector3 posIni;
	private Vector3 posFin;
	public int monete;
	private Descr_Armature descr_Armature;

	public AudioClip Equip;
	public AudioClip Buy;

	public bool back_menu = false;
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
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/English/Armour/Armature");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/English/Armour/SoloNomi");
        }
        else
        {
            equipaggiato = Resources.Load<AudioClip>("Localization/Italian/Armour/FrasiArmature/Var 9");
            comprato_equipaggiato = Resources.Load<AudioClip>("Localization/Italian/Armour/FrasiArmature/Var 10");
            PocheMonete = AudioGestor.AudioClipArrayLoading("Localization/Italian/Armour/FrasiArmature/NoMoney");
            AudioSpiegazioni = AudioGestor.AudioClipArrayLoading("Localization/Italian/Armour/Armature");
            nomeOggetti = AudioGestor.AudioClipArrayLoading("Localization/Italian/Armour/SoloNomi");
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
				audioSource.Stop ();
				if (numButton < (ButtonMenu.Length - 1))
				{
					numButton++;
					descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
					ButtonMenu[numButton].Select ();
					descr_Armature.Descrizione();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
					
				}
				else 
				{
					numButton = 0;
					transform.localPosition = posIni;
					ButtonMenu[numButton].Select ();
					descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
					descr_Armature.Descrizione();
					audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
				}
			}
			else
			{
				numButton = 0;
				transform.localPosition = posIni;
				ButtonMenu[numButton].Select ();
				descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
				descr_Armature.Descrizione();
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
				descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
				descr_Armature.Descrizione();
				audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
			}
			else 
			{
				if (ButtonMenu[n].interactable == true)
				{
					numButton = (ButtonMenu.Length - 1);
					transform.localPosition = posFin;
					ButtonMenu[numButton].Select ();
					descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
					descr_Armature.Descrizione();
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
							descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
							descr_Armature.Descrizione();
							audioSource.PlayOneShot (AudioSpiegazioni[numButton]);
						}
					}
				}
			}
		}
	
	}


	public void EquipaggiaArmatura ()
	{
		audioSource.Stop ();
		if(PlayerPrefs.GetInt ("Monete") > descr_Armature.Costo && ! PlayerPrefs.HasKey (descr_Armature.playerprefs))
		{
			PlayerPrefs.SetInt ("Monete",PlayerPrefs.GetInt ("Monete") - descr_Armature.Costo);
			PlayerPrefs.SetInt (descr_Armature.playerprefs,1);
			PlayerPrefs.SetFloat("PesoArmatura", descr_Armature.Peso);
			PlayerPrefs.SetInt ("Armatura", descr_Armature.Armatura);
			PlayerPrefs.SetInt("ArmaturaMax",descr_Armature.Armatura);
			StartCoroutine ("BuyEquip");
		}
		else if (PlayerPrefs.HasKey (descr_Armature.playerprefs))
		{
			PlayerPrefs.SetFloat("PesoArmatura", descr_Armature.Peso);
			PlayerPrefs.SetInt("Armatura", descr_Armature.Armatura);
			StartCoroutine ("Equipp");
		}
		else if (PlayerPrefs.GetInt ("Monete") < descr_Armature.Costo)
		{
			int i = Random.Range (0,PocheMonete.Length);
			audioSource.PlayOneShot (PocheMonete[i]);
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
            audioSource.Stop();
            if (numButton < (ButtonMenu.Length - 1))
            {
                numButton++;
                descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
                ButtonMenu[numButton].Select();
                descr_Armature.Descrizione();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);

            }
            else
            {
                numButton = 0;
                transform.localPosition = posIni;
                ButtonMenu[numButton].Select();
                descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
                descr_Armature.Descrizione();
                audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
            }
        }
        else
        {
            numButton = 0;
            transform.localPosition = posIni;
            ButtonMenu[numButton].Select();
            descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
            descr_Armature.Descrizione();
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
            descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
            descr_Armature.Descrizione();
            audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
        }
        else
        {
            if (ButtonMenu[n].interactable == true)
            {
                numButton = (ButtonMenu.Length - 1);
                transform.localPosition = posFin;
                ButtonMenu[numButton].Select();
                descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
                descr_Armature.Descrizione();
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
                        descr_Armature = ButtonMenu[numButton].GetComponent<Descr_Armature>();
                        descr_Armature.Descrizione();
                        audioSource.PlayOneShot(AudioSpiegazioni[numButton]);
                    }
                }
            }
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