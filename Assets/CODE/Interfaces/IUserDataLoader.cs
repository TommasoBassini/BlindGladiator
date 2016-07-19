using System;
using TIconBLU.Services;

/// <summary>
/// Load an object from playerPref 
/// </summary>
public interface IUserDataLoader : IService
{
	 T Load<T>(String varName);
	 void Save<T>(String varName,T obj);
}


