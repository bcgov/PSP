# EF Core Model - Database First

## Status

> Accepted

> January 2021

## Context

The PSP project previously used the following pattern to save lists of related child entities in EF:
https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities#handling-deletes

This was done on a per repository basis, where a repository is defined as a class that handles all business and database logic required to save an entity and related entities to the database.

The issue with this architecture was primarily code duplication. Every single repository contained some version of this pattern, sometimes with minor variations. This reduced the readability of these interfaces. This pattern also introduced the risk of unintentional changes, since there was no way to "limit" which passed child entities should be updated.

## Decision

The development team evaluated two options:
1) The API would continue to be responsible for determining what to add/update/delete, but the above pattern would be implemented as a generic method. The generic method accepts a list of navigation properties that should be updated from the passed entity.
2) The frontend would determine what entities should be add/update/delete within the database. Essentially metadata would be added to REST endpoints that require the modification of child entities. These endpoints would annotate entities being passed to the backend with add/update/delete flags. The backend would then process that metadata and use it to determine what operations should be performed on the passed entities.

The team decided on option #1. The following documents that decision and the conversation that was had:

### Pros(Client side)
Reduced network traffic.	
Repo/service layer for each operation is more structured in add/update/delete style.
Uniform add/remove/delete operations independent of frontend.
### Cons(Client side)
More work client side to determine list of changes and to track that list accurately.	
Computational overhead pushed to client

### Pros(Server side)
Very low boilerplate on both server and client	A lot of complexity is being hidden by the updateChild method, which is complex and needs reflection to function properly.
EF handles update checking.
### Cons(Server side)
Debugging is likely more complicated. Mitigated by adding more logging
EF handles update checking. Mitigated by adding more logging
Individual add/update/delete looks different then updating a list on the service layer.


Based on this conversation, a lot of our problems are caused by tight coupling between the database and the services, not necessarily client vs server side determination of changes.           

Decision:
Dev’s discussed above at length. Determined that there are few differences between the above pro’s/cons. Agreed that client side determination makes sense in the case of batch imports, but not in the case of updating lists of child entities. Server-side determination will be used for child entity updates
Devs determined that an endpoint with the ability to update the parent, as well as zero or more children would be helpful to allow the backend to provide update services that are independent of the FE implementation. Decided upon spending some additional time to determine a ChangeSet controller model that would optionally contain the parent and zero or more children. Any entities present in this object would then be updated via independent services. Ie. If a changeset was passed with a parent, as well as children A and D. the updateParent service function would be called, as well as the updateAChildren and updateDChildren service functions (all on the ParentService). updateBChildren and updateCChildren would not be called in this example because the B and C child collections were not part of the passed changeSet.


## Consequences
Parent row version and id now being passed when updating child entities even if logically from a REST standpoint that information should not be required.
Cleaner repositories
Entity repositories impose more strict limits on what child entity modifications are accepted, limiting unintentional changes.
Additional research required on the ideal structure of REST endpoints to support changesets.



