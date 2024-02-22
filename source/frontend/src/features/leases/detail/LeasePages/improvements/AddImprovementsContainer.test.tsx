import { RenderOptions, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useFormikContext } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import React from 'react';
import { act } from 'react-test-renderer';

import { IAddLeaseContainerProps } from '@/features/leases/add/AddLeaseContainer';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';
import { defaultApiLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fillInput, renderAsync } from '@/utils/test-utils';

import { AddImprovementsContainer } from './AddImprovementsContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

const onSuccessMock = jest.fn();

const SaveButton = () => {
  const { submitForm } = useFormikContext();
  return <button onClick={submitForm}>Save</button>;
};

describe('Add Improvements container component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddLeaseContainerProps> & {
        improvements?: ApiGen_Concepts_PropertyImprovement[];
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{
          lease: {
            ...defaultApiLease(),
            id: 1,
            rowVersion: 1,
          },
          setLease: noop,
        }}
      >
        <AddImprovementsContainer
          formikRef={React.createRef()}
          onEdit={noop}
          improvements={renderOptions.improvements ?? []}
          loading={false}
          onSuccess={onSuccessMock}
        >
          <SaveButton />
        </AddImprovementsContainer>
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

  it('displays all three improvement types', async () => {
    const {
      component: { findByText },
    } = await setup({});

    await findByText('Other Improvements');
    await findByText('Residential Improvements');
    await findByText('Commercial Improvements');
  });

  it('displays existing improvements', async () => {
    const {
      component: { findByDisplayValue, findByText },
    } = await setup({
      improvements: [
        {
          propertyImprovementTypeCode: { id: 'COMMBLDG' },
          address: 'test address 1',
        } as ApiGen_Concepts_PropertyImprovement,
        {
          propertyImprovementTypeCode: { id: 'OTHER' },
          address: 'test address 2',
        } as ApiGen_Concepts_PropertyImprovement,
        {
          propertyImprovementTypeCode: { id: 'RTA' },
          address: 'test address 3',
        } as ApiGen_Concepts_PropertyImprovement,
      ],
    });

    await findByText('Other Improvements');
    await findByText('Residential Improvements');
    await findByText('Commercial Improvements');
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

    mockAxios.onPut().reply(200, []);
    act(() => userEvent.click(getByText('Save')));
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

    mockAxios.onPut().reply(200, []);
    act(() => userEvent.click(getByText('Save')));
    await waitFor(() => {
      expect(JSON.parse(mockAxios.history.put[0].data)).toHaveLength(2);
    });
  });

  it('displays the improvement types in order', async () => {
    const { component } = await setup({
      improvements: [
        {
          propertyImprovementTypeCode: { id: 'COMMBLDG' },
          address: 'test address 1',
        } as ApiGen_Concepts_PropertyImprovement,
        {
          propertyImprovementTypeCode: { id: 'OTHER' },
          address: 'test address 2',
        } as ApiGen_Concepts_PropertyImprovement,
        {
          propertyImprovementTypeCode: { id: 'RTA' },
          address: 'test address 3',
        } as ApiGen_Concepts_PropertyImprovement,
      ],
    });

    //Snapshot shows the correct order
    expect(component.asFragment()).toMatchSnapshot();
  });
});

const expectedFormData =
  '[{"id":null,"leaseId":1,"lease":null,"propertyImprovementTypeCode":{"id":"COMMBLDG","description":null,"displayOrder":null,"isDisabled":false},"improvementDescription":"","structureSize":"structure 1","address":"address 1","appCreateTimestamp":"1970-01-01T00:00:00","appLastUpdateTimestamp":"1970-01-01T00:00:00","appLastUpdateUserid":null,"appCreateUserid":null,"appLastUpdateUserGuid":null,"appCreateUserGuid":null,"rowVersion":null},{"id":null,"leaseId":1,"lease":null,"propertyImprovementTypeCode":{"id":"RTA","description":null,"displayOrder":null,"isDisabled":false},"improvementDescription":"","structureSize":"structure 2","address":"address 2","appCreateTimestamp":"1970-01-01T00:00:00","appLastUpdateTimestamp":"1970-01-01T00:00:00","appLastUpdateUserid":null,"appCreateUserid":null,"appLastUpdateUserGuid":null,"appCreateUserGuid":null,"rowVersion":null},{"id":null,"leaseId":1,"lease":null,"propertyImprovementTypeCode":{"id":"OTHER","description":null,"displayOrder":null,"isDisabled":false},"improvementDescription":"","structureSize":"structure 3","address":"address 3","appCreateTimestamp":"1970-01-01T00:00:00","appLastUpdateTimestamp":"1970-01-01T00:00:00","appLastUpdateUserid":null,"appCreateUserid":null,"appLastUpdateUserGuid":null,"appCreateUserGuid":null,"rowVersion":null}]';
