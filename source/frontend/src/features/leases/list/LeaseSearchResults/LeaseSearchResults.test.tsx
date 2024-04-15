import { getEmptyAddress } from '@/mocks/address.mock';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { getEmptyLeaseTenant } from '@/mocks/lease.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease, getEmptyProperty } from '@/models/defaultInitializers';
import { render, RenderOptions } from '@/utils/test-utils';

import { ILeaseSearchResultsProps, LeaseSearchResults } from './LeaseSearchResults';

const setSort = vi.fn();

// render component under test
const setup = (
  renderOptions: RenderOptions & Partial<ILeaseSearchResultsProps> = { results: [] },
) => {
  const { results, ...rest } = renderOptions;
  const utils = render(<LeaseSearchResults results={results ?? []} setSort={setSort} />, {
    ...rest,
  });
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  // long css selector to: get the FIRST header or table, then get the SVG up/down sort buttons
  const sortButtons = utils.container.querySelector(
    '.table .thead .tr .sortable-column div',
  ) as HTMLElement;
  return {
    ...utils,
    tableRows,
    sortButtons,
  };
};

const mockResults: ApiGen_Concepts_Lease[] = [
  {
    ...getEmptyLease(),
    id: 1,
    lFileNo: 'L-123-456',
    programName: 'TRAN-IT',
    tenants: [
      {
        ...getEmptyLeaseTenant(),
        person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
      },
    ],
    fileProperties: [
      {
        ...getEmptyPropertyLease(),
        property: {
          ...getEmptyProperty(),
          id: 123,
          pin: 123,
          address: { ...getEmptyAddress(), streetAddress1: '123 mock st' },
        },
      },
    ],
  },
  {
    ...getEmptyLease(),
    id: 2,
    lFileNo: 'L-999-888',
    programName: 'TRAN-IT',
    tenants: [
      {
        ...getEmptyLeaseTenant(),
        person: { ...getEmptyPerson(), firstName: 'Chester', surname: 'Tester' },
      },
    ],

    fileProperties: [
      {
        ...getEmptyPropertyLease(),
        property: {
          ...getEmptyProperty(),
          id: 124,
          pin: 999,
          address: { ...getEmptyAddress(), streetAddress1: '456 mock st' },
        },
      },
    ],
  },
];

describe('Lease Search Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockResults });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('Lease / License details do not exist in PIMS inventory');
    expect(toasts[0]).toBeVisible();
  });
});
