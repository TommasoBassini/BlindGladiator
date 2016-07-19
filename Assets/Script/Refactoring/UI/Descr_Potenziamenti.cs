using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Descr_Potenziamenti : MonoBehaviour 
{

	public Text text;
	public Text descrizione_potenziamento;
	
	public string AumentoStatistiche;
	public string potenziamento;
	public string costo;
	public string descrizione;
	
	public int Aumento_Statistiche;
	public int Costo; 

	public void Descrizione ()
	{
		text.text = ("Potenzamento " + potenziamento + " " + AumentoStatistiche + "%" + "\n" + "Costo " + costo + "\n") ;
		text.text.Replace ("\n", "\n");
		descrizione_potenziamento.text = descrizione;
	}
}
