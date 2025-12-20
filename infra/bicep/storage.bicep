/* Params */
@description('Base name for the application')
param appName string = 'LedgerFlow'

@description('Environment name (dev, prod, etc)')
param environment string = 'DEV'

@description('Azure region')
param location string = resourceGroup().location

@description('Blob container name for uploaded files')
param receiptsContainerName string = 'files'


/* Variables */
var storageAccountName = toLower('${appName}-SA-${toUpper(environment)}')


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

resource receiptsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2025-06-01' = {
  parent: blobService
  name: receiptsContainerName
  properties: {
    publicAccess: 'None'
  }
}


/* Outputs */
output storageAccountName string = storageAccount.name
output storageAccountId string = storageAccount.id
output blobEndpoint string = storageAccount.properties.primaryEndpoints.blob
output containerName string = receiptsContainer.name
