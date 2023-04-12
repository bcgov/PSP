import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import find from 'lodash/find';
import { mockProperties } from 'mocks/filterDataMock';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';
import { networkSlice } from 'store/slices/network/networkSlice';

import { useApiContacts } from './pims-api/useApiContacts';
import { useGenerateLetter } from './useGenerateLetter';

const generateFn = jest.fn();
const getPersonConcept = jest.fn();

jest.mock('features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('./pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConcept,
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

const setup = (values?: any) => {
  const { result } = renderHook(useGenerateLetter, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};

describe('useGenerateLetter functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(mockAcquisitionFileResponse());
      expect(generateFn).toHaveBeenCalled();
    });
  });
  it('makes requests to expected api endpoints if a team member is a property coordinator', async () => {
    const generate = setup();
    await act(async () => {
      await generate({
        ...mockAcquisitionFileResponse(),
        acquisitionTeam: [
          {
            id: 1,
            personId: 1,
            person: {
              id: 1,
              isDisabled: false,
              surname: 'Smith',
              firstName: 'Bob',
              middleNames: 'Billy',
              preferredName: 'Tester McTest',
              personOrganizations: [],
              personAddresses: [],
              contactMethods: [],
              comment: 'This is a test comment.',
              rowVersion: 2,
            },
            personProfileTypeCode: 'PROPCOORD',
            personProfileType: {
              id: 'PROPCOORD',
              description: 'Property Coordinator',
              isDisabled: false,
            },
            isDisabled: false,
            rowVersion: 2,
          },
        ],
      });
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConcept).toHaveBeenCalled();
    });
  });
});
