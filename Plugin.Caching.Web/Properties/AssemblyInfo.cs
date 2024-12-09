using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("f4664df2-87f3-4bb6-a271-ff0da4d4fa2a")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.Caching.Web")]
#else

[assembly: AssemblyTitle("Plugin.Caching.Web")]
[assembly: AssemblyDescription("System.Web.Caching.Cache is used for caching")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.Caching.Web")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2024")]
#endif