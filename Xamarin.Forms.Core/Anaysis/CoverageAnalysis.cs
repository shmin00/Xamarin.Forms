using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public class CoverageAnalysis
	{
		static IDictionary<string, int> s_callee = null;

		public static void Start()
		{

#if NETSTANDARD2_0
			if (s_callee == null)
			{
				s_callee = new Dictionary<string, int>();
			}
			else
			{
				s_callee.Clear();
			}
			Init();
#else
            Log.Warning("Coverage", $"Not Supported");
#endif
		}

		public static void Stop()
		{
#if NETSTANDARD2_0
			if (s_callee == null)
				return;

			var covered = s_callee.Count(kv => kv.Value > 0);
			var coverage = ((double)covered / s_callee.Count) * 100;
			Log.Warning("Coverage", $"coverage: {covered}/{s_callee.Count}({coverage.ToString("N2")})");
			foreach (var kv in s_callee)
			{
				Log.Warning("Coverage", $"{kv.Key} {kv.Value}");
			}
#else
            Log.Warning("Coverage", $"Not Supported");
#endif
		}

#if NETSTANDARD2_0
		static void Init()
		{
			Log.Warning("Coverage", $"Initialized...");
			var asm = typeof(VisualElement).Assembly;
			var visualElementTypeInfo = typeof(VisualElement).GetTypeInfo();
			var cellTypeInfo = typeof(Cell).GetTypeInfo();
			var elements = from dt in asm.DefinedTypes
						   where (visualElementTypeInfo.IsAssignableFrom(dt) || cellTypeInfo.IsAssignableFrom(dt)) && !dt.IsInterface && !dt.IsAbstract
						   where dt.GetConstructors().Length > 0
						   orderby dt.FullName
						   select dt;

			foreach (var el in elements)
			{
				var methods = from m in el.GetMethods()
							  where m.GetCustomAttributes(typeof(EditorBrowsableAttribute), false).Length <= 0
							  where m.DeclaringType.Name != typeof(Object).Name
							  where m.DeclaringType.Name != typeof(BindableObject).Name
							  where m.DeclaringType.Name != typeof(Element).Name
							  where !m.Name.StartsWith("remove_")
							  orderby m.Name
							  select m;

				foreach (var m in methods)
				{
					var key = $"{m.ReflectedType.FullName}.{m.Name}";
					if (s_callee.ContainsKey(key))
					{
						s_callee[key]++;
					}
					else
					{
						s_callee[key] = 0;
					}
				}
			}
		}
#endif

		internal static void AddCallee(object instance, MethodBase method)
		{
			if (s_callee == null)
				return;

			if (instance is Xamarin.Forms.VisualElement || instance is Xamarin.Forms.Cell)
			{
				var key = $"{instance?.GetType().FullName}.{method?.Name}";
				if (s_callee.ContainsKey(key))
				{
					if (s_callee[key] < 1)
						Log.Warning("Coverage", $"{key}");

					s_callee[key]++;
				}
			}
		}
	}
}