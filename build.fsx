// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./bin/Debug"
let dirsToClean = ["./bin/"; "./obj/"]

// Targets
Target "Clean" (fun _ ->
    dirsToClean |> List.iter (fun d -> CleanDir d)
)

Target "BuildApp" (fun _ ->
    !! "./NBlast.Api/*.fsproj" ++
        "./NBlast.Api/*.fsproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildTests" (fun _ ->
    !! "./**/*.fsproj"
      |> MSBuildDebug buildDir "Build"
      |> Log "TestBuild-Output: "
)

Target "RunTests" (fun _ ->
    let testDlls = !! (buildDir + "/*Tests.dll")
    testDlls
        |> xUnit (fun p -> 
            {p with 
                ToolPath = "./packages/xunit.runners/tools/xunit.console.clr4.exe";
                ShadowCopy = false;
                HtmlOutput = false;
                XmlOutput = false;
                OutputDir = buildDir })
)

Target "Default" (fun _ ->
    trace "Hello World from FAKE"
)

// Dependencies
"Clean"
  ==> "BuildTests"
  ==> "RunTests"

// start build
RunTargetOrDefault "RunTests"