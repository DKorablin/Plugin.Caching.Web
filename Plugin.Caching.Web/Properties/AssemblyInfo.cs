using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("f4664df2-87f3-4bb6-a271-ff0da4d4fa2a")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.Caching.Web")]
#else

[assembly: AssemblyDescription("System.Web.Caching.Cache is used for caching")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2024")]
#endif