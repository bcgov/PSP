import { useKeycloak } from '@react-keycloak/web';
import noop from 'lodash/noop';

import { Claims } from '@/constants/claims';
import { DocumentRow } from '@/features/documents/ComposedDocument';
import { mockDocumentResponse, mockDocumentsResponse } from '@/mocks/documents.mock';
import { cleanup, mockKeycloak, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { DocumentResults, IDocumentResultProps } from './DocumentResults';
import { get } from 'lodash';

const setSort = vi.fn();

const onViewDetails = vi.fn();
const onDelete = vi.fn();
const onPreview = vi.fn();

// render component under test
const setup = (renderOptions: RenderOptions & Partial<IDocumentResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;
  const utils = render(
    <DocumentResults
      sort={{}}
      results={results ?? []}
      setSort={setSort}
      onViewDetails={onViewDetails}
      onDelete={onDelete}
      onPreview={onPreview}
    />,
    {
      ...rest,
      useMockAuthentication: true,
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
    // Finding elements
    getDocumentFileNameLink: (documentId: number) =>
      utils.container.querySelector(
        `button#document-view-filename-link-${documentId}`,
      ) as HTMLElement,
    getDocumentFileNameText: (documentId: number) =>
      utils.container.querySelector(
        `span#document-view-filename-text-${documentId}`,
      ) as HTMLElement,
    getDocumentProcessingIcon: (rowNumber: number) =>
      utils.container.querySelector(`svg#document-processing-${rowNumber}`) as SVGElement,
    getDocumentProcessingErrorIcon: (rowNumber: number) =>
      utils.container.querySelector(`svg#document-error-${rowNumber}`) as SVGElement,
    getDocumenDeleteIcon: (rowNumber: number) =>
      utils.container.querySelector(`svg#document-delete-${rowNumber}`) as SVGElement,
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
    vi.restoreAllMocks();
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
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT],
    });

    const viewButtons = await getAllByTestId('document-view-button');
    expect(viewButtons[0]).toBeVisible();
  });

  it('displays document filename as link', async () => {
    const { getDocumentFileNameLink, getDocumentFileNameText } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW],
    });

    const filenameLink = await getDocumentFileNameLink(20);
    expect(filenameLink).toBeVisible();

    expect(getDocumentFileNameText(20)).toBeNull();
  });

  it('displays document filename as plain text', async () => {
    mockKeycloak({ claims: [] });
    const { getDocumentFileNameLink, getDocumentFileNameText } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
    });

    const filenameText = await getDocumentFileNameText(20);
    expect(filenameText).toBeVisible();

    expect(getDocumentFileNameLink(20)).toBeNull();
  });

  it('displays document processing icon', async () => {
    const { getDocumentProcessingIcon } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
    });

    expect(getDocumentProcessingIcon(2)).toBeVisible();
  });

  it('displays document PIMS ERROR processing icon', async () => {
    const { getDocumentProcessingErrorIcon } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
    });

    expect(getDocumentProcessingErrorIcon(3)).toBeVisible();
  });

  it('displays document MAYAN ERROR processing icon', async () => {
    const { getDocumentProcessingErrorIcon } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
    });

    expect(getDocumentProcessingErrorIcon(4)).toBeVisible();
  });

  it('displays document delete button', async () => {
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
    });

    const deleteButtons = await getAllByTestId('document-delete-button');
    expect(deleteButtons[0]).toBeVisible();
  });

  it('displays default number of entries of 10', async () => {
    const largeDataset = Array.from({ length: 15 }, (id: number) =>
      DocumentRow.fromApi(mockDocumentResponse(id)),
    );
    const { findByText } = setup({ results: largeDataset, claims: [Claims.DOCUMENT_VIEW] });
    expect(await findByText('1 - 10 of 15')).toBeVisible();
  });

  it('previews a document when text clicked', async () => {
    const { getDocumentFileNameLink } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW],
    });

    const filenameLink = await getDocumentFileNameLink(20);
    userEvent.click(filenameLink);

    expect(onPreview).toHaveBeenCalled();
  });

  it('views a document when eye icon clicked', async () => {
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_EDIT],
    });

    const viewButton = getAllByTestId('document-view-button')[0];
    userEvent.click(viewButton);
    expect(onViewDetails).toHaveBeenCalled();
  });

  it('deletes a document when delete icon clicked', async () => {
    const { getAllByTestId } = setup({
      results: mockDocumentsResponse().map(x => DocumentRow.fromApi(x)),
      claims: [Claims.DOCUMENT_VIEW, Claims.DOCUMENT_DELETE],
    });

    const deleteButton = getAllByTestId('document-delete-button')[0];
    userEvent.click(deleteButton);
    expect(onDelete).toHaveBeenCalled();
  });
});
