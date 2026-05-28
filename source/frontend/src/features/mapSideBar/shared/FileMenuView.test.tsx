import { Claims } from '@/constants/index';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import FileMenuView, { IFileMenuProps } from './FileMenuView';

const onSelectFileSummary = vi.fn();
const onSelectProperty = vi.fn();
const onEditProperties = vi.fn();
const mockRequestFlyToBounds = vi.fn();

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
        mockMapMachine: { ...mapMachineBaseMock, requestFlyToBounds: mockRequestFlyToBounds },
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

  it('renders Lat/Long for properties when no other identifier is available ', () => {
    const mockFile: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      fileProperties: [
        {
          id: 1,
          location: { coordinate: { x: -123.49, y: 48.43 } },
          property: {
            ...getMockApiProperty(),
            id: 37,
            pid: null,
            pin: null,
            latitude: 48.43,
            longitude: -123.49,
          },
          isActive: true,
          propertyName: null,
          file: null,
          fileId: 1,
          boundary: null,
          displayOrder: 1,
          propertyId: 37,
          rowVersion: 1,
        },
      ],
    };
    setup({ props: { file: mockFile } });
    expect(screen.getByText('48.430000, -123.490000')).toBeVisible();
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
    const propertyTwo = screen.getByTestId('menu-item-property-1');
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

  it('renders indices correctly based on location presence', () => {
    const fileWithMixedLocations = {
      ...mockAcquisitionFileResponse(),
      fileProperties: [
        {
          id: 1,
          location: { coordinate: { x: 1, y: 1 } },
          property: { id: 1 },
          isActive: true,
          propertyName: 'Prop A',
        },
        {
          id: 2,
          location: null,
          property: { id: 2, location: null },
          isActive: true,
          propertyName: 'Prop B',
        },
        {
          id: 3,
          location: { coordinate: { x: 2, y: 2 } },
          property: { id: 3 },
          isActive: true,
          propertyName: 'Prop C',
        },
        {
          id: 4,
          location: null,
          property: { id: 4, location: null },
          isActive: true,
          propertyName: 'Prop D',
        },
      ] as ApiGen_Concepts_FileProperty[],
    };

    setup({ props: { file: fileWithMixedLocations } });

    const prop1Row = screen.getByTestId('menu-item-row-1');
    expect(prop1Row).toHaveTextContent('1');

    const prop2Row = screen.getByTestId('menu-item-row-2');
    expect(prop2Row).not.toHaveTextContent('2');

    const prop3Row = screen.getByTestId('menu-item-row-3');
    expect(prop3Row).toHaveTextContent('2');

    const prop4Row = screen.getByTestId('menu-item-row-4');
    expect(prop4Row).not.toHaveTextContent('3');
    expect(prop4Row).not.toHaveTextContent('4');
  });
});
