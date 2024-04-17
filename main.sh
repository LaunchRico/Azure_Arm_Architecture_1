#!/bin/bash
#
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

az group create --name $resourceGroup --location $location --tags owner=$owner

#Execute networkin json file
#az deployment group create --resource-group $resourceGroup --template-file ./network/armTemplate.json --parameters vnetName=$vnetName vnetAddressPrefix=$vnetAddressPrefix privateSubnetName=$privateSubnetName privateSubnetAddressPrefix=$privateSubnetAddressPrefix location=$location

#Execute storage json file
az deployment group create --resource-group $resourceGroup --template-file ./storage/armTemplate.json --parameters location=$location storageAccountName=$storageAccountName queueName=$queueName
