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
ipaddr="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`" \
&& curl -k https://$ipaddr:8081/_explorer/emulator.pem > cosmosdbemulator.crt \
&& sudo cp cosmosdbemulator.crt /usr/local/share/ca-certificates/cosmosdbemulator.crt \
&& sudo update-ca-certificates

then go into "Ports" and change port protocol of 8081 to https

then go to the https local address exposed appending "/_explorer/index.html"



# Workflow to be documented
5101 - WisdomPetMedicine.Api
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "name1",
  "breed": "Beagle",
  "sex": 0,
  "color": "Brown",
  "dateOfBirth": "2022-07-02T16:32:22.845Z",
  "species": "Dog"
}

PetQuery

TransferToHospital

Adopter

RequestAdoption

Set PhoneNumber

ApproveAdoption

