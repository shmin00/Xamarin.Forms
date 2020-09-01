using System;
using System.Reflection;
using Xamarin.Forms;

[module: Interceptor]

namespace Xamarin.Forms
{
	[AttributeUsage(AttributeTargets.Module)]
	class InterceptorAttribute : Attribute
	{
		MethodBase _method;
		object _instance;

		public void Init(object instance, MethodBase method)
		{
			_instance = instance;
			_method = method;
		}

		public void OnEntry()
		{
			if(_instance != null && _method != null)
				CoverageAnalysis.AddCallee(_instance, _method);
		}
	}
}