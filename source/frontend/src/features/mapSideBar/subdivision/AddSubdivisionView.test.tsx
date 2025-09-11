import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { IMapSelectorContainerProps } from '@/components/propertySelector/MapSelectorContainer';
import { PropertySelectorPidSearchContainerProps } from '@/components/propertySelector/search/PropertySelectorPidSearchContainer';
import Claims from '@/constants/claims';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { SubdivisionFormModel } from './AddSubdivisionModel';
import AddSubdivisionView, { IAddSubdivisionViewProps } from './AddSubdivisionView';

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

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls getPrimaryAddressByPid when destination property is activated', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    await setup();
    await act(async () => {
      mapSelectorProps.addSelectedProperties([
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID_PADDED: '123-456-789',
            },
          },
        },
      ]);
    });
    expect(getPrimaryAddressByPid).toHaveBeenCalledWith('123-456-789');
  });

  it('does not call for address if property has no pid', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    await setup();
    await act(async () => {
      mapSelectorProps.addSelectedProperties([
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID_PADDED: undefined,
            },
          },
        },
      ]);
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
    const initialFormModel = new SubdivisionFormModel();
    initialFormModel.sourceProperty = { ...getMockApiProperty(), pid: 111111111 };
    initialFormModel.destinationProperties = [];

    const { getByTitle, queryByText } = await setup({
      props: {
        subdivisionInitialValues: initialFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it('selected destination properties can be removed', async () => {
    const initialFormModel = new SubdivisionFormModel();
    initialFormModel.destinationProperties = [{ ...getMockApiProperty(), pid: 111111111 }];

    const { getByTitle, queryByText } = await setup({
      props: {
        subdivisionInitialValues: initialFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it('property area only has at most 4 digits', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    const initialFormModel = new SubdivisionFormModel();
    getPrimaryAddressByPid.mockImplementation(() => Promise.resolve(undefined));

    const { queryByDisplayValue } = await setup({
      props: {
        subdivisionInitialValues: initialFormModel,
      },
    });

    await act(async () => {
      mapSelectorProps.addSelectedProperties([
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID_PADDED: '123-456-789',
              LAND_AREA: 1.12345,
            },
          },
        },
      ]);
    });

    expect(getPrimaryAddressByPid).toHaveBeenCalledWith('123-456-789');

    expect(queryByDisplayValue('1.12')).toBeNull();
    expect(queryByDisplayValue('1.123')).toBeNull();
    expect(queryByDisplayValue('1.1235')).toBeVisible();
    expect(queryByDisplayValue('1.12345')).toBeNull();
  });
});
