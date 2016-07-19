using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

public class SecurityUtils
{
	//LOOK AT SUBVERSIONS.xml!! it override this vars
	static public String PRODUCTCHAR    = "E";
	static public String PRODUCTCODE    = "8";
	static public String PRODUCTVERSION = "2";	
	static public String KEY            = "3,6,7,1,4,8,3,6,5,0";
	
	///===================================================================================
	/// <summary>
	/// Gets the device unique ID.
	/// </returns>
	static public String GetDeviceUniqueID(){
		String result = SystemInfo.deviceUniqueIdentifier;
#if UNITY_STANDALONE_WIN
		result ="";
		try{
			result = GetMacAddress();
		}catch(Exception error){
			result = SystemInfo.deviceUniqueIdentifier;//no network card
		} 
#endif
		
#if UNITY_STANDALONE_OSX
		result = SystemInfo.deviceUniqueIdentifier; //ci sono altri sistemi?
#endif
		return result;
	}

	static public void SetProductInfo(string p_char,string p_version, string p_code, string p_key){
		PRODUCTCHAR = p_char;
		PRODUCTVERSION = p_version;
		PRODUCTCODE = p_code;
		KEY = p_key;
	}

	///===================================================================================
	/// <summary>
	/// Gets the user unique ID ( -- COPYCODE -- ) from device unique ID.
	/// </returns>
	static public String GetUserUniqueID(){
	
		String str="";		
		str = GetDeviceUniqueID();//SystemInfo.deviceUniqueIdentifier;
		
				System.Security.Cryptography.MD5 hash = System.Security.Cryptography.MD5.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                str = Convert.ToBase64String(hash.Hash);
		
		char[] fc = str.ToCharArray();
		str = ""+Clamp(fc[0])+Clamp(fc[1])+Clamp(fc[3])+Clamp(fc[6])+Clamp(fc[10])+Clamp(fc[14])+Clamp(fc[12])+PRODUCTCHAR;
		return str.ToUpper();		
	}
	
	///===================================================================================
	/// <summary>
	/// AUX:_ clamp char to A-Z
	/// </summary>
	static private char Clamp(char charToClamp){
		int cI = (int)charToClamp;
		cI=cI%10+48;
		cI=(int)Mathf.Clamp(cI,48,57);	
		return (char)cI;
	}
	
	///===================================================================================
	/// <summary>
	/// Gets the user code from user unique id ( -- COPYCODE -- )
	/// </summary>
	static public String GetUserCode(String CopyCode)
    {
        int I = 0, IC = 0, IK = 0;
        byte C;
        int Digits;

        //if (CopyCode.Length!=10) return "ERRORE";

        String result = "";
        Digits = 10;
        IC = CopyCode.Length;
        Char[] CopyCodeCharArray = CopyCode.ToCharArray();
        String[] EncryptKeyCharArray = KEY.Split(',');

        IK = ((int)CopyCodeCharArray[7]) % (EncryptKeyCharArray.Length);

        for (I = 0; I < Digits; I++)
        {
            int cci = (int)CopyCodeCharArray[IC - 1]; //ord(CopyCode[IC]
            int eki = int.Parse("" + EncryptKeyCharArray[IK]); //EncryptKey[IK]
            C = (byte)((cci + eki) % 10 + 48);
            Char CharC = System.Convert.ToChar(C);

            IC--;
            IK++;
            if (IC == 0) IC = CopyCode.Length;
            if (IK > EncryptKeyCharArray.Length - 1) IK = 0;
            result = result + CharC;
        }
        return result+PRODUCTCODE+PRODUCTVERSION;
    }
	

	///===================================================================================
	/// <summary>
	/// Gets MAC address (unity device unique id is deprecated)
	/// </summary>
	static public String GetMacAddress(){
		IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
		String info="";
        foreach (NetworkInterface adapter in nics)
        {
            PhysicalAddress address = adapter.GetPhysicalAddress();
            byte[] bytes = address.GetAddressBytes();
         string mac = null;
            for (int i = 0; i < bytes.Length; i++)
            {
                mac = string.Concat(mac +(string.Format("{0}", bytes[i].ToString("X2"))));
                if (i != bytes.Length - 1)
                {
                    mac = string.Concat(mac + "-");
                }
            }
         info += mac + "\n";

            info += "\n";
        }
       return info;
	}
	
	///===================================================================================
	static public void Test(){
		Debug.Log(GetUserUniqueID());
		
		Debug.Log(GetUserCode(GetUserUniqueID()));
	}
	
}


