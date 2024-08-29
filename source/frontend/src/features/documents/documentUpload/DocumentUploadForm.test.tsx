import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';

import { SelectOption } from '@/components/common/form';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Mayan_DocumentTypeMetadataType } from '@/models/api/generated/ApiGen_Mayan_DocumentTypeMetadataType';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fireEvent, render, RenderOptions, screen } from '@/utils/test-utils';

import { BatchUploadFormModel, DocumentUploadFormData } from '../ComposedDocument';
import DocumentUploadForm from './DocumentUploadForm';

const history = createMemoryHistory();

const onUploadDocument = vi.fn();
const getDocumentMetadata = vi.fn();
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
      document_stub_expiration_interval: null,
      document_stub_pruning_enabled: null,
      filename_generator_backend: '',
      filename_generator_backend_arguments: '',
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
  const setup = (
    renderOptions: RenderOptions & {
      initialValues: DocumentUploadFormData;
      maxDocumentCount?: number;
    },
  ) => {
    const formikRef = React.createRef<FormikProps<BatchUploadFormModel>>();

    const utils = render(
      <DocumentUploadForm
        documentTypes={mockDocumentTypesResponse()}
        isLoading={false}
        getDocumentMetadata={getDocumentMetadata}
        maxDocumentCount={renderOptions.maxDocumentCount ?? 10}
        onDocumentsSelected={onDocumentSelected}
        onUploadDocument={onUploadDocument}
        initialDocumentType={'AMMEND'}
        formikRef={formikRef}
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
      formikRef,
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

  it('should call onUploadDocument when Submit button is clicked', async () => {
    const { getByTestId, formikRef } = setup({ initialValues });

    expect(formikRef.current).not.toBeNull();

    // get the upload button
    const uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await act(async () => {
      fireEvent.change(uploader, {
        target: { files: [file] },
      });
    });

    await act(async () => formikRef.current.submitForm());

    expect(onUploadDocument).toHaveBeenCalled();
  });

  it('should display the number of attached files', async () => {
    const { getByTestId, formikRef } = setup({ initialValues });

    expect(formikRef.current).not.toBeNull();

    // get the upload button
    const uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await act(async () => {
      fireEvent.change(uploader, {
        target: { files: [file] },
      });
    });

    expect(
      await screen.findByText(/You have attached 1 files. Do you want to proceed/i),
    ).toBeVisible();
  });

  it('should warn the user when the number of attached files exceeds the allowed maximum of documents', async () => {
    const { getByTestId, formikRef } = setup({ initialValues, maxDocumentCount: 2 });

    expect(formikRef.current).not.toBeNull();

    // get the upload button
    const uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await act(async () => {
      fireEvent.change(uploader, {
        target: { files: [file, file, file] },
      });
    });

    expect(
      await screen.findByText(
        /You have a limit of 2 files per time. Some of your files have not been uploaded at this time/i,
      ),
    ).toBeVisible();
  });
});
