%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command "Set-ExecutionPolicy Unrestricted -Force"
IF EXIST C:\www\DSCC_CW1_MVCWebApp_8466 rmdir C:\www\DSCC_CW1_MVCWebApp_8466
mkdir C:\www\DSCC_CW1_MVCWebApp_8466

cd c:\temp

%SystemRoot%\sysnative\WindowsPowerShell\v1.0\powershell.exe -command ".\installwebsite.ps1"