Stop-Process -Name "Microsoft.Azure.Cosmos.*"

# Cosmos DB Emulator Installation
$confirmation = Read-Host "Do you want to install Cosmos Database Emulator? (y/n)"
if ($confirmation -eq 'y') {
    Write-Host "Installing Azure Cosmos Db Emulator"
    $installer = "$PSScriptRoot/cosmosEmulatorInstaller.msi"
    curl https://aka.ms/cosmosdb-emulator -O $installer
    Start-Process -Wait -FilePath msiexec -ArgumentList /i, $installer
    Remove-Item $installer
}

# Custom Certificate Generation to work with host.docker.internal
$certFriendlyName="DocumentDbEmulatorCertificate"
$pfxPath=$PSScriptRoot + '\' + 'cosmosdbemulator.pfx'
# Remove any existing cosmos emulator certs, then generate a new one
if (Test-Path $pfxPath) {
    Write-Host "Removing existing PFX cert"
    Remove-Item -Path $pfxPath
}
Get-ChildItem -path Cert:\LocalMachine\My -Recurse | where {$_.FriendlyName -eq $certFriendlyName} | Remove-Item # Note that this doesn't remove in cert manager for some reason
$cosmosDb = Get-ChildItem HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall | % { Get-ItemProperty $_.PsPath } | where {$_.DisplayName -eq 'Azure Cosmos DB Emulator'} | Select InstallLocation
cd $cosmosDb.InstallLocation
.\Microsoft.Azure.Cosmos.Emulator.exe /GenCert=host.docker.internal
# Start sleep to allow cert generation to propagate before continuing
Start-Sleep -s 5

# Export cert to script directory
$pfxPwd=ConvertTo-SecureString 'SecurePwdGoesHere' -asplaintext -force
Get-ChildItem -path Cert:\LocalMachine\My -Recurse | where {$_.FriendlyName -eq $certFriendlyName} | Export-PfxCertificate -Filepath $pfxPath -Password $pfxPwd