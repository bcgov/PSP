# Scripts

This directory contains configuration files that are used with the kartozageoserver image.
The configuration files are copied into the image at runtime, providing a default workspace, data source, and layer. By default, the data source will connect to the dockerized local PSP database. The layer will expose all properties within PIMS_PROPERTY_LOCATION_VW with non-null location data.

# To Run

the docker-compose file at the root of this repository has been updated to include this docker container. As a result, running `make up` at the root of the repository will result in this container being started as well (note, the image for this container is sizeable, the initial download will take some time).

The default username/password is configured in the .env file

# Requirements

You must create a .env file within this directory in order for the geoserver docker image to run successfully. A sample .env file can be found within gen-env-files.sh

You can use make env in the root git directory to generate the .env and datastore.xml required for the geoserver image to run.

# Geoserver Configuration

The geoserver configuration provided here is 1-1 represented within the web ui of the running geoserver docker container. Therefore, any changes made to the psp workspace within the web ui will update the configuration files here. Those updates can then be committed back to source, in the case of a configuration update that should be shared with the rest of the team. Note, because the configuration is saved here, any changes to the configuration will persist, even if the docker image is deleted. To reset the configuration, simply manually revert any changes using git.

# Database Connectivity

By default, the datastore.xml file generated for the geoserver by the gen-env-files.sh script should create a variable called DB_PASSWORD that should be set to the same value as the password used for the admin account of the MSSQL database. If you are adding the geoserver docker image for the first time to a repository where the database .env is already configured simply run `make env` and skip entering any username values (press enter). By default the gen-env-files.sh script does not overwrite pre-existing .env files, and it will also read the password from ./database/mssql/.env if one exists. This should result in only the datastore.xml file being created with the correct password.

*NOTE* the database password can be found in ../database/mssql/.env

# Frontend Connectivity

the frontend proxy configuration within setupProxy.js has been updated to automatically route and requests to ogs-internal to the running geoserver container. Currently, this only supports the inventory layer. Eventually, the highway layer should be added as well to fully mimic the behaviour of the dev/test/prod geoserver instances. *NOTE*: the local geoserver instance does *NOT* currently support keycloak authentication.