$cert = New-SelfSignedCertificate -Subject "CN=invapi.localhost" -CertStoreLocation "cert:\LocalMachine\My" -KeyExportPolicy Exportable -KeySpec Signature -KeyLength 2048 -HashAlgorithm "SHA256" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1")

# Create PFX
$pwd = ConvertTo-SecureString -String "password" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath ".\aspnetapp.pfx" -Password $pwd

# Create CER (public key only)
Export-Certificate -Cert $cert -FilePath ".\aspnetapp.crt"

Write-Host "Certificate generated successfully at ./certs/aspnetapp.pfx" 