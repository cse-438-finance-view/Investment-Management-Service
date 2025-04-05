# Investment Management Service

## Deployment with Docker

### Requirements
- Docker
- Docker Compose

### Deployment Steps

1. Clone the project to your server:
```bash
git clone <repo-url>
cd Investment-Management-Service
```

2. Build the Docker image and start the service:
```bash
docker-compose up -d
```

3. The API will be accessible at:
- HTTP: http://server-ip:8080
- HTTPS: https://server-ip:8081
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

If you need to modify the environment configuration, edit the docker-compose.yml file. 