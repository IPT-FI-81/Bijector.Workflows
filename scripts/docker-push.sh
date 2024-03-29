#!/bin/bash
DOCKER_TAG=$TRAVIS_BRANCH

case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_TAG=latest
    ;;
  "develop")
    DOCKER_TAG=dev
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -t bijector.workflows:$DOCKER_TAG .
docker tag bijector.workflows:$DOCKER_TAG $DOCKER_USERNAME/bijector.workflows:$DOCKER_TAG
docker push $DOCKER_USERNAME/bijector.workflows:$DOCKER_TAG