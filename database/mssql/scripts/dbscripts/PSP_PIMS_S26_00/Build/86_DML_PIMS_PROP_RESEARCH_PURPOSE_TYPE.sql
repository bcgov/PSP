/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROP_RESEARCH_PURPOSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROP_RESEARCH_PURPOSE_TYPE
GO

INSERT INTO PIMS_PROP_RESEARCH_PURPOSE_TYPE (PROP_RESEARCH_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'RDCLOSE',  N'Road Closure'),
  (N'ACQUIRE',  N'Acquisition'),
  (N'DISPOSE',  N'Disposition'),
  (N'LICLEASE', N'License/Lease'),
  (N'FNTREATY', N'FN/Treaty'),
  (N'LNDXCHG',  N'Land Exchange'),
  (N'INQUIRY',  N'Inquiry'),
  (N'FORM12',   N'Form 12'),
  (N'FOI',      N'FOI'),
  (N'RESEARCH', N'Research'),
  (N'DOTHER',   N'District Other'),
  (N'OTHER',    N'Other'),
  (N'ISSUE',    N'Issue'),
  (N'CLASS',    N'Classification'),
  (N'LEGAL',    N'Legal'),
  (N'LISSUE',   N'Land Issue'),
  (N'TAC',      N'TAC'),
  (N'REGINT',   N'Registered Interest'),
  (N'UNREGINT', N'Unregistered Interest'),
  (N'MOTIOWND', N'MoTI Owned'),
  (N'HWY',      N'Highway'),
  (N'UNKNOWN',  N'Unknown');
GO
