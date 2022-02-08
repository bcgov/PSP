#!/bin/bash

echo 'Enter a username for the keycloak database.'
read -p 'Username: ' varKeycloakDb

echo 'Enter a username for the keycloak realm administrator'
read -p 'Username: ' varKeycloak

echo 'Enter a username for the API database.'
read -p 'Username: ' varApiDb

passvar=$(grep -Po 'DB_PASSWORD=\K.*$' ./database/mssql/.env)

if [ -z "$passvar" ]
then
    # Generate a random password that satisfies MSSQL password requirements.
    echo 'A password is randomly being generated.'
    passvar=$(date +%s | sha256sum | base64 | head -c 29)A8!
    echo $passvar
fi

# Set environment variables.
# Keycloak
if test -f "./auth/keycloak/.env"; then
    echo "./auth/keycloak/.env exists"
else
echo \
"PROXY_ADDRESS_FORWARDING=true
# DB_VENDOR=POSTGRES
# DB_ADDR=keycloak-db
# DB_DATABASE=keycloak
# DB_USER=$varKeycloakDb
# DB_PASSWORD=$passvar
KEYCLOAK_USER=$varKeycloak
KEYCLOAK_PASSWORD=$passvar
KEYCLOAK_IMPORT=/tmp/realm-export.json -Dkeycloak.profile.feature.scripts=enabled -Dkeycloak.profile.feature.upload_scripts=enabled
KEYCLOAK_LOGLEVEL=WARN
ROOT_LOGLEVEL=WARN" >> ./auth/keycloak/.env
fi

# Keycloak Database
if test -f "./auth/postgres/.env"; then
    echo "./auth/postgres/.env exists"
else
echo \
"POSTGRESQL_DATABASE=keycloak
POSTGRESQL_USER=$varKeycloakDb
POSTGRESQL_PASSWORD=$passvar
" >> ./auth/postgres/.env
fi

# API Database
if test -f "./database/mssql/.env"; then
    echo "./database/mssql/.env exists"
else
echo \
"ACCEPT_EULA=Y
MSSQL_SA_PASSWORD=$passvar
MSSQL_PID=Developer
TZ=America/Los_Angeles
DB_NAME=pims
DB_USER=admin
DB_PASSWORD=$passvar
SERVER_NAME=localhost,5433
TIMEOUT_LENGTH=120" >> ./database/mssql/.env
fi

# API
if test -f "./backend/api/.env"; then
    echo "./backend/api/.env exists"
else
echo \
"ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://*:8080
DB_PASSWORD=$passvar
Keycloak__Secret=
Keycloak__ServiceAccount__Secret=" >> ./backend/api/.env
fi

# DAL DB migration
if test -f "./backend/dal/.env"; then
    echo "./backend/dal/.env exists"
else
echo \
"ConnectionStrings__PIMS=Server=localhost,5433;Database=pims;User Id=$varApiDb;
DB_PASSWORD=$passvar" >> ./backend/dal/.env
fi

# Application
if test -f "./frontend/.env"; then
    echo "./frontend/.env exists"
else
echo \
"NODE_ENV=development
API_URL=http://backend:8080/
CHOKIDAR_USEPOLLING=true" >> ./frontend/.env
fi

# Keycloak sync tool
if test -f "./tools/keycloak/sync/.env"; then
    echo "./tools/keycloak/sync/.env exists"
else
echo \
"# Local
ASPNETCORE_ENVIRONMENT=Local
Auth__Keycloak__Secret=" >> ./tools/keycloak/sync/.env
fi

if test -f "./geoserver/.env"; then
    echo "./geoserver/.env already exists"
else
echo \
"COMPOSE_PROJECT_NAME=kartozageoserver

