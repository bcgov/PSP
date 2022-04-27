import { Input, TextArea } from 'components/common/form';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Form, Formik, getIn } from 'formik';
import { LtsaOrders, OrderParent, ParcelInfo, TaxAuthority } from 'interfaces/ltsaModels';
import { noop } from 'lodash';
import moment from 'moment';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { SectionField } from '../SectionField';
import {
  InlineMessage,
  StyledFormSection,
  StyledInlineMessageSection,
  StyledSectionHeader,
} from '../SectionStyles';
import LtsaChargeSubForm from './LtsaChargeSubForm';
import LtsaDuplicateTitleSubForm from './LtsaDuplicateTitleSubForm';
import LtsaLandSubForm from './LtsaLandSubForm';
import LtsaOwnershipInformationForm from './LtsaOwnershipInformationForm';
import LtsaTransferSubForm from './LtsaTransferSubForm';

export interface ILtsaTabViewProps {
  ltsaData?: LtsaOrders;
  ltsaRequestedOn?: Date;
}

export const LtsaTabView: React.FunctionComponent<ILtsaTabViewProps> = ({
  ltsaData,
  ltsaRequestedOn,
}) => {
  const titleNameSpace = 'titleOrders.0.orderedProduct.fieldedData';
  const isLoading = ltsaData === undefined;

  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      <Formik initialValues={ltsaData ?? defaultLtsaData} onSubmit={noop} enableReinitialize={true}>
        <StyledForm>
          {ltsaRequestedOn && (
            <StyledInlineMessageSection>
              <InlineMessage>
                This data was retrived from LTSA on{' '}
                {moment(ltsaRequestedOn).format('DD-MMM-YYYY h:mm A')}
              </InlineMessage>
            </StyledInlineMessageSection>
          )}
          <StyledFormSection>
            <StyledSectionHeader>Title Details</StyledSectionHeader>
            <SectionField label="Title number">
              <Input
                disabled
                field={withNameSpace(titleNameSpace, 'titleIdentifier.titleNumber')}
              />
            </SectionField>
            <SectionField label="Land title district">
              <Input
                disabled
                field={withNameSpace(titleNameSpace, 'titleIdentifier.landTitleDistrict')}
              />
            </SectionField>
            <SectionField label="Taxation authorities">
              <TextArea
                disabled
                field={withNameSpace(titleNameSpace, 'taxAuthorities')}
                mapFunction={(taxAuthority: TaxAuthority) => taxAuthority.authorityName}
              />
            </SectionField>
          </StyledFormSection>
          <StyledFormSection>
            <LtsaLandSubForm nameSpace={titleNameSpace} />
          </StyledFormSection>
          <StyledFormSection>
            <LtsaOwnershipInformationForm nameSpace={titleNameSpace} />
          </StyledFormSection>
          <StyledFormSection>
            <LtsaChargeSubForm nameSpace={titleNameSpace} />
          </StyledFormSection>
          <StyledFormSection>
            <LtsaDuplicateTitleSubForm nameSpace={titleNameSpace} />
          </StyledFormSection>
          <StyledFormSection>
            <LtsaTransferSubForm nameSpace={titleNameSpace} />
          </StyledFormSection>
          <StyledFormSection>
            <StyledSectionHeader>Notes</StyledSectionHeader>
            <SectionField label="Miscellaneous notes">
              <p>{getIn(ltsaData, 'parcelInfo.orderedProduct.fieldedData.miscellaneousNotes')}</p>
            </SectionField>
            <SectionField label="Parcel status">
              <Input disabled field="parcelInfo.orderedProduct.fieldedData.status" />
            </SectionField>
          </StyledFormSection>
        </StyledForm>
      </Formik>
    </>
  );
};

export const StyledForm = styled(Form)`
  position: relative;
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
    }
  }
`;

const defaultLtsaData: LtsaOrders = {
  titleOrders: [
    {
      productType: OrderParent.ProductTypeEnum.Title,
      orderedProduct: {
        fieldedData: {
          titleIdentifier: {
            titleNumber: '',
            landTitleDistrict: '',
            taxAuthorities: [],
          },
        },
      },
    },
  ],
  parcelInfo: {
    productType: OrderParent.ProductTypeEnum.ParcelInfo,
    orderedProduct: {
      fieldedData: {
        parcelIdentifier: '',
        status: ParcelInfo.StatusEnum.ACTIVE,
        registeredTitlesCount: 0,
        pendingApplicationCount: 0,
        miscellaneousNotes: '',
        legalDescription: [],
      },
    },
  },
};

export default LtsaTabView;
