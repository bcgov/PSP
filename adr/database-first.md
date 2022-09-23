# EF Core Model - Database First

## Status

> Accepted

> October  2021

## Context

The PSP project originally used EF Core Code First to generate the Database. However, due to MOTI database standardization rules, such as history, FKs, column naming, and triggers,
convential EF Core Code first was not feasible. In addition, database changes requested by MOTI came in the form of generated schema from Aqua Data Studio, often thousands of lines long.
As a result of this, creating Code first migrations was error prone, would take multiple days each sprint, and would often result in schema that was not identical to what was expected by 
Aqua Data Studio. This schema often did not conform to MOTI standards due to limitations on EF Core Code First tooling.

## Decision

The decision was made to change the database from code first to database first in IS15. This required that the database was built from Aqua Data Studio scripts, and then using EF Core's scaffold,
would generate all EF core entities. All existing entities were deleted. All Fluent EF Configurations were also removed as they would no longer be used.  

## Consequences

Loss of control over generated EF entities. more difficult to implement inheritence based logic. Mitigated by partial entity classes.
Loss of EF Core tooling. Now completely reliant on provided DB scripts to update database. This has introduced a manual database migration during release that was not previously necessary.
Reliance on provided db scripts has been costly. Scripts have frequently contained errors or have not been compatible with non-empty databases. This has led to lengthy unscheduled troubleshooting.
Loss of auditing. Now reliant on DB scripts to provide accurate picture of changes made to a database. This has proved error prone.
Much faster implementation time on new database schema. Since no Manual coding is required, updating entities from schema changes only takes time if those entity changes affect existing application logic.
Deployed database schema meets MOTI standards, or at least, is closer to them.
No more issues with alignment between designed schema and actual application schema.



