import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IAddLeaseContainerProps } from 'features/leases';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease, ILeaseImprovement } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import { AddImprovementsContainer } from './AddImprovementsContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('Add lease container component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddLeaseContainerProps> & { improvements?: Partial<ILeaseImprovement>[] } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{
          lease: {
            ...defaultLease,
            improvements: renderOptions.improvements ?? ([] as any),
            id: 1,
          },
          setLease: noop,
        }}
      >
        <AddImprovementsContainer />
      </LeaseStateContext.Provider>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('cancels the form', async () => {
    const {
      component: { getAllByText },
    } = await setup({});
    userEvent.click(getAllByText('Cancel')[0]);
    expect(history.location.pathname).toBe('/lease/1/improvements');
  });

  it('displays all three improvement types', async () => {
    const {
      component: { findByText },
    } = await setup({});

    await findByText('Other Improvements');
    await findByText('Residential');
    await findByText('Commercial');
  });

  it('displays existing improvements', async () => {
    const {
      component: { findByDisplayValue, findByText },
    } = await setup({
      improvements: [
        { propertyImprovementTypeId: 'COMMBLDG', address: 'test address 1' },
        { propertyImprovementTypeId: 'OTHER', address: 'test address 2' },
        { propertyImprovementTypeId: 'RTA', address: 'test address 3' },
      ],
    });

    await findByText('Other Improvements');
    await findByText('Residential');
    await findByText('Commercial');
    await findByDisplayValue('test address 1');
    await findByDisplayValue('test address 2');
    await findByDisplayValue('test address 3');
  });

  it('saves the form with minimal data', async () => {
    const {
      component: { getByText, container },
    } = await setup({});
    await fillInput(container, 'improvements.0.address', 'address 1');
    await fillInput(container, 'improvements.0.structureSize', 'structure 1');
    await fillInput(container, 'improvements.0.description', 'description 1');
    await fillInput(container, 'improvements.1.address', 'address 2');
    await fillInput(container, 'improvements.1.structureSize', 'structure 2');
    await fillInput(container, 'improvements.1.description', 'description 2');
    await fillInput(container, 'improvements.2.address', 'address 3');
    await fillInput(container, 'improvements.2.structureSize', 'structure 3');
    await fillInput(container, 'improvements.2.description', 'description 3');

    mockAxios.onPut().reply(200, {});
    userEvent.click(getByText('Save'));
    await waitFor(() => {
      expect(mockAxios.history.put[0].data).toEqual(expectedFormData);
    });
  });

  it('does not save improvements with no content', async () => {
    const {
      component: { getByText, container },
    } = await setup({});

    await fillInput(container, 'improvements.0.address', 'address 1');
    await fillInput(container, 'improvements.0.structureSize', 'structure 1');
    await fillInput(container, 'improvements.0.description', 'description 1');
    await fillInput(container, 'improvements.1.address', 'address 2');
    await fillInput(container, 'improvements.1.structureSize', 'structure 2');
    await fillInput(container, 'improvements.1.description', 'description 2');

    mockAxios.onPut().reply(200, {});
    userEvent.click(getByText('Save'));
    await waitFor(() => {
      expect(JSON.parse(mockAxios.history.put[0].data).improvements).toHaveLength(2);
    });
  });
});

const expectedFormData =
  '{"organizations":[],"persons":[],"properties":[],"improvements":[{"propertyImprovementTypeId":"COMMBLDG","propertyImprovementType":"","description":"","structureSize":"structure 1","address":"address 1"},{"propertyImprovementTypeId":"RTA","propertyImprovementType":"","description":"","structureSize":"structure 2","address":"address 2"},{"propertyImprovementTypeId":"OTHER","propertyImprovementType":"","description":"","structureSize":"structure 3","address":"address 3"}],"securityDeposits":[],"securityDepositReturns":[],"startDate":"2020-01-01","lFileNo":"","tfaFileNo":0,"psFileNo":"","programName":"","motiName":"Moti, Name, Name","amount":0,"renewalCount":0,"landArea":"","areaUnit":"","tenantNotes":[],"insurances":[],"isResidential":false,"isCommercialBuilding":false,"isOtherImprovement":false,"returnNotes":"","terms":[],"tenants":[],"statusType":{"id":"ACTIVE","description":"Active","isDisabled":false},"region":{"regionCode":1,"regionName":"South Coast Region"},"programType":{"id":"OTHER","description":"Other","isDisabled":false},"paymentReceivableType":{"id":"RCVBL","description":"Receivable","isDisabled":false},"categoryType":{"id":"COMM","description":"Commercial","isDisabled":false},"purposeType":{"id":"BCFERRIES","description":"BC Ferries","isDisabled":false},"responsibilityType":{"id":"HQ","description":"Headquarters","isDisabled":false},"initiatorType":{"id":"PROJECT","description":"Project","isDisabled":false},"type":{"id":"LSREG","description":"Lease - Registered","isDisabled":false},"id":1}';
