%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command "Set-ExecutionPolicy Unrestricted -Force"


IF NOT EXIST c:\temp mkdir c:\temp 
cd c:\temp
IF EXIST c:\temp\deletewebsite.ps1 %SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command ".\deletewebsite.ps1"

%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command "rm C:\www\DSCC_CW1_MVCWebApp_8466 -Recurse -Force"
del /q "c:\temp\*.*"