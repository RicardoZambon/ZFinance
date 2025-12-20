/* Params */
@description('Base name for the application')
param appName string = 'LedgerFlow'

@description('Environment name (dev, prod, etc)')
param environment string = 'DEV'

@description('Azure region')
param location string = resourceGroup().location

@description('Blob container name for uploaded files')
param filesContainerName string = 'files'


/* Variables */
var storageAccountName = toLower('${appName}sta${environment}')


/* Resources */
resource storageAccount 'Microsoft.Storage/storageAccounts@2025-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    allowBlobPublicAccess: false
    minimumTlsVersion: 'TLS1_2'
    accessTier: 'Hot'
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2025-06-01' = {
  parent: storageAccount
  name: 'default'
}

resource filesContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2025-06-01' = {
  parent: blobService
  name: filesContainerName
  properties: {
    publicAccess: 'None'
  }
}


/* Outputs */
@description('The name of the Storage Account')
output storageAccountName string = storageAccount.name

@description('The resource ID of the Storage Account')
output storageAccountId string = storageAccount.id

@description('The Blob endpoint of the Storage Account')
output blobEndpoint string = storageAccount.properties.primaryEndpoints.blob

@description('The name of the Blob container for uploaded files')
output containerName string = filesContainer.name
