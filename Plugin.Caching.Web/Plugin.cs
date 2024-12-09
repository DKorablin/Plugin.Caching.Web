using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Interface.Caching;
using SAL.Flatbed;

namespace Plugin.Caching.Web
{
	public class Plugin : IPlugin, ICachePlugin
	{
		private TraceSource _trace;

		internal TraceSource Trace => this._trace ?? (this._trace = Plugin.CreateTraceSource<Plugin>());

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			if(WebCacheModule.IsValid)
				return true;
			else
			{
				this.Trace.TraceEvent(TraceEventType.Error, 10, "Plugin {0} requires {1}", this, typeof(System.Web.Caching.Cache));
				return false;
			}
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
			=> true;

		Int32 ICachePlugin.Count => 1;

		ICacheModule ICachePlugin.this[String name] => WebCacheModule.Instance;

		Boolean ICachePlugin.Remove(String name)
		{
			this.Trace.TraceEvent(TraceEventType.Warning, 5, "HttpCache plugin does not support Remove operation. Instance '{0}' still active.", name);
			return false;
		}

		public IEnumerator<ICacheModule> GetEnumerator()
		{
			yield return WebCacheModule.Instance;
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}