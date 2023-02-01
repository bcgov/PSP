import { Claims } from 'constants/index';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import { EditFormNames } from '../EditFormNames';
import AcquisitionMenu, { IAcquisitionMenuProps } from './AcquisitionMenu';

// mock auth library
jest.mock('@react-keycloak/web');

const onChange = jest.fn();
const setContainerState = jest.fn();

const testData = ['one', 'two', 'three'];

describe('AcquisitionMenu component', () => {
  // render component under test
  const setup = (props: IAcquisitionMenuProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionMenu
        selectedIndex={props.selectedIndex}
        items={props.items}
        onChange={props.onChange}
        setContainerState={props.setContainerState}
      />,
      {
        useMockAuthentication: true,
        claims: [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup({
      items: testData,
      selectedIndex: 0,
      onChange,
      setContainerState,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', () => {
    const { getByText } = setup({
      items: testData,
      selectedIndex: 0,
      onChange,
      setContainerState,
    });

    expect(getByText('one')).toBeVisible();
    expect(getByText('two')).toBeVisible();
    expect(getByText('three')).toBeVisible();
  });

  it('renders selected item with different style', () => {
    const { getByTestId } = setup({
      items: testData,
      selectedIndex: 1,
      onChange,
      setContainerState,
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');
  });

  it('allows the selected item to be changed', () => {
    const { getByText } = setup({
      items: testData,
      selectedIndex: 1,
      onChange,
      setContainerState,
    });

    const lastItem = getByText('three');
    userEvent.click(lastItem);

    expect(onChange).toHaveBeenCalledWith(2);
  });

  it(`renders the edit button for users with property edit permissions`, () => {
    const { getByTitle } = setup(
      {
        items: testData,
        selectedIndex: 1,
        onChange,
        setContainerState,
      },
      { claims: [Claims.ACQUISITION_EDIT] },
    );

    const button = getByTitle('Change properties');
    expect(button).toBeVisible();

    userEvent.click(button);

    expect(setContainerState).toHaveBeenCalledWith({
      isEditing: true,
      activeEditForm: EditFormNames.propertySelector,
    });
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    const { queryByTitle } = setup(
      {
        items: testData,
        selectedIndex: 1,
        onChange,
        setContainerState,
      },
      { claims: [Claims.ACQUISITION_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    expect(button).toBeNull();
  });
});
