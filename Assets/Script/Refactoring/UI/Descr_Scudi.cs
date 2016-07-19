using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Descr_Scudi : MonoBehaviour 
{
	
	public Text text;
	public Text descrizione_scudo;

	public string BloccoScudo;
	public string peso;
	public string costo;
	public string descrizione;
	public string playerprefs;

	public float Peso;
	public int PercBloccoScudo;
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
		text.text = ("Protection of "+ BloccoScudo + "%" + "\n" + "weight " + peso + "\n" + "Price " + costo + "\n") ;
		text.text.Replace ("\n", "\n");
		descrizione_scudo.text = descrizione;
	}
}