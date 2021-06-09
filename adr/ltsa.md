# Ltsa - Title, ParcelInfo, Strata Common Property Services

## Status

> WIP

> June 8, 2021

## Context

PIMS requires a way to get title information, detailed parcel information and information about strata common property for a given parcel identifier.

Use Cases;

- For a given pid, find all title summaries related to that PID, such as title number.
- For a given title number, get indefeasible title information from Ltsa.
- For a given pid, retrieve all parcel information from Ltsa, such as legal description.
- For a given strata plan number, retrieve all strata common property information.
- Display the results of the above requests in user editable forms.

The **Land Title and Survey Authority Title Direct Service** is a mixed billed/free service offered and supported by **LTSA**.

Additional Information here;

- [Agreement JIRA](https://jira.th.gov.bc.ca/browse/PSP-630)
- [Title Direct V4 documentation](https://jira.th.gov.bc.ca/secure/attachment/163561/Title%20Direct%20V4%20Search%20ICD.pdf)

## Decision

The decision is to integrate with the **LTSA** as a number of required fields are only available via that service.

The key features **LTSA** has are;

- REST Api with jwt based token access to available endpoints.
- Request based billing model for charged apis (Due to agreement, there is no charge for LTSA orders).

## Consequences

The primary benefit of integrating with **LTSA** is that a large amount of property data is only available via this service.
In order to support automatic form field population of MOTI properties, LTSA integration is required. Automatic form population provides

- Decreased time required to add/update property information
- Reduced user error and increased data quality
- Single source of truth for relevant fields
