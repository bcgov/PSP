import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import PropertyMapSelectorSubForm, {
  IPropertyMapSelectorSubFormProps,
} from './PropertyMapSelectorSubForm';

const onClickDraftMarker = vi.fn();

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

describe('PropertySelectorSubForm component', () => {
  const setup = (renderOptions: RenderOptions & IPropertyMapSelectorSubFormProps) => {
    // render component under test
    const utils = render(
      <PropertyMapSelectorSubForm
        onClickDraftMarker={renderOptions.onClickDraftMarker}
        selectedProperty={renderOptions.selectedProperty ?? getMockSelectedFeatureDataset()}
      />,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { asFragment } = setup({ onClickDraftMarker });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    setup({
      onClickDraftMarker,
      selectedProperty: {
        ...mockFeatureSet,
        parcelFeature: {
          ...mockFeatureSet.parcelFeature,
          properties: {
            ...mockFeatureSet.parcelFeature?.properties,
            PID: '123-456-789',
            PIN: 1111222,
            LEGAL_DESCRIPTION: 'A legal description',
            PLAN_NUMBER: 'VIP3881',
          },
        },
      },
    });
    expect(screen.getByText('123-456-789')).toBeVisible();
    expect(screen.getByText('1111222')).toBeVisible();
    expect(screen.getByText('A legal description')).toBeVisible();
    expect(screen.getByText('VIP3881')).toBeVisible();
    expect(screen.getByText('1 - South Coast')).toBeVisible();
    expect(screen.getByText('2 - Vancouver Island')).toBeVisible();
  });
});
