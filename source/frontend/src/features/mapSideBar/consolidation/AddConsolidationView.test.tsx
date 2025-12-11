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
import { pidFormatter } from '@/utils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { ConsolidationFormModel } from './AddConsolidationModel';
import AddConsolidationView, { IAddConsolidationViewProps } from './AddConsolidationView';

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

const initialValues = new ConsolidationFormModel();

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

describe('Add Consolidation View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddConsolidationViewProps> } = {},
  ) => {
    const ref = createRef<FormikProps<ConsolidationFormModel>>();
    const utils = render(
      <AddConsolidationView
        {...renderOptions.props}
        formikRef={ref}
        loading={renderOptions.props?.loading ?? false}
        displayFormInvalid={renderOptions.props?.displayFormInvalid ?? false}
        onCancel={onCancel}
        onSave={onSave}
        onSubmit={onSubmit}
        consolidationInitialValues={
          renderOptions.props?.consolidationInitialValues ?? initialValues
        }
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
    property.pid = 88999888;
    await act(async () => {
      pidSelectorProps.setSelectProperty(property);
    });
    const text = await screen.findByText(pidFormatter(property.pid?.toString() ?? ''));
    expect(text).toBeVisible();
  });

  it('selected source properties can be removed', async () => {
    const initialFormModel = new ConsolidationFormModel();
    initialFormModel.sourceProperties = [{ ...getMockApiProperty(), pid: 111111111 }];

    const { getByTitle, queryByText } = await setup({
      props: {
        consolidationInitialValues: initialFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it('selected destination property can be removed', async () => {
    const initialFormModel = new ConsolidationFormModel();
    initialFormModel.destinationProperty = { ...getMockApiProperty(), pid: 111111111 };
    initialFormModel.sourceProperties = [];
    const { getByTitle, queryByText } = await setup({
      props: {
        consolidationInitialValues: initialFormModel,
      },
    });

    const button = getByTitle('remove');
    await act(async () => {
      userEvent.click(button);
    });
    expect(queryByText('111-111-111')).toBeNull();
  });

  it('property area only has at most 4 digits', async () => {
    const initialFormModel = new ConsolidationFormModel();
    const { queryByText } = await setup({
      props: {
        consolidationInitialValues: initialFormModel,
      },
    });

    const property = getMockApiProperty();
    property.landArea = 1.12345;
    await act(async () => {
      pidSelectorProps.setSelectProperty(property);
    });

    expect(queryByText('1.12')).toBeNull();
    expect(queryByText('1.123')).toBeNull();
    expect(queryByText('1.1235')).toBeVisible();
    expect(queryByText('1.12345')).toBeNull();
  });
});
