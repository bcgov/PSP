import React from 'react';

import { render, RenderOptions, screen } from '@/utils/test-utils';

import { ISimpleSectionHeaderProps, SimpleSectionHeader } from './SimpleSectionHeader';
import { StyledAddButton } from './styles';
import { FaMoneyCheck } from 'react-icons/fa';

describe('SimpleSectionHeader component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<React.PropsWithChildren<ISimpleSectionHeaderProps>>;
    } = {},
  ) => {
    const utils = render(
      <SimpleSectionHeader
        title={renderOptions?.props?.title ?? 'TEST TITLE'}
        className={renderOptions?.props?.className ?? ''}
      >
        {renderOptions?.props?.children ?? null}
      </SimpleSectionHeader>,
      {
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the header title', () => {
    setup({ props: { title: 'LOREM IPSUM' } });
    expect(screen.getByText('LOREM IPSUM')).toBeVisible();
  });

  it('renders children sub-components on the right end of the header', () => {
    setup({
      props: {
        title: 'LOREM IPSUM',
        children: (
          <StyledAddButton>
            <FaMoneyCheck className="mr-2" />
            Generate
          </StyledAddButton>
        ),
      },
    });

    expect(screen.getByText('Generate')).toBeVisible();
  });
});
