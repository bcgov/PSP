# MAYAN-EDMS

[Mayan official site](https://www.mayan-edms.com/)
[Mayan documentation](https://docs.mayan-edms.com/index.html)

EDMS Functionality

- Documents such as forms, contracts, reports, and letters.
- Managing the entire document lifecycle, from its creation onwards, where live editing in the system is possible.
- Providing audit trails, showing who accessed a document and which changes were made.
- Check-in, check-out, and locking, where the changes of a user won’t overwrite the changes of the previous user.
- Controlling and managing different versions and making sure there's one single source of truth.
- Activating a prior version of a document.
- Sharing documents, where it is easy to circulate large files.
- Tags, author name, document name and the description of a publication.

## Prerequisites

- Docker and docker-compose must be installed and running.
- The docker psp network must be setup.

## Setup

```
docker-compose up --detach
```

This will start the webservice on the 7080 port. Runing on the

If the admin password has not been changed, mayan will create a new one for the admin and display it until it is changed.

## License

```
Copyright 2021 Province of British Columbia

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
