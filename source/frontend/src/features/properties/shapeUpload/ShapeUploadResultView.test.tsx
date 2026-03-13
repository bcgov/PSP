import * as Styled from 'styled-components';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { UploadResponseModel } from './models';
import { IShapeUploadResultViewProps, ShapeUploadResultView } from './ShapeUploadResultView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock useTheme to provide consistent colors for testing
vi.spyOn(Styled, 'useTheme').mockReturnValue({
  bcTokens: {
    iconsColorSuccess: 'green',
    iconsColorDanger: 'red',
  },
} as Styled.DefaultTheme);

describe('ShapeUploadResultView', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IShapeUploadResultViewProps> } = {},
  ) => {
    return render(
      <ShapeUploadResultView
        uploadResult={renderOptions.props?.uploadResult ?? new UploadResponseModel('example.zip')}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot for success', () => {
    const uploadResult = new UploadResponseModel('example.zip');
    uploadResult.isSuccess = true;
    const { asFragment } = setup({ props: { uploadResult } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('matches snapshot for failure', () => {
    const uploadResult = new UploadResponseModel('example.zip');
    uploadResult.isSuccess = false;
    uploadResult.errorMessage = 'Upload failed';
    const { asFragment } = setup({ props: { uploadResult } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows success message and icon when uploadResult.isSuccess is true', () => {
    const uploadResult = new UploadResponseModel('example.zip');
    uploadResult.isSuccess = true;
    setup({ props: { uploadResult } });

    expect(screen.getByText(/Boundary file uploaded successfully/i)).toBeInTheDocument();
    expect(screen.getByText('example.zip')).toBeInTheDocument();
    const icon = screen.getByTestId('file-check-icon');
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveAttribute('color', 'green');
  });

  it('shows failure message, icon, and error when uploadResult.isSuccess is false', () => {
    const uploadResult = new UploadResponseModel('example.zip');
    uploadResult.isSuccess = false;
    uploadResult.errorMessage = 'Upload failed';
    setup({ props: { uploadResult } });

    expect(screen.getByText(/Boundary file upload failed/i)).toBeInTheDocument();
    expect(screen.getByText('example.zip')).toBeInTheDocument();
    expect(screen.getByText('Upload failed')).toBeInTheDocument();
    const icon = screen.getByTestId('file-error-icon');
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveAttribute('color', 'red');
  });

  it('truncates long file names', () => {
    const longName = 'a'.repeat(120) + '.zip';
    const uploadResult = new UploadResponseModel(longName);
    uploadResult.isSuccess = true;
    setup({ props: { uploadResult } });

    // Should display truncated file name (100 chars)
    expect(screen.getByText(`${longName.substring(0, 97)}...`)).toBeInTheDocument();
  });
});
