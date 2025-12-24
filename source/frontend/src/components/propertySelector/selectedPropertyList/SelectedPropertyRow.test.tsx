import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { UploadResponseModel } from '@/features/properties/shapeUpload/models';
import {
  IShapeUploadModalProps,
  ShapeUploadModal,
} from '@/features/properties/shapeUpload/ShapeUploadModal';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { exists } from '@/utils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import SelectedPropertyRow, { ISelectedPropertyRowProps } from './SelectedPropertyRow';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

vi.mock('@/features/properties/shapeUpload/ShapeUploadModal');
vi.mocked(ShapeUploadModal).mockImplementation((props: IShapeUploadModalProps) => {
  return props.display ? (
    <div data-testid="shape-upload-modal">
      <span data-testid="prop-id">{props.propertyIdentifier}</span>
      <button
        data-testid="modal-close"
        onClick={() => {
          const fakeResult = new UploadResponseModel('fakefile.shp');
          fakeResult.isSuccess = true;
          fakeResult.boundary = getMockPolygon();
          if (typeof props.onClose === 'function') {
            props.onClose(fakeResult);
          }
        }}
      >
        close
      </button>
    </div>
  ) : null;
});

const onRemove = vi.fn();

describe('SelectedPropertyRow component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ISelectedPropertyRowProps> } = {},
  ) => {
    // render component under test
    const utils = render(
      <Formik
        onSubmit={noop}
        initialValues={{
          properties: [
            exists(renderOptions.props?.property)
              ? renderOptions.props?.property
              : new PropertyForm(),
          ],
        }}
      >
        {() => (
          <SelectedPropertyRow
            property={renderOptions.props?.property ?? new PropertyForm()}
            index={renderOptions.props?.index ?? 0}
            onRemove={onRemove}
            showDisable={renderOptions.props?.showDisable ?? false}
            nameSpace="properties.0"
            canUploadShapefile={renderOptions.props?.canUploadShapefile ?? false}
            onUploadShapefile={renderOptions.props?.onUploadShapefile ?? vi.fn()}
            onRemoveShapefile={renderOptions.props?.onRemoveShapefile ?? vi.fn()}
          />
        )}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
        mockMapMachine: renderOptions.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    await act(async () => {});

    return { ...utils };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('fires onRemove when remove button is clicked', async () => {
    await setup();
    const removeButton = screen.getByTitle('remove');
    await act(async () => userEvent.click(removeButton));
    expect(onRemove).toHaveBeenCalled();
  });

  it('calls map machine when reposition button is clicked', async () => {
    await setup();
    const moveButton = screen.getByTitle('move-pin-location');
    await act(async () => userEvent.click(moveButton));
    expect(mapMachineBaseMock.startReposition).toHaveBeenCalled();
  });

  it('displays pid', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: '111-111-111',
      },
    };
    await setup({ props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet) } });
    expect(screen.getByText('PID: 111-111-111')).toBeVisible();
  });

  it('falls back to pin', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: undefined,
        PIN: 1234,
      },
    };
    await setup({ props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet) } });
    expect(screen.getByText('PIN: 1234')).toBeVisible();
  });

  it('falls back to plan number', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        SURVEY_PLAN_NUMBER: 'VIP123',
        PID_PADDED: undefined,
        PIN: undefined,
      },
    };
    await setup({ props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet) } });
    expect(screen.getByText('Plan #: VIP123')).toBeVisible();
  });

  it('falls back to lat/lng', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.location = { lat: 4, lng: 5 };

    await setup({ props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet) } });
    expect(screen.getByText('5.000000, 4.000000')).toBeVisible();
  });

  it('falls back to address', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.location = undefined;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: undefined,
        PIN: undefined,
        SURVEY_PLAN_NUMBER: undefined,
        STREET_ADDRESS_1: 'a test address',
      },
    };
    await setup({ props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet) } });
    expect(screen.getByText('Address: a test address')).toBeVisible();
  });

  it('shows Inactive as selected when isActive is false', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.isActive = false;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    expect(screen.getByDisplayValue('Inactive')).toBeInTheDocument();
  });

  it('shows Active as selected when isActive is true', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.isActive = true;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    expect(screen.getByDisplayValue('Active')).toBeInTheDocument();
  });

  // New tests for shapefile upload functionality
  it('renders upload button when canUploadShapefile is true and property has no custom shape', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    await setup({
      props: {
        property: PropertyForm.fromFeatureDataset(mockFeatureSet),
        canUploadShapefile: true,
      },
    });
    expect(screen.getByTestId('upload-shapefile-0')).toBeInTheDocument();
  });

  it('does not render upload button when canUploadShapefile is false or undefined', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    await setup({
      props: {
        property: PropertyForm.fromFeatureDataset(mockFeatureSet),
        canUploadShapefile: false,
      },
    });
    expect(screen.queryByTestId('upload-shapefile-0')).toBeNull();
  });

  it('opens ShapeUploadModal when upload button is clicked and passes property identifier, then calls onUploadShapefile on close', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: '222-222-222',
      },
    };

    const onUploadShapefile = vi.fn();
    await setup({
      props: {
        property: PropertyForm.fromFeatureDataset(mockFeatureSet),
        canUploadShapefile: true,
        onUploadShapefile,
      },
    });

    const uploadBtn = screen.getByTestId('upload-shapefile-0');
    await act(async () => userEvent.click(uploadBtn));

    // modal (mock) should be visible and show propertyIdentifier
    expect(screen.getByTestId('shape-upload-modal')).toBeVisible();
    expect(screen.getByTestId('prop-id')).toHaveTextContent('222-222-222');

    const closeBtn = screen.getByTestId('modal-close');
    await act(async () => userEvent.click(closeBtn));
    expect(onUploadShapefile).toHaveBeenCalledWith(
      expect.objectContaining<Partial<UploadResponseModel>>({
        fileName: 'fakefile.shp',
        isSuccess: true,
      }),
    );
  });

  it('renders Delete Shape button instead of upload button when property has a custom shape', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.fileBoundary = getMockPolygon();

    await setup({
      props: {
        property: PropertyForm.fromFeatureDataset(mockFeatureSet),
        canUploadShapefile: true,
      },
    });
    expect(screen.getByTestId('remove-shape-0')).toBeInTheDocument();
  });

  it('calls onRemoveShapefile when Delete Shape button is clicked', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.fileBoundary = getMockPolygon();

    const onRemoveShapefile = vi.fn();
    await setup({
      props: {
        property: PropertyForm.fromFeatureDataset(mockFeatureSet),
        canUploadShapefile: true,
        onRemoveShapefile,
      },
    });

    const deleteBtn = screen.getByTestId('remove-shape-0');
    await act(async () => userEvent.click(deleteBtn));
    expect(onRemoveShapefile).toHaveBeenCalled();
  });

  it('does not render status dropdown when property is retired', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        IS_RETIRED: true,
      },
    } as any;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    // Retired properties show text instead of dropdown
    expect(screen.queryByRole('combobox')).toBeNull();
    expect(screen.getByText('Retired')).toBeInTheDocument();
  });

  it('renders status dropdown when property is not retired', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        IS_RETIRED: false,
      },
    } as any;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    expect(screen.getByRole('combobox')).toBeInTheDocument();
  });

  it('renders status dropdown when property is active', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        IS_RETIRED: false,
      },
    } as any;
    mockFeatureSet.isActive = true;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    expect(screen.getByRole('combobox')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Active')).toBeInTheDocument();
  });

  it('renders status dropdown when property is inactive', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        IS_RETIRED: false,
      },
    } as any;
    mockFeatureSet.isActive = false;

    await setup({
      props: { property: PropertyForm.fromFeatureDataset(mockFeatureSet), showDisable: true },
    });

    expect(screen.getByRole('combobox')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Inactive')).toBeInTheDocument();
  });
});
