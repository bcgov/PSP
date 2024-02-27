import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';

import { useGenerateLetter } from '../hooks/useGenerateLetter';

const generateFn = jest.fn();
const getAcquisitionFileFn = jest.fn<ApiGen_Concepts_AcquisitionFile | undefined, any[]>();
const getPersonConceptFn = jest.fn();
const getOrganizationConceptFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFile: { execute: getAcquisitionFileFn },
}));

jest.mock('@/hooks/pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
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

  getAcquisitionFileFn.mockReturnValue(acquisitionResponse);

  const { result } = renderHook(useGenerateLetter, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateLetter functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
    });
  });
  it('makes requests to expected api endpoints if a team member is a property coordinator with person', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          personId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organization: null,
          organizationId: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConceptFn).toHaveBeenCalled();
    });
  });

  it('makes requests to expected api endpoints if a team member is a property coordinator with org', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          organizationId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organization: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getOrganizationConceptFn).toHaveBeenCalled();
    });
  });
});
