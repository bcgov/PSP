import { useKeycloak } from '@react-keycloak/web';
import { createMemoryHistory } from 'history';

import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, RenderResult } from '@/utils/test-utils';

import DocumentsPage from './DocumentsPage';

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => {
    return {
      retrieveDocumentRelationship: vi.fn(),
      retrieveDocumentRelationshipLoading: false,
    };
  },
}));

vi.mock('@/features/documents/hooks/useDocumentProvider', () => ({
  useDocumentProvider: () => {
    return {
      getDocumentRelationshipTypes: vi.fn(),
      getDocumentRelationshipTypesLoading: false,
      getDocumentTypes: vi.fn(),
      getDocumentTypesLoading: false,
    };
  },
}));

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};
const history = createMemoryHistory();

describe('Lease Documents Page', () => {
  const setup = (renderOptions: RenderOptions = {}): RenderResult => {
    // render component under test
    const result = render(<DocumentsPage />, {
      ...renderOptions,
      store: storeState,
      history,
      useMockAuthentication: true,
    });
    return result;
  };

  it('renders as expected', () => {
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });
});
