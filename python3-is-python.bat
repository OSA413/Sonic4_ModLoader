@echo off

REM this makes a redirect from python3 to python
REM you will probably need administrator privileges to run this script

setlocal EnableDelayedExpansion

for /f "delims=;" %%i in ('where python') do (
	set py=%%i
	goto :end
)
:end

mklink "%py:.exe=3.exe%" "%py%"

pause