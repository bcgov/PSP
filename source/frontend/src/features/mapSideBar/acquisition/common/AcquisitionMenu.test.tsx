import { Claims } from '@/constants/index';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import AcquisitionMenu, { IAcquisitionMenuProps } from './AcquisitionMenu';

// mock auth library
jest.mock('@react-keycloak/web');

const onChange = jest.fn();
const onShowPropertySelector = jest.fn();

const testData = ['one', 'two', 'three'];

describe('AcquisitionMenu component', () => {
  // render component under test
  const setup = (
    props: Omit<IAcquisitionMenuProps, 'onChange' | 'onShowPropertySelector'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <AcquisitionMenu
        acquisitionFileId={props.acquisitionFileId}
        selectedIndex={props.selectedIndex}
        items={props.items}
        onChange={onChange}
        onShowPropertySelector={onShowPropertySelector}
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
      acquisitionFileId: 1,
      items: testData,
      selectedIndex: 0,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', () => {
    const { getByText } = setup({
      acquisitionFileId: 1,
      items: testData,
      selectedIndex: 0,
    });

    expect(getByText('one')).toBeVisible();
    expect(getByText('two')).toBeVisible();
    expect(getByText('three')).toBeVisible();
  });

  it('renders selected item with different style', () => {
    const { getByTestId } = setup({
      acquisitionFileId: 1,
      items: testData,
      selectedIndex: 1,
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');
  });

  it('allows the selected item to be changed', () => {
    const { getByText } = setup({
      acquisitionFileId: 1,
      items: testData,
      selectedIndex: 1,
    });

    const lastItem = getByText('three');
    userEvent.click(lastItem);

    expect(onChange).toHaveBeenCalledWith(2);
  });

  it(`renders the edit button for users with property edit permissions`, () => {
    const { getByTitle } = setup(
      {
        acquisitionFileId: 1,
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_EDIT] },
    );

    const button = getByTitle('Change properties');
    expect(button).toBeVisible();

    userEvent.click(button);

    expect(onShowPropertySelector).toHaveBeenCalled();
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    const { queryByTitle } = setup(
      {
        acquisitionFileId: 1,
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    expect(button).toBeNull();
  });
});
