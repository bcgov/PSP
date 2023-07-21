import { createMemoryHistory } from 'history';

import { render, RenderOptions } from '@/utils/test-utils';

import AssociationHeader, { IAssociationHeaderProps } from './AssociationHeader';

const history = createMemoryHistory();

describe('AssociationHeader component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IAssociationHeaderProps) => {
    const component = render(
      <AssociationHeader
        icon={renderOptions.icon}
        title={renderOptions.title}
        count={renderOptions.count}
      />,
      {
        history,
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({ icon: <>An Icon</>, title: 'Test title', count: 3 });
    expect(asFragment()).toMatchSnapshot();
  });

  it('Shows the information on the header', () => {
    const iconContent = 'test icon';
    const titleText = 'test title';
    const testCount = 3;

    const { getByText } = setup({
      icon: <>{iconContent}</>,
      title: titleText,
      count: 3,
    });
    expect(getByText(iconContent)).toBeVisible();
    expect(getByText(titleText)).toBeVisible();
    expect(getByText(testCount)).toBeVisible();
  });
});
