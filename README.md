# Checking how out of memory works in orleans grain.

## Build docker image

```sh
docker build -t orleans-oom .
```

## Run application in docker

```sh
docker run --rm --memory 50m --name orleans-oom orleans-oom 
```

## Stop running docker

```sh
docker stop orleans-oom
```

## Monitor docker memory

```sh
docker stats orleans-oom
```
