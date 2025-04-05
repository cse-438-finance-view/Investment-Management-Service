#!/bin/bash

# Generate private key
openssl genrsa -out aspnetapp.key 2048

# Generate CSR
openssl req -new -key aspnetapp.key -out aspnetapp.csr -subj "/CN=invapi.localhost"

# Generate certificate
openssl x509 -req -days 365 -in aspnetapp.csr -signkey aspnetapp.key -out aspnetapp.crt

# Create PFX
openssl pkcs12 -export -out aspnetapp.pfx -inkey aspnetapp.key -in aspnetapp.crt -password pass:password

echo "Certificate generated successfully at ./certs/aspnetapp.pfx" 