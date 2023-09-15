import { useKeycloak } from '@react-keycloak/web';
import { noop } from 'lodash';

import { Claims } from '@/constants/claims';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import { mockDocumentsResponse } from '@/mocks/documents.mock';
import { cleanup, mockKeycloak, render, RenderOptions } from '@/utils/test-utils';

import { DocumentResults, IDocumentResultProps } from './DocumentResults';

const setSort = jest.fn();

// mock auth library
jest.mock('@react-keycloak/web');

(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      organizations: [1],
      roles: [],
    },
    subject: 'test',
  },
});

// render component under test
const setup = (renderOptions: RenderOptions & Partial<IDocumentResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <DocumentResults
      sort={{}}
      results={results ?? []}
      setSort={setSort}
      onViewDetails={noop}
      onDelete={noop}
    />,
    {
      ...rest,
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  // long css selector to: get the FIRST header or table, then get the SVG up/down sort buttons
  const sortButtons = utils.container.querySelector(
    '.table .thead .tr .sortable-column div',
  ) as HTMLElement;
  return {
    ...utils,
    tableRows,
    sortButtons,
  };
};

describe('Document Results Table', () => {
  beforeEach(() => {
    mockKeycloak({ claims: [] });
    setSort.mockClear();
  });
  afterEach(() => {
    cleanup();
  });
  afterAll(() => {
    jest.restoreAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('No matching Documents found');
    expect(toasts[0]).toBeVisible();
  });

  it('displays document view button', async () => {
    mockKeycloak({ claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT] });
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const viewButtons = await getAllByTestId('document-view-button');
    expect(viewButtons[0]).toBeVisible();
  });

  it('displays document filename as link', async () => {
    mockKeycloak({ claims: [Claims.DOCUMENT_VIEW] });
    const { queryByTestId, getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const filenameLink = await getAllByTestId('document-view-filename-link');
    expect(filenameLink[0]).toBeVisible();

    expect(queryByTestId('document-view-filename-text')).toBeNull();
  });

  it('displays document filename as plain text', async () => {
    mockKeycloak({ claims: [] });
    const { queryByTestId, getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const filenameText = await getAllByTestId('document-view-filename-text');
    expect(filenameText[0]).toBeVisible();

    expect(queryByTestId('document-view-filename-link')).toBeNull();
  });

  it('displays document delete button', async () => {
    mockKeycloak({ claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE] });
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const deleteButtons = await getAllByTestId('document-delete-button');
    expect(deleteButtons[0]).toBeVisible();
  });

  it('displays document delete button', async () => {
    mockKeycloak({ claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE] });
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const deleteButtons = await getAllByTestId('document-delete-button');
    expect(deleteButtons[0]).toBeVisible();
  });
});
