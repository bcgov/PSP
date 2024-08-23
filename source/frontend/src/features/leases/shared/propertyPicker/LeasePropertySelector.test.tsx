import { Formik, FormikProps } from 'formik';
import noop from 'lodash/noop';
import React from 'react';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { FormLeaseProperty, getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { getMockPolygon } from '@/mocks/geometries.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyRegion } from '@/models/layers/motRegionalBoundary';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { EmptyPropertyLocation } from '@/models/layers/pimsPropertyLocationView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
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
      FormLeaseProperty.fromMapProperty({ pid: '123-456-789' }),
      FormLeaseProperty.fromMapProperty({ pin: '1111222' }),
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

  it('should pre-populate the region if a property is selected', async () => {
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
      pimsFeature: {
        type: 'Feature',
        properties: { ...EmptyPropertyLocation, PROPERTY_ID: 1 },
        geometry: getMockPolygon(),
      },
      parcelFeature: {
        type: 'Feature',
        properties: { ...emptyPmbcParcel },
        geometry: getMockPolygon(),
      },
      regionFeature: {
        type: 'Feature',
        properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
        geometry: getMockPolygon(),
      },
      districtFeature: null,
      municipalityFeature: null,
      highwayFeature: null,
      selectingComponentId: null,
      crownLandLeasesFeature: null,
      crownLandLicensesFeature: null,
      crownLandTenuresFeature: null,
      crownLandInventoryFeature: null,
      crownLandInclusionsFeature: null,
    };

    // verify that upon map click the lease region is auto-selected based on the property region
    await act(async () => rerender());
    expect(formikRef.current.values.regionId).toBe('1');
  });
});
