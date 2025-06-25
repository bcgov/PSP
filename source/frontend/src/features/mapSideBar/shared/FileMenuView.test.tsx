import { Claims } from '@/constants/index';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import FileMenuView, { IFileMenuProps } from './FileMenuView';

const onSelectFileSummary = vi.fn();
const onSelectProperty = vi.fn();
const onEditProperties = vi.fn();

describe('FileMenuView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<IFileMenuProps> } = {}) => {
    const rendered = render(
      <FileMenuView
        file={renderOptions.props?.file ?? mockAcquisitionFileResponse()}
        currentFilePropertyId={renderOptions.props?.currentFilePropertyId ?? 0}
        canEdit={renderOptions.props?.canEdit ?? false}
        isInNonEditableState={renderOptions.props?.isInNonEditableState ?? false}
        onSelectFileSummary={renderOptions.props?.onSelectFileSummary ?? onSelectFileSummary}
        onSelectProperty={renderOptions.props?.onSelectProperty ?? onSelectProperty}
        onEditProperties={renderOptions.props?.onEditProperties ?? onEditProperties}
      />,
      {
        useMockAuthentication: true,
        claims: [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return { ...rendered };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the file properties in a list ', () => {
    setup();
    expect(screen.getByText('023-214-937')).toBeVisible();
    expect(screen.getByText('024-996-777')).toBeVisible();
  });

  it('renders the currently selected property with different style', () => {
    setup({ props: { currentFilePropertyId: 2 } });

    const fileSummary = screen.getByTestId('menu-item-summary');
    const propertyOne = screen.getByTestId('menu-item-row-1');
    const propertyTwo = screen.getByTestId('menu-item-row-2');

    expect(propertyTwo).toHaveClass('selected');
    expect(propertyOne).not.toHaveClass('selected');
    expect(fileSummary).not.toHaveClass('selected');
  });

  it('allows the selected property to be changed', async () => {
    setup();
    const propertyTwo = screen.getByTestId('menu-item-row-2');
    await act(async () => userEvent.click(propertyTwo));
    expect(onSelectProperty).toHaveBeenCalledWith(2);
  });

  it('allows the file summary to be selected', async () => {
    setup();
    const fileSummary = screen.getByTitle('File Details');
    await act(async () => userEvent.click(fileSummary));
    expect(onSelectFileSummary).toHaveBeenCalled();
  });

  it('renders the edit button for users with edit permissions', async () => {
    setup({ props: { canEdit: true } });

    const button = screen.getByTitle('Change properties');
    const icon = screen.queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeVisible();
    expect(icon).toBeNull();

    await act(async () => userEvent.click(button));
    expect(onEditProperties).toHaveBeenCalled();
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    setup({ props: { canEdit: false } });

    const button = screen.queryByTitle('Change properties');
    const icon = screen.queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(button).toBeNull();
    expect(icon).toBeNull();
  });

  it(`renders the warning icon instead of the edit button for users when file in final state`, () => {
    setup({
      props: {
        canEdit: true,
        isInNonEditableState: true,
      },
    });

    const button = screen.queryByTitle('Change properties');
    const icon = screen.queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');

    expect(button).toBeNull();
    expect(icon).toBeVisible();
  });
});
