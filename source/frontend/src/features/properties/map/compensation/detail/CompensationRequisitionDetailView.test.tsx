import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { getMockApiCompensation } from 'mocks/mockCompensations';
import { act, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import CompensationRequisitionDetailView, {
  CompensationRequisitionDetailViewProps,
} from './CompensationRequisitionDetailView';

const setEditMode = jest.fn();

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

describe('Compensation Detail View Component', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailViewProps> },
  ) => {
    // render component under test
    const component = render(
      <CompensationRequisitionDetailView
        compensation={renderOptions?.props?.compensation ?? getMockApiCompensation()}
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        gstConstant={renderOptions.props?.gstConstant ?? 0}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
      />,
      {
        ...renderOptions,
        history: history,
        claims: renderOptions?.claims ?? [Claims.COMPENSATION_REQUISITION_VIEW],
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.restoreAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('Edit Compensation Button not displayed without claims', async () => {
    const { queryByTitle } = setup({
      claims: [Claims.COMPENSATION_REQUISITION_VIEW],
    });

    const editButton = queryByTitle('Edit compensation requisition');
    expect(editButton).toBeNull();
  });

  it('Can click on the Edit Compensation Button', async () => {
    const { getByTitle } = setup({
      claims: [Claims.COMPENSATION_REQUISITION_EDIT],
    });

    const editButton = getByTitle('Edit compensation requisition');
    expect(editButton).toBeVisible();
    act(() => userEvent.click(editButton));
    await waitFor(() => {
      expect(setEditMode).toHaveBeenCalled();
    });
  });
});
