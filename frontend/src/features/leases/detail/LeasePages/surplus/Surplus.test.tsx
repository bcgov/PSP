import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease, IProperty } from 'interfaces';
import { noop } from 'lodash';
import { mockOrganization, mockPerson, mockProperties } from 'mocks/filterDataMock';
import { prettyFormatDate } from 'utils';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';

import Surplus from './Surplus';

const history = createMemoryHistory();

describe('Lease Surplus Declaration', () => {
  const setup = (renderOptions: RenderOptions & { lease?: IFormLease } = {}): RenderResult => {
    // render component under test
    const result = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <Surplus />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );
    return result;
  };

  it('renders as expected', () => {
    const result = setup({
      lease: { ...defaultFormLease, persons: [mockPerson], organizations: [mockOrganization] },
    });
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('Table row count is equal to the number of properties', () => {
    const result = setup({
      lease: {
        ...defaultFormLease,
        properties: mockProperties,
        persons: [mockPerson],
        organizations: [mockOrganization],
      },
    });
    const rows = result.getAllByRole('row');
    expect(rows.length).toBe(mockProperties.length + 1);
  });

  it('Default type value is unknown', () => {
    const testProperty: IProperty = mockProperties[0];
    testProperty.surplusDeclaration = undefined;
    const result = setup({
      lease: {
        ...defaultFormLease,
        properties: [testProperty],
        persons: [mockPerson],
        organizations: [mockOrganization],
      },
    });
    const dataRow = result.getAllByRole('row')[1];
    const columns = dataRow.querySelectorAll('[role="cell"]');

    expect(columns[1].textContent).toBe('Unknown');
  });

  it('Values are displayed', () => {
    const testProperty: IProperty = mockProperties[0];
    testProperty.surplusDeclaration = {
      type: { id: 'YES', isDisabled: false, description: 'Yes' },
      date: '2021-01-01',
      comment: 'Test Comment',
    };

    const result = setup({
      lease: {
        ...defaultFormLease,
        properties: [testProperty],
        persons: [mockPerson],
        organizations: [mockOrganization],
      },
    });
    const dataRow = result.getAllByRole('row')[1];
    const columns = dataRow.querySelectorAll('[role="cell"]');

    expect(columns[0].textContent).toBe(testProperty.pid);
    expect(columns[1].textContent).toBe(testProperty.surplusDeclaration?.type?.description);
    expect(columns[2].textContent).toBe(prettyFormatDate(testProperty.surplusDeclaration?.date));
    expect(columns[3].textContent).toBe(testProperty.surplusDeclaration?.comment);
  });
});
