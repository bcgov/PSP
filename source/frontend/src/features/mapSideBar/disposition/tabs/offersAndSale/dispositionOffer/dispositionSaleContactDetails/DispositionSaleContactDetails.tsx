import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { StyledLink } from '@/components/common/styles';
import { ApiGen_Concepts_DispositionSalePurchaser } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaser';
import { ApiGen_Concepts_DispositionSalePurchaserAgent } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaserAgent';
import { ApiGen_Concepts_DispositionSalePurchaserSolicitor } from '@/models/api/generated/ApiGen_Concepts_DispositionSalePurchaserSolicitor';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IDispositionSaleContactDetailsProps {
  contactInformation:
    | ApiGen_Concepts_DispositionSalePurchaser
    | ApiGen_Concepts_DispositionSalePurchaserSolicitor
    | ApiGen_Concepts_DispositionSalePurchaserAgent;
  primaryContactLabel?: string | null;
}

const DispositionSaleContactDetails: React.FunctionComponent<
  IDispositionSaleContactDetailsProps
> = ({ contactInformation, primaryContactLabel }) => {
  const labelValue = primaryContactLabel ? primaryContactLabel : 'Primary contact';
  const primaryContact = contactInformation.primaryContact ?? null;

  return (
    <>
      <StyledLink
        target="_blank"
        rel="noopener noreferrer"
        to={
          contactInformation.personId
            ? `/contact/P${contactInformation.personId}`
            : `/contact/O${contactInformation.organizationId}`
        }
      >
        <span>
          {contactInformation.personId
            ? formatApiPersonNames(contactInformation.person)
            : contactInformation.organization?.name ?? ''}
        </span>
        <FaExternalLinkAlt className="ml-2" size="1rem" />
      </StyledLink>
      {primaryContact && (
        <>
          <StyledLabel>{labelValue}:</StyledLabel>
          <StyledSecondaryLink
            target="_blank"
            rel="noopener noreferrer"
            to={`/contact/P${contactInformation.primaryContactId}`}
          >
            <span>{formatApiPersonNames(primaryContact)}</span>
            <FaExternalLinkAlt className="ml-2" size="1rem" />
          </StyledSecondaryLink>
        </>
      )}
    </>
  );
};

export default DispositionSaleContactDetails;

const StyledLabel = styled.label`
  font-size: 16px;
  padding: 0 0.5rem;
  font-style: italic;
`;

export const StyledSecondaryLink = styled(Link)`
  padding: 0 0.5rem;
`;
