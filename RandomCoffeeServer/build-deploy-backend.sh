#!/usr/bin/env bash

# Install Yandex.Cloud CLI
# https://cloud.yandex.ru/docs/cli/quickstart


set -e # exit on error

pushd "$(dirname "$0")" # go to script dir
trap popd EXIT # go back on exit

YC_REGISTRY=cr.yandex/crpo6ja0bdtjh62k0k5q
SERVICE_ACCOUNT_FOR_CONTAINER_ID=ajetcgg74r7dannaebvk

DB_CONTAINER_ID=bbav4smuip0b69kjf7pk
ROUNDS_CONTAINER_ID=bbaahbf1jjhilsgqsc2s

REGISTRY_REPO_API=coffee-api
REGISTRY_REPO_DB=coffee-db-actualizer
REGISTRY_REPO_ROUNDS=coffee-make-rounds

if [[ -z $1 ]]; then
  echo 'No tag version specified'
  exit 1
fi;

function build_and_push {
  registry_repo="$1"
  version="$2"
  services="$3"
  min_instances="${4:-0}"
  
  image="${YC_REGISTRY}/${registry_repo}:${version}"
  
  docker build . --build-arg APPLICATION_MODES="${services}" -t "${image}"
  docker push "${image}"
  
  yc serverless container revision deploy --container-name "${registry_repo}" \
    --execution-timeout 20s \
    --service-account-id "${SERVICE_ACCOUNT_FOR_CONTAINER_ID}" \
    --min-instances $min_instances \
    --image "${image}"
}

build_and_push $REGISTRY_REPO_API $1 "ApiServer"
build_and_push $REGISTRY_REPO_DB $1 "DbUpdaterOnRequest"
build_and_push $REGISTRY_REPO_ROUNDS $1 "RoundsMakerOnRequest"

token=$(yc iam create-token)

curl -X POST https://$DB_CONTAINER_ID.containers.yandexcloud.net/update-db \
  --dump-header - \
  -H "Authorization: Bearer $token"

curl -X POST https://$ROUNDS_CONTAINER_ID.containers.yandexcloud.net/make-rounds \
  --dump-header - \
  -H "Authorization: Bearer $token"