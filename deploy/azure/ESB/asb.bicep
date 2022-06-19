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
  name: 'pet-transferred-to-hospital'
  parent: azbus
}

resource petflaggedsub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-11-01' = {
  name: 'pet-flagged-for-adoption'
  parent: petflaggedtopic
}

resource pettransferredsub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2021-11-01' = {
  name: 'pet-transferred-to-hospital'
  parent: pettransferredtopic
}
