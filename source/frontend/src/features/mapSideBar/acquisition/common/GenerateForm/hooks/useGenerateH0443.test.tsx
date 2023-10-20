import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProperties } from '@/hooks/repositories/useProperties';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_Property } from '@/models/api/Property';

import { useGenerateH0443 } from './useGenerateH0443';

const getPropertiesFn = jest.fn<Api_Property[], any[]>();
const generateFn = jest.fn();
const getAcquisitionFileFn = jest.fn<Api_AcquisitionFile | undefined, any[]>();
const getPersonConceptFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFile: { execute: getAcquisitionFileFn },
}));

jest.mock('@/hooks/repositories/useProperties');
(useProperties as jest.Mock).mockImplementation(() => ({
  getMultiplePropertiesById: { execute: getPropertiesFn },
}));

jest.mock('@/hooks/pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
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

const setup = (params?: { storeValues?: any; acquisitionResponse?: Api_AcquisitionFile }) => {
  var acquisitionResponse = mockAcquisitionFileResponse();
  if (params?.acquisitionResponse !== undefined) {
    acquisitionResponse = params.acquisitionResponse;
  }

  getAcquisitionFileFn.mockReturnValue(acquisitionResponse);

  const { result } = renderHook(useGenerateH0443, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateH0443 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
    });
  });

  it('makes requests to expected api endpoints for each required team member', async () => {
    const responseWithTeam: Api_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          personId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
        },
        {
          id: 2,
          acquisitionFileId: 1,
          personId: 2,
          teamProfileTypeCode: 'PROPAGENT',
          rowVersion: 2,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConceptFn).toHaveBeenCalledTimes(2);
    });
  });

  it('makes requests to expected api endpoints if there are properties', async () => {
    const responseWithTeam: Api_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      fileProperties: [
        {
          propertyId: 1,
        },
        {
          propertyId: 2,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getPropertiesFn).toHaveBeenCalledWith([1, 2]);
    });
  });
});
