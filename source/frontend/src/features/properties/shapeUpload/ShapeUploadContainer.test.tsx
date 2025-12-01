import { feature } from '@turf/turf';
import { FormikProps } from 'formik';
import { FeatureCollection } from 'geojson';
import { createRef } from 'react';

import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import { ShapeUploadModel, UploadResponseModel } from './models';
import { IShapeUploadContainerProps, ShapeUploadContainer } from './ShapeUploadContainer';
import { IShapeUploadFormProps } from './ShapeUploadForm';
import { IShapeUploadResultViewProps } from './ShapeUploadResultView';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockView = vi.fn((props: IShapeUploadFormProps) => <div data-testid="view" />);
const mockResultsView = vi.fn((props: IShapeUploadResultViewProps) => (
  <div data-testid="results-view" />
));

describe('ShapeUploadContainer', () => {
  const mockGeoJson: FeatureCollection = {
    type: 'FeatureCollection',
    features: [feature(getMockPolygon())],
  };

  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IShapeUploadContainerProps> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ShapeUploadModel>>();
    const rendered = render(
      <ShapeUploadContainer
        formikRef={formikRef}
        uploadResult={renderOptions?.props?.uploadResult ?? null}
        propertyIdentifier={renderOptions?.props?.propertyIdentifier ?? 'property-123'}
        onUploadFile={renderOptions?.props?.onUploadFile ?? vi.fn()}
        View={mockView}
        ResultsView={mockResultsView}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    await act(async () => {});

    return {
      ...rendered,
      formikRef,
    };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders ResultsView when uploadResult exists', async () => {
    const uploadResult = new UploadResponseModel('file.zip');
    uploadResult.isSuccess = true;
    uploadResult.boundary = getMockPolygon();

    await setup({
      props: { uploadResult },
    });

    expect(screen.getByTestId('results-view')).toBeInTheDocument();
    expect(mockResultsView).toHaveBeenCalledWith(
      expect.objectContaining<IShapeUploadResultViewProps>({ uploadResult }),
      expect.anything(),
    );
  });

  it('renders View when uploadResult is null', async () => {
    const { formikRef } = await setup();

    expect(screen.getByTestId('view')).toBeInTheDocument();
    expect(mockView).toHaveBeenCalledWith(
      expect.objectContaining({
        formikRef: formikRef,
        isLoading: false,
        propertyIdentifier: 'property-123',
        onUploadFile: expect.any(Function),
      }),
      expect.anything(),
    );
  });

  it('calls onUploadFile with success when toGeoJson resolves', async () => {
    const mockOnUploadFile = vi.fn();
    const file = new File(['dummy'], 'file.zip');
    const shapeUploadModel = new ShapeUploadModel();
    shapeUploadModel.file = file;

    vi.spyOn(shapeUploadModel, 'toGeoJson').mockResolvedValue(mockGeoJson);

    await setup({ props: { onUploadFile: mockOnUploadFile } });

    // Get the onUploadFile prop from View
    const callProps = mockView.mock.calls[0][0];
    await act(async () => {
      await callProps.onUploadFile(shapeUploadModel);
    });

    expect(mockOnUploadFile).toHaveBeenCalledWith(
      expect.objectContaining<Partial<UploadResponseModel>>({
        isSuccess: true,
        fileName: 'file.zip',
        boundary: getMockPolygon(),
      }),
    );
  });

  it('calls onUploadFile with error when toGeoJson rejects', async () => {
    const mockOnUploadFile = vi.fn();
    const file = new File(['dummy'], 'file.zip');
    const shapeUploadModel = new ShapeUploadModel();
    shapeUploadModel.file = file;

    const errorMsg = 'GeoJson error';
    vi.spyOn(shapeUploadModel, 'toGeoJson').mockRejectedValue(new Error(errorMsg));

    await setup({ props: { onUploadFile: mockOnUploadFile } });

    const callProps = mockView.mock.calls[0][0];
    await act(async () => {
      await callProps.onUploadFile(shapeUploadModel);
    });

    expect(mockOnUploadFile).toHaveBeenCalledWith(
      expect.objectContaining<Partial<UploadResponseModel>>({
        isSuccess: false,
        fileName: 'file.zip',
        boundary: null,
        errorMessage: errorMsg,
      }),
    );
  });

  it('sets isLoading true during upload', async () => {
    const file = new File(['dummy'], 'file.zip');
    const shapeUploadModel = new ShapeUploadModel();
    shapeUploadModel.file = file;

    let resolveGeoJson: (value: any) => void;
    vi.spyOn(shapeUploadModel, 'toGeoJson').mockImplementation(
      () =>
        new Promise(resolve => {
          resolveGeoJson = resolve;
        }),
    );

    await setup();

    const callProps = mockView.mock.calls[0][0];

    // Start upload
    act(() => {
      callProps.onUploadFile(shapeUploadModel);
    });

    // isLoading should be true while promise is pending
    expect(mockView.mock.calls[mockView.mock.calls.length - 1][0].isLoading).toBe(true);

    // Resolve upload
    await act(async () => {
      resolveGeoJson(mockGeoJson);
    });

    // isLoading should be false after upload
    expect(mockView.mock.calls[mockView.mock.calls.length - 1][0].isLoading).toBe(false);
  });
});