IMAGE_VERSION=9.0-jdk11-openjdk-slim-buster
GS_VERSION=2.19.2
GEOSERVER_PORT=8600
# Build Arguments
JAVA_HOME=/usr/local/openjdk-11
WAR_URL=http://downloads.sourceforge.net/project/geoserver/GeoServer/2.19.2/geoserver-2.19.2-war.zip
ACTIVATE_ALL_STABLE_EXTENTIONS=1
ACTIVATE_ALL_COMMUNITY_EXTENTIONS=1
GEOSERVERUSER_UID=1000
GEOSERVERUSERS_GID=10001
RECREATE_DATADIR=FALSE
# Generic Env variables
GEOSERVER_ADMIN_USER=admin
GEOSERVER_ADMIN_PASSWORD=admin
# https://docs.geoserver.org/latest/en/user/datadirectory/setting.html
GEOSERVER_DATA_DIR=/opt/geoserver/data_dir
# https://docs.geoserver.org/latest/en/user/data/raster/gdal.html#external-footprints-data-directory
FOOTPRINTS_DATA_DIR=/opt/footprints_dir
# https://docs.geoserver.org/latest/en/user/geowebcache/config.html#changing-the-cache-directory
GEOWEBCACHE_CACHE_DIR=/opt/geoserver/data_dir/gwc
# Show the tomcat manager in the browser
TOMCAT_EXTRAS=true
# https://docs.geoserver.org/stable/en/user/production/container.html#optimize-your-jvm
INITIAL_MEMORY=2G
# https://docs.geoserver.org/stable/en/user/production/container.html#optimize-your-jvm
MAXIMUM_MEMORY=4G
# https://docs.geoserver.org/stable/en/user/security/webadmin/csrf.html
GEOSERVER_CSRF_DISABLED=true
# Path where .ttf and otf font should be added
FONTS_DIR=/opt/fonts
# JVM Startup option for encoding
ENCODING='UTF8'
# JVM Startup option for timezone
TIMEZONE='GMT'
# DB backend to activate disk quota storage in PostgreSQL DB. Only permitted value is 'POSTGRES'
DB_BACKEND=
# https://docs.geoserver.org/latest/en/user/production/config.html#disable-the-auto-complete-on-web-administration-interface-login
LOGIN_STATUS=on
# https://docs.geoserver.org/latest/en/user/production/config.html#disable-the-geoserver-web-administration-interface
WEB_INTERFACE=false
# Rendering settings
ENABLE_JSONP=true 
MAX_FILTER_RULES=20 
OPTIMIZE_LINE_WIDTH=false
# Install the stable plugin specified in https://github.com/kartoza/docker-geoserver/blob/master/build_data/stable_plugins.txt
STABLE_EXTENSIONS=sqlserver-plugin
# Install the community edition plugins specified in https://github.com/kartoza/docker-geoserver/blob/master/build_data/community_plugins.txt
COMMUNITY_EXTENSIONS=
# SSL Settings explained here https://github.com/AtomGraph/letsencrypt-tomcat
SSL=false
HTTP_PORT=8080 
HTTP_PROXY_NAME= 
HTTP_PROXY_PORT= 
HTTP_REDIRECT_PORT= 
HTTP_CONNECTION_TIMEOUT=20000 
HTTPS_PORT=8443 
HTTPS_MAX_THREADS=150 
HTTPS_CLIENT_AUTH= 
HTTPS_PROXY_NAME= 
HTTPS_PROXY_PORT= 
JKS_FILE=letsencrypt.jks 
JKS_KEY_PASSWORD='geoserver' 
KEY_ALIAS=letsencrypt 
JKS_STORE_PASSWORD='geoserver' 
P12_FILE=letsencrypt.p12 
PKCS12_PASSWORD='geoserver' 
LETSENCRYPT_CERT_DIR=/etc/letsencrypt
CHARACTER_ENCODING='UTF-8' 
# Clustering  variables
# Activates clustering using JMS cluster plugin
CLUSTERING=False
# cluster env variables specified https://docs.geoserver.org/stable/en/user/community/jms-cluster/index.html
CLUSTER_DURABILITY=true 
BROKER_URL= 
READONLY=disabled 
RANDOMSTRING=23bd87cfa327d47e 
INSTANCE_STRING=ac3bcba2fa7d989678a01ef4facc4173010cd8b40d2e5f5a8d18d5f863ca976f 
TOGGLE_MASTER=true 
TOGGLE_SLAVE=true 
EMBEDDED_BROKER=enabled" >> ./geoserver/.env
fi

if test -f "./geoserver/geoserver_data/psp/mssql/datastore.xml"; then
    echo "./geoserver/geoserver_data/psp/mssql/datastore.xml already exists"
else
echo \
"<dataStore>
  <id>DataStoreInfoImpl--47c96824:17c20b17224:-7ff6</id>
  <name>mssql</name>
  <type>Microsoft SQL Server</type>
  <enabled>true</enabled>
  <workspace>
    <id>WorkspaceInfoImpl-7e0faa3c:17c208de014:-7ff6</id>
  </workspace>
  <connectionParameters>
    <entry key=\"schema\">dbo</entry>
    <entry key=\"Evictor run periodicity\">300</entry>
    <entry key=\"Force spatial index usage via hints\">false</entry>
    <entry key=\"fetch size\">1000</entry>
    <entry key=\"Expose primary keys\">false</entry>
    <entry key=\"validate connections\">true</entry>
    <entry key=\"Connection timeout\">20</entry>
    <entry key=\"Use native geometry serialization\">false</entry>
    <entry key=\"Batch insert size\">1</entry>
    <entry key=\"database\">pims</entry>
    <entry key=\"Integrated Security\">false</entry>
    <entry key=\"port\">1433</entry>
    <entry key=\"passwd\">plain:$passvar</entry>
    <entry key=\"min connections\">1</entry>
    <entry key=\"host\">database</entry>
    <entry key=\"dbtype\">sqlserver</entry>
    <entry key=\"namespace\">psp</entry>
    <entry key=\"max connections\">10</entry>
    <entry key=\"Evictor tests per run\">3</entry>
    <entry key=\"Test while idle\">true</entry>
    <entry key=\"Use Native Paging\">true</entry>
    <entry key=\"user\">admin</entry>
    <entry key=\"Max connection idle time\">300</entry>
  </connectionParameters>
  <__default>false</__default>
  <dateCreated>2021-09-26 06:35:16.373 UTC</dateCreated>
  <dateModified>2021-09-26 20:42:24.266 UTC</dateModified>
</dataStore>" >> ./geoserver/geoserver_data/psp/mssql/datastore.xml
fi



echo 'Before running all the docker containers, update the .env files with the Keycloak Client Secret (pims-service-account).'
