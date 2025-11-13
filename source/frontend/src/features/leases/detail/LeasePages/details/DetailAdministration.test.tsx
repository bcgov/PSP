import { createMemoryHistory } from 'history';

import { DetailAdministration, IDetailAdministrationProps } from '@/features/leases';
import { mockApiProperty } from '@/mocks/filterData.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_CodeTypes_LeaseProgramTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseProgramTypes';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions, screen } from '@/utils/test-utils';

const history = createMemoryHistory();

describe('DetailAdministration component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailAdministrationProps = { lease: getEmptyLease() },
  ) => {
    // render component under test
    const utils = render(<DetailAdministration lease={renderOptions.lease} />, {
      ...renderOptions,
      history,
    });

    return { ...utils };
  };

  it('renders minimally as expected', () => {
    const { asFragment } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...mockApiProperty,
              areaUnit: toTypeCode('test'),
              landArea: 123,
            },
          },
        ],
        type: {
          id: 'test',
          description: 'testLeaseType',
          isDisabled: false,
          displayOrder: 0,
        },
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { asFragment } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...mockApiProperty,
              areaUnit: toTypeCode('test'),
              landArea: 123,
            },
          },
        ],
        amount: 1,
        description: 'a test description',
        programName: 'A program',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        primaryArbitrationCity: 'VICTORIA',
        expiryDate: '2022-01-01',
        hasDigitalLicense: true,
        hasPhysicalLicense: false,
        startDate: '2020-01-01',
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders all other fields', () => {
    setup({
      lease: {
        ...getEmptyLease(),
        programType: {
          id: ApiGen_CodeTypes_LeaseProgramTypes.OTHER,
          description: '',
          isDisabled: false,
          displayOrder: 0,
        },
        otherProgramType: 'other program type',
        type: {
          id: ApiGen_CodeTypes_LeaseLicenceTypes.OTHER,
          description: 'Other',
          isDisabled: false,
          displayOrder: 0,
        },
        otherType: 'other type',
        leasePurposes: [
          {
            id: 36,
            leaseId: 31,
            leasePurposeTypeCode: {
              id: ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
              description: 'Other*',
              isDisabled: false,
              displayOrder: 99,
            },
            purposeOtherDescription: 'PLAY POKER',
            appCreateTimestamp: '2024-07-25T21:18:52.71',
            appLastUpdateTimestamp: '2024-07-25T21:18:52.71',
            appLastUpdateUserid: 'EHERRERA',
            appCreateUserid: 'EHERRERA',
            appLastUpdateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
            appCreateUserGuid: '939a27d0-76cd-49b0-b474-53166adb73da',
            rowVersion: 1,
          },
        ],
      },
    });

    expect(screen.getByText('PLAY POKER')).toBeVisible();
    expect(screen.getByText('other program type')).toBeVisible();
    expect(screen.getByText('Other - other type')).toBeVisible();
  });

  it('does not render other fields if values not set to other', () => {
    setup({
      lease: {
        ...getEmptyLease(),
        programType: {
          id: ApiGen_CodeTypes_LeaseProgramTypes.RESRENTAL,
          description: 'Residential Rentals',
          isDisabled: false,
          displayOrder: 0,
        },
        otherProgramType: 'other program type',
        type: {
          id: ApiGen_CodeTypes_LeaseLicenceTypes.AMNDAGREE,
          description: 'Amending Agreement',
          isDisabled: false,
          displayOrder: 0,
        },
        otherType: 'other type',
        leasePurposes: [],
      },
    });

    expect(screen.queryByDisplayValue('PLAY POKER')).toBeNull();
    expect(screen.queryByDisplayValue('other program type')).toBeNull();
    expect(screen.queryByDisplayValue('other type')).toBeNull();
  });

  it('renders the program name', () => {
    setup({
      lease: {
        ...getEmptyLease(),
        programName: 'A program',
      },
    });
    expect(screen.getByText('A program')).toBeVisible();
  });

  it('renders the primary arbitration city', async () => {
    setup({
      lease: {
        ...getEmptyLease(),
        primaryArbitrationCity: 'Vancouver',
      },
    });

    expect(screen.getByText('Vancouver')).toBeVisible();
  });
});
