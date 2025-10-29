import { Claims } from '@/constants/claims';
import { render, RenderOptions } from '@/utils/test-utils';
import { DocumentSearchResults, IDocumentSearchResultsProps } from './DocumentSearchResults';
import { mockDocumentSearchResultsResponse } from '@/mocks/documents.mock';
import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<IDocumentSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <DocumentSearchResults results={results ?? mockDocumentSearchResultsResponse()} />,
    {
      claims: [Claims.PROJECT_VIEW, Claims.ACQUISITION_VIEW],
      ...rest,
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

describe('Documents Search Results Table', () => {
  it('matches snapshot', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    const toasts = await findAllByText('No matching Documents found');

    expect(tableRows.length).toBe(0);
    expect(toasts[0]).toBeVisible();
  });

  it('displays the Property identifier', async () => {
    const { tableRows, getByTestId } = setup({
      results: [
        {
          acquisitionDocuments: [],
          dispositionDocuments: [],
          leaseDocuments: [],
          managementDocuments: [],
          mgmtActivitiesDocuments: [],
          projectDocuments: [],
          propertiesDocuments: [
            {
              property: {
                id: 37,
                propertyType: null,
                anomalies: [],
                tenures: [],
                roadTypes: [],
                status: null,
                dataSource: null,
                region: null,
                district: null,
                dataSourceEffectiveDateOnly: '2021-08-31',
                latitude: 992594.3513207644,
                longitude: 1213247.77464335,
                isRetired: false,
                pphStatusUpdateUserid: null,
                pphStatusUpdateTimestamp: null,
                pphStatusUpdateUserGuid: null,
                isRwyBeltDomPatent: false,
                pphStatusTypeCode: null,
                address: {
                  id: 1,
                  streetAddress1: '45 - 904 Hollywood Crescent',
                  streetAddress2: 'Living in a van',
                  streetAddress3: 'Down by the River',
                  municipality: 'Hollywood North',
                  provinceStateId: 1,
                  province: null,
                  countryId: 1,
                  country: {
                    id: 1,
                    code: 'CA',
                    description: 'Canada',
                    displayOrder: 1,
                  },
                  districtCode: null,
                  district: null,
                  region: null,
                  regionCode: null,
                  countryOther: null,
                  postal: 'V6Z 5G7',
                  latitude: null,
                  longitude: null,
                  comment: null,
                  rowVersion: 2,
                },
                pid: 15937551,
                pin: null,
                planNumber: null,
                isOwned: false,
                areaUnit: null,
                landArea: 1,
                isVolumetricParcel: false,
                volumetricMeasurement: null,
                volumetricUnit: null,
                volumetricType: null,
                landLegalDescription: null,
                municipalZoning: null,
                location: {
                  coordinate: {
                    x: 1213247.77464335,
                    y: 992594.3513207644,
                  },
                },
                boundary: {
                  type: 'Polygon',
                  coordinates: [
                    [
                      [1213242.4695999988, 992600.2574000051],
                      [1213243.7601999992, 992600.5253999997],
                      [1213242.576600001, 992597.9130000025],
                      [1213243.576, 992575.8795999978],
                      [1213253.3860000002, 992580.7179999985],
                      [1213252.4069999997, 992602.3270000033],
                      [1213251.9290000002, 992612.8686000016],
                      [1213242.113199999, 992608.0316000003],
                      [1213242.4695999988, 992600.2574000051],
                    ],
                  ],
                },
                generalLocation: null,
                historicalFileNumbers: [],
                surplusDeclarationType: null,
                surplusDeclarationComment: null,
                surplusDeclarationDate: '0001-01-01',
                rowVersion: 3,
              },
              id: 1,
              parentId: '37',
              parentNameOrNumber: '015-937-551',
              document: {
                id: 23,
                fileName: 'Form12_Carbone_Template.docx',
                documentType: {
                  id: 67,
                  documentType: 'BRIENOTE',
                  documentTypeDescription: 'Briefing notes',
                  documentTypePurpose: null,
                  mayanId: 85,
                  isDisabled: false,
                  appCreateTimestamp: '2025-10-27T22:09:06.29',
                  appLastUpdateTimestamp: '2025-10-27T22:12:24.617',
                  appLastUpdateUserid: 'EHERRERA',
                  appCreateUserid: 'EHERRERA',
                  appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                  appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                  rowVersion: 2,
                },
                statusTypeCode: {
                  id: 'DRAFT',
                  description: 'Draft',
                  isDisabled: false,
                  displayOrder: 2,
                },
                documentQueueStatusTypeCode: {
                  id: 'SUCCESS',
                  description: 'Success',
                  isDisabled: false,
                  displayOrder: 5,
                },
                mayanDocumentId: 4,
                appCreateTimestamp: '2025-10-27T22:31:17.967',
                appLastUpdateTimestamp: '2025-10-27T22:31:32.39',
                appLastUpdateUserid: 'service',
                appCreateUserid: 'EHERRERA',
                appLastUpdateUserGuid: '00000000-0000-0000-0000-000000000000',
                appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                rowVersion: 2,
              },
              relationshipType: ApiGen_CodeTypes_DocumentRelationType.Properties,
              appCreateTimestamp: '2025-10-27T22:31:18.087',
              appLastUpdateTimestamp: '2025-10-27T22:31:18.087',
              appLastUpdateUserid: 'EHERRERA',
              appCreateUserid: 'EHERRERA',
              appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              rowVersion: 1,
            },
          ],
          researchDocuments: [],
          documentRelationships: [
            {
              id: 1,
              parentId: '37',
              parentNameOrNumber: '015-937-551',
              document: {
                id: 23,
                fileName: 'Form12_Carbone_Template.docx',
                documentType: {
                  id: 67,
                  documentType: 'BRIENOTE',
                  documentTypeDescription: 'Briefing notes',
                  documentTypePurpose: null,
                  mayanId: 85,
                  isDisabled: false,
                  appCreateTimestamp: '2025-10-27T22:09:06.29',
                  appLastUpdateTimestamp: '2025-10-27T22:12:24.617',
                  appLastUpdateUserid: 'EHERRERA',
                  appCreateUserid: 'EHERRERA',
                  appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                  appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                  rowVersion: 2,
                },
                statusTypeCode: {
                  id: 'DRAFT',
                  description: 'Draft',
                  isDisabled: false,
                  displayOrder: 2,
                },
                documentQueueStatusTypeCode: {
                  id: 'SUCCESS',
                  description: 'Success',
                  isDisabled: false,
                  displayOrder: 5,
                },
                mayanDocumentId: 4,
                appCreateTimestamp: '2025-10-27T22:31:17.967',
                appLastUpdateTimestamp: '2025-10-27T22:31:32.39',
                appLastUpdateUserid: 'service',
                appCreateUserid: 'EHERRERA',
                appLastUpdateUserGuid: '00000000-0000-0000-0000-000000000000',
                appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
                rowVersion: 2,
              },
              relationshipType: ApiGen_CodeTypes_DocumentRelationType.Properties,
              appCreateTimestamp: '2025-10-27T22:31:18.087',
              appLastUpdateTimestamp: '2025-10-27T22:31:18.087',
              appLastUpdateUserid: 'EHERRERA',
              appCreateUserid: 'EHERRERA',
              appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
              rowVersion: 1,
            },
          ],
          id: 23,
          fileName: 'Form12_Carbone_Template.docx',
          documentType: {
            id: 67,
            documentType: 'BRIENOTE',
            documentTypeDescription: 'Briefing notes',
            documentTypePurpose: null,
            mayanId: 85,
            isDisabled: false,
            appCreateTimestamp: '2025-10-27T22:09:06.29',
            appLastUpdateTimestamp: '2025-10-27T22:12:24.617',
            appLastUpdateUserid: 'EHERRERA',
            appCreateUserid: 'EHERRERA',
            appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
            appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
            rowVersion: 2,
          },
          statusTypeCode: {
            id: 'DRAFT',
            description: 'Draft',
            isDisabled: false,
            displayOrder: 2,
          },
          documentQueueStatusTypeCode: {
            id: 'SUCCESS',
            description: 'Success',
            isDisabled: false,
            displayOrder: 5,
          },
          mayanDocumentId: 4,
          appCreateTimestamp: '2025-10-27T22:31:17.967',
          appLastUpdateTimestamp: '2025-10-27T22:31:32.39',
          appLastUpdateUserid: 'service',
          appCreateUserid: 'EHERRERA',
          appLastUpdateUserGuid: '00000000-0000-0000-0000-000000000000',
          appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
          rowVersion: 2,
        },
      ],
      claims: [Claims.PROPERTY_VIEW,]
    });

    const propertyIdentifier = getByTestId('property-id-37');
    expect(propertyIdentifier).toBeInTheDocument();
    expect(tableRows.length).toBe(1);
  });
});
