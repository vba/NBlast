
mono ".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "NLog" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "NLog.Contrib" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "FSharp.Core.3" "-OutputDirectory" "packages" "-ExcludeVersion"

mono ".nuget\NuGet.exe" "Install" "xUnit" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "xUnit.Runners" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "FluentAssertions" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "AutoFixture" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "Topshelf" "-OutputDirectory" "packages" "-ExcludeVersion"

mono ".nuget\NuGet.exe" "Install" "Microsoft.AspNet.WebApi.owinSelfHost" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "Lucene.Net" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "Lucene.Net.Contrib" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "FSharp.Collections.ParallelSeq" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "Unity" "-OutputDirectory" "packages" "-ExcludeVersion"
mono ".nuget\NuGet.exe" "Install" "AutoFixture" "-OutputDirectory" "packages" "-ExcludeVersion"