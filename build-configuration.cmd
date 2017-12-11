@echo off

%FAKE% %NYX% "target=clean" -st
%FAKE% %NYX% "target=RestoreNugetPackages" -st

IF NOT [%1]==[] (set RELEASE_NUGETKEY="%1")
IF NOT [%2]==[] (set RELEASE_TARGETSOURCE="%2")

SET SUMMARY="Contains contracts for DDD/CQRS development"
SET DESCRIPTION="Contains contracts for DDD/CQRS development"

%FAKE% %NYX% appName=Elders.Cronus.DomainModeling appSummary=%SUMMARY% appDescription=%DESCRIPTION% nugetserver=%NUGET_SOURCE_DEV_PUSH% nugetkey=%RELEASE_NUGETKEY%  nugetPackageName=Cronus.DomainModeling
IF errorlevel 1 (echo Faild with exit code %errorlevel% & exit /b %errorlevel%)
