using System;
using System.Collections.Generic;

namespace TIconBLU.Services{
	
	/// <summary>
	/// Service Locator implementation
	/// </summary>
	class ServiceLocator : IServiceLocator
	{ 
		#region Properties
			private IDictionary<object, IService> services;
		#endregion
		
		#region Constructors
			internal ServiceLocator()
			{
				services   =  new Dictionary<object, IService>();
			}	
		#endregion	
		
		#region by IOC Methods
			//===========================================================================
			/// <summary>
			/// Add Service
			/// </summary>
			public IService Add<T>(IService service) where T:IService{
				if(this.services.ContainsKey(typeof(T))) return this.services[typeof(T)];
			
				this.services.Add(typeof(T),service);
				return service;
			}
			
			//===========================================================================
			/// <summary>
			/// Get Service
			/// </summary>
			public IService Get<T>() where T:IService{
				try{return (T)services[typeof(T)];}
				catch (KeyNotFoundException error){
					UnityEngine.Debug.LogError( error );
					throw new ApplicationException("The requested service is not registered"+typeof(T).Name);
				}
			}
		#endregion
		
		#region by Hash Methods
			//===========================================================================
			/// <summary>
			/// Add Service
			/// </summary>
			/// <param name="name">
			/// name of service <see cref="String"/>
			/// </param>
			/// <param name="service">
			/// the service <see cref="IService"/>
			/// </param>
			/// <returns>
			/// the service, itself <see cref="IService"/>
			/// </returns>
			public IService Add(String name, IService service){
				if(this.services.ContainsKey(name)) return this.services[name];
				this.services[name] = service;
				return service;
			}
			
			//===========================================================================
			/// <summary>
			/// Get Service by name
			/// </summary>
			/// <param name="type">
			/// name of service <see cref="Type"/>
			/// </param>
			/// <returns>
			/// the service, if exists <see cref="IService"/>
			/// </returns>
			public IService Get(String name){
				try{return this.services[name];}
				catch (KeyNotFoundException error ){
					UnityEngine.Debug.LogError(error );
					throw new ApplicationException("The requested service ("+name+") is not registered");
				}
			}
		
			//===========================================================================
			/// <summary>
			/// Get service by type parameter
			/// </summary>
			/// <param name="type">
			/// class type of service <see cref="Type"/>
			/// </param>
			/// <returns>
			/// the service, if exists <see cref="IService"/>
			/// </returns>
			public IService Get(Type type){
				if(type==null){
					UnityEngine.Debug.Log("AAA");
				}
				//Trace.Log("*******"+type);
				try{return services[type];}
				catch (KeyNotFoundException error ){
					UnityEngine.Debug.LogError(error );
					throw new ApplicationException("The requested service is not registered");
				}
			}

			//===========================================================================
			/// <summary>
			/// Remove a service by name 
			/// </summary>
			/// <param name="name">
			/// the name of service to remove <see cref="String"/>
			/// </param>
			public void Remove(String name){
				if(this.services.ContainsKey(name)) this.services.Remove((object)name);
			}
		
			//===========================================================================
			/// <summary>
			/// Remove service 
			/// </summary>
			/// <param name="service">
			/// the service to remove <see cref="IService"/>
			/// </param>
			public void Remove(IService service){
				//if(this.services.Contains(service)) this.services.Remove((object)(service.GetType()));
			}
		
			//===========================================================================
			/// <summary>
			/// Add service passing name and an already instantiated IService  
			/// </summary>
			/// <param name="name">
			/// name of service <see cref="String"/>
			/// </param>
			/// <param name="service">
			/// the service <see cref="IService"/>
			/// </param>
			/// <returns>
			/// the service itself <see cref="IService"/>
			/// </returns>
			public IService Add<T>(String name,IService service) where T:IService{
				Add<T>(service);
				Add(name,service);
				return service;
			}
		
			
		#endregion
		
		#region Aux
			//===========================================================================
			/// <summary>
			/// Clear, removing all services 
			/// </summary>
			public void Clear(){
				/*
					//todo: call delete of old Iservices?
					foreach(IService service in services.Values){
						//service = null;
					}
				*/
				services = new Dictionary<object, IService>();
			}	
		#endregion
	}
}