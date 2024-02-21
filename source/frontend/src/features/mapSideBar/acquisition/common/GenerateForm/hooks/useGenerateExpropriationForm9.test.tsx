import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockContactOrganizationWithOnePerson } from '@/mocks/contacts.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { ExpropriationForm9Model } from '../../../tabs/expropriation/models';
import { useGenerateExpropriationForm9 } from './useGenerateExpropriationForm9';

const generateFn = jest
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });
const getAcquisitionFileFn = jest.fn<Promise<ApiGen_Concepts_AcquisitionFile | undefined>, any[]>();
const getAcquisitionFilePropertiesFn = jest.fn<
  Promise<ApiGen_Concepts_Property[] | undefined>,
  any[]
>();
const getOrganizationConceptFn = jest.fn();
const getInterestHoldersFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFile: { execute: getAcquisitionFileFn },
  getAcquisitionProperties: { execute: getAcquisitionFilePropertiesFn },
}));

jest.mock('@/hooks/repositories/useInterestHolderRepository');
(useInterestHolderRepository as jest.Mock).mockImplementation(() => ({
  getAcquisitionInterestHolders: { execute: getInterestHoldersFn },
}));

jest.mock('@/hooks/pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getOrganizationConcept: getOrganizationConceptFn,
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

  const { result } = renderHook(useGenerateExpropriationForm9, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateExpropriationForm5 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => generate(1, new ExpropriationForm9Model()));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesFn).toHaveBeenCalled();
    expect(getInterestHoldersFn).toHaveBeenCalled();
    expect(getOrganizationConceptFn).not.toHaveBeenCalled();
  });

  it('makes requests to expected api endpoints when expropriation authority is provided', async () => {
    const expropFormValues = new ExpropriationForm9Model();
    expropFormValues.expropriationAuthority.contact = getMockContactOrganizationWithOnePerson();
    const generate = setup();
    await act(async () => generate(1, expropFormValues));

    expect(generateFn).toHaveBeenCalled();
    expect(getAcquisitionFilePropertiesFn).toHaveBeenCalled();
    expect(getInterestHoldersFn).toHaveBeenCalled();
    expect(getOrganizationConceptFn).toHaveBeenCalled();
  });

  it('throws an error if no acquisition file is found', async () => {
    const generate = setup();
    getAcquisitionFileFn.mockResolvedValue(undefined);
    await expect(generate(1, new ExpropriationForm9Model())).rejects.toThrow(
      'Acquisition file not found',
    );
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(1, new ExpropriationForm9Model())).rejects.toThrow(
      'Failed to generate file',
    );
  });
});
