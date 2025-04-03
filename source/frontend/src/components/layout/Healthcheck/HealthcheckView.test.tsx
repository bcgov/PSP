import HealthcheckView, { IHealthCheckIssue, IHealthCheckViewProps } from './HealthcheckView';
import { act, render, RenderOptions, userEvent, screen } from '@/utils/test-utils';

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
});
