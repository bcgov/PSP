import { Formik, FormikProps } from 'formik';
import noop from 'lodash/noop';
import React from 'react';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyRegion } from '@/models/layers/motRegionalBoundary';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { emptyPropertyLocation } from '@/models/layers/pimsPropertyLocationView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import * as mapUtils from '@/utils/mapPropertyUtils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import LeasePropertySelector from './LeasePropertySelector';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('LeasePropertySelector component', () => {
  // render component under test
  const setup = (
    props: {
      initialForm: LeaseFormModel;
    },
    renderOptions: RenderOptions = {},
  ) => {
    const formikRef = React.createRef<FormikProps<LeaseFormModel>>();
    const utils = render(
      <Formik<LeaseFormModel>
        initialValues={props.initialForm}
        onSubmit={noop}
        innerRef={formikRef}
      >
        {props => <LeasePropertySelector formikProps={props} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        claims: [],
        mockMapMachine: renderOptions.mockMapMachine,
      },
    );

    return {
      ...utils,
      formikRef,
      rerender: () => {
        utils.rerender(
          <Formik<LeaseFormModel>
            initialValues={props.initialForm}
            onSubmit={noop}
            innerRef={formikRef}
          >
            {props => <LeasePropertySelector formikProps={props} />}
          </Formik>,
        );
      },
    };
  };

  let testForm: LeaseFormModel;

  beforeEach(() => {
    testForm = getDefaultFormLease();
    testForm.lFileNo = 'Test name';
    testForm.properties = [
      FormLeaseProperty.fromMapProperty({
        pid: '123-456-789',
        latitude: 44,
        longitude: -77,
        fileLocation: { lat: 44, lng: -77 },
      }),
      FormLeaseProperty.fromMapProperty({
        pin: '1111222',
        latitude: 44,
        longitude: -77,
        fileLocation: { lat: 44, lng: -77 },
      }),
    ];
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialForm: testForm });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders default message when list of properties is empty', async () => {
    setup({ initialForm: { ...testForm, properties: [] } });
    expect(await screen.findByText(/No Properties selected/i)).toBeVisible();
  });

  it('renders list of properties', async () => {
    setup({ initialForm: testForm });

    expect(await screen.findByText('PID: 123-456-789')).toBeVisible();
    expect(await screen.findByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked and popup is confirmed', async () => {
    setup({ initialForm: testForm });

    // click remove button and confirm the popup
    const pidRow = screen.getAllByTitle('remove')[0];
    await act(async () => userEvent.click(pidRow));
    const ok = screen.getByTitle('ok-modal');
    await act(async () => userEvent.click(ok));

    expect(screen.queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    setup({ initialForm: testForm });

    expect(screen.getByTitle('1')).toBeInTheDocument();
    expect(screen.getByTitle('2')).toBeInTheDocument();
  });

  // TODO: fix tests affected by the removal of the property selector tool
  it.skip('should pre-populate the region if a property is selected', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      isSelecting: true,
      selectingComponentId: undefined,
      mapLocationFeatureDataset: null,
    };

    const leaseWithoutProperties = testForm;
    leaseWithoutProperties.properties = [];

    const { rerender, formikRef } = setup(
      { initialForm: leaseWithoutProperties },
      { mockMapMachine: testMockMachine },
    );

    // no region should be selected by default
    expect(formikRef.current.values.regionId).toBe('');

    // simulate a map click via the map state machine
    testMockMachine.isSelecting = true;
    testMockMachine.mapLocationFeatureDataset = {
      location: { lng: -120.69195885, lat: 50.25163372 },
      fileLocation: null,
      pimsFeatures: [
        {
          type: 'Feature',
          properties: { ...emptyPropertyLocation, PROPERTY_ID: 1, PID: 1 },
          geometry: getMockPolygon(),
        },
      ],
      parcelFeatures: [
        {
          type: 'Feature',
          properties: { ...emptyPmbcParcel, PID_NUMBER: 1 },
          geometry: getMockPolygon(),
        },
      ],
      regionFeature: {
        type: 'Feature',
        properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
        geometry: getMockPolygon(),
      },
      districtFeature: null,
      municipalityFeatures: null,
      highwayFeatures: null,
      selectingComponentId: null,
      crownLandLeasesFeatures: null,
      crownLandLicensesFeatures: null,
      crownLandTenuresFeatures: null,
      crownLandInventoryFeatures: null,
      crownLandInclusionsFeatures: null,
    };

    // verify that upon map click the lease region is auto-selected based on the property region
    await act(async () => rerender());
    expect(formikRef.current.values.regionId).toBe('1');
  });

  // TODO: fix tests affected by the removal of the property selector tool
  it.skip('should display a warning when adding a property to the inventory', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      isSelecting: true,
      selectingComponentId: undefined,
      mapLocationFeatureDataset: null,
    };

    const { rerender, formikRef } = setup(
      { initialForm: testForm },
      { mockMapMachine: testMockMachine },
    );

    expect(formikRef.current.values.properties.length).toBe(testForm.properties.length);

    // simulate a map click via the map state machine
    testMockMachine.isSelecting = true;
    testMockMachine.mapLocationFeatureDataset = {
      location: { lng: -120.69195885, lat: 50.25163372 },
      fileLocation: null,
      pimsFeatures: null, // no PIMS property was found - meaning this property will be added to the inventory.
      parcelFeatures: [
        {
          type: 'Feature',
          properties: { ...emptyPmbcParcel },
          geometry: getMockPolygon(),
        },
      ],
      regionFeature: {
        type: 'Feature',
        properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
        geometry: getMockPolygon(),
      },
      districtFeature: null,
      municipalityFeatures: null,
      highwayFeatures: null,
      selectingComponentId: null,
      crownLandLeasesFeatures: null,
      crownLandLicensesFeatures: null,
      crownLandTenuresFeatures: null,
      crownLandInventoryFeatures: null,
      crownLandInclusionsFeatures: null,
    };

    // verify that upon map click the user gets a confirmation popup to add the property to inventory
    await act(async () => rerender());

    expect(
      await screen.findByText(
        'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
      ),
    ).toBeVisible();

    const okButton = screen.getByTitle('ok-modal');
    await act(async () => userEvent.click(okButton));

    // the property gets added to the lease form successfully
    expect(formikRef.current.values.properties.length).toBe(testForm.properties.length + 1);
  });

  // TODO: fix tests affected by the removal of the property selector tool
  it.skip('should update the property lat/lng when file marker is repositioned', async () => {
    // mock these functions to simplify unit test execution
    const spy = vi.spyOn(mapUtils, 'isLatLngInFeatureSetBoundary');
    spy.mockImplementationOnce(() => true);

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      selectingComponentId: undefined,
      mapLocationFeatureDataset: null,
      // provide fake logic for map marker repositioning
      startReposition: vi.fn<
        [
          repositioningFeatureDataset: SelectedFeatureDataset,
          index: number,
          selectingComponentId?: string,
        ],
        void
      >((featureSet, index, _) => {
        testMockMachine.isRepositioning = true;
        testMockMachine.repositioningFeatureDataset = featureSet;
        testMockMachine.repositioningPropertyIndex = index;
      }),
    };

    const { rerender, formikRef } = setup(
      { initialForm: testForm },
      { mockMapMachine: testMockMachine },
    );

    // this is the original lat/lng of the property
    expect(formikRef.current.values.properties[0].property.fileLocation).toStrictEqual({
      lng: -77,
      lat: 44,
    });

    // click the button to reposition the first property marker
    const moveButton = screen.getAllByTitle('move-pin-location')[0];
    await act(async () => userEvent.click(moveButton));

    // map state machine method should have been called to start repositioning file marker on the map
    expect(testMockMachine.startReposition).toHaveBeenCalled();

    // simulate file marker repositioning via the map state machine
    testMockMachine.isRepositioning = true;
    testMockMachine.mapLocationFeatureDataset = {
      location: { lng: -120, lat: 50 }, // new lat/lng for the marker
      fileLocation: null,
      pimsFeatures: [
        {
          type: 'Feature',
          properties: { ...emptyPropertyLocation, PROPERTY_ID: 1 },
          geometry: getMockPolygon(),
        },
      ],
      parcelFeatures: [
        {
          type: 'Feature',
          properties: { ...emptyPmbcParcel },
          geometry: getMockPolygon(),
        },
      ],
      regionFeature: {
        type: 'Feature',
        properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
        geometry: getMockPolygon(),
      },
      districtFeature: null,
      municipalityFeatures: null,
      highwayFeatures: null,
      selectingComponentId: null,
      crownLandLeasesFeatures: null,
      crownLandLicensesFeatures: null,
      crownLandTenuresFeatures: null,
      crownLandInventoryFeatures: null,
      crownLandInclusionsFeatures: null,
    };

    await act(async () => rerender());

    // verify that upon map click the file marker is repositioned and the new lat/lng is saved
    expect(formikRef.current.values.properties[0].property.fileLocation).toStrictEqual({
      lng: -120,
      lat: 50,
    });
  });
});
