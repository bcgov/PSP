import { Form } from 'react-bootstrap';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSectionParagraph } from '@/components/common/styles';

import { DetailAcquisitionFileOwner } from '../../../models/DetailAcquisitionFileOwner';
import { IAcquisitionOwnersSummaryViewProps } from './AcquisitionOwnersSummaryContainer';

const AcquisitionOwnersSummaryView: React.FC<IAcquisitionOwnersSummaryViewProps> = ({
  isLoading,
  ownersList,
}) => {
  const ownerDetailList = ownersList?.map(o => DetailAcquisitionFileOwner.fromApi(o));

  if (isLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <>
      <StyledSectionParagraph>
        Each property in this file should be owned by the owner(s) in this section
      </StyledSectionParagraph>
      {ownerDetailList?.map((owner, index) => {
        return (
          <span key={`owner-${index}-${owner.ownerName}`} data-testid={`owner[${index}]`}>
            <SectionField label={null}>
              <Form.Check
                inline
                label="Primary Contact"
                name={`${index}-is-primary-contact`}
                type="radio"
                id={`${index}-is-primary-contact`}
                checked={owner.isPrimary}
                disabled
              />
            </SectionField>
            <SectionField label="Name">{owner.ownerName}</SectionField>
            <SectionField
              label="Other name"
              tooltip="Additional name for Individual (ex: alias or maiden name or space for long last name) Corporation (ex: Doing Business as) or placeholder for the Last name/Corporate name 2 field from Title"
            >
              {owner.ownerOtherName}
            </SectionField>
            <SectionField label="Mailing address">
              <StyledAddressWrapper>{owner.ownerDisplayAddress}</StyledAddressWrapper>
            </SectionField>
            <SectionField label="Email">{owner.ownerContactEmail}</SectionField>
            <SectionField label="Phone">{owner.ownerContactPhone}</SectionField>
            {index < ownerDetailList.length - 1 && <hr></hr>}
          </span>
        );
      })}
    </>
  );
};

export default AcquisitionOwnersSummaryView;

const StyledAddressWrapper = styled.div`
  white-space: pre-line;
`;
