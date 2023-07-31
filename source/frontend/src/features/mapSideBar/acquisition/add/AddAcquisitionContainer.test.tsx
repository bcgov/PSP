import { createMemoryHistory } from 'history';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { Api_User } from '@/models/api/User';
import { emptyRegion } from '@/models/layers/motRegionalBoundary';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, renderAsync, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { AcquisitionOwnerFormModel, OwnerAddressFormModel } from '../common/models';
import { AddAcquisitionContainer, IAddAcquisitionContainerProps } from './AddAcquisitionContainer';
import { AcquisitionForm } from './models';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();

const onClose = jest.fn();

const DEFAULT_PROPS: IAddAcquisitionContainerProps = {
  onClose,
};

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

jest.mock('@/hooks/repositories/useUserInfoRepository');
(useUserInfoRepository as jest.MockedFunction<typeof useUserInfoRepository>).mockReturnValue({
  retrieveUserInfo: jest.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
        region: { id: 1 },
      },
      {
        id: 2,
        userId: 5,
        regionCode: 2,
        region: { id: 2 },
      },
    ],
  } as Api_User,
});

// Mock API service calls

jest.mock('@/hooks/repositories/useAcquisitionProvider');
const addAcquisitionFile = jest.fn();
(useAcquisitionProvider as jest.MockedFunction<typeof useAcquisitionProvider>).mockReturnValue({
  addAcquisitionFile: {
    execute: addAcquisitionFile as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
} as ReturnType<typeof useAcquisitionProvider>);

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('AddAcquisitionContainer component', () => {
  // render component under test
  const setup = async (
    props: IAddAcquisitionContainerProps = DEFAULT_PROPS,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = await renderAsync(
      <>
        <AddAcquisitionContainer {...props} />
      </>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
        history,
      },
    );

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="fileName"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getAcquisitionTypeDropdown: () =>
        utils.container.querySelector(`select[name="acquisitionType"]`) as HTMLSelectElement,
      getFundingTypeDropdown: () =>
        utils.container.querySelector(`select[name="fundingTypeCode"]`) as HTMLSelectElement,
      getFundingOtherTextbox: () =>
        utils.container.querySelector(
          `input[name="fundingTypeOtherDescription"]`,
        ) as HTMLInputElement,
      getOwner: (index = 0) => {
        return {
          givenNameTextbox: () =>
            utils.container.querySelector(
              `input[name="owners[${index}].givenName"]`,
            ) as HTMLInputElement,
          address: {
            streetAddress1: () =>
              utils.container.querySelector(
                `input[name="owners[${index}].address.streetAddress1"]`,
              ) as HTMLInputElement,
            municipality: () =>
              utils.container.querySelector(
                `input[name="owners[${index}].address.municipality"]`,
              ) as HTMLInputElement,
            postal: () =>
              utils.container.querySelector(
                `input[name="owners[${index}].address.postal"]`,
              ) as HTMLInputElement,
            countryDropdown: () =>
              utils.container.querySelector(
                `select[name="owners[${index}].address.countryId"]`,
              ) as HTMLSelectElement,
            countryOther: () =>
              utils.container.querySelector(
                `input[name="owners[${index}].address.countryOther"]`,
              ) as HTMLInputElement,
          },
        };
      },
    };
  };

  let formValues: AcquisitionForm;

  beforeEach(() => {
    formValues = new AcquisitionForm();
    formValues.fileName = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';
    formValues.acquisitionType = 'CONSEN';
    formValues.region = '1';
    formValues.project = { id: 0, text: 'Test Project' };
    formValues.fundingTypeCode = 'OTHER';
    formValues.fundingTypeOtherDescription = 'A different type of funding';
    addAcquisitionFile.mockResolvedValue(mockAcquisitionFileResponse(1, formValues.fileName));
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText, getNameTextbox, getRegionDropdown } = await setup();

    const formTitle = getByText(/Create Acquisition File/i);
    const input = getNameTextbox();
    const select = getRegionDropdown();

    expect(formTitle).toBeVisible();
    expect(input).toBeVisible();
    expect(input.tagName).toBe('INPUT');
    expect(select).toBeVisible();
    expect(select.tagName).toBe('SELECT');
  });

  it('should close the form when Cancel button is clicked', async () => {
    const { getCancelButton, getByText } = await setup();

    expect(getByText(/Create Acquisition File/i)).toBeVisible();
    userEvent.click(getCancelButton());

    expect(onClose).toBeCalled();
  });

  it('should pre-populate the region if a property is selected', async () => {
    const testMockMahine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      selectedFeatureDataset: {
        location: { lng: -120.69195885, lat: 50.25163372 },
        pimsFeature: null,
        parcelFeature: null,
        regionFeature: {
          type: 'Feature',
          properties: { ...emptyRegion, REGION_NUMBER: 1, REGION_NAME: 'South Coast Region' },
          geometry: {
            type: 'Polygon',
            coordinates: [[[-120.69195885, 50.25163372]]],
          },
        },
        districtFeature: null,
        municipalityFeature: null,
      },
    };
    (useMapStateMachine as unknown as jest.Mock<Partial<IMapStateMachineContext>>).mockReturnValue(
      testMockMahine,
    );

    await act(async () => {
      setup();
    });
    const text = await screen.findByDisplayValue(/South Coast Region/i);
    expect(text).toBeVisible();
  });

  it('should not pre-populate the region if a property is selected and the region cannot be determined', async () => {
    const testMockMahine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      selectedFeatureDataset: {
        location: { lng: -120.69195885, lat: 50.25163372 },
        pimsFeature: null,
        parcelFeature: null,
        regionFeature: {
          type: 'Feature',
          properties: { ...emptyRegion, REGION_NUMBER: 4 },
          geometry: {
            type: 'Polygon',
            coordinates: [[[-120.69195885, 50.25163372]]],
          },
        },
        districtFeature: null,
        municipalityFeature: null,
      },
    };
    (useMapStateMachine as unknown as jest.Mock<Partial<IMapStateMachineContext>>).mockReturnValue(
      testMockMahine,
    );

    await act(async () => {
      setup();
    });
    const text = await screen.findByDisplayValue(/Select region.../i);
    expect(text).toBeVisible();
  });

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    let testObj: any = undefined;

    await act(async () => {
      testObj = await setup(DEFAULT_PROPS);
    });

    const {
      getSaveButton,
      getNameTextbox,
      getAcquisitionTypeDropdown,
      getRegionDropdown,
      getFundingTypeDropdown,
      getFundingOtherTextbox,
    } = testObj;

    await act(async () => {
      userEvent.paste(getNameTextbox(), formValues.fileName as string);
      userEvent.selectOptions(getAcquisitionTypeDropdown(), formValues.acquisitionType as string);
      userEvent.selectOptions(getRegionDropdown(), formValues.region as string);
      userEvent.selectOptions(getFundingTypeDropdown(), formValues.fundingTypeCode as string);
      userEvent.paste(getFundingOtherTextbox(), formValues.fundingTypeOtherDescription);

      userEvent.click(getSaveButton());
    });

    const expectedValues = formValues.toApi();
    expect(addAcquisitionFile).toBeCalledWith(expectedValues, []);
    expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/1');
  });

  it(`should save the form with owner address information when 'Other' country is selected and no province is supplied`, async () => {
    const mockOwner = new AcquisitionOwnerFormModel();
    mockOwner.givenName = 'Space Toad';
    mockOwner.isPrimaryContact = 'true';
    mockOwner.address = new OwnerAddressFormModel();
    mockOwner.address.streetAddress1 = 'Test Street';
    mockOwner.address.streetAddress2 = '';
    mockOwner.address.streetAddress3 = '';
    mockOwner.address.municipality = 'Space Station';
    mockOwner.address.postal = '99999';
    mockOwner.address.countryId = 4; // OTHER country
    mockOwner.address.countryOther = 'Outer Space';

    formValues.owners = [mockOwner];

    let testObj: unknown = undefined;
    await act(async () => {
      testObj = await setup(DEFAULT_PROPS);
    });

    const {
      getSaveButton,
      getNameTextbox,
      getAcquisitionTypeDropdown,
      getRegionDropdown,
      getFundingTypeDropdown,
      getFundingOtherTextbox,
      getOwner,
      getByTestId,
    } = testObj as Awaited<ReturnType<typeof setup>>;

    const owner = getOwner(0);

    await act(async () => {
      userEvent.paste(getNameTextbox(), formValues.fileName as string);
      userEvent.selectOptions(getAcquisitionTypeDropdown(), formValues.acquisitionType as string);
      userEvent.selectOptions(getRegionDropdown(), formValues.region as string);
      userEvent.selectOptions(getFundingTypeDropdown(), formValues.fundingTypeCode as string);
      userEvent.paste(getFundingOtherTextbox(), formValues.fundingTypeOtherDescription);
      // add an owner
      userEvent.click(getByTestId('add-file-owner'));
    });

    expect(owner.givenNameTextbox()).toBeVisible();

    await act(async () => {
      userEvent.paste(owner.givenNameTextbox(), mockOwner.givenName!);
      userEvent.paste(owner.address.streetAddress1(), mockOwner.address?.streetAddress1!);
      userEvent.paste(owner.address.municipality(), mockOwner.address?.municipality!);
      userEvent.paste(owner.address.postal(), mockOwner.address?.postal!);
      userEvent.selectOptions(
        owner.address.countryDropdown(),
        mockOwner.address?.countryId?.toString() as string,
      );
    });

    await act(async () => {
      userEvent.paste(owner.address.countryOther(), mockOwner.address?.countryOther!);
    });

    await act(async () => userEvent.click(getSaveButton()));

    const expectedValues = formValues.toApi();
    expect(addAcquisitionFile).toBeCalledWith(expectedValues, []);
    expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/1');
  });
});
