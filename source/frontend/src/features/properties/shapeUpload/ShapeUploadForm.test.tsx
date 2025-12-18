import { FormikProps } from 'formik';
import { createRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { ShapeUploadModel } from './models';
import { IShapeUploadFormProps, ShapeUploadForm } from './ShapeUploadForm';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ShapeUploadForm', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IShapeUploadFormProps> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ShapeUploadModel>>();
    const rendered = render(
      <ShapeUploadForm
        formikRef={formikRef}
        isLoading={renderOptions.props?.isLoading ?? false}
        propertyIdentifier={renderOptions?.props?.propertyIdentifier}
        onUploadFile={renderOptions.props?.onUploadFile ?? vi.fn()}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return { ...rendered, formikRef };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows loading backdrop when isLoading is true', async () => {
    setup({ props: { isLoading: true } });
    expect(screen.getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('renders drag-and-drop file input', async () => {
    setup();
    expect(screen.getByTestId('upload-input')).toBeInTheDocument();
  });

  it('displays file name after file is uploaded', async () => {
    setup();

    const file = new File(['dummy'], 'example.zip', { type: 'application/zip' });
    const input = screen.getByTestId('upload-input');
    await act(async () => {
      await userEvent.upload(input, file);
    });

    expect(screen.getByText('example.zip')).toBeInTheDocument();
  });

  it('shows correct message with propertyIdentifier', async () => {
    setup({ props: { propertyIdentifier: 'property-456' } });

    const file = new File(['dummy'], 'example.zip', { type: 'application/zip' });
    const input = screen.getByTestId('upload-input');
    await act(async () => {
      await userEvent.upload(input, file);
    });

    expect(
      screen.getByText(/You have attached a shapefile for property: property-456/i),
    ).toBeInTheDocument();
  });

  it('shows correct message without propertyIdentifier', async () => {
    setup();

    const file = new File(['dummy'], 'example.zip', { type: 'application/zip' });
    const input = screen.getByTestId('upload-input');
    await act(async () => {
      await userEvent.upload(input, file);
    });

    expect(
      screen.getByText(/You have attached a shapefile. Do you want to proceed and save/i),
    ).toBeInTheDocument();
  });

  it('calls onUploadFile on submit with correct values', async () => {
    const onUploadFile = vi.fn().mockResolvedValue(undefined);
    const { formikRef } = setup({ props: { onUploadFile } });

    // Upload file and submit the form
    const file = new File(['dummy'], 'example.zip', { type: 'application/zip' });
    const input = screen.getByTestId('upload-input');
    await act(async () => {
      await userEvent.upload(input, file);
    });
    await act(async () => formikRef.current?.submitForm());

    expect(onUploadFile).toHaveBeenCalledWith(expect.objectContaining({ file }));
  });

  it('accepts only .zip files', async () => {
    setup();

    const zipFile = new File(['dummy'], 'file.zip', { type: 'application/zip' });
    const txtFile = new File(['dummy'], 'file.txt', { type: 'text/plain' });
    const input = screen.getByTestId('upload-input');
    await act(async () => {
      await userEvent.upload(input, zipFile);
    });
    expect(screen.getByText('file.zip')).toBeInTheDocument();
    // Try uploading a txt file (should not show file name)
    await act(async () => {
      await userEvent.upload(input, txtFile);
    });
    expect(screen.queryByText('file.txt')).not.toBeInTheDocument();
  });

  it('renders DisplayError when form validation fails', async () => {
    const { formikRef } = setup();
    await act(async () => {
      formikRef.current?.setFieldError('file', 'File upload error');
      formikRef.current?.setFieldTouched('file', true);
    });
    expect(screen.getByText('File upload error')).toBeInTheDocument();
  });
});
