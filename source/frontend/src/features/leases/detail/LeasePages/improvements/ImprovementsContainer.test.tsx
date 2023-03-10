import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { createMemoryHistory } from 'history';
import { defaultLease, ILeaseImprovement } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { renderAsync, RenderOptions } from 'utils/test-utils';

import ImprovementsContainer from './ImprovementsContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('Improvements Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & { improvements?: Partial<ILeaseImprovement>[] } = {},
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
        <ImprovementsContainer
          isEditing={false}
          formikRef={React.createRef()}
        ></ImprovementsContainer>
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

  it('displays a message if there are no improvements', async () => {
    const {
      component: { getByText },
    } = await setup({ improvements: [] });

    expect(
      getByText(
        'If this lease/license includes any commercial, residential or other improvements on the property, switch to edit mode to add details to this record.',
      ),
    ).toBeVisible();
  });
});
