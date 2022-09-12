import { Claims } from 'constants/claims';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import { ActivityControlsBar, IActivityControlsBarProps } from './ActivityControlsBar';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');
const setEditMode = jest.fn();
const onEditRelatedProperties = jest.fn();

describe('ActivityControlsBar test', () => {
  const setup = (renderOptions: RenderOptions & IActivityControlsBarProps) => {
    // render component under test
    const component = render(
      <ActivityControlsBar
        editMode={renderOptions.editMode}
        setEditMode={renderOptions.setEditMode}
        onEditRelatedProperties={renderOptions.onEditRelatedProperties}
      />,
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
    const { asFragment } = setup({ editMode: false, setEditMode, onEditRelatedProperties });
    expect(asFragment()).toMatchSnapshot();
  });

  it('hides the edit button when in edit mode', async () => {
    const { queryByTitle } = setup({ editMode: true, setEditMode, onEditRelatedProperties });
    expect(queryByTitle('edit')).toBeNull();
  });

  it('hides the edit button when user does not have edit activity claim', async () => {
    const { queryByTitle } = setup({
      editMode: true,
      setEditMode,
      claims: [],
      onEditRelatedProperties,
    });
    expect(queryByTitle('edit')).toBeNull();
  });

  it('hides the Related Properties button when user does not have correct claims', async () => {
    const { queryByText } = setup({
      editMode: true,
      setEditMode,
      claims: [],
      onEditRelatedProperties,
    });
    expect(queryByText('Related properties')).toBeNull();
  });

  it('calls expected function when edit is clicked', async () => {
    const { getByTitle } = setup({
      editMode: false,
      setEditMode,
      onEditRelatedProperties,
    });
    const editButton = getByTitle('edit');
    userEvent.click(editButton);
    expect(setEditMode).toHaveBeenCalled();
  });

  it('calls expected function when related properties is clicked', async () => {
    const { getByText } = setup({
      editMode: true,
      setEditMode,
      onEditRelatedProperties,
    });
    const relatedPropertiesButton = getByText('Related properties');
    userEvent.click(relatedPropertiesButton);
    expect(onEditRelatedProperties).toHaveBeenCalled();
  });
});
