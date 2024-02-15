import { AcquisitionStatus } from '@/constants/acquisitionFileStatus';
import { Claims, Roles } from '@/constants/index';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
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
        acquisitionFile={props.acquisitionFile}
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
      acquisitionFile: mockAcquisitionFileResponse(),
      items: testData,
      selectedIndex: 0,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', () => {
    const { getByText } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      items: testData,
      selectedIndex: 0,
    });

    expect(getByText('one')).toBeVisible();
    expect(getByText('two')).toBeVisible();
    expect(getByText('three')).toBeVisible();
  });

  it('renders selected item with different style', () => {
    const { getByTestId } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      items: testData,
      selectedIndex: 1,
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');
  });

  it('allows the selected item to be changed', () => {
    const { getByText } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
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
        acquisitionFile: mockAcquisitionFileResponse(),
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_EDIT] },
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
        acquisitionFile: mockAcquisitionFileResponse(),
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeNull();
    expect(icon).toBeNull();
  });

  it(`renders the warning icon instead of the edit button for users`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          fileStatusTypeCode: { id: AcquisitionStatus.Complete },
        },
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_EDIT] },
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeNull();
    expect(icon).toBeVisible();
  });

  it(`it does not render the warning icon instead of the edit button for system admins`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          fileStatusTypeCode: { id: AcquisitionStatus.Complete },
        },
        items: testData,
        selectedIndex: 1,
      },
      { claims: [Claims.ACQUISITION_EDIT], roles: [Roles.SYSTEM_ADMINISTRATOR] },
    );

    const button = queryByTitle('Change properties');
    const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeVisible();
    expect(icon).toBeNull();
  });
});
