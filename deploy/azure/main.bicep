//az deployment group create --resource-group fb-linkedin-ddd-course --template-file main.bicep
param location string = resourceGroup().location

resource azbus 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  location: location
  name: 'fb-course-azbus'
  properties: {
    zoneRedundant: false
  }
}

resource petflaggedtopic 'Microsoft.ServiceBus/namespaces/topics@2021-11-01' = {
  name: 'pet-flagged-for-adoption'
  parent: azbus
}

resource pettransferredtopic 'Microsoft.ServiceBus/namespaces/topics@2021-11-01' = {
  name: 'Pet-transferred-to-hospital'
  parent: azbus
}

resource petflaggedsub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-11-01' = {
  name: 'pet-flagged-for-adoption'
  parent: petflaggedtopic
}

resource pettransferredsub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-11-01' = {
  name: 'Pet-transferred-to-hospital'
  parent: pettransferredtopic
}

// Commented because no free tier available today for west europe.
// Error : Message: {\"code\":\"ServiceUnavailable\",\"message\":\"Sorry, we are currently experiencing high demand in West Europe region, 
// and cannot fulfill your request at this time. To request region access for your subscription, please follow this link https://aka.ms/cosmosdbquota 
// for more details on how to create a region access request.

// var accountName = 'cosmos-${uniqueString(resourceGroup().id)}'

// var databaseName = 'Patients'

// resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2021-11-15-preview' = {
//   name: toLower(accountName)
//   location: location
//   properties: {
//     enableFreeTier: true
//     databaseAccountOfferType: 'Standard'
//     consistencyPolicy: {
//       defaultConsistencyLevel: 'Session'
//     }
//     locations: [
//       {
//         locationName: location
//       }
//     ]
//   }
// }

// resource cosmosDB 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-11-15-preview' = {
//   name: '${cosmosAccount.name}/${toLower(databaseName)}'
//   properties: {
//     resource: {
//       id: databaseName
//     }
//     options: {
//       throughput: 400
//     }
//   }
// }
