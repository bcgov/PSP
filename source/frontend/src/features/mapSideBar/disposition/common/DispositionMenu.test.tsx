import { DispositionFileStatus } from '@/constants/dispositionFileStatus';
import { Claims, Roles } from '@/constants/index';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import DispositionMenu, { IDispositionMenuProps } from './DispositionMenu';

// mock auth library
jest.mock('@react-keycloak/web');

const onChange = jest.fn();
const onShowPropertySelector = jest.fn();

const testData = ['one', 'two', 'three'];

describe('DispositionMenu component', () => {
  // render component under test
  const setup = (
    props: Omit<IDispositionMenuProps, 'onChange' | 'onShowPropertySelector'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <DispositionMenu
        dispositionFile={props.dispositionFile}
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
      dispositionFile: mockDispositionFileResponse(),
      items: testData,
      selectedIndex: 0,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', () => {
    const { getByText } = setup({
      dispositionFile: mockDispositionFileResponse(),
      items: testData,
      selectedIndex: 0,
    });

    expect(getByText('one')).toBeVisible();
    expect(getByText('two')).toBeVisible();
    expect(getByText('three')).toBeVisible();
  });

  it('renders selected item with different style', () => {
    const { getByTestId } = setup({
      dispositionFile: mockDispositionFileResponse(),
      items: testData,
      selectedIndex: 1,
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');
  });

  it('allows the selected item to be changed', () => {
    const { getByText } = setup({
      dispositionFile: mockDispositionFileResponse(),
      items: testData,
      selectedIndex: 1,
    });

    const lastItem = getByText('three');
    userEvent.click(lastItem);

    expect(onChange).toHaveBeenCalledWith(2);
  });

  it(`renders the edit button for users with property edit permissions`, () => {
    const { getByTitle, queryByTestId } = setup(
      {
        dispositionFile: mockDispositionFileResponse(),
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.DISPOSITION_EDIT] },
    );

    const button = getByTitle('Change properties');
    expect(button).toBeVisible();

    userEvent.click(button);

    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(onShowPropertySelector).toHaveBeenCalled();
    expect(icon).toBeNull();
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: mockDispositionFileResponse(),
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.DISPOSITION_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeNull();
    expect(icon).toBeNull();
  });

  it(`renders the warning icon instead of the edit button for users`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          fileStatusTypeCode: { id: DispositionFileStatus.Complete },
        },
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.DISPOSITION_EDIT] },
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeNull();
    expect(icon).toBeVisible();
  });

  it(`it does not render the warning icon instead of the edit button for system admins`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileResponse(),
          fileStatusTypeCode: { id: DispositionFileStatus.Complete },
        },
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.DISPOSITION_EDIT], roles: [Roles.SYSTEM_ADMINISTRATOR] },
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeVisible();
    expect(icon).toBeNull();
  });
});
