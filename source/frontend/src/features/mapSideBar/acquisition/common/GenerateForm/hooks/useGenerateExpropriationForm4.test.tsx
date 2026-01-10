import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ExpropriationForm4Model } from '@/features/mapSideBar/acquisition/tabs/expropriation/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { fromContactSummary } from '@/interfaces';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { getMockRepositoryObj } from '@/utils/test-utils';

import { useGenerateExpropriationForm4 } from './useGenerateExpropriationForm4';

const generateFn = vi
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getOrganizationConceptFn = vi.fn();

const getAcquisitionFileMock = getMockRepositoryObj();
const getAcquisitionFilePropertiesMock = getMockRepositoryObj();
const getInterestHoldersMock = getMockRepositoryObj();

vi.mock('@/features/documents/hooks/useDocumentGenerationRepository');
vi.mocked(useDocumentGenerationRepository, { partial: true }).mockReturnValue({
  generateDocumentDownloadWrappedRequest: generateFn,
});

vi.mock('@/hooks/repositories/useAcquisitionProvider');
vi.mocked(useAcquisitionProvider, { partial: true }).mockReturnValue({
  getAcquisitionFile: getAcquisitionFileMock,
  getAcquisitionProperties: getAcquisitionFilePropertiesMock,
});

vi.mock('@/hooks/repositories/useInterestHolderRepository');
vi.mocked(useInterestHolderRepository, { partial: true }).mockReturnValue({
  getAcquisitionInterestHolders: getInterestHoldersMock,
});

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mocked(useApiContacts, { partial: true }).mockReturnValue({
  getOrganizationConcept: getOrganizationConceptFn,
});

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

  getAcquisitionFileMock.execute.mockResolvedValue(acquisitionResponse);

  const { result } = renderHook(useGenerateExpropriationForm4, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateExpropriationForm4 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => generate(1, new ExpropriationForm4Model()));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesMock.execute).toHaveBeenCalled();
    expect(getInterestHoldersMock.execute).toHaveBeenCalled();
    expect(getOrganizationConceptFn).not.toHaveBeenCalled();
  });

  it('makes requests to expected api endpoints when expropriation authority is provided', async () => {
    const expropFormValues = new ExpropriationForm4Model();
    expropFormValues.expropriationAuthority.contact = fromContactSummary(
      getMockContactOrganizationWithOnePerson(),
    );
    const generate = setup();
    await act(async () => generate(1, expropFormValues));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesMock.execute).toHaveBeenCalled();
    expect(getInterestHoldersMock.execute).toHaveBeenCalled();
    expect(getOrganizationConceptFn).toHaveBeenCalled();
  });

  it('throws an error if no acquisition file is found', async () => {
    const generate = setup();
    getAcquisitionFileMock.execute.mockResolvedValue(undefined);
    await expect(generate(1, new ExpropriationForm4Model())).rejects.toThrow(
      'Acquisition file not found',
    );
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(1, new ExpropriationForm4Model())).rejects.toThrow(
      'Failed to generate file',
    );
  });
});
