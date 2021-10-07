import { InlineFlexDiv } from 'components/common/styles';
import { Breadcrumb } from 'react-bootstrap';
import styled from 'styled-components';

export const LeaseH3 = styled.h3`
  padding: 2rem;
`;

export const LeaseHeaderRight = styled.div`
  white-space: nowrap;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  min-width: 2rem;
  flex-wrap: nowrap;
  @media only screen and (max-width: 715px) {
    display: none;
  }
`;

export const LeaseHeaderText = styled(InlineFlexDiv)`
  justify-content: center;
  padding: 0 2rem;
`;

export const LeaseHeader = styled(InlineFlexDiv)`
  border-radius: 1rem 1rem 0 0;
  grid-area: leaseheader;
  background-color: ${props => props.theme.css.slideOutBlue};
  color: white;
  align-items: center;
  justify-content: center;
  label {
    padding-right: 1rem;
    margin: 0;
  }
`;

export const LeaseBreadcrumb = styled(Breadcrumb)`
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
