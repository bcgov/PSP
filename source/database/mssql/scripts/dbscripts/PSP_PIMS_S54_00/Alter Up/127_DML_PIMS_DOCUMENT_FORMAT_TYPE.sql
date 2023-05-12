/* -----------------------------------------------------------------------------
Populate the PIMS_DOCUMENT_FORMAT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-01  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_FORMAT_TYPE
GO

INSERT INTO PIMS_DOCUMENT_FORMAT_TYPE (DOCUMENT_FORMAT_TYPE_CODE, DESCRIPTION, EFFECTIVE_DATE)
VALUES
  ('TXT',  'Text file',                                 CONVERT(DATE, '1900-01-01')),
  ('PDF',  'Portable Document Format',                  CONVERT(DATE, '1900-01-01')),
  ('DOCX', 'Microsoft Office Open XML document',        CONVERT(DATE, '1900-01-01')),
  ('DOC',  'Microsoft Word document',                   CONVERT(DATE, '1900-01-01')),
  ('XLSX', 'Microsoft Office Open XML worksheet sheet', CONVERT(DATE, '1900-01-01')),
  ('XLS',  'Microsoft Excel worksheet sheet (97�2003)', CONVERT(DATE, '1900-01-01')),
  ('HTML', 'HTML HyperText Markup Language',            CONVERT(DATE, '1900-01-01')),
  ('ODT',  'OpenDocument text document',                CONVERT(DATE, '1900-01-01')),
  ('PNG',  'Portable Network Graphic',                  CONVERT(DATE, '1900-01-01')),
  ('JPG',  'Joint Photographic Experts Group',          CONVERT(DATE, '1900-01-01')),
  ('BMP',  'Microsoft Windows Bitmap formatted image',  CONVERT(DATE, '1900-01-01')),
  ('TIF',  'Tagged Image File Format',                  CONVERT(DATE, '1900-01-01')),
  ('TIFF', 'Tagged Image File Format',                  CONVERT(DATE, '1900-01-01')),
  ('JPEG', 'Joint Photographic Experts Group',          CONVERT(DATE, '1900-01-01')),
  ('GIF',  'Graphics Interchange Format',               CONVERT(DATE, '1900-01-01')),
  ('SHP',  'ESRI shapefile',                            CONVERT(DATE, '1900-01-01')),
  ('GML',  'Geography Markup Language file',            CONVERT(DATE, '1900-01-01')),
  ('KML',  'Keyhole Markup Language',                   CONVERT(DATE, '1900-01-01')),
  ('KMZ',  'Google Earth Placemark File',               CONVERT(DATE, '1900-01-01'));
GO
