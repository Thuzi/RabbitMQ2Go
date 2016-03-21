"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\src\RabbitMQ2Go.sln" /target:rebuild /p:Configuration=Release /verbosity:Detailed
nuget pack ..\src\RabbitMQ2Go\RabbitMQ2Go.nuspec -NoPackageAnalysis
