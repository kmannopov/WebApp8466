Import-Module WebAdministration
$iisAppPoolName = "DSCC_CW1_MVCWebApp_8466"
$iisAppPoolDotNetVersion = "v4.0"
$iisAppName = "MVCWebApp_8466"
$directoryPath = "C:\www\8466"

#stop the default web site so we can use port :80
Stop-WebSite 'Default Web Site'

#set the autostart property so we don't have the default site kick back on after a reboot
cd IIS:\Sites\
Set-ItemProperty 'Default Web Site' serverAutoStart False

#navigate to the app pools root
cd IIS:\AppPools\

#check if the app pool exists
if (!(Test-Path $iisAppPoolName -pathType container))
{
    #create the app pool
    $appPool = New-Item $iisAppPoolName
    $appPool | Set-ItemProperty -Name "managedRuntimeVersion" -Value $iisAppPoolDotNetVersion
}

#navigate to the sites root
cd IIS:\Sites\

#check if the site exists
if (Test-Path $iisAppName -pathType container)
{
    return
}

#create the site
$iisApp = New-Item $iisAppName -bindings @{protocol="http";bindingInformation=":80:"} -physicalPath $directoryPath
$iisApp | Set-ItemProperty -Name "applicationPool" -Value $iisAppPoolName
Set-ItemProperty $iisAppName serverAutoStart True