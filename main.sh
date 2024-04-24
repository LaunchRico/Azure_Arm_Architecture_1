#!/bin/bash
#Main script to run the infraestructure

resourceGroup="rg-mmr-sbox-westus-001"
location="westus"
owner="mmartin@launchcg.com"
vnetName="vnet-mmr-sbox-westus-001"
vnetAddressPrefix="10.3.0.0/16"
privateSubnetName="subnet-mmr-private-sbox-westus-001"
privateSubnetAddressPrefix="10.3.1.0/24"
storageAccountName="stmmrsboxwestus001"
queueName="queuemmrsboxwestus001"
appServicePlan="plan-mmr-sbox-westus-001"
webAppName="app-mmr-sbox-westus-001"
functionStorageAccount="stmmrfnsboxwestus001"
functionAppName="func-mmr-sbox-westus-001"
serverFarmSubnetName="subnet-mmr-serverfarm-sbox-westus-001"
serverFarmSubnetAddressPrefix="10.3.2.0/24"
GHSecret="PUBLISH_PROFILE"
GHFunctionSecret="FUNCTION_PUBLISH_PROFILE"
repoOwner="LaunchRico"
repoName="Azure_Arm_Architecture_1"
pipelineName="web_app_storage.yml"
functionPipelineName="function_app.yml"
sqlServerName="sql-mmr-sbox-westus-001"
adminUserName="mmradministrator"
adminLoginPassword="alBjFg8nen"
sqlDatabaseName="db-mmr-sbox-westus-001"

az group create --name $resourceGroup --location $location --tags owner=$owner

#Execute networkin json file
az deployment group create --resource-group $resourceGroup --template-file ./network/armTemplate.json --parameters vnetName=$vnetName vnetAddressPrefix=$vnetAddressPrefix privateSubnetName=$privateSubnetName privateSubnetAddressPrefix=$privateSubnetAddressPrefix location=$location serverFarmSubnetName=$serverFarmSubnetName serverFarmSubnetAddressPrefix=$serverFarmSubnetAddressPrefix

#Execute storage json file
az deployment group create --resource-group $resourceGroup --template-file ./storage/armTemplate.json --parameters location=$location storageAccountName=$storageAccountName queueName=$queueName sqlServerName=$sqlServerName adminUsername=$adminUserName administratorLoginPassword=$adminLoginPassword sqlDatabaseName=$sqlDatabaseName virtualNetworkName=$vnetName serverFarmSubnetName=$serverFarmSubnetName

#Execute app json file
az deployment group create --resource-group $resourceGroup --template-file ./app/armTemplate.json --parameters location=$location webAppName=$webAppName appServicePlan=$appServicePlan functionAppName=$functionAppName functionStorageAccount=$storageAccountName vnetName=$vnetName subnetName=$serverFarmSubnetName

#Add Application settings
az webapp config appsettings set --name $webAppName --resource-group $resourceGroup --settings AzureWebJobsStorage="DefaultEndpointsProtocol=https;AccountName=$storageAccountName;AccountKey=$(az storage account keys list --resource-group $resourceGroup --account-name $storageAccountName --query [0].value -o tsv);EndpointSuffix=core.windows.net"

#Adding connection string to Azure function
tempVariable=$(az sql db show-connection-string --server $sqlServerName --name $sqlDatabaseName --auth-type SqlPassword --client ado.net)

databaseConnectionString=${tempVariable/<username>/$adminUserName}
databaseConnectionString=${databaseConnectionString/<password>/$adminLoginPassword}

#Assigning connection string to azure function
az webapp config connection-string set --name $functionAppName --resource-group $resourceGroup --settings "SqlConnection=$databaseConnectionString" --connection-string-type SQLAzure

#Retriving publish profile
publishProfile=$(az webapp deployment list-publishing-profiles --name $webAppName --resource-group $resourceGroup --xml)
echo "$publishProfile" | gh secret set $GHSecret --repo $repoOwner/$repoName

#Retriving function publish profile
functionPublishProfile=$(az webapp deployment list-publishing-profiles --name $functionAppName --resource-group $resourceGroup --xml)
echo "$functionPublishProfile" | gh secret set $GHFunctionSecret --repo $repoOwner/$repoName

#Run pipeline
gh workflow run $pipelineName --repo $repoOwner/$repoName

gh workflow run $functionPipelineName --repo $repoOwner/$repoName