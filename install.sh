
mono ".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "NLog" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "NLog.Contrib" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "FSharp.Core.3" "-OutputDirectory" "packages" "-ExcludeVersion"


#mono ".nuget\NuGet.exe" "Install" "Microsoft.Net.Http" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.AspNet.WebApi.Core" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.AspNet.WebApi.Owin" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.Owin.Host.HttpListener" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.Owin.Hosting" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Owin" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Nancy.Owin" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Nancy" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.AspNet.WebApi.Client" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Newtonsoft.Json" "-OutputDirectory" "packages" "-ExcludeVersion"
#mono ".nuget\NuGet.exe" "Install" "Microsoft.Owin" "-OutputDirectory" "packages" "-ExcludeVersion"

mono ".nuget\NuGet.exe" "Install" "xUnit" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "xUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "FluentAssertions" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "AutoFixture" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "Topshelf" "-OutputDirectory" "packages" "-ExcludeVersion"


mono ".nuget\NuGet.exe" "Install" "Microsoft.AspNet.WebApi.owinSelfHost" "-OutputDirectory" "packages" "-ExcludeVersion"