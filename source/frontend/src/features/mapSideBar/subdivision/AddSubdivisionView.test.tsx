import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import {
  render,
  RenderOptions,
  waitFor,
  screen,
  getByTitle,
  userEvent,
  act,
} from '@/utils/test-utils';
import { SubdivisionFormModel } from './AddSubdivisionModel';
import AddSubdivisionView, { IAddSubdivisionViewProps } from './AddSubdivisionView';
import PropertySelectorPidSearchContainer, {
  PropertySelectorPidSearchContainerProps,
} from '@/components/propertySelector/search/PropertySelectorPidSearchContainer';
import MapSelectorContainer, {
  IMapSelectorContainerProps,
} from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { AreaUnitTypes } from '@/constants';

const history = createMemoryHistory();

const onCancel = vi.fn();
const onSave = vi.fn();
const onSubmit = vi.fn();
const getPrimaryAddressByPid = vi.fn();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

const initialValues = new SubdivisionFormModel();

let pidSelectorProps: PropertySelectorPidSearchContainerProps;
let mapSelectorProps: IMapSelectorContainerProps;

const PidSelectorView: React.FC<PropertySelectorPidSearchContainerProps> = props => {
  pidSelectorProps = props;
  return <span>Content Rendered</span>;
};

const TestView: React.FunctionComponent<IMapSelectorContainerProps> = props => {
  mapSelectorProps = props;
  return <span>Content Rendered</span>;
};

describe('Add Subdivision View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddSubdivisionViewProps> } = {},
  ) => {
    const ref = createRef<FormikProps<SubdivisionFormModel>>();
    const utils = render(
      <AddSubdivisionView
        {...renderOptions.props}
        formikRef={ref}
        loading={renderOptions.props?.loading ?? false}
        displayFormInvalid={renderOptions.props?.displayFormInvalid ?? false}
        onCancel={onCancel}
        onSave={onSave}
        onSubmit={onSubmit}
        subdivisionInitialValues={renderOptions.props?.subdivisionInitialValues ?? initialValues}
        getPrimaryAddressByPid={getPrimaryAddressByPid}
        PropertySelectorPidSearchComponent={PidSelectorView}
        MapSelectorComponent={TestView}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_ADD],
        history: history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  const testProperty: IMapProperty = {
    propertyId: 123,
    pid: '123-456-789',
    planNumber: 'SPS22411',
    address: 'Test address 123',
    region: 1,
    regionName: 'South Coast',
    district: 5,
    districtName: 'Okanagan-Shuswap',
  };

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls getPrimaryAddressByPid when destination property is activated', async () => {
    await setup();
    await act(async () => {
      mapSelectorProps.addSelectedProperties([testProperty]);
    });
    expect(getPrimaryAddressByPid).toHaveBeenCalledWith(testProperty.pid);
  });

  it('does not call for address if property has no pid', async () => {
    await setup();
    await act(async () => {
      mapSelectorProps.addSelectedProperties([{ ...testProperty, pid: undefined }]);
    });
    const text = await screen.findByText('Selected property must have a PID');
    expect(text).toBeVisible();
  });

  it('The PID search result is added to the source property', async () => {
    await setup();
    const property = getMockApiProperty();
    await act(async () => {
      pidSelectorProps.setSelectProperty(property);
    });
    const text = await screen.findByText(property.pid?.toString() ?? '');
    expect(text).toBeVisible();
  });

  it('selected source property can be removed', async () => {
    const { getByTitle, queryByText } = await setup({
      props: {
        subdivisionInitialValues: {
          sourceProperty: { ...getMockApiProperty(), pid: 111 - 111 - 111 },
          destinationProperties: [],
        } as unknown as SubdivisionFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it('selected destination properties can be removed', async () => {
    const { getByTitle, queryByText } = await setup({
      props: {
        subdivisionInitialValues: {
          destinationProperties: [{ ...getMockApiProperty(), pid: 111 - 111 - 111 }],
        } as unknown as SubdivisionFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it.only('property area only has 3 digits', async () => {
    const initialFormModel = new SubdivisionFormModel();
    getPrimaryAddressByPid.mockImplementation(() => Promise.resolve(undefined));
    const { queryByDisplayValue } = await setup({
      props: {
        subdivisionInitialValues: initialFormModel,
      },
    });

    const mapProperty: IMapProperty = {
      propertyId: 123,
      pid: '123-456-789',
      planNumber: 'SPS22411',
      address: 'Test address 123',
      region: 1,
      regionName: 'Some test region',
      district: 5,
      districtName: 'Okanagan-Shuswap',
      areaUnit: AreaUnitTypes.SquareMeters,
      landArea: 1.12345,
    };

    await act(async () => {
      mapSelectorProps.addSelectedProperties([mapProperty]);
    });

    expect(getPrimaryAddressByPid).toHaveBeenCalledWith(testProperty.pid);

    expect(queryByDisplayValue('1.1234')).toBeNull();
    expect(queryByDisplayValue('1.123')).toBeVisible();
    expect(queryByDisplayValue('1.12')).toBeNull();
  });
});
