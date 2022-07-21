# PIMS CLAMAV templates

The following is a copy of the CLAMAV templates used within the PIMS application.

Note that these are simply copies of the templates stored in the original repo at: https://github.com/bcgov/clamav/tree/ocp4

The files in our repo are merely included for reference.

## Build Configuration

Create the build configuration using:

oc process -f ./openshift/templates/clamav-bc.conf | oc create -f -

https://github.com/bcgov/clamav/blob/ocp4/openshift/templates/clamav-bc.conf

ensure that you manually build and tag the image for all desired target environments (dev, test, uat, prod)

## Deploy Configuration

Create the deployment configuration using:

oc process -f ./openshift/templates/clamav-dc.conf -p IMAGE_NAMESPACE=3cd915-tools -p TAG_NAME=latest | oc create -f -

https://github.com/bcgov/clamav/blob/ocp4/openshift/templates/clamav-dc.conf

Note that the above can be replaced by any tag, and it is recommended to create one clamav tag for each environment

## PIMS API Configuration

In order to consume the clamav service, the following environment variables are required in PIMS-API:

Av**HostUri clamav
Av**Port 3310
