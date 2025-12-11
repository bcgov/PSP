import axios, { AxiosError } from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createRef } from 'react';

import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { getMockApiAddress } from '@/mocks/address.mock';
import { getMockFullyAttributedParcel } from '@/mocks/faParcelLayerResponse.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockApiProperty, getMockApiPropertyFile } from '@/mocks/properties.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fakeText,
  fillInput,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { FileForm } from '../../models';
import UpdateProperties, { IUpdatePropertiesProps } from './UpdateProperties';

const mockAxios = new MockAdapter(axios);

const setIsShowingPropertySelector = vi.fn();
const onSuccess = vi.fn();
const updateFileProperties = vi.fn();

describe('UpdateProperties component', () => {
  // render component under test
  const setup = async (
    props: Partial<IUpdatePropertiesProps>,
    renderOptions: RenderOptions = {},
  ) => {
    const formikRef = createRef<FormikProps<FileForm>>();
    const utils = render(
      <SideBarContextProvider>
        <UpdateProperties
          file={props.file ?? getMockResearchFile()}
          setIsShowingPropertySelector={setIsShowingPropertySelector}
          onSuccess={onSuccess}
          updateFileProperties={updateFileProperties}
          confirmBeforeAdd={props.confirmBeforeAdd ?? vi.fn()}
          canRemove={props.canRemove ?? vi.fn()}
          formikRef={formikRef}
        />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        claims: [],
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    // Wait for effects to complete
    await act(async () => {});

    return {
      ...utils,
      formikRef,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
    mockAxios.onGet().reply(200, getMockResearchFile());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('renders a row with an address', async () => {
    const { getByText } = await setup({
      file: {
        ...getMockResearchFile(),
        fileProperties: [
          {
            id: 3,
            isActive: null,
            propertyId: 443,
            property: {
              ...getMockApiProperty(),
              pid: undefined,
              pin: undefined,
              latitude: undefined,
              longitude: undefined,
              id: 443,
              anomalies: [],
              tenures: [],
              roadTypes: [],
              region: {
                id: 1,
                description: 'South Coast Region',
                isDisabled: false,
                displayOrder: null,
              },
              district: {
                id: 2,
                description: 'Vancouver Island District',
                isDisabled: false,
                displayOrder: null,
              },
              dataSourceEffectiveDateOnly: '2022-10-05T00:00:00',
              isRwyBeltDomPatent: false,
              address: {
                ...getMockApiAddress(),
                id: 1,
                streetAddress1: '45 - 904 Hollywood Crescent',
                streetAddress2: 'Living in a van',
                streetAddress3: 'Down by the River',
                municipality: 'Hollywood North',
                postal: 'V6Z 5G7',
                rowVersion: 2,
              },
              isOwned: false,
              landArea: 0,
              isVolumetricParcel: false,
              rowVersion: 3,
            },
            rowVersion: 1,
            displayOrder: null,
            fileId: 1,
            propertyName: null,
            location: null,
            file: null,
          },
        ],
      },
    });
    expect(getByText(/45 - 904 Ho/, { exact: false })).toBeVisible();
  });

  it('save button displays modal', async () => {
    const { getByText } = await setup({});
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(
      await screen.findByText(/You have made changes to the properties in this file./),
    ).toBeVisible();
  });

  it('saving and confirming the modal saves the properties', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText, container } = await setup({});

    await act(() => {
      fillInput(container, 'properties.0.name', 'test property name');
    });

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    const saveConfirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(saveConfirmButton));

    expect(updateFileProperties).toHaveBeenCalled();
    expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
    expect(onSuccess).toHaveBeenCalled();
  });

  it('should preserve the order of properties when saving', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText } = await setup(
      {
        file: {
          ...getMockResearchFile(),
          fileProperties: [
            {
              ...getMockApiPropertyFile(),
              // existing property
              property: { ...getMockApiProperty(), pid: 123456789, id: 1 },
            },
          ],
        },
      },
      {
        mockMapMachine: {
          ...mapMachineBaseMock,
          // properties to be added to the current file via the map state machine (ie working list, etc)
          selectedFeatures: [
            {
              location: { lng: -120.69195885, lat: 50.25163372 },
              fileLocation: null,
              pimsFeature: null,
              parcelFeature: getMockFullyAttributedParcel('111-111-111'),
              regionFeature: null,
              districtFeature: null,
              selectingComponentId: null,
              municipalityFeature: null,
            },
            {
              location: { lng: -120.69195885, lat: 50.25163372 },
              fileLocation: null,
              pimsFeature: null,
              parcelFeature: getMockFullyAttributedParcel('222-222-222'),
              regionFeature: null,
              districtFeature: null,
              selectingComponentId: null,
              municipalityFeature: null,
            },
            {
              location: { lng: -120.69195885, lat: 50.25163372 },
              fileLocation: null,
              pimsFeature: null,
              parcelFeature: getMockFullyAttributedParcel('333-333-333'),
              regionFeature: null,
              districtFeature: null,
              selectingComponentId: null,
              municipalityFeature: null,
            },
          ],
        },
      },
    );
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    const saveConfirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(saveConfirmButton));

    expect(updateFileProperties).toHaveBeenCalledWith(
      expect.objectContaining({
        fileProperties: expect.arrayContaining([
          expect.objectContaining({
            property: expect.objectContaining({ id: 1 }),
            displayOrder: 0,
          }),
          expect.objectContaining({
            property: expect.objectContaining({ pid: 111111111 }),
            displayOrder: 1,
          }),
          expect.objectContaining({
            property: expect.objectContaining({ pid: 222222222 }),
            displayOrder: 2,
          }),
          expect.objectContaining({
            property: expect.objectContaining({ pid: 333333333 }),
            displayOrder: 3,
          }),
        ]),
      }),
      expect.any(Array),
    );
  });

  it('saves lat/long based properties', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText } = await setup(
      {
        file: {
          ...getMockResearchFile(),
          fileProperties: [
            {
              ...getMockApiPropertyFile(),
              // existing property
              property: { ...getMockApiProperty(), pid: 123456789, id: 1 },
            },
          ],
        },
      },
      {
        mockMapMachine: {
          ...mapMachineBaseMock,
          // properties to be added to the current file via the map state machine (ie working list, etc)
          selectedFeatures: [
            {
              location: { lng: -120.69195885, lat: 50.25163372 },
              fileLocation: null,
              pimsFeature: null,
              parcelFeature: null,
              regionFeature: null,
              districtFeature: null,
              selectingComponentId: null,
              municipalityFeature: null,
            },
          ],
        },
      },
    );
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    const saveConfirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(saveConfirmButton));

    expect(updateFileProperties).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ApiGen_Concepts_File>>({
        fileProperties: expect.arrayContaining<Partial<ApiGen_Concepts_FileProperty>>([
          // existing property
          expect.objectContaining({
            property: expect.objectContaining<Partial<ApiGen_Concepts_Property>>({
              id: 1,
              pid: 123456789,
            }),
            displayOrder: 0,
          }),
          // lat/long based property
          expect.objectContaining({
            property: expect.objectContaining<Partial<ApiGen_Concepts_Property>>({
              location: { coordinate: { y: 50.25163372, x: -120.69195885 } },
            }),
            displayOrder: 1,
          }),
        ]),
      }),
      expect.any(Array),
    );
  });

  it('if the update fails with a 409 the associated entities modal is displayed', async () => {
    updateFileProperties.mockRejectedValue({
      isAxiosError: true,
      code: '409',
    } as Partial<AxiosError>);
    const { getByText } = await setup({});
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    const saveConfirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(saveConfirmButton));

    expect(updateFileProperties).toHaveBeenCalled();
    expect(screen.getByText('Property with associations'));
    expect(screen.getByText(/This property can not be removed from the file/i));
  });

  it('validates form values upon submission', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const { getByText, container } = await setup({});

    await act(() => {
      fillInput(container, 'properties.0.name', fakeText(550)); // invalid value
    });

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(await screen.findByText(/Please check form fields for errors/i)).toBeVisible();
  });

  it('cancel button cancels component if no actions taken', async () => {
    const { getByText } = await setup({});
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));

    expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
    expect(updateFileProperties).not.toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it('cancel button displays modal', async () => {
    const { getByText, container } = await setup({});

    await act(() => {
      fillInput(container, 'properties.0.name', 'test property name');
    });

    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));

    expect(
      await screen.findByText(/If you choose to cancel now, your changes will not be saved./i),
    ).toBeVisible();
  });

  it('closes the form without saving when cancelling', async () => {
    const { getByText, container } = await setup({});

    await act(() => {
      fillInput(container, 'properties.0.name', 'test property name');
    });

    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));

    expect(
      await screen.findByText(/If you choose to cancel now, your changes will not be saved./i),
    ).toBeVisible();

    const confirmButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(confirmButton));

    // modal is dismissed and form is closed without saving
    expect(setIsShowingPropertySelector).toHaveBeenCalledWith(false);
    expect(updateFileProperties).not.toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });

  it('removes property index if canRemove returns true', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const canRemove = vi.fn().mockResolvedValue(true);
    const { getByTestId, getByText } = await setup({ canRemove });

    const removeButton = getByTestId('delete-property-0');
    await act(async () => userEvent.click(removeButton));

    expect(canRemove).toHaveBeenCalled();
    expect(getByText('No Properties selected'));
  });

  it('displays warning modal if canRemove returns false', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    const canRemove = vi.fn().mockResolvedValue(false);
    const { getByTestId } = await setup({ canRemove });

    const removeButton = getByTestId('delete-property-0');
    await act(async () => userEvent.click(removeButton));

    expect(canRemove).toHaveBeenCalled();
    expect(screen.getByText('Property with associations'));
    expect(screen.getByText(/This property can not be removed from the file/i));
  });

  it('adds lat/long based properties to the file', async () => {
    updateFileProperties.mockResolvedValue(getMockResearchFile());
    await setup(
      {
        file: {
          ...getMockResearchFile(),
          fileProperties: [
            {
              ...getMockApiPropertyFile(),
              // existing property
              property: { ...getMockApiProperty(), pid: 123456789, id: 1 },
            },
          ],
        },
      },
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

    const addPropertyButton = screen.getByText('Add selected property');
    expect(addPropertyButton).toBeInTheDocument();
    await act(async () => userEvent.click(addPropertyButton));

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
