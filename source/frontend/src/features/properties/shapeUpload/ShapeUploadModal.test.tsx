import { feature } from '@turf/turf';

import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { IShapeUploadModalProps, ShapeUploadModal } from './ShapeUploadModal';
import { ShapeUploadModel, UploadResponseModel } from './models';

vi.spyOn(ShapeUploadModel.prototype, 'toGeoJson').mockResolvedValue({
  type: 'FeatureCollection',
  features: [feature(getMockPolygon())],
});

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ShapeUploadModal component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IShapeUploadModalProps> } = {},
  ) => {
    const rendered = render(
      <ShapeUploadModal
        display={renderOptions?.props?.display ?? true}
        setDisplay={renderOptions?.props?.setDisplay ?? vi.fn()}
        propertyIdentifier={renderOptions?.props?.propertyIdentifier ?? 'property-123'}
        onClose={renderOptions?.props?.onClose ?? vi.fn()}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    await act(async () => {});

    return {
      ...rendered,
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders correctly', async () => {
    await setup();
    expect(document.body).toMatchSnapshot();
  });

  it('renders Yes and No when no upload result', async () => {
    await setup();

    const ok = screen.getByTitle('ok-modal');
    const cancel = screen.getByTitle('cancel-modal');

    expect(ok).toHaveTextContent('Yes');
    expect(cancel).toHaveTextContent('No');
  });

  it('submits form on Yes and then shows Close (upload complete)', async () => {
    await setup();

    // Click Yes to submit form
    const ok = screen.getByTitle('ok-modal');
    await act(async () => {
      userEvent.click(ok);
    });

    // After upload result set, OK button text should change to 'Close' and cancel should not be present
    const okAfter = screen.getByTitle('ok-modal');
    expect(okAfter).toHaveTextContent('Close');

    const cancelAfter = screen.queryByTitle('cancel-modal');
    expect(cancelAfter).toBeNull();
  });

  it('closes modal without confirmation when there are no unsaved changes', async () => {
    const onClose = vi.fn();
    await setup({ props: { onClose } });

    // Close modal without making any changes
    const cancel = screen.getByTitle('cancel-modal');
    await act(async () => {
      userEvent.click(cancel);
    });

    expect(onClose).toHaveBeenCalledTimes(1);
    expect(onClose).toHaveBeenCalledWith(null);
  });

  it('shows confirmation when closing with unsaved changes', async () => {
    const onClose = vi.fn();
    await setup({ props: { onClose } });

    // Simulate attaching a file to make form dirty
    const file = new File(['dummy content'], 'example.zip', { type: 'application/zip' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      await userEvent.upload(fileInput, file);
    });

    // Attempt to close modal
    const cancel = screen.getByTitle('cancel-modal');
    await act(async () => {
      userEvent.click(cancel);
    });

    // Confirmation message should be displayed
    expect(await screen.findByText(/Unsaved updates will be lost/i)).toBeVisible();
    expect(onClose).not.toHaveBeenCalled();
  });

  it('shows confirmation when cancelling with unsaved changes and then closes on second cancel', async () => {
    const onClose = vi.fn();
    await setup({ props: { onClose } });

    // Simulate attaching a file to make form dirty
    const file = new File(['dummy content'], 'example.zip', { type: 'application/zip' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      await userEvent.upload(fileInput, file);
    });

    // First cancel should show the unsaved confirmation message
    const cancel = screen.getByTitle('cancel-modal');
    await act(async () => {
      userEvent.click(cancel);
    });

    expect(await screen.findByText(/Unsaved updates will be lost/i)).toBeVisible();

    // Second cancel should proceed to call onClose with null (no uploadResult)
    await act(async () => {
      userEvent.click(cancel);
    });

    expect(onClose).toHaveBeenCalledTimes(1);
    expect(onClose).toHaveBeenCalledWith(null);
  });

  it('clicking Close calls onClose with the upload result', async () => {
    const onClose = vi.fn();
    await setup({ props: { onClose } });

    // Simulate attaching a file to make form dirty
    const file = new File(['dummy content'], 'example.zip', { type: 'application/zip' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      await userEvent.upload(fileInput, file);
    });

    // Click Yes to submit form
    const ok = screen.getByTitle('ok-modal');
    await act(async () => {
      userEvent.click(ok);
    });

    // Click Close
    const close = screen.getByTitle('ok-modal');
    await act(async () => {
      userEvent.click(close);
    });

    expect(onClose).toHaveBeenCalledTimes(1);
    expect(onClose).toHaveBeenCalledWith(
      expect.objectContaining<Partial<UploadResponseModel>>({
        isSuccess: true,
        fileName: 'example.zip',
        boundary: getMockPolygon(),
      }),
    );
  });
});
