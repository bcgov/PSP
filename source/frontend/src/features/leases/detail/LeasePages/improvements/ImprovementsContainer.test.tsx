import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import React from 'react';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { mockLookups } from '@/mocks/lookups.mock';
import { defaultApiLease } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { renderAsync, RenderOptions } from '@/utils/test-utils';

import { ImprovementsContainer } from './ImprovementsContainer';
import { ILeaseImprovementForm } from './models';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

const onSuccessMock = jest.fn();

describe('Improvements Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & { improvements?: Partial<ILeaseImprovementForm>[] } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <LeaseStateContext.Provider
        value={{
          lease: {
            ...defaultApiLease(),
            id: 1,
          },
          setLease: noop,
        }}
      >
        <ImprovementsContainer
          isEditing={false}
          formikRef={React.createRef()}
          onSuccess={onSuccessMock}
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
        'There are no commercial, residential, or other improvements indicated with this lease/license.',
      ),
    ).toBeVisible();
  });
});
