import { useKeycloak } from '@react-keycloak/web';
import { screen } from '@testing-library/dom';
import { fireEvent } from '@testing-library/dom';
import { cleanup, render, wait } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import * as actionTypes from 'constants/actionTypes';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { IParcel } from 'interfaces';
import * as _ from 'lodash';
import { mockBuildingWithAssociatedLand, mockDetails, mockParcel } from 'mocks/filterDataMock';
import { act } from 'react-dom/test-utils';
import { Route } from 'react-router-dom';
import VisibilitySensor from 'react-visibility-sensor';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { networkSlice } from 'store/slices/network/networkSlice';
import { propertiesSlice } from 'store/slices/properties';
import { defaultTenant } from 'tenants';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import PimsInventoryContainer from './PimsInventoryContainer';

jest.mock(
  'react-visibility-sensor',
  (): typeof VisibilitySensor => ({ children, partialVisibility, ...rest }: any) => (
    <div {...rest}>{typeof children === 'function' ? children({ isVisible: true }) : children}</div>
  ),
);
const mockAxios = new MockAdapter(axios);

jest.mock('@react-keycloak/web');
jest.spyOn(_, 'debounce').mockImplementation(
  jest.fn((fn: any) => {
    return fn as (...args: any[]) => any;
  }) as any,
);
jest.spyOn(global, 'fetch').mockImplementation(
  () =>
    Promise.resolve({
      json: () => Promise.resolve(JSON.stringify(defaultTenant)),
    }) as Promise<Response>,
);

const featureResponse = {
  features: [
    {
      type: 'Feature',
      geometry: { type: 'Point', coordinates: [1, 2] },
      properties: { PID: '123-456-789' },
    },
  ],
  type: 'FeatureCollection',
};
jest.mock('components/maps/leaflet/LayerPopup');
const handleParcelDataLayerResponse = jest.fn();

const mockStore = configureMockStore([thunk]);
const getStore = (parcelDetail?: IParcel) =>
  mockStore({
    [leafletMouseSlice.name]: {
      mapClickEvent: {
        originalEvent: { timeStamp: 1 },
        latlng: { lat: 1, lng: 2 },
      },
    },
    [networkSlice.name]: {
      [actionTypes.GET_PARCEL_DETAIL]: {
        status: 200,
      },
    },
    [propertiesSlice.name]: {
      parcelDetail: {
        parcelDetail: parcelDetail,
        propertyTypeId: 0,
      },
      parcels: [],
      draftParcels: [],
    },
    [lookupCodesSlice.name]: {
      lookupCodes: [
        { name: 'BC', code: 'BC', id: '1', isDisabled: false, type: API.PROVINCE_CODE_SET_NAME },
        {
          name: 'Victoria',
          id: '1',
          isDisabled: false,
          type: API.AMINISTRATIVE_AREA_CODE_SET_NAME,
        },
      ],
    },
    [propertiesSlice.name]: { parcels: [], draftParcels: [] },
  });

const history = createMemoryHistory({
  getUserConfirmation: (message, callback) => {
    callback(true);
  },
});

const renderContainer = ({ store }: any) =>
  render(
    <TestCommonWrapper store={store ?? getStore()} history={history}>
      <Route path="/mapView/:id?">
        <PimsInventoryContainer />
      </Route>
    </TestCommonWrapper>,
  );

