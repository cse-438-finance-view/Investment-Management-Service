$certPath = ".\certs\aspnetapp.crt"

# Sertifikayı Güvenilir Kök Sertifika Yetkililerine ekle
Import-Certificate -FilePath $certPath -CertStoreLocation "Cert:\LocalMachine\Root"

Write-Host "Sertifika başarıyla güvenilir sertifikalar listesine eklendi."
Write-Host "Tarayıcınızı yeniden başlatın ve https://localhost:8081 adresini tekrar deneyin." 