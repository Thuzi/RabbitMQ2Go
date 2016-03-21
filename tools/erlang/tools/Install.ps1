param($installPath, $toolsPath, $package, $project)

$erlangVersionFolder = "erts-7.3"

$toolsErlangPath = [io.path]::combine($toolsPath, "binary")

$bindir = [io.path]::combine($toolsErlangPath, $erlangVersionFolder, "bin") -replace "\\","\\"
$rootdir = $toolsErlangPath -replace "\\","\\"
$iniFileContent =  [string]::join("`n", "[erlang]", "Bindir=" + $bindir, "Progname=erl", "Rootdir=" + $rootdir) + "`n"

[string[]] $iniFiles = [io.path]::combine($toolsErlangPath, "bin", "erl.ini"), [io.path]::combine($toolsErlangPath, $erlangVersionFolder, "bin", "erl.ini")
foreach ($iniFile in $iniFiles)
{
	"Writing ini file at: " + $iniFile
	[io.file]::writealltext($iniFile, $iniFileContent)
}
