import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import { mockLookups } from 'mocks';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { ActivityDescription, IActivityDescriptionProps } from './ActivityDescription';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');

describe('ActivityDescription test', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IActivityDescriptionProps>) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={getMockActivityResponse()}>
        <ActivityDescription nameSpace="" editMode={renderOptions?.editMode ?? false} />
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        claims: renderOptions?.claims ?? [Claims.ACTIVITY_EDIT, Claims.PROPERTY_EDIT],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('displays value from formik activity', async () => {
    const { getByText } = setup();
    expect(getByText('test description')).toBeVisible();
  });

  it('displays editable field in edit mode', async () => {
    const { getByDisplayValue } = setup({ editMode: true });
    expect(getByDisplayValue('test description')).toBeVisible();
  });

  it('overrides edit mode if user does not have claim', async () => {
    const { queryByDisplayValue } = setup({ editMode: true, claims: [] });
    expect(queryByDisplayValue('test description')).toBeNull();
  });
});
