# s3cmd required
# > sudo apt-get install s3cmd
# then https://cloud.yandex.ru/docs/storage/tools/s3cmd

# use ./build-and-deploy-front.sh --no-build to publish current ./build directory 

set -e # exit on error

pushd "$(dirname "$0")" # go to script dir
trap popd EXIT          # go back on exit

DEPLOY_SA_LOCAL_KEY='.keys/coffee-deploy-sa'
DEPLOY_SA_STATIC_KEY_SECRET_NAME=coffee-deploy-sa-static-key-value
DEPLOY_SA_ID=aje34sdpmjb1legmn0md
DEPLOY_SA_STATIC_KEY_ID=YCAJEZS7WVrtrEOG2XVp4ZsaZ

function get_static_key {
  if [[ ! -f "$DEPLOY_SA_LOCAL_KEY" ]]; then
    mkdir -p "$(dirname ${DEPLOY_SA_LOCAL_KEY})"
    yc lockbox payload get --name coffee-deploy-sa-static-key-value | grep text_value | sed 's/.*: \(.*\)/\1/g' >"$DEPLOY_SA_LOCAL_KEY"
  fi

  # todo не протухнет?
  cat $DEPLOY_SA_LOCAL_KEY
}
DEPLOY_SA_STATIC_KEY_VALUE=$(get_static_key)

if [[ -z ${1+x} ]]; then
  npm run build
elif [[ "$1" != '--no-build' ]]; then
  echo "Unknown first parameter $1" >&2
  exit 1
fi;

s3cmd --access_key "$DEPLOY_SA_STATIC_KEY_ID" \
  --secret_key "$DEPLOY_SA_STATIC_KEY_VALUE" \
  --bucket-location ru-central \
  --host storage.yandexcloud.net \
  --host-bucket '(bucket)s.storage.yandexcloud.net' \
  sync --guess-mime-type --no-mime-magic --delete-removed build/ s3://p372-coffee/
