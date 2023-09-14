import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { Api_AcquisitionFileOwner } from '@/models/api/AcquisitionFile';
import { render, RenderOptions } from '@/utils/test-utils';

import { IAcquisitionOwnersSummaryViewProps } from './AcquisitionOwnersSummaryContainer';
import AcquisitionOwnersSummaryView from './AcquisitionOwnersSummaryView';

jest.mock('@react-keycloak/web');

const mockAcquisitionFile = mockAcquisitionFileResponse(1);

const mockViewProps: IAcquisitionOwnersSummaryViewProps = {
  ownersList: mockAcquisitionFile.acquisitionFileOwners,
  isLoading: false,
};

const ownerIndividual = mockAcquisitionFileOwnersResponse(1)[0];
const ownerCorporate = mockAcquisitionFileOwnersResponse(1)[1];

describe('Acquisition File Owners View component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IAcquisitionOwnersSummaryViewProps> },
  ) => {
    const utils = render(
      <AcquisitionOwnersSummaryView
        {...renderOptions.props}
        ownersList={renderOptions.props?.ownersList ?? mockViewProps.ownersList}
        isLoading={renderOptions.props?.isLoading ?? false}
      />,
      {
        ...renderOptions,
        claims: [],
      },
    );
    return {
      ...utils,
      getIsPrymaryContactRadioButton: (index = 0) => {
        const radio = utils.container.querySelector(
          `input[name="${index}-is-primary-contact"]`,
        ) as HTMLInputElement;

        return radio;
      },
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('Renders Component as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('Display the Person Owner data', () => {
    const { getByText, getIsPrymaryContactRadioButton, findAllByDisplayValue } = setup({
      props: { ownersList: [ownerIndividual], isLoading: false },
    });

    expect(getIsPrymaryContactRadioButton()).toBeChecked();
    expect(getByText('JOHH DOE')).toBeVisible();
    expect(getByText('Sr.')).toBeVisible();
    expect(getByText('jonh@doe.ca')).toBeVisible();
    expect(getByText('111-111-1111')).toBeVisible();
    expect(findAllByDisplayValue('North Podunk, British Columbia, IH8 B0B')).not.toBeNull();
  });

  it('Display the Organization Owner data with Incorporation and Registration Number', () => {
    const { getByText, getIsPrymaryContactRadioButton } = setup({
      props: { ownersList: [ownerCorporate], isLoading: false },
    });

    expect(getIsPrymaryContactRadioButton()).not.toBeChecked();

    expect(getByText('FORTIS BC (Inc#:9999 / Reg#:12345)')).toBeVisible();
    expect(
      getByText(
        /123 Main Street PO Box 123 Next to the Dairy Queen East Podunk, British Columbia, I4M B0B Canada/,
      ),
    ).toBeVisible();
  });

  it('Display the Organization Owner data with Incorporation and NO Registration Number', () => {
    const ownerTest = { ...ownerCorporate, registrationNumber: null } as Api_AcquisitionFileOwner;
    const { getByText, getIsPrymaryContactRadioButton } = setup({
      props: { ownersList: [ownerTest], isLoading: false },
    });

    expect(getIsPrymaryContactRadioButton()).not.toBeChecked();
    expect(getByText('FORTIS BC (Inc#:9999)')).toBeVisible();
    expect(
      getByText(
        /123 Main Street PO Box 123 Next to the Dairy Queen East Podunk, British Columbia, I4M B0B Canada/,
      ),
    ).toBeVisible();
    expect(getByText('contact@fortis.ca')).toBeVisible();
    expect(getByText('775-111-1111')).toBeVisible();
  });

  it('Display the Organization Owner data with NO Incorporation and Registration Number', () => {
    const ownerTest = { ...ownerCorporate, incorporationNumber: null } as Api_AcquisitionFileOwner;
    const { getByText, getIsPrymaryContactRadioButton } = setup({
      props: { ownersList: [ownerTest], isLoading: false },
    });

    expect(getIsPrymaryContactRadioButton()).not.toBeChecked();
    expect(getByText('FORTIS BC (Reg#:12345)')).toBeVisible();
    expect(getByText('contact@fortis.ca')).toBeVisible();
    expect(getByText('775-111-1111')).toBeVisible();
    expect(
      getByText(
        /123 Main Street PO Box 123 Next to the Dairy Queen East Podunk, British Columbia, I4M B0B Canada/,
      ),
    ).toBeVisible();
  });

  it('renders owner address other country information', () => {
    const ownerTest = {
      ...ownerCorporate,
      incorporationNumber: null,
      address: {
        country: { code: 'OTHER', description: 'Other' },
        countryOther: 'test name',
      },
    } as Api_AcquisitionFileOwner;
    const { getByText } = setup({
      props: { ownersList: [ownerTest], isLoading: false },
    });
    expect(getByText('Other - test name')).toBeVisible();
  });
});
