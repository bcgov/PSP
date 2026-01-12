import { Formik, FormikProps } from 'formik';
import noop from 'lodash/noop';
import { createRef } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
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
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';
import ResearchProperties from './ResearchProperties';

// Mock ShapeUploadModal in order to control its behavior in tests
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

let testForm: ResearchForm;

describe('ResearchProperties component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      initialForm: ResearchForm;
      confirmBeforeAdd?: (propertyForm: PropertyForm) => Promise<boolean>;
    },
  ) => {
    // render component under test
    const formikRef = createRef<FormikProps<ResearchForm>>();
    const rendered = render(
      <Formik innerRef={formikRef} initialValues={renderOptions.initialForm} onSubmit={noop}>
        <ResearchProperties confirmBeforeAdd={vi.fn()} />
      </Formik>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
        useMockAuthentication: true,
      },
    );

    await act(async () => {});

    return {
      ...rendered,
      formikRef,
    };
  };

  beforeEach(() => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    testForm = new ResearchForm();
    testForm.name = 'Test name';
    testForm.properties = [
      PropertyForm.fromFeatureDataset({
        ...mockFeatureSet,
        pimsFeature: {
          ...mockFeatureSet.pimsFeature,
          properties: {
            ...mockFeatureSet.pimsFeature?.properties,
            PID_PADDED: '123-456-789',
          },
        },
      }),
      PropertyForm.fromFeatureDataset({
        ...mockFeatureSet,
        pimsFeature: {
          ...mockFeatureSet.pimsFeature,
          properties: {
            ...mockFeatureSet.pimsFeature?.properties,
            PIN: 1111222,
          },
        },
      }),
    ];
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = await setup({ initialForm: testForm });

    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('renders empty list', async () => {
    const { getByText } = await setup({ initialForm: new ResearchForm() });
    expect(getByText('No Properties selected')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getByTestId, queryByText } = await setup({ initialForm: testForm });
    const pidRow = getByTestId('delete-property-0');

    await act(async () => {
      userEvent.click(pidRow);
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('properties are prefixed by svg with incrementing id', async () => {
    const { getByTitle } = await setup({ initialForm: testForm });

    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('adds lat/long based properties to the file', async () => {
    const { getByText } = await setup({
      initialForm: new ResearchForm(),
      mockMapMachine: {
        ...mapMachineBaseMock,
        // this "fakes" a click on the map to add lat/long based properties
        mapLocationFeatureDataset: {
          selectingComponentId: null,
          location: { lat: 50.25163372, lng: -120.69195885 },
          fileLocation: null,
          pimsFeatures: [],
          parcelFeatures: [],
          regionFeature: null,
          districtFeature: null,
          municipalityFeatures: [],
          highwayFeatures: [],
          crownLandLeasesFeatures: [],
          crownLandLicensesFeatures: [],
          crownLandTenuresFeatures: [],
          crownLandInventoryFeatures: [],
          crownLandInclusionsFeatures: [],
        },
      },
    });

    const addButton = getByText('Add selected property');
    expect(addButton).toBeVisible();
    await act(async () => userEvent.click(addButton));

    // Verify that the map machine was called to prepare the lat/long property for addition to the file
    expect(mapMachineBaseMock.prepareForCreation).toHaveBeenCalledWith(
      expect.arrayContaining([
        expect.objectContaining<Partial<SelectedFeatureDataset>>({
          location: { lat: 50.25163372, lng: -120.69195885 },
        }),
      ]),
    );
  });

  it('updates property boundary when shapefile is uploaded', async () => {
    const { formikRef } = await setup({ initialForm: testForm });

    const uploadButton = screen.getByTestId('upload-shapefile-0');
    await act(async () => userEvent.click(uploadButton));

    // Modal should be displayed
    expect(screen.getByTestId('shape-upload-modal')).toBeVisible();

    const closeButton = screen.getByTestId('modal-close');
    await act(async () => userEvent.click(closeButton));

    // Modal should be closed
    expect(screen.queryByTestId('shape-upload-modal')).toBeNull();

    // Verify that the property boundary was updated in the formik values
    expect(formikRef.current?.values.properties[0].fileBoundary).toEqual(getMockPolygon());
  });

  it('removes custom property boundary when Remove Shape is clicked', async () => {
    const { formikRef } = await setup({
      initialForm: testForm,
    });

    // Manually set a custom boundary on the first property
    act(() => {
      formikRef.current?.setFieldValue('properties[0].fileBoundary', getMockPolygon());
    });

    const removeButton = screen.getByTestId('remove-shape-0');
    await act(async () => userEvent.click(removeButton));

    // confirm removal in modal
    expect(screen.getByText(/Are you sure you want to remove this uploaded shape/i)).toBeVisible();
    const confirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(confirmButton));

    // Verify that the property boundary was removed in the formik values
    expect(formikRef.current?.values.properties[0].fileBoundary).toBeUndefined();
  });
});
