import { Breadcrumb as BsBreadcrumb } from 'react-bootstrap';
import styled from 'styled-components';

export const LeftAlignDiv = styled.div`
  text-align: left;
`;

export const InlineFlexDiv = styled.div`
  display: flex;
  flex-wrap: nowrap;
`;

export const Breadcrumb = styled(BsBreadcrumb)`
  .breadcrumb-item:not(:first-child)::before {
    content: '>';
    width: 0px;
    margin-right: 1rem;
  }
  .breadcrumb {
    li a {
      max-height: 24px;
    }
  }
  grid-area: breadcrumb;
  ol {
    background-color: white;
  }
`;
