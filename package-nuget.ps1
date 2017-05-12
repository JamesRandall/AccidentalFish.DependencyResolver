Param(
	[switch]$pushLocal,
	[switch]$pushNuget,
	[switch]$cleanup
)

if (Test-Path -Path nuget-powershell) 
{
	rmdir nuget-powershell -Recurse
}
if (Test-Path -Path nuget-cmdline) 
{
	rmdir nuget-cmdline -Recurse
}
rm *.nupkg

msbuild .\Source\AccidentalFish.DependencyResolver\AccidentalFish.DependencyResolver.csproj /t:pack
msbuild .\Source\AccidentalFish.DependencyResolver.Autofac\AccidentalFish.DependencyResolver.Autofac.csproj /t:pack
msbuild .\Source\AccidentalFish.DependencyResolver.MicrosoftNetStandard\AccidentalFish.DependencyResolver.MicrosoftNetStandard.csproj /t:pack
.\nuget pack .\Source\AccidentalFish.DependencyResolver.Unity\AccidentalFish.DependencyResolver.Unity.csproj
.\nuget pack .\Source\AccidentalFish.DependencyResolver.Ninject\AccidentalFish.DependencyResolver.Ninject.csproj

if ($pushLocal)
{
	cp *.nupkg \MicroserviceAnalyticPackageRepository
	cp .\Source\AccidentalFish.DependencyResolver\bin\debug\*.nupkg \MicroserviceAnalyticPackageRepository
	cp .\Source\AccidentalFish.DependencyResolver.Autofac\bin\debug\*.nupkg \MicroserviceAnalyticPackageRepository
	cp .\Source\AccidentalFish.DependencyResolver.MicrosoftNetStandard\bin\debug\*.nupkg \MicroserviceAnalyticPackageRepository
}

if ($pushNuget)
{
	.\nuget push *.nupkg -source nuget.org
}

if ($cleanup)
{
	rmdir nuget-powershell -Recurse
	rm *.nupkg
}
