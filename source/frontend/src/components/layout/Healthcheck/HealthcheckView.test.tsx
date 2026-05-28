import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import HealthcheckView, { IHealthCheckIssue, IHealthCheckViewProps } from './HealthcheckView';

const mockHealthcheckIssues: IHealthCheckIssue[] = [
  {
    key: 'PimsApi',
    msg: 'The PIMS server is currently unavailable, PIMS will not be useable until this is resolved.',
  },
];

describe('Healthcheck View component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IHealthCheckViewProps> } = {},
  ) => {
    const utils = render(
      <HealthcheckView
        {...renderOptions.props}
        systemChecked={renderOptions.props?.systemChecked ?? true}
        systemDegraded={renderOptions.props?.systemDegraded ?? true}
        systemChecks={renderOptions.props?.systemChecks ?? mockHealthcheckIssues}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders 'See the full list here' link`, async () => {
    const { getByTestId } = await setup({
      props: {
        systemDegraded: true,
        systemChecks: [
          {
            key: 'Mayan',
            msg: 'The PIMS Document server is experiencing service degradation, you will be unable to view, download or upload documents until resolved.',
          },
          ...mockHealthcheckIssues,
        ],
      },
    });
    expect(getByTestId('healthcheck-full-list-lnk')).toBeVisible();
  });

  it(`shows modal with full list after clicking the link`, async () => {
    const { getByTestId, getByText } = await setup({
      props: {
        systemDegraded: true,
        systemChecks: [
          ...mockHealthcheckIssues,
          {
            key: 'Mayan',
            msg: 'The PIMS Document server is experiencing service degradation, you will be unable to view, download or upload documents until resolved.',
          },
        ],
      },
    });

    const link = getByTestId('healthcheck-full-list-lnk');
    expect(link).toBeVisible();
    await act(() => userEvent.click(link));
    expect(
      getByText(/The PIMS Document server is experiencing service degradation/i),
    ).toBeVisible();
  });

  it('works as expected when system is degraded and issues array is empty', async () => {
    const { getByLabelText, getByText } = await setup({
      props: {
        systemDegraded: true,
        systemChecks: [],
      },
    });
    expect(getByLabelText('System degraded icon')).toBeVisible();
    expect(
      getByText(
        'The system is currently experiencing service degradation, you may experience issues using the application until this is resolved.',
      ),
    ).toBeVisible();
  });
});
