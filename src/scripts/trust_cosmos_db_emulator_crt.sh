#!/bin/bash

# Save current working directory
PWD=`pwd`
pushd $PWD

# Find and move to the location of this script
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $DIR

if [ -n "$1" ]; then
    COSMOS_DB_EMULATOR_PFX=$1
else
    COSMOS_DB_EMULATOR_PFX="./cosmosdbemulator.pfx"
fi
COSMOS_DB_EMULATOR_PFX_PASSWORD="SecurePwdGoesHere"
CERT_TO_TRUST="cosmosdbemulator.crt"

# Generate .crt file if pfx exists
if [ -f "$COSMOS_DB_EMULATOR_PFX" ]; then
    openssl pkcs12 -in $COSMOS_DB_EMULATOR_PFX -clcerts -nokeys -out cosmosdbemulator.crt -passin pass:$COSMOS_DB_EMULATOR_PFX_PASSWORD;
fi

# Trust Cert (will end located in /etc/ssl/certs/ based on *.crt name as a *.pem, e.g. /etc/ssl/certs/cosmosdbemulator.pem for cosmosdbemulator.crt)
if [ -f "$CERT_TO_TRUST" ]; then
    cp $CERT_TO_TRUST /usr/local/share/ca-certificates/
    update-ca-certificates
    rm $CERT_TO_TRUST;
fi

# Restore working directory
popd