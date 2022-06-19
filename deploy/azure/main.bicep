//az deployment group create --resource-group fb-linkedin-ddd-course --template-file main.bicep
param location string = resourceGroup().location
param sqlAdministratorLogin string
@secure()
param sqlAdministratorLoginPassword string

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

var accountName = 'cosmos-${uniqueString(resourceGroup().id)}'
var databaseName = 'WisdomPetMedicine'
var containerName = 'Patients'

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2022-02-15-preview' = {
  name: toLower(accountName)
  location: location
  properties: {
    enableFreeTier: true
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: [
      {
        locationName: location
      }
    ]
  }
}

resource cosmosDB 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2022-02-15-preview' = {
  name: databaseName
  parent: cosmosAccount
  properties: {
    resource: {
      id: databaseName
    }
    options: {
      throughput: 1000
    }
  }
}

resource container 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2022-02-15-preview' = {
  name: containerName
  parent: cosmosDB
  properties: {
    resource: {
      id: containerName
      partitionKey: {
        paths: [
          '/aggregateId'
        ]
        kind: 'Hash'
      }
    }
  }
}

var serverName = 'sql-${uniqueString(resourceGroup().id)}'

resource sqlserver 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
  }
}

resource sqlserverName_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2014-04-01' = {
  name: '${sqlserver.name}/AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

var lawName = 'law-${uniqueString(resourceGroup().id)}'

resource law 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: lawName
  location: location
}

var appInsName = 'appins-${uniqueString(resourceGroup().id)}'

resource appinsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: law.id
  }
}

output hospitalCS string = 'Data Source=tcp:${sqlserver.properties.fullyQualifiedDomainName},1433;Initial Catalog=Hospital;User Id=${sqlAdministratorLogin}@${sqlserver.properties.fullyQualifiedDomainName};Password=${sqlAdministratorLoginPassword};'
output petCS string = 'Data Source=tcp:${sqlserver.properties.fullyQualifiedDomainName},1433;Initial Catalog=Pet;User Id=${sqlAdministratorLogin}@${sqlserver.properties.fullyQualifiedDomainName};Password=${sqlAdministratorLoginPassword};'
output rescueCS string = 'Data Source=tcp:${sqlserver.properties.fullyQualifiedDomainName},1433;Initial Catalog=Rescue;User Id=${sqlAdministratorLogin}@${sqlserver.properties.fullyQualifiedDomainName};Password=${sqlAdministratorLoginPassword};'

// todo:
// add acr
// add kv and put every secret in there
// add aks with integrated ingress, acr and kv
// deploy jaeger
