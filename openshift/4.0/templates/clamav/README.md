# PIMS CLAMAV templates

The following is a copy of the CLAMAV templates used within the PIMS application.

Note that these are simply copies of the templates stored in the original repo at: https://github.com/bcgov/clamav/tree/ocp4

The files in our repo are merely included for reference.

## Build Configuration

Create the build configuration using:

oc process -f ./openshift/4.0/templates/clamav/clamav-bc.conf | oc create -f -

https://github.com/bcgov/clamav/blob/ocp4/openshift/templates/clamav-bc.conf

ensure that you manually build and tag the image for all desired target environments (dev, test, uat, prod)

## Deploy Configuration

Create the deployment configuration using:

oc process -f ./openshift/4.0/templates/clamav/clamav-dc.conf -p IMAGE_NAMESPACE=3cd915-tools -p TAG_NAME=latest | oc create -f -

https://github.com/bcgov/clamav/blob/ocp4/openshift/templates/clamav-dc.conf

Note that the above can be replaced by any tag, and it is recommended to create one clamav tag for each environment

## Automatic Updates

Create the cron job configuration using:

oc process -f ./openshift/4.0/templates/clamav/clamav-cron.conf -p CLAMAV_NAME=<insert name of clamav pod in target environment> | oc create -f -

Next, import the ose-cli image into the 3cd915-tools namespace:

oc import-image openshift4/ose-cli:v4.11.0-202210061001.p0.g262ac9c.assembly.stream --from=registry.redhat.io/openshift4/ose-cli:v4.11.0-202210061001.p0.g262ac9c.assembly.stream --confirm

And then retag the imported image:

oc tag ose-cli:v4.11.0-202210061001.p0.g262ac9c.assembly.stream ose-cli:latest

## PIMS API Configuration

In order to consume the clamav service, the following environment variables are required in PIMS-API:

Av**HostUri clamav
Av**Port 3310
