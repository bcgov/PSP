import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILayerPopupFlyoutProps, LayerPopupFlyout } from './LayerPopupFlyout';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onViewPropertyInfo = vi.fn();
const onCreateResearchFile = vi.fn();
const onCreateAcquisitionFile = vi.fn();
const onCreateLeaseLicense = vi.fn();
const onCreateDispositionFile = vi.fn();
const onCreateSubdivision = vi.fn();
const onCreateConsolidation = vi.fn();

describe('LayerPopupFlyout component', () => {
  const setup = (renderOptions?: RenderOptions & Partial<ILayerPopupFlyoutProps>) => {
    // render component under test
    const component = render(
      <LayerPopupFlyout
        isInPims={renderOptions?.isInPims ?? false}
        isRetiredProperty={renderOptions?.isRetiredProperty ?? false}
        onViewPropertyInfo={onViewPropertyInfo}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateLeaseLicense={onCreateLeaseLicense}
        onCreateDispositionFile={onCreateDispositionFile}
        onCreateSubdivision={onCreateSubdivision}
        onCreateConsolidation={onCreateConsolidation}
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
    vi.resetAllMocks();
  });

  it('renders property view link by default', async () => {
    const { getByText, getAllByRole } = setup();

    expect(getByText('View Property info')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(1);
  });

  it('calls onViewPropertyInfo when link clicked', async () => {
    const { getByText } = setup();

    const link = getByText('View Property info');
    await act(async () => userEvent.click(link));
    expect(onViewPropertyInfo).toHaveBeenCalled();
  });

  it('renders the link for creating a research file if user has claim', async () => {
    const { getByText, getAllByRole } = setup({ claims: [Claims.RESEARCH_ADD] });

    expect(getByText('Research File')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(2);
  });

  it('calls onCreateResearchFile when link clicked', async () => {
    const { getByText } = setup({ claims: [Claims.RESEARCH_ADD] });

    const link = getByText('Research File');
    await act(async () => userEvent.click(link));
    expect(onCreateResearchFile).toHaveBeenCalled();
  });

  it('renders the link for creating an acquisition file if user has claim', async () => {
    const { getByText, getAllByRole } = setup({ claims: [Claims.ACQUISITION_ADD] });

    expect(getByText('Acquisition File')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(2);
  });

  it('calls onCreateAcquisitionFile when link clicked', async () => {
    const { getByText } = setup({ claims: [Claims.ACQUISITION_ADD] });

    const link = getByText('Acquisition File');
    await act(async () => userEvent.click(link));
    expect(onCreateAcquisitionFile).toHaveBeenCalled();
  });

  it('renders the link for creating a disposition file if user has claim', async () => {
    const { getByText, getAllByRole } = setup({ claims: [Claims.DISPOSITION_ADD] });

    expect(getByText('Disposition File')).toBeVisible();
    expect(getAllByRole('button')).toHaveLength(2);
  });

  it('calls onCreateDispositionFile when link clicked', async () => {
    const { getByText } = setup({ claims: [Claims.DISPOSITION_ADD] });

    const link = getByText('Disposition File');
    await act(async () => userEvent.click(link));
    expect(onCreateDispositionFile).toHaveBeenCalled();
  });

  it('hides consolidation and subdivision links id not in pims', async () => {
    const { queryByText } = setup({ isInPims: false, claims: [Claims.PROPERTY_ADD] });

    const subdivisionLink = queryByText('Create Subdivision');
    expect(subdivisionLink).not.toBeInTheDocument();
    const consolidationLink = queryByText('Create Consolidation');
    expect(consolidationLink).not.toBeInTheDocument();
  });

  it('calls onCreateSubdivision when link clicked', async () => {
    const { getByText } = setup({ isInPims: true, claims: [Claims.PROPERTY_ADD] });

    const link = getByText('Create Subdivision');
    await act(async () => userEvent.click(link));
    expect(onCreateSubdivision).toHaveBeenCalled();
  });

  it('Hides options for retired properties leaving only Research File', async () => {
    const { queryByText } = setup({
      isInPims: true,
      isRetiredProperty: true,
      claims: [
        Claims.PROPERTY_ADD,
        Claims.DISPOSITION_ADD,
        Claims.LEASE_ADD,
        Claims.ACQUISITION_ADD,
        Claims.RESEARCH_ADD,
      ],
    });

    expect(queryByText('Create:')).toBeInTheDocument();
    expect(queryByText('Research File')).toBeInTheDocument();

    expect(queryByText('Acquisition File')).not.toBeInTheDocument();
    expect(queryByText('Lease/License')).not.toBeInTheDocument();
    expect(queryByText('Disposition File')).not.toBeInTheDocument();
    expect(queryByText('Create Subdivision')).not.toBeInTheDocument();
    expect(queryByText('Create Consolidation')).not.toBeInTheDocument();
  });
});
