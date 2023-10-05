import Api_TypeCode from '@/models/api/TypeCode';
import { render, RenderOptions } from '@/utils/test-utils';

import { IMultiselectTextListProps, MultiselectTextList } from './MultiselectTextList';

const mockOptions: Api_TypeCode<string>[] = [
  { id: 'FOO', description: 'Foo' },
  { id: 'BAR', description: 'Bar' },
  { id: 'BAZ', description: 'Baz' },
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
    jest.clearAllMocks();
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
