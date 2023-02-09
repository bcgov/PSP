import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import styled from 'styled-components';

import { IAcquisitionOwnersSummaryViewProps } from './AcquistionOwnersSummaryContainer';

const AcquisitionOwnersSummaryView: React.FC<IAcquisitionOwnersSummaryViewProps> = ({
  isLoading,
  ownersList,
}) => {
  if (isLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Section header="Owners">
      {ownersList?.map((owner, index) => {
        return (
          <>
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
            {index < ownersList.length - 1 && <hr></hr>}
          </>
        );
      })}
    </Section>
  );
};

export default AcquisitionOwnersSummaryView;

const StyledAddressWrapper = styled.div`
  white-space: pre-line;
`;
