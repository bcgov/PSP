import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultActivityFilter } from 'interfaces/IActivityResults';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { AddActivityForm, IAddActivityFormProps } from './AddActivityForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockTemplateTypes = [{ value: 1, code: 'GENERAL', label: 'General' }];
describe('ActivityFilterForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddActivityFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <AddActivityForm onAddActivity={jest.fn()} templateTypes={mockTemplateTypes} />
      </Formik>,
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

  beforeAll(() => {
    mockAxios
      .onGet(new RegExp(`/researchFiles/activity-templates/*`))
      .reply(200, mockTemplateTypes);
  });

  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultActivityFilter },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders activityType with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultActivityFilter },
    });

    await fillInput(container, 'activityType', 'Survey', 'select');

    expect(getByTestId('add-activity-type')).not.toBeNull();
  });
});
