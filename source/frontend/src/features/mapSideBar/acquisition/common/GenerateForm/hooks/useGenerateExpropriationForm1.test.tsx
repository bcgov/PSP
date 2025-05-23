import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { fromContactSummary } from '@/interfaces';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { DocumentGenerationRequest } from '@/models/api/DocumentGenerationRequest';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { Api_GenerateExpropriationForm1 } from '@/models/generate/acquisition/GenerateExpropriationForm1';

import { ExpropriationForm1Model } from '../../../tabs/expropriation/models';
import { useGenerateExpropriationForm1 } from './useGenerateExpropriationForm1';

const generateFn = vi
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getAcquisitionFileFn = vi.fn();
const getAcquisitionFilePropertiesFn = vi.fn();
const getPersonConceptFn = vi.fn();
const getOrganizationConceptFn = vi.fn();
const getInterestHoldersFn = vi.fn();

vi.mock('@/features/documents/hooks/useDocumentGenerationRepository');
vi.mocked(useDocumentGenerationRepository).mockImplementation(
  () =>
    ({
      generateDocumentDownloadWrappedRequest: generateFn,
    } as unknown as ReturnType<typeof useDocumentGenerationRepository>),
);

vi.mock('@/hooks/repositories/useAcquisitionProvider');
vi.mocked(useAcquisitionProvider).mockImplementation(
  () =>
    ({
      getAcquisitionFile: { execute: getAcquisitionFileFn } as any,
      getAcquisitionProperties: { execute: getAcquisitionFilePropertiesFn } as any,
    } as unknown as ReturnType<typeof useAcquisitionProvider>),
);

vi.mock('@/hooks/repositories/useInterestHolderRepository');
vi.mocked(useInterestHolderRepository).mockImplementation(
  () =>
    ({
      getAcquisitionInterestHolders: { execute: getInterestHoldersFn },
    } as unknown as ReturnType<typeof useInterestHolderRepository>),
);

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mocked(useApiContacts).mockImplementation(
  () =>
    ({
      getPersonConcept: getPersonConceptFn,
      getOrganizationConcept: getOrganizationConceptFn,
    } as unknown as ReturnType<typeof useApiContacts>),
);

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

  const { result } = renderHook(useGenerateExpropriationForm1, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateExpropriationForm1 functions', () => {
  beforeEach(() => {
    getInterestHoldersFn.mockResolvedValue(
      mockAcquisitionFileResponse().acquisitionFileInterestHolders,
    );
  });

  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => generate(1, new ExpropriationForm1Model()));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesFn).toHaveBeenCalled();
    expect(getInterestHoldersFn).toHaveBeenCalled();
    // owner solicitor call
    expect(getPersonConceptFn).toHaveBeenCalled();
    // expropriation authority call (not present)
    expect(getOrganizationConceptFn).not.toHaveBeenCalled();
  });

  it('makes requests to expected api endpoints when expropriation authority is provided', async () => {
    const expropFormValues = new ExpropriationForm1Model();
    expropFormValues.expropriationAuthority.contact = fromContactSummary(
      getMockContactOrganizationWithOnePerson(),
    );
    const generate = setup();
    await act(async () => generate(1, expropFormValues));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesFn).toHaveBeenCalled();
    expect(getInterestHoldersFn).toHaveBeenCalled();
    // owner solicitor call
    expect(getPersonConceptFn).toHaveBeenCalled();
    // expropriation authority call
    expect(getOrganizationConceptFn).toHaveBeenCalled();
  });

  it('throws an error if no acquisition file is found', async () => {
    const generate = setup();
    getAcquisitionFileFn.mockResolvedValue(undefined);
    await expect(generate(1, new ExpropriationForm1Model())).rejects.toThrow(
      'Acquisition file not found',
    );
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(1, new ExpropriationForm1Model())).rejects.toThrow(
      'Failed to generate file',
    );
  });
});
