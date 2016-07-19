using UnityEngine;
using System.Collections;

public class PlayerStat : MonoBehaviour 
{
	private int Vita = 100;
	private float Velocita = 0.25f;
	private float VelocitaPlayer;
	private int AttaccoMin;
	private int AttaccoMax;

	private int BloccoScudo;
	
	private float PesoPlayer;
	private int Armatura;

    public bool isEnglish;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void OnLevelWasLoaded() 
	{
		PlayerPrefs.SetInt("VitaMax",100);
		BloccoScudo = PlayerPrefs.GetInt("BloccoScudo");
		AttaccoMin = PlayerPrefs.GetInt("AttaccoMin");
		AttaccoMax = PlayerPrefs.GetInt("AttaccoMax");
		Armatura = PlayerPrefs.GetInt ("Armatura");
		PesoPlayer = (PlayerPrefs.GetFloat ("PesoArmatura") + PlayerPrefs.GetFloat ("PesoScudo") + PlayerPrefs.GetFloat ("PesoSpada"));
		Velocita = 0.25f - (PesoPlayer/ 1000);
		PlayerPrefs.SetFloat ("PesoTotale",PlayerPrefs.GetFloat ("PesoArmatura") + PlayerPrefs.GetFloat ("PesoScudo") + PlayerPrefs.GetFloat ("PesoSpada"));
		PlayerPrefs.SetFloat ("Velocita",Velocita);

		Debug.Log ("Monete : " + PlayerPrefs.GetInt ("Monete"));
		Debug.Log ( "pot vel : " +PlayerPrefs.GetInt ("PotenziamentoVelocita"));

	

	}
}
