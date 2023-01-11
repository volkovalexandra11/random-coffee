#!/usr/bin/env bash

# Install Yandex.Cloud CLI
# https://cloud.yandex.ru/docs/cli/quickstart


set -e # exit on error

pushd "$(dirname "$0")" # go to script dir
trap popd EXIT # go back on exit

YC_REGISTRY=cr.yandex/crpo6ja0bdtjh62k0k5q
REGISTRY_REPO=coffee

if [[ -z $1 ]]; then
  echo 'No tag version specified'
  exit 1
fi;

IMAGE="${YC_REGISTRY}/${REGISTRY_REPO}:$1"

docker build . -t "${IMAGE}"
docker push "${IMAGE}"

yc serverless container revision deploy --container-name coffee \
  --execution-timeout 10s \
  --service-account-id ajetcgg74r7dannaebvk \
  --min-instances 1 \
  --image "${IMAGE}"