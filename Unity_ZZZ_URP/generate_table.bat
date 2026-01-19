set GEN_CLIENT=.\DataTables\Luban\Luban.dll
set CONF_ROOT=.\DataTables

dotnet  %GEN_CLIENT% ^
	-t client ^
	-c cs-simple-json ^
	-d json ^
	--conf %CONF_ROOT%\luban.conf ^
	-x outputCodeDir=.\Assets\ZZZ\Script\Table ^
	-x outputDataDir=.\Assets\ZZZ\Resources\Json ^
	-x lineEnding=LF ^
	-x code.lineEnding=LF ^
	-x data.lineEndin=LF
pause