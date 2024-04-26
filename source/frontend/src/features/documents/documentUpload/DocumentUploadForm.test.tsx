import { createMemoryHistory } from 'history';

import { SelectOption } from '@/components/common/form';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fireEvent, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { DocumentUploadFormData } from '../ComposedDocument';
import DocumentUploadForm from './DocumentUploadForm';

const history = createMemoryHistory();

const submitForm = vi.fn();
const handleSubmit = vi.fn();

const handleCancelClick = vi.fn();
const onUploadDocument = vi.fn();
const onDocumentTypeChange = vi.fn();
const onDocumentSelected = vi.fn();

const documentStatusOptions: SelectOption[] = [
  { label: '', value: 'NONE' },
  { label: '', value: 'DRAFT' },
  { label: '', value: 'APPROVD' },
  { label: '', value: 'SIGND' },
  { label: '', value: 'FINAL' },
  { label: '', value: 'AMENDD' },
  { label: '', value: 'CNCLD' },
];

const documentTypeMetadataType: ApiGen_Mayan_DocumentTypeMetadataType[] = [
  {
    id: 1,
    document_type: {
      id: 1,
      label: 'BC Assessment Search',
      delete_time_period: null,
      delete_time_unit: null,
      trash_time_period: null,
      trash_time_unit: null,
    },
    metadata_type: {
      default: '',
      id: 1,
      label: 'Tag',
      lookup: '',
      name: 'Tag',
      parser: '',
      parser_arguments: '',
      url: '',
      validation: '',
      validation_arguments: '',
    },
    required: true,
    url: null,
  },
];

describe('DocumentUploadView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: DocumentUploadFormData }) => {
    const utils = render(
      <DocumentUploadForm
        documentTypes={mockDocumentTypesResponse()}
        isLoading={false}
        mayanMetadataTypes={documentTypeMetadataType}
        onDocumentTypeChange={onDocumentTypeChange}
        onDocumentSelected={onDocumentSelected}
        onUploadDocument={onUploadDocument}
        onCancel={handleCancelClick}
        initialDocumentType={'AMMEND'}
        formikRef={{ current: { submitForm, dirty: true } } as any}
        documentStatusOptions={documentStatusOptions}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        history,
      },
    );

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  let initialValues: DocumentUploadFormData;
  let file: File;
  beforeEach(() => {
    initialValues = new DocumentUploadFormData(
      'AMEND',
      'BC Assessment Search',
      documentTypeMetadataType,
    );
    initialValues.documentTypeId = '1';
    file = new File(['(⌐□_□)'], 'test.png', { type: 'image/png' });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    setup({ initialValues });
    await act(async () => {});
    expect(document.body).toMatchSnapshot();
  });

  it('renders the field', async () => {
    const { getByTestId } = setup({ initialValues });
    const textarea = getByTestId('document-type');

    expect(textarea).toBeVisible();
    await act(async () => {});
  });

  it('displays input for metadata types', async () => {
    const { getByTestId } = setup({ initialValues });
    const textarea = await getByTestId('metadata-input-Tag');

    expect(textarea).toBeVisible();
    await act(async () => {});
  });

  it.skip('should submit form when Submit button is clicked', async () => {
    const { getByTestId } = setup({ initialValues });

    const save = await getByTestId('save');
    // get the upload button
    const uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await waitFor(() =>
      fireEvent.change(uploader, {
        target: { files: [file] },
      }),
    );
    await act(async () => userEvent.click(save));

    expect(handleSubmit).toBeCalled();
    expect(handleCancelClick).not.toBeCalled();
  });
});
