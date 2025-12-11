import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { ManagementFormModel } from '../models/ManagementFormModel';
import ManagementPropertiesSubForm from './ManagementPropertiesSubForm';

const confirmBeforeAdd = vi.fn();

describe('ManagementPropertiesSubForm component', () => {
  const setup = async (
    props: { initialForm: ManagementFormModel },
    renderOptions: RenderOptions = {},
  ) => {
    const formikRef = createRef<FormikProps<ManagementFormModel>>();
    const utils = render(
      <Formik innerRef={formikRef} initialValues={props.initialForm} onSubmit={vi.fn()}>
        {formikProps => (
          <ManagementPropertiesSubForm
            formikProps={formikProps}
            confirmBeforeAdd={confirmBeforeAdd}
          />
        )}
      </Formik>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
      },
    );

    // Wait for any async effects to complete
    await act(async () => {});

    return {
      ...utils,
      formikRef,
    };
  };

  let testForm: ManagementFormModel;

  beforeEach(() => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    testForm = new ManagementFormModel();
    testForm.fileProperties = [
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
    const { getByText } = await setup({ initialForm: new ManagementFormModel() });
    expect(getByText('No Properties selected')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getByTestId, queryByText } = await setup({ initialForm: testForm });
    const pidRow = getByTestId('delete-property-0');
    await act(async () => userEvent.click(pidRow));

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = await setup({ initialForm: testForm });
    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('adds lat/long based properties to the file', async () => {
    const { getByText } = await setup(
      { initialForm: new ManagementFormModel() },
      {
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
      },
    );

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
});