describe('Parcel Detail PimsInventoryContainer', () => {
  process.env.REACT_APP_TENANT = 'CITZ';

  // clear mocks before each test
  beforeEach(() => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          agencies: [1],
          roles: [Claims.PROPERTY_EDIT],
        },
        subject: 'test',
      },
    });
    (useLayerQuery as jest.Mock).mockReturnValue({
      findOneWhereContains: async () => featureResponse,
      findByPid: async () => featureResponse,
      handleParcelDataLayerResponse: handleParcelDataLayerResponse,
    });
    mockAxios.reset();
    mockAxios.onAny().reply(200, {});
  });
  afterEach(() => {
    history.push({ search: '' });
    jest.resetAllMocks();
    cleanup();
  });
  afterAll(() => {
    jest.clearAllMocks();
    cleanup();
  });

  describe('basic data loading and display', () => {
    it('sidebar is hidden', async () => {
      await act(async () => {
        const { container } = renderContainer({});
        expect(container.firstChild).toMatchSnapshot();
      });
    });

    it('Empty parcel sidebar matches snapshot', async () => {
      await act(async () => {
        history.push('/mapview?sidebar=true');
        const { container } = renderContainer({});
        expect(container.firstChild).toMatchSnapshot();
      });
    });

    it('parcel sidebar snapshot loads by id', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1');
        const { container, findByText } = renderContainer({});
        mockAxios.reset();
        mockAxios.onGet().reply(200, mockDetails[0]);
        await findByText('test name');
        expect(container.firstChild).toMatchSnapshot();
      });
    });

    it('removes the parcel id when the sidebar is closed', () => {
      history.push('/mapview/?sidebar=false&parcelId=1');
      renderContainer({
        store: getStore(mockDetails[0]),
      });
      wait(() => expect(history.location.pathname).toEqual('/mapview/'));
    });

    it('displays add parcel text appropriately', () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand');
      const { getByText } = renderContainer({});
      expect(getByText('Submit Land (to inventory)')).toBeInTheDocument();
    });

    it('displays add building text appropriately', () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBuilding');
      const { getByText } = renderContainer({});
      expect(getByText('Submit a Building (to inventory)')).toBeInTheDocument();
    });

    it('displays add subdivision text appropriately', () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addSubdivisionLand');
      const { getByText } = renderContainer({});
      expect(getByText('Submit Subdivision (to inventory)')).toBeInTheDocument();
    });
  });
  describe('edit button display as rem', () => {
    beforeEach(() => {
      jest.resetAllMocks();
      process.env.REACT_APP_TENANT = 'CITZ';
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          userInfo: {
            agencies: [1],
            roles: [Claims.PROPERTY_EDIT],
          },
          subject: 'test',
        },
      });
      mockAxios.reset();
    });

    it('edit button displayed in view mode if user belongs to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        mockAxios.onGet().reply(200, mockDetails[0]);
        const { findByTestId } = renderContainer({});

        await wait(async () => {
          const editButton = await findByTestId('edit');
          expect(editButton).toBeInTheDocument();
        });
      });
    });

    it('edit button not displayed if user does not belong to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], agencyId: 2 };
        mockAxios.onGet().reply(200, parcel);
        const { queryByTestId } = renderContainer({});

        const editButton = await queryByTestId('edit');
        expect(editButton).not.toBeInTheDocument();
      });
    });

    it('edit button not displayed if property in SPP project', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        const { queryByTestId } = renderContainer({});

        const editButton = await queryByTestId('edit');
        expect(editButton).not.toBeInTheDocument();
      });
    });
  });
  describe('move pin functionality', () => {
    it('sets form values based on the map click location', async () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.reset();
      mockAxios.onGet().reply(200, [{ ...parcel, propertyTypeId: 0 }]);
      renderContainer({});
      act(() => {
        const landSearchMarker = screen.getByTestId('land-search-marker');
        fireEvent.click(landSearchMarker);
        history.push('/mapview/?sidebar=false&sidebarContext=addBareLand&disabled=false');
      });
      await wait(() => expect(screen.queryByText('Address')).toBeNull());
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      await wait(() => expect(screen.getByDisplayValue('1234 mock Street')).toBeVisible());
    });

    it('does nothing if the lat/lng is not specified', async () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.reset();
      mockAxios.onGet().reply(200, [{ ...parcel, propertyTypeId: 0 }]);
      renderContainer({
        store: mockStore({
          [leafletMouseSlice.name]: {
            mapClickEvent: {
              originalEvent: { timeStamp: 1 },
              latlng: undefined,
            },
          },
          [lookupCodesSlice.name]: {
            lookupCodes: [],
          },
          [propertiesSlice.name]: {
            parcelDetail: {},
            parcels: [],
            draftParcels: [],
          },
        }),
      });
      act(() => {
        const landSearchMarker = screen.getByTestId('land-search-marker');
        fireEvent.click(landSearchMarker);
        history.push('/mapview/?sidebar=false&sidebarContext=addBareLand&disabled=false');
      });
      await wait(() => expect(screen.queryByText('Address')).toBeNull());
    });

    it('sets form values based on the map click location by PIN', async () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.reset();
      mockAxios
        .onGet()
        .reply(200, [{ ...parcel, propertyTypeId: 0, pid: undefined, pin: 'pin', id: undefined }]);
      renderContainer({});
      act(() => {
        const landSearchMarker = screen.getByTestId('land-search-marker');
        fireEvent.click(landSearchMarker);
        history.push('/mapview/?sidebar=false&sidebarContext=addBareLand&disabled=false');
      });
      await wait(() => expect(screen.queryByText('Address')).toBeNull());
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    });

    it('sets form values based on the map click location for non-parcels', async () => {
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
      const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
      mockAxios.reset();
      mockAxios.onGet().reply(200, [{ ...parcel, propertyTypeId: 1 }]);
      renderContainer({});
      act(() => {
        const landSearchMarker = screen.getByTestId('land-search-marker');
        fireEvent.click(landSearchMarker);
        history.push('/mapview/?sidebar=false&sidebarContext=addBareLand&disabled=false');
      });
      await wait(() => expect(screen.queryByText('Address')).toBeNull());
      history.push('/mapview/?sidebar=true&sidebarContext=addBareLand&disabled=true');
    });
  });
  describe('edit button display as admin', () => {
    beforeEach(() => {
      jest.resetAllMocks();
      process.env.REACT_APP_TENANT = 'CITZ';
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          userInfo: {
            agencies: [1],
            roles: [Claims.ADMIN_PROPERTIES],
          },
          subject: 'test',
        },
      });
    });

    it('edit button displayed in view mode if admin belongs to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        mockAxios.onGet().reply(200, mockDetails[0]);
        const { findByTestId } = renderContainer({});

        const editButton = await findByTestId('edit');
        expect(editButton).toBeInTheDocument();
      });
    });

    it('edit button displayed if admin does not belong to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], agencyId: 2 };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId } = renderContainer({});

        const editButton = await findByTestId('edit');
        expect(editButton).toBeInTheDocument();
      });
    });

    it('edit button displayed if property in SPP project and user is admin', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId } = renderContainer({});

        const editButton = await findByTestId('edit');
        expect(editButton).toBeInTheDocument();
      });
    });
  });
  describe('delete button display as admin', () => {
    beforeEach(() => {
      jest.resetAllMocks();
      process.env.REACT_APP_TENANT = 'CITZ';
      (useKeycloak as jest.Mock).mockReturnValue({
        keycloak: {
          userInfo: {
            agencies: [1],
            roles: [Claims.ADMIN_PROPERTIES],
          },
          subject: 'test',
        },
      });
    });

    it('delete button displayed in view mode if admin belongs to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        mockAxios.onGet().reply(200, mockDetails[0]);
        const { findByTestId } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();
      });
    });

    it('delete button displayed if admin does not belong to same agency as property', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], agencyId: 2 };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();
      });
    });

    it('delete button displayed if property in SPP project and user is admin', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();
      });
    });

    it('delete button displayed if property in SPP project and user is admin', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();
      });
    });

    it('delete button pops up modal and delete functionality works as expected for parcels', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        mockAxios.onDelete().reply(200);
        const { findByTestId, findByText } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();

        fireEvent.click(deleteButton);
        const deleteModalButton = await findByText('Delete');
        fireEvent.click(deleteModalButton);
        await wait(() => {
          expect(mockAxios.history.delete).toHaveLength(1);
        });
      });
    });

    it('delete button pops up modal and delete functionality works as expected for buildings', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=true');
        const building = { ...mockDetails[0], projectNumbers: ['SPP-10000'], propertyTypeId: 1 };
        mockAxios.onGet().reply(200, building);
        mockAxios.onDelete().reply(200);
        const { findByTestId, findByText } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();

        fireEvent.click(deleteButton);
        const deleteModalButton = await findByText('Delete');
        fireEvent.click(deleteModalButton);
        await wait(() => {
          expect(mockAxios.history.delete).toHaveLength(1);
        });
      });
    });
    it('delete button pops up modal and cancel functionality works as expected', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&parcelId=1&disabled=true');
        const parcel = { ...mockDetails[0], projectNumbers: ['SPP-10000'] };
        mockAxios.onGet().reply(200, parcel);
        const { findByTestId, findByText } = renderContainer({});

        const deleteButton = await findByTestId('delete');
        expect(deleteButton).toBeInTheDocument();

        fireEvent.click(deleteButton);
        const cancelModalButton = await findByText('Cancel');
        fireEvent.click(cancelModalButton);
      });
    });
  });
  describe('modify associated land functionality', () => {
    beforeEach(() => {
      mockAxios.resetHistory();
      mockAxios.reset();
      process.env.REACT_APP_TENANT = 'CITZ';
    });

    it('displays the leased land other form when the corresponding radio button is clicked', async () => {
      // setup
      history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
      const building = {
        ...mockBuildingWithAssociatedLand,
        name: 'Modify assoc. land 12345',
        parcels: [],
      };
      mockAxios.onGet().reply(200, building);
      const { findByText, getByLabelText, getByText } = renderContainer({});

      mockAxios.onPut().reply(200, building);
      await act(async () => {
        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
      });
      await wait(async () => {
        expect(mockAxios.history.put.length).toBe(1);
        const associatedLandText = await screen.findByText('Modify assoc. land 12345');
        expect(associatedLandText).toBeVisible();
      });

      // act
      const ownershipButton = await findByText('Land Ownership');
      ownershipButton.click();
      const otherRadio = getByLabelText('Other');
      otherRadio.click();
      expect(getByText('Describe the land ownership situation for this parcel.')).toBeVisible();
    });
    it('displays the leased land owned form when the corresponding radio button is clicked', async () => {
      // setup
      history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
      const building = {
        ...mockBuildingWithAssociatedLand,
        name: 'Modify assoc. land 12345',
        parcels: [],
      };
      mockAxios.onGet().reply(200, building);
      const { findByText, getByLabelText, getByText } = renderContainer({});

      mockAxios.onPut().reply(200, building);
      await act(async () => {
        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
      });
      await wait(async () => {
        expect(mockAxios.history.put.length).toBe(1);
        const associatedLandText = await screen.findByText('Modify assoc. land 12345');
        expect(associatedLandText).toBeVisible();
      });

      // act
      const ownershipButton = await findByText('Land Ownership');
      ownershipButton.click();
      const ownedRadio = getByLabelText('This building is on land owned by my agency');
      ownedRadio.click();
      expect(
        getByText('Click Continue to enter the details of this associated parcel'),
      ).toBeVisible();
    });
    it('saves the building when clicked', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = { ...mockBuildingWithAssociatedLand, parcels: [] };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().reply(200, building);

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          expect(mockAxios.history.put.length).toBe(1);
          const associatedLandText = await screen.findByText('Review associated land information');
          expect(associatedLandText).toBeVisible();
        });
      });
    });
    it('displays an error toast if the save action fails', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = { ...mockBuildingWithAssociatedLand, parcels: [] };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().reply(500, null);

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        const failedBuildingSaveToast = await screen.findByText('Failed to update Building.');
        expect(failedBuildingSaveToast).toBeVisible();
      });
    });
    it('uses the most recent data from the api response', async () => {
      await act(async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().reply(200, building);

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          expect(mockAxios.history.put.length).toBe(1);
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
      });
    });
    describe('add tab functionality', () => {
      it('adds tabs', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [{ ...mockParcel, classificationId: '' }],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const addTabButton = await screen.findByTestId('add-tab');
        act(() => {
          fireEvent.click(addTabButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-2');
          expect(oldDeleteIcon).toBeInTheDocument();
        });
      });
      it('allows tabs to be clicked', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [mockParcel, { ...mockParcel, name: 'test 2' }],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const addTabButton = (await screen.findAllByText('test 2'))[0];
        act(() => {
          fireEvent.click(addTabButton);
        });
        await wait(async () => {
          const activeTabButton = (await screen.findAllByText('test 2'))[0];
          expect(activeTabButton.parentElement).toHaveClass('active');
        });
      });

      it('displays a tab for each parcel', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [mockParcel, mockParcel],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const addTabButtons = await screen.findAllByText('test name');
        await wait(async () => {
          //2 of these will be the tab headers, the other two are the tabs.
          expect(addTabButtons).toHaveLength(4);
        });
      });
    });
    describe('tab delete functionality', () => {
      beforeEach(() => {
        mockAxios.resetHistory();
        mockAxios.reset();
        jest.resetAllMocks();
        process.env.REACT_APP_TENANT = 'CITZ';
        (useKeycloak as jest.Mock).mockReturnValue({
          keycloak: {
            userInfo: {
              agencies: [1],
              roles: [Claims.PROPERTY_EDIT],
            },
            subject: 'test',
          },
        });
      });
      it('allows tabs to be deleted', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [mockParcel],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, building);
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const deleteIcon = await screen.findByTestId('delete-parcel-1');
        act(() => {
          fireEvent.click(deleteIcon);
        });
        await wait(async () => {
          const removeText = await screen.findByText('Really Remove Associated Parcel?');
          expect(removeText).toBeVisible();
        });
        const okButton = await screen.findByText('Ok');
        act(() => {
          fireEvent.click(okButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-1');
          expect(oldDeleteIcon).not.toBeInTheDocument();
        });
      }, 10000);
      it('does not delete buildings if there are errors', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [{ ...mockParcel, classificationId: '' }],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const deleteIcon = await screen.findByTestId('delete-parcel-1');
        act(() => {
          fireEvent.click(deleteIcon);
        });
        await wait(async () => {
          const removeText = await screen.findByText('Really Remove Associated Parcel?');
          expect(removeText).toBeVisible();
        });
        const okButton = await screen.findByText('Ok');
        act(() => {
          fireEvent.click(okButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-1');
          expect(oldDeleteIcon).toBeInTheDocument();
        });
      }, 10000);
      it('does not delete tabs if there are errors', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [{ ...mockParcel, classificationId: '' }],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const deleteIcon = await screen.findByTestId('delete-parcel-1');
        act(() => {
          fireEvent.click(deleteIcon);
        });
        await wait(async () => {
          const removeText = await screen.findByText('Really Remove Associated Parcel?');
          expect(removeText).toBeVisible();
        });
        const okButton = await screen.findByText('Ok');
        act(() => {
          fireEvent.click(okButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-1');
          expect(oldDeleteIcon).toBeInTheDocument();
        });
      }, 10000);
      it('deletes tabs if there is no leasedlandmetadata', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [{ ...mockParcel }],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building, leasedLandMetadata: [] });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const deleteIcon = await screen.findByTestId('delete-parcel-1');
        act(() => {
          fireEvent.click(deleteIcon);
        });
        await wait(async () => {
          const removeText = await screen.findByText('Really Remove Associated Parcel?');
          expect(removeText).toBeVisible();
        });
        const okButton = await screen.findByText('Ok');
        act(() => {
          fireEvent.click(okButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-1');
          expect(oldDeleteIcon).not.toBeInTheDocument();
        });
      }, 10000);
      it('deletes tabs if there are multiple tabs and no leasedlandmetadata', async () => {
        history.push('/mapview/?sidebar=true&buildingId=1&disabled=false');
        const building = {
          ...mockBuildingWithAssociatedLand,
          name: 'Modify assoc. land 12345',
          parcels: [mockParcel, mockParcel],
        };
        mockAxios.onGet().reply(200, building);
        const { findByText } = renderContainer({});

        mockAxios.onPut().replyOnce(200, { ...building, leasedLandMetadata: [] });
        mockAxios.onPut().replyOnce(200, { ...building, parcels: [], leasedLandMetadata: [] });

        const reviewButton = await findByText('Review & Submit');
        reviewButton.click();
        const modifyButton = await findByText('Modify Associated Land');
        modifyButton.click();
        await wait(async () => {
          const associatedLandText = await screen.findByText('Modify assoc. land 12345');
          expect(associatedLandText).toBeVisible();
        });
        const deleteIcon = await screen.findByTestId('delete-parcel-1');
        act(() => {
          fireEvent.click(deleteIcon);
        });
        await wait(async () => {
          const removeText = await screen.findByText('Really Remove Associated Parcel?');
          expect(removeText).toBeVisible();
        });
        const okButton = await screen.findByText('Ok');
        act(() => {
          fireEvent.click(okButton);
        });
        await wait(async () => {
          const oldDeleteIcon = await screen.queryByTestId('delete-parcel-2');
          expect(oldDeleteIcon).not.toBeInTheDocument();
        });
      }, 10000);
    });
  });
});
