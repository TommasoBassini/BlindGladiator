﻿using UnityEngine;
using System.Collections;

public class Menu_Titolo : MonoBehaviour 
{

	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Application.LoadLevel (1);
	}
}
