using UnityEngine;
using System.Collections;

public class Save_Scene_Load : MonoBehaviour 
{
	public int NScenaDaCaricare;

	void Start () 
	{
		PlayerPrefs.SetInt ("ScenaDaCaricare",NScenaDaCaricare);
	}

}
