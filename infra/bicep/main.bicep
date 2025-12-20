targetScope = 'subscription'

/* Params */
@description('Environment name (dev, prod, etc)')
param environment string

@description('Azure region')
param location string = 'eastus'

@description('Base application name')
param appName string = 'LedgerFlow'

/* Variables */
var resourceGroupName = '${appName}-${toUpper(environment)}'

/* Resources */
resource ledgerFlowRg 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: resourceGroupName
  location: location
  tags: {
    application: appName
    environment: toUpper(environment)
  }
}

/* Modules */

/* Storage */
module storage './storage.bicep' = {
  name: 'ledgerflow-storage'
  scope: ledgerFlowRg
  params: {
    appName: appName
    environment: toUpper(environment)
    location: location
  }
}

/* File Upload */
module fileUpload './fileupload.bicep' = {
  name: 'ledgerflow-fileupload'
  scope: ledgerFlowRg
  params: {
    appName: appName
    blobContainerName: storage.outputs.containerName
    blobEndpoint: storage.outputs.blobEndpoint
    storageAccountId: storage.outputs.storageAccountId
    storageAccountName: storage.outputs.storageAccountName
    environment: toUpper(environment)
    location: location
  }
}
