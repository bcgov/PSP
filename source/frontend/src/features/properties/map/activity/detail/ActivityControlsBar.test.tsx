import { Claims } from 'constants/claims';
import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { getMockActivityResponse } from 'mocks/mockActivities';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import { ActivityControlsBar, IActivityControlsBarProps } from './ActivityControlsBar';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');
const onEditRelatedProperties = jest.fn();

describe('ActivityControlsBar test', () => {
  const setup = (renderOptions: RenderOptions & IActivityControlsBarProps) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={getMockActivityResponse()}>
        <ActivityControlsBar onEditRelatedProperties={renderOptions.onEditRelatedProperties} />
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        claims: renderOptions.claims ?? [Claims.ACTIVITY_EDIT, Claims.PROPERTY_EDIT],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('Renders as expected', async () => {
    const { asFragment } = setup({ onEditRelatedProperties });
    expect(asFragment()).toMatchSnapshot();
  });

  it('hides the edit button when in edit mode', async () => {
    const { queryByTitle } = setup({ onEditRelatedProperties });
    expect(queryByTitle('edit')).toBeNull();
  });

  it('hides the Related Properties button when user does not have correct claims', async () => {
    const { queryByText } = setup({
      claims: [],
      onEditRelatedProperties,
    });
    expect(queryByText('Related properties')).toBeNull();
  });

  it('calls expected function when related properties is clicked', async () => {
    const { getByText } = setup({
      onEditRelatedProperties,
    });
    const relatedPropertiesButton = getByText('Related properties');
    userEvent.click(relatedPropertiesButton);
    expect(onEditRelatedProperties).toHaveBeenCalled();
  });
});
