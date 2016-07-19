using System;
using System.IO;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// 
/// 
/// </summary>

public class LauncherUtils  {
	
	#region Const
		//const string 	
	#endregion
	
	#region Properties
		//registry wrapper
		private ILauncherRegistry _registry;
		public  ILauncherRegistry registry {get{
			if(_registry==null){
				#if UNITY_STANDALONE_WIN
						_registry = new LauncherRegistry_WIN();
				#endif
				#if UNITY_STANDALONE_OSX
						//_registry = new LauncherRegistry_MAC();
				#endif			
			}
			return _registry;
		}}
		
		//web server url
		private String  webServerURL;
	#endregion
	
	
	#region Setup
		/// =====================================================================
		/// <summary>
		/// SetUp
		/// </summary>
		/// =====================================================================
		public void SetUp(String localRegistryPath,String webServerUrl){
			try{
			registry.SetUp(localRegistryPath);
			}catch(Exception error){}
			webServerURL = webServerUrl;
		}
	
	#endregion
	
	#region Run Methods
		/// =====================================================================
		/// <summary>
		/// Runs external application by filename
		/// </summary>
		/// =====================================================================
		public bool RunExternalApplication(String fileName){
			//run external process. todo: check Mac OSX compatibility
			FileInfo applicationFileInfo = new FileInfo(fileName);
			System.Diagnostics.Process.Start(applicationFileInfo.FullName);
		
			return true; //to check success
		}
		
		/// =====================================================================
		/// <summary>
		/// Runs external application by registry key
		/// </summary>
		/// =====================================================================
		public bool RunExternalApplicationFromRegistry(String key){
			String applicationPath = registry.Read(key);
			if(String.IsNullOrEmpty(applicationPath)) return false;
			return RunExternalApplication(applicationPath);
		}
	
		/// =====================================================================
		/// <summary>
		/// Return true if registry key's not empty
		/// </summary>
		/// =====================================================================
		public bool HasExternalApplicationFromRegistry(String key){
			String applicationPath = registry.Read(key);
			if(String.IsNullOrEmpty(applicationPath)) return false;
			return true;
		}
	#endregion
	
	#region Versions Methods
		/// =====================================================================
		/// <summary>
		/// Return true if versionA > versionB
		/// </summary>
		/// =====================================================================
		public bool VersionsCompareGreater(String versionA,String versionB){
			if(String.IsNullOrEmpty(versionA)) return false;
			if(String.IsNullOrEmpty(versionB)) return false;
		
			float vAf = float.Parse(versionA);
			float vBf = float.Parse(versionB);
			return vAf<vBf;
		}	
	#endregion
	
	
	#region WWW
		public void GetServerValue(String key,WWWUtils.WWWResponseCallback callback){
			WWWForm wwwFormData = new WWWForm();
			wwwFormData.AddField("CLASS_NAME"      ,"Registry");
			wwwFormData.AddField("FUNCTION_NAME"   ,"GetValue");
			wwwFormData.AddField("FUNCTION_PARAM_0",key);
			
			WWWUtils.CreateRequest(webServerURL,wwwFormData,callback);
		}	
	#endregion
	
	
}


public interface ILauncherRegistry{
	void   SetUp(String registryPath);
	
	String Read(String key);
	void   Write(String key,String val);
}

#if UNITY_STANDALONE_WIN
public class LauncherRegistry_WIN:ILauncherRegistry  {
	
	#region Properties
		private Microsoft.Win32.RegistryKey registryPath    = Microsoft.Win32.Registry.LocalMachine;
		private String                      registrySubPath = "SOFTWARE\\" + "LAUNCHER";
	#endregion
	
	#region Methods
		/// =====================================================================
		/// <summary>
		/// Sets up registry information
		/// </summary>
		/// =====================================================================
		public void SetUp(String registryPath){
			registrySubPath  = registryPath;
		}
		
		/// =====================================================================
		/// <summary>
		/// Read 
		/// </summary>
		/// ===================================================================== 
		public String Read(String key){
		    Microsoft.Win32.RegistryKey rk  = registryPath;
		    Microsoft.Win32.RegistryKey sk  = rk.OpenSubKey(registrySubPath);

			if ( sk == null ) return null;
	
	        try{
	            return (string)sk.GetValue(key.ToUpper());
	        }catch (Exception e){
	            Debug.Log( "Reading registry " + key.ToUpper()+" "+e);
	        }
			
			return null;
		}
	
	
		/// =====================================================================
		/// <summary>
		/// Write 
		/// </summary>
		/// ===================================================================== 
		public void Write(String key,String val){
			try{
				//Debug.Log("WriteStep0|"+registryPath+"|"+registrySubPath+"|"+key+"|"+val);
			    Microsoft.Win32.RegistryKey rk = registryPath ;
			    Microsoft.Win32.RegistryKey sk = rk.CreateSubKey(registrySubPath);
			    sk.SetValue(key.ToUpper(), val,Microsoft.Win32.RegistryValueKind.ExpandString);
			}catch (Exception e){
			    Debug.Log( "Writing registry " + key.ToUpper()+" "+e);
			}		
		}
	#endregion
}
#endif

#if UNITY_STANDALONE_OSX
	//todo: save/load a .plist file under ~/Library/Preferences/ 


#endif


