import { screen } from '@testing-library/react';
import { apiLeaseToFormLease } from 'features/leases/leaseUtils';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockApiPerson, mockOrganization } from 'mocks/filterDataMock';
import { getMockLeaseWithNoTenants } from 'mocks/mockLease';
import React from 'react';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import PrimaryContactWarningModal from './PrimaryContactWarningModal';

const history = createMemoryHistory();

describe('PrimaryContactWarningModal component', () => {
  const setup = (
    renderOptions: RenderOptions & { lease?: IFormLease; saveCallback?: Function } = {},
  ) => {
    // render component under test
    const component = render(
      <PrimaryContactWarningModal
        lease={renderOptions.lease}
        saveCallback={renderOptions.saveCallback}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, persons: [mockApiPerson], organizations: [mockOrganization] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('calls saveCallback on save', () => {
    const saveCallback = jest.fn();
    const { component } = setup({
      saveCallback: saveCallback,
      lease: { ...defaultFormLease, persons: [mockApiPerson, mockApiPerson] },
    });
    const { getByText } = component;
    const save = getByText('Save');
    userEvent.click(save);

    expect(saveCallback).toHaveBeenCalled();
  });

  it('displays all organization tenants that have multiple persons and no primary contact', () => {
    setup({
      lease: apiLeaseToFormLease(getMockLeaseWithNoTenants()),
      saveCallback: noop,
    });
    const tenantText = screen.getByText(content =>
      content.includes('French Mouse Property Management'),
    );

    expect(tenantText).toBeVisible();
  });
});
