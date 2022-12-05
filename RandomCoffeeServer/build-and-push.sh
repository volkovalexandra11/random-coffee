#!/usr/bin/env bash

# Install Yandex.Cloud CLI
# https://cloud.yandex.ru/docs/cli/quickstart


set -e # exit on error

pushd $0 # go to script dir
trap popd EXIT # go back on exit

YC_REGISTRY=cr.yandex/crpsr3s2k1f4hijq8aua
REGISTRY_REPO=coffee

if [[ -z $1 ]]; then
  echo 'No tag version specified'
  exit 1
fi;

CONTAINER="${YC_REGISTRY}/${REGISTRY_REPO}:$1"

docker build . -t "${CONTAINER}"
docker push "${CONTAINER}"
