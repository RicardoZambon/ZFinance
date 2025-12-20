/* Params */
@description('Base name for the application')
param appName string = 'LedgerFlow'

@description('Blob container name for uploaded files')
param blobContainerName string

@description('Storage Account Blob endpoint')
param blobEndpoint string

@description('Storage Account ID')
param storageAccountId string

@description('Storage Account Name')
param storageAccountName string

@description('Environment name (dev, prod, etc)')
param environment string = 'DEV'

@description('Azure region')
param location string = resourceGroup().location


/* Variables */
var appInsightsName = '${appName}-AppInsights-${toUpper(environment)}'
var functionAppName = '${appName}-${toUpper(environment)}'

// Built-in Azure role definition IDs are stable across all Azure subscriptions
// See: https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles
var builtInRoleDefinitionIds = {
  storageBlobDataContributor: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

/*
 * Application Insights
 */
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

/*
 * App Service Plan (Consumption)
 */
resource hostingPlan 'Microsoft.Web/serverfarms@2025-03-01' = {
  name: '${functionAppName}-plan'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

/*
 * Function App
 */
resource functionApp 'Microsoft.Web/sites@2025-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: blobEndpoint
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'Blob__ContainerName'
          value: blobContainerName
        }
      ]
    }
    httpsOnly: true
    ftpsState: 'FtpsOnly'
  }
}

/*
 * Reference to existing Storage Account
 */
resource storageAccount 'Microsoft.Storage/storageAccounts@2025-06-01' existing = {
  name: storageAccountName
}

/*
 * Role Assignment - Grant Function App access to Storage Account
 */
resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccountId, functionApp.id, builtInRoleDefinitionIds.storageBlobDataContributor)
  scope: storageAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', builtInRoleDefinitionIds.storageBlobDataContributor)
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
