import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILayerPopupFlyoutProps, LayerPopupFlyout } from './LayerPopupFlyout';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onViewPropertyInfo = jest.fn();
const onCreateResearchFile = jest.fn();
const onCreateAcquisitionFile = jest.fn();
const onCreateLeaseLicense = jest.fn();

describe('LayerPopupFlyout component', () => {
  const setup = (renderOptions?: RenderOptions & Partial<ILayerPopupFlyoutProps>) => {
    // render component under test
    const component = render(
      <LayerPopupFlyout
        onViewPropertyInfo={onViewPropertyInfo}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateLeaseLicense={onCreateLeaseLicense}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
        useMockAuthentication: true,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('renders property view link by default', async () => {
    const { getByText, getAllByRole } = setup();

    expect(getByText('View more property info')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(1);
  });

  it('calls onViewPropertyInfo when link clicked', async () => {
    const { getByText } = setup();

    const link = getByText('View more property info');
    userEvent.click(link);
    expect(onViewPropertyInfo).toHaveBeenCalled();
  });

  it('renders the link for creating a research file if user has claim', async () => {
    const { getByText, getAllByRole } = setup({ claims: [Claims.RESEARCH_ADD] });

    expect(getByText('Research File - Create new')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(2);
  });

  it('calls onCreateResearchFile when link clicked', async () => {
    const { getByText } = setup({ claims: [Claims.RESEARCH_ADD] });

    const link = getByText('Research File - Create new');
    userEvent.click(link);
    expect(onCreateResearchFile).toHaveBeenCalled();
  });

  it('renders the link for creating an acquisition file if user has claim', async () => {
    const { getByText, getAllByRole } = setup({ claims: [Claims.ACQUISITION_ADD] });

    expect(getByText('Acquisition File - Create new')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(2);
  });

  it('calls onCreateAcquisitionFile when link clicked', async () => {
    const { getByText } = setup({ claims: [Claims.ACQUISITION_ADD] });

    const link = getByText('Acquisition File - Create new');
    userEvent.click(link);
    expect(onCreateAcquisitionFile).toHaveBeenCalled();
  });
});
