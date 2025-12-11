import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { render, RenderOptions, screen } from '@/utils/test-utils';

import PropertyMapSelectorSubForm, {
  IPropertyMapSelectorSubFormProps,
} from './PropertyMapSelectorSubForm';
import { getMockLocationFeatureDataset } from '@/mocks/featureset.mock';

const onClickDraftMarker = vi.fn();

const mockStore = configureMockStore([thunk]);

const store = mockStore({});

describe('PropertySelectorSubForm component', () => {
  const setup = (renderOptions: RenderOptions & IPropertyMapSelectorSubFormProps) => {
    // render component under test
    const utils = render(
      <PropertyMapSelectorSubForm
        onClickDraftMarker={renderOptions.onClickDraftMarker}
        selectedProperty={renderOptions.selectedProperty ?? getMockLocationFeatureDataset()}
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
    const mockFeatureSet = getMockLocationFeatureDataset();
    setup({
      onClickDraftMarker,
      selectedProperty: {
        ...mockFeatureSet,
        parcelFeatures: [
          {
            ...mockFeatureSet.parcelFeatures[0],
            properties: {
              ...mockFeatureSet.parcelFeatures[0]?.properties,
              PID: '123-456-789',
              PIN: 1111222,
              LEGAL_DESCRIPTION: 'A legal description',
              PLAN_NUMBER: 'VIP3881',
            },
          },
        ],
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
