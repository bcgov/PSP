# EF Core Model - Database First

## Status

> Accepted

> January 2021

## Context

The PSP project was originally designed with two conceptual layers: they controller layer and the service layer. The controller layer was responsible for all REST related logic, such as authentication, authorization and serializing and deserializing json. The service layer was responsible for all business logic as well as database logic.

This design suffers from a few issues, but in general violates SOLID design principles. Specifically, single responsibility principle (repository handles business and db logic). As a result, code reuse is difficult as each repository method is specific to a given operation.

## Decision

The decision was made to separate the service layer into a repository and a service layer. The service layer is now responsible for business logic. The repository layer is now responsible for database logic. The controller will now only have access to the service layer (which will be part of the pims.api project). The service layer will handle all database access.

## Consequences

Simplified database interfaces/methods
Easier to compose multiple database operations in a single transaction
Separation of concerns and abstractions clearer
Unit testing business logic easier, no longer requires a mocked database
Existing test methods need to be re-written to better support new architecture
Reduces need for unique models/maps per endpoint



