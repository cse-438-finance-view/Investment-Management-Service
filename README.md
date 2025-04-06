# Investment Management Service

## Deployment with Docker

### Requirements
- Docker
- Docker Compose
- RabbitMQ (running on the host machine)

### SSL Certificate Generation
Before running the service, you need to generate SSL certificates:

#### Windows:
```powershell
cd ./certs
powershell -ExecutionPolicy Bypass -File ./generate-cert.ps1
```

#### Linux/macOS:
```bash
cd ./certs
chmod +x generate-cert.sh
./generate-cert.sh
```

### Deployment Steps

1. Clone the project to your server:
```bash
git clone <repo-url>
cd Investment-Management-Service
```

2. Generate SSL certificates (see above)

3. Build the Docker image and start the service:
```bash
docker-compose up -d
```

4. The API will be accessible at:
- HTTP: http://server-ip:8080
- Swagger: http://server-ip:8080/swagger

### Container Management

- To stop the service:
```bash
docker-compose down
```

- To view logs:
```bash
docker-compose logs -f api
```

### Environment Configuration

The application is configured to connect to the Neon PostgreSQL cloud database that is already deployed. No additional database setup is required.

The application expects a RabbitMQ instance running on the host machine which will be accessed via host.docker.internal.

If you need to modify the environment configuration, edit the docker-compose.yml file. 