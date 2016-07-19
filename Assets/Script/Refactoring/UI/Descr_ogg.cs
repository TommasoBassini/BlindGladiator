using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Descr_ogg : MonoBehaviour 
{
	public Text text;
	public Text descrizione_arma;
	public string attacco;
	public string peso;
	public string costo;
	public string descrizione;
	public string playerprefs;

	public int AttaccoMin;
	public int AttaccoMax;
	public float Peso;
	public int Costo;
	
	public string GruppoSblocco;
	private Button button;

	void Start ()
	{
		button = this.gameObject.GetComponent<Button>();
		if (PlayerPrefs.HasKey (GruppoSblocco))
			button.interactable = true;
		playerprefs = this.gameObject.name;
	}

	public void Descrizione ()
	{
		text.text = ("Attack " + attacco + "\n" + "weight " + peso + "\n" + "Price " + costo + "\n") ;
		text.text.Replace ("\n", "\n");
		descrizione_arma.text = descrizione;
	}
	
}
