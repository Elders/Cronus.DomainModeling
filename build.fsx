#I @"./bin/tools/FAKE/tools/"
#r @"./bin/tools/FAKE/tools/FakeLib.dll"
#load @"./bin/tools/SourceLink.Fake/tools/SourceLink.fsx"

open Fake
open Fake.Git
open Fake.FSharpFormatting
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open System
open System.IO

let applicationName = "Elders.Cronus.DomainModeling"
let buildDir = @"./bin/Release" @@ applicationName
let release = LoadReleaseNotes "RELEASE_NOTES.md"

let projectName = "Cronus.DomainModeling"
let projectSummary = "Domain modeling for Cronus"
let projectDescription = "Domain modeling for Cronus"
let projectAuthors = ["Nikolai Mynkow"; "Simeon Dimov"; "Blagovest Petrov";]

let packages = ["Cronus.DomainModeling", projectDescription]
let nugetDir = "./bin/nuget"

Target "Clean" (fun _ -> CleanDirs [buildDir])

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo @"./src/Elders.Cronus.DomainModeling/Properties/AssemblyInfo.cs"
           [Attribute.Title "Elders.Cronus.DomainModeling"
            Attribute.Description "Elders.Cronus.DomainModeling"
            Attribute.Product "Elders.Cronus.DomainModeling"
            Attribute.Version release.AssemblyVersion
            Attribute.InformationalVersion release.AssemblyVersion
            Attribute.FileVersion release.AssemblyVersion]
)

Target "Build" (fun _ ->
    !! @"./src/*.sln" 
        |> MSBuildRelease buildDir "Build"
        |> Log "Build-Output: "
)

Target "CreateNuGet" (fun _ ->
    for package,description in packages do
    
        let nugetToolsDir = nugetDir @@ "lib" @@ "net40-full"
        CleanDir nugetToolsDir

        match package with
        | p when p = projectName ->
            CopyDir nugetToolsDir buildDir allFiles
        
        let nuspecFile = package + ".nuspec"
        NuGet (fun p ->
            {p with
                Authors = projectAuthors
                Project = package
                Description = description
                Version = release.NugetVersion
                Summary = projectSummary
                ReleaseNotes = release.Notes |> toLines
                AccessKey = getBuildParamOrDefault "nugetkey" ""
                Publish = hasBuildParam "nugetkey"
                ToolPath = "./tools/NuGet/nuget.exe"
                OutputPath = nugetDir
                WorkingDir = nugetDir 
                SymbolPackage = NugetSymbolPackage.Nuspec }) nuspecFile
)

Target "Release" (fun _ ->
    StageAll ""
    let notes = String.concat "; " release.Notes
    Commit "" (sprintf "Bump version to %s. %s" release.NugetVersion notes)
    Branches.push ""

    Branches.tag "" release.NugetVersion
    Branches.pushTag "" "origin" release.NugetVersion
)

// Dependencies
"Clean"
    ==> "AssemblyInfo"
    ==> "Build"
    ==> "CreateNuGet"
    ==> "Release"
 
// start build
RunParameterTargetOrDefault "target" "Build"