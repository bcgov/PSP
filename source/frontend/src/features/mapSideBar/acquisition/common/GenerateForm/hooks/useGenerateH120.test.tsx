import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from '@/hooks/repositories/useH120CategoryRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { emptyApiInterestHolder } from '@/mocks/interestHolder.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';

import { useGenerateH120 } from './useGenerateH120';

const generateFn = jest
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getAcquisitionFileFn = jest.fn<Promise<ApiGen_Concepts_AcquisitionFile | undefined>, any[]>();
const getAcquisitionPropertiesFn = jest.fn();
const getAcquisitionCompReqH120s = jest.fn();
const getH120sCategoryFn = jest.fn();
const getInterestHoldersFn = jest.fn();
const getPersonConceptFn = jest.fn();
const getOrganizationConceptFn = jest.fn();
const findElectoralDistrictFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useH120CategoryRepository');
(useH120CategoryRepository as jest.Mock).mockImplementation(() => ({
  execute: getH120sCategoryFn,
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFile: { execute: getAcquisitionFileFn },
  getAcquisitionProperties: { execute: getAcquisitionPropertiesFn },
  getAcquisitionCompReqH120s: { execute: getAcquisitionCompReqH120s },
}));

jest.mock('@/hooks/repositories/useInterestHolderRepository');
(useInterestHolderRepository as jest.Mock).mockImplementation(() => ({
  getAcquisitionInterestHolders: { execute: getInterestHoldersFn },
}));

jest.mock('@/hooks/pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
  getOrganizationConcept: getOrganizationConceptFn,
}));

jest.mock('@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer');
(useAdminBoundaryMapLayer as jest.Mock).mockImplementation(() => ({
  findElectoralDistrict: findElectoralDistrictFn,
}));

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

const setup = (params?: {
  storeValues?: any;
  acquisitionResponse?: ApiGen_Concepts_AcquisitionFile;
}) => {
  var acquisitionResponse = mockAcquisitionFileResponse();
  if (params?.acquisitionResponse !== undefined) {
    acquisitionResponse = params.acquisitionResponse;
  }

  getAcquisitionFileFn.mockResolvedValue(acquisitionResponse);

  const { result } = renderHook(useGenerateH120, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateH120 functions', () => {
  beforeEach(() => {
    findElectoralDistrictFn.mockResolvedValue({ properties: { ED_NAME: 'MOCK DISTRICT' } });
    getAcquisitionPropertiesFn.mockResolvedValue(mockAcquisitionFileResponse().fileProperties);
  });

  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => generate(getMockApiDefaultCompensation()));
    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionPropertiesFn).toHaveBeenCalled();
    expect(getInterestHoldersFn).toHaveBeenCalled();
    expect(findElectoralDistrictFn).toHaveBeenCalled();
  });

  it('makes request to get person concept for compensation payee', async () => {
    const apiCompensation: ApiGen_Concepts_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      acquisitionFileTeam: {
        id: 101,
        acquisitionFileId: 2,
        personId: 8,
        teamProfileTypeCode: 'MOTILAWYER',
        rowVersion: 1,
        organization: null,
        person: null,
        primaryContact: null,
        primaryContactId: null,
        teamProfileType: null,
        organizationId: null,
      },
    };
    const generate = setup();
    await act(async () => generate(apiCompensation));
    expect(getPersonConceptFn).toHaveBeenCalled();
  });

  it('makes request to get organization concept for compensation payee', async () => {
    const apiCompensation: ApiGen_Concepts_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      acquisitionFileTeam: {
        id: 101,
        acquisitionFileId: 2,
        organizationId: 8,
        teamProfileTypeCode: 'MOTILAWYER',
        rowVersion: 1,
        organization: null,
        person: null,
        personId: null,
        primaryContact: null,
        primaryContactId: null,
        teamProfileType: null,
      },
    };
    const generate = setup();
    await act(async () => generate(apiCompensation));
    expect(getOrganizationConceptFn).toHaveBeenCalled();
  });

  it('searches interest holder array for compensation payee ', async () => {
    const apiCompensationWithInterestHolder: ApiGen_Concepts_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      interestHolderId: 14,
    };
    const apiInterestHolder: ApiGen_Concepts_InterestHolder = {
      ...emptyApiInterestHolder,
      interestHolderId: 14,
      acquisitionFileId: 2,
      personId: 8,
      person: {
        id: 8,
        isDisabled: false,
        surname: 'Smith',
        firstName: 'Devin',
        personOrganizations: [],
        personAddresses: [],
        contactMethods: [],
        rowVersion: 1,
        comment: null,
        middleNames: null,
        preferredName: null,
      },
      rowVersion: 1,
    };
    getInterestHoldersFn.mockResolvedValue([apiInterestHolder]);

    const generate = setup();
    await act(async () => generate(apiCompensationWithInterestHolder));
    expect(apiCompensationWithInterestHolder.interestHolder?.person).toStrictEqual(
      expect.objectContaining(apiInterestHolder.person),
    );
  });
  it('throws an error if no compensation is passed', async () => {
    const generate = setup();
    await expect(generate({} as any)).rejects.toThrow(
      'user must choose a valid compensation requisition in order to generate a document',
    );
  });

  it('throws an error if no acquisition file is found', async () => {
    const generate = setup();
    getAcquisitionFileFn.mockResolvedValue(undefined);
    await expect(generate(getMockApiDefaultCompensation())).rejects.toThrow(
      'Acquisition file not found',
    );
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(getMockApiDefaultCompensation())).rejects.toThrow(
      'Failed to generate file',
    );
  });
});
