using System;

namespace TIconBLU.Services{
	
	/// <summary>
	/// Interface for service locator pattern 
	/// </summary>
	public interface IServiceLocator
	{ 
		//by IOC
		IService Add<T>(IService service) where T:IService;
		IService Get<T>() where T:IService;
		IService Get(Type type);
		//by String name
		IService Add(String name, IService service);
		IService Get(String name);
		
		//both
		IService Add<T>(String name,IService service) where T:IService;
		
		void Remove(String name);
		void Remove(IService service);
		
		//remove all services
		void Clear();
	}
	
}