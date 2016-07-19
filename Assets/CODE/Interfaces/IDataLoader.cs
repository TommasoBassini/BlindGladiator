using System;
using TIconBLU.Services;

/// <summary>
/// Load an object from files 
/// </summary>
public interface IDataLoader : IService
{
	 T Load<T>(String filePath);
}


