// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./bin"
let dirsToClean = ["./out"; "./bin"; "./obj"]

// Targets
Target "Clean" (fun _ ->
    dirsToClean |> List.iter (fun d -> CleanDir d)
)

Target "BuildApps" (fun _ ->
    [
      ("./NBlast.Api/*.fsproj", "api"); 
      ("./NBlast.Fixtures.Console/*.fsproj", "fixtures")
    ] |> Seq.iter (fun app ->
       !! (fst app) |> MSBuildDebug (buildDir + "/" + (snd app)) "Build"  |> ignore
    ) |> ignore
)

Target "BuildTests" (fun _ ->
    !! "./**/*.fsproj"
      |> MSBuildDebug (buildDir + "/tests") "Build"
      |> Log "TestBuild-Output: "
)

Target "RunTests" (fun _ ->
    let testDlls = !! (buildDir + "/tests/*Tests.dll")
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
  ==> "BuildApps"
  ==> "BuildTests"
  ==> "RunTests"

// start build
RunTargetOrDefault "RunTests"