export const mockDocumentDetailResponse = () => ({
  status: 'Success',
  payload: {
    id: 6,
    label: 'PSP-4385 ETL - Add Acquisition File Data and Personnel mapping.xlsx',
    datetime_created: '2023-01-11T19:42:11.566873Z',
    description: '',
    file_latest: {
      id: 6,
      comment: '',
      encoding: 'binary',
      filename: 'PSP-4385 ETL - Add Acquisition File Data and Personnel mapping.xlsx',
      mimetype: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      size: 18873,
      timestamp: '2023-01-11T19:42:11.601866Z',
    },
    document_type: {
      id: 22,
      label: 'CDOGS template',
      delete_time_period: 30,
      delete_time_unit: 'days',
    },
  },
  httpStatusCode: 'OK',
});
