import { render, RenderOptions } from 'utils/test-utils';

import { DetailAcquisitionFileOwner } from '../models';
import { IAcquisitionOwnersSummaryViewProps } from './AcquisitionOwnersSummaryContainer';
import AcquisitionOwnersSummaryView from './AcquisitionOwnersSummaryView';

jest.mock('@react-keycloak/web');

const ownerPerson: DetailAcquisitionFileOwner = {
  ownerName: 'JOHN DOE',
  ownerOtherName: 'SR.',
  ownerDisplayAddress: `456 Souris Street \n
    PO Box 250 \n
    A Hoot and a holler from the A&W \n
    North Podunk, British Columbia, IH8 B0B \n
    Canada`,
};

const ownerOrganization: DetailAcquisitionFileOwner = {
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

describe('Acquistion File Owners View component', () => {
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
    const result = setup();

    expect(result.getByText(ownerPerson.ownerName as string)).toBeVisible();
    expect(result.getByText(ownerPerson.ownerOtherName as string)).toBeVisible();
    expect(result.findAllByDisplayValue('North Podunk, British Columbia, IH8 B0B')).not.toBeNull();
  });

  it('Display the Organization Owner data', () => {
    const result = setup();

    expect(result.getByText(ownerOrganization.ownerName as string)).toBeVisible();
    expect(result.getByText(ownerOrganization.ownerOtherName as string)).toBeVisible();
    expect(result.findAllByDisplayValue('East Podunk, British Columbia, I4M B0B')).not.toBeNull();
  });
});
