import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { render, RenderOptions } from '@/utils/test-utils';

import { IMultiselectTextListProps, MultiselectTextList } from './MultiselectTextList';

const mockOptions: ApiGen_Base_CodeType<string>[] = [
  {
    id: 'FOO',
    description: 'Foo',
    isDisabled: false,
    displayOrder: null,
  },
  {
    id: 'BAR',
    description: 'Bar',
    isDisabled: false,
    displayOrder: null,
  },
  {
    id: 'BAZ',
    description: 'Baz',
    isDisabled: false,
    displayOrder: null,
  },
];

describe('MultiselectTextList component', () => {
  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IMultiselectTextListProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <MultiselectTextList
        {...renderOptions.props}
        values={renderOptions.props?.values ?? []}
        displayValue={renderOptions.props?.displayValue ?? 'description'}
      />,
      {
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays existing values if they exist', () => {
    const { getByText } = setup({ props: { values: mockOptions } });

    expect(getByText(mockOptions[0].description!)).toBeVisible();
    expect(getByText(mockOptions[1].description!)).toBeVisible();
    expect(getByText(mockOptions[2].description!)).toBeVisible();
  });
});
