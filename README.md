# advanced-azure-microservices-with-dotnet-for-developers

repo created following and integrating with the course https://www.linkedin.com/learning/advanced-azure-microservices-with-dot-net-for-developers

# Dotnet Foundation

this repo also is inspired from https://github.com/dotnet-architecture/eShopOnContainers

test repo for my public experiments

OhMyPosh installation is custom but strongly inspired by  https://github.com/JanDeDobbeleer/oh-my-posh/blob/main/.devcontainer/devcontainer.json

# Honorable mention
https://github.com/Azure/azure-cosmos-dotnet-v3/issues/1232#issuecomment-632942023

#Guide to see cosmosdb privately exposed:
In Bash

ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

docker pull mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator

docker run -p 8081:8081 -p 10251:10251 -p 10252:10252 -p 10253:10253 -p 10254:10254  -m 3g --cpus=2.0 --name=test-linux-emulator -e AZURE_COSMOS_EMULATOR_PARTITION_COUNT=10 -e AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE=true -e AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE=$ipaddr -it mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator

ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

curl -k https://$ipaddr:8081/_explorer/emulator.pem > cosmosdbemulator.crt

sudo cp cosmosdbemulator.crt /usr/local/share/ca-certificates/cosmosdbemulator.crt

sudo update-ca-certificates

then go into "Ports" and change port protocol of 8081 to https

then go to the https local address exposed appending "/_explorer/index.html"