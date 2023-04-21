import { render, RenderOptions } from 'utils/test-utils';

import { DetailAcquisitionFileOwner } from '../models';
import { IAcquisitionOwnersSummaryViewProps } from './AcquisitionOwnersSummaryContainer';
import AcquisitionOwnersSummaryView from './AcquisitionOwnersSummaryView';

jest.mock('@react-keycloak/web');

const ownerPerson: DetailAcquisitionFileOwner = {
  isPrimary: true,
  ownerName: 'JOHN DOE',
  ownerOtherName: 'SR.',
  ownerDisplayAddress: `456 Souris Street \n
    PO Box 250 \n
    A Hoot and a holler from the A&W \n
    North Podunk, British Columbia, IH8 B0B \n
    Canada`,
};

const ownerOrganization: DetailAcquisitionFileOwner = {
  isPrimary: false,
  ownerName: 'FORTIS BC (9999)',
  ownerOtherName: 'LTD.',
  ownerDisplayAddress: `123 Main Street \n
    PO Box 123 \n
    Next to the Dairy Queen \n
    East Podunk, British Columbia, I4M B0B \n
    Canada`,
};

const mockViewProps: IAcquisitionOwnersSummaryViewProps = {
  ownersList: [ownerPerson, ownerOrganization],
  isLoading: false,
};

describe('Acquisition File Owners View component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionOwnersSummaryView
        ownersList={mockViewProps.ownersList}
        isLoading={mockViewProps.isLoading}
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
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('Display the Person Owner data', () => {
    const { getByText, getIsPrymaryContactRadioButton, findAllByDisplayValue } = setup();

    expect(getIsPrymaryContactRadioButton()).toBeChecked();
    expect(getByText(ownerPerson.ownerName as string)).toBeVisible();
    expect(getByText(ownerPerson.ownerOtherName as string)).toBeVisible();

    expect(findAllByDisplayValue('North Podunk, British Columbia, IH8 B0B')).not.toBeNull();
  });

  it('Display the Organization Owner data', () => {
    const { getByText, getIsPrymaryContactRadioButton, findAllByDisplayValue } = setup();

    expect(getIsPrymaryContactRadioButton(1)).not.toBeChecked();
    expect(getByText(ownerOrganization.ownerName as string)).toBeVisible();
    expect(getByText(ownerOrganization.ownerOtherName as string)).toBeVisible();
    expect(findAllByDisplayValue('East Podunk, British Columbia, I4M B0B')).not.toBeNull();
  });
});
