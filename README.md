# OrderPickingSystem
Master's thesis - Autonomous order picking system based on the AlphaBot2 robot

![AlphaBot2 image](./assets/alphabot2.jpg)

## Docker
### Building image
You have to build image from main directory (it would work the same with github actions)
`docker build . -f sources/apps/{Selected Application}/Dockerfile --target release -t {Selected Application}:latest`

Example
`docker build . -f sources/apps/Api/Dockerfile --target release -t api:latest`

### Running containers
`cd sources/compose`
`docker compose --profile {Selected profile} up --abort-on-container-exit; docker compose --profile {Selected profile} down`

Example
`cd sources/compose`
`docker compose --profile full up --abort-on-container-exit; docker compose --profile full down`

### Check Api
Run
`curl -v http://localhost:8080`