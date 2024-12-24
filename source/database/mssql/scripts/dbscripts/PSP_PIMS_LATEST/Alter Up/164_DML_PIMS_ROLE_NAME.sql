UPDATE
    PIMS_ROLE
SET
    NAME = 'Lease License functional',
    CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE
    NAME = 'Lease/License functional';

UPDATE
    PIMS_ROLE
SET
    NAME = 'Lease License read-only',
    CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE
    NAME = 'Lease/License read-only';