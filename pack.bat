@echo off
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe xunit.bdd.sln /p:Configuration=Release /nologo
nuget pack xunit.bdd\xunit.bdd.csproj -Prop Configuration=Release
pause