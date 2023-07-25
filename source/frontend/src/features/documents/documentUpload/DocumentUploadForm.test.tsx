import { createMemoryHistory } from 'history';

import { SelectOption } from '@/components/common/form';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Storage_DocumentTypeMetadataType } from '@/models/api/DocumentStorage';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fireEvent, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { DocumentUploadFormData } from '../ComposedDocument';
import DocumentUploadForm from './DocumentUploadForm';

const history = createMemoryHistory();

const submitForm = jest.fn();
const handleSubmit = jest.fn();

const handleCancelClick = jest.fn();
const onUploadDocument = jest.fn();
const onDocumentTypeChange = jest.fn();

const documentStatusOptions: SelectOption[] = [
  { label: '', value: 'NONE' },
  { label: '', value: 'DRAFT' },
  { label: '', value: 'APPROVD' },
  { label: '', value: 'SIGND' },
  { label: '', value: 'FINAL' },
  { label: '', value: 'AMENDD' },
  { label: '', value: 'CNCLD' },
];

const documentTypeMetadataType: Api_Storage_DocumentTypeMetadataType[] = [
  {
    id: 1,
    document_type: {
      id: 1,
      label: 'BC Assessment Search',
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
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    setup({ initialValues });
    expect(document.body).toMatchSnapshot();
  });

  it('renders the field', () => {
    const { getByTestId } = setup({ initialValues });
    const textarea = getByTestId('document-type');

    expect(textarea).toBeVisible();
  });

  it('displays input for metadata types', async () => {
    const { getByTestId } = setup({ initialValues });
    const textarea = await getByTestId('metadata-input-Tag');

    expect(textarea).toBeVisible();
  });

  it('save button to be disabled', async () => {
    const { getByTestId } = setup({ initialValues });

    const save = await getByTestId('save');

    expect(save).toHaveAttribute('disabled');
  });

  it('should validate required inputs', async () => {
    const { findByText, getByTestId } = setup({ initialValues });

    const save = await getByTestId('save');
    // get the upload button
    let uploader = getByTestId('upload-input');

    // simulate ulpoad event and wait until finish
    await waitFor(() =>
      fireEvent.change(uploader, {
        target: { files: [file] },
      }),
    );
    userEvent.click(save);

    expect(await findByText(/Mandatory fields are required./i)).toBeVisible();
  });

  it('should cancel form when Cancel button is clicked', async () => {
    const { getByTestId } = setup({ initialValues });

    const cancel = await getByTestId('cancel');
    act(() => {
      userEvent.click(cancel);
    });
    expect(handleCancelClick).toBeCalled();
    expect(handleSubmit).not.toBeCalled();
  });

  it.skip('should submit form when Submit button is clicked', async () => {
    const { getByTestId } = setup({ initialValues });

    const save = await getByTestId('save');
    // get the upload button
    let uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await waitFor(() =>
      fireEvent.change(uploader, {
        target: { files: [file] },
      }),
    );
    userEvent.click(save);

    expect(handleSubmit).toBeCalled();
    expect(handleCancelClick).not.toBeCalled();
  });
});
