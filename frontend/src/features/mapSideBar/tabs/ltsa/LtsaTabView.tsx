import { Input, TextArea } from 'components/common/form';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Form, Formik, getIn } from 'formik';
import { LtsaOrders, OrderParent, ParcelInfo, TaxAuthority } from 'interfaces/ltsaModels';
import { noop } from 'lodash';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { SectionFieldWrapper } from '../SectionFieldWrapper';
import { StyledFormSection, StyledScrollable, StyledSectionHeader } from '../SectionStyles';
import LtsaChargeSubForm from './LtsaChargeSubForm';
import LtsaDuplicateTitleSubForm from './LtsaDuplicateTitleSubForm';
import LtsaLandSubForm from './LtsaLandSubForm';
import LtsaTransferSubForm from './LtsaTransferSubForm';

export interface ILtsaTabViewProps {
  ltsaData?: LtsaOrders;
}

export const LtsaTabView: React.FunctionComponent<ILtsaTabViewProps> = ({ ltsaData }) => {
  const titleNameSpace = 'titleOrders.0.orderedProduct.fieldedData';
  return (
    <StyledScrollable>
      <LoadingBackdrop show={ltsaData === undefined} parentScreen={true} />
      <Formik initialValues={ltsaData ?? defaultLtsaData} onSubmit={noop} enableReinitialize={true}>
        <StyledForm>
          <StyledFormSection>
            <StyledSectionHeader>Title Details</StyledSectionHeader>
            <SectionFieldWrapper label="Title number">
              <Input
                disabled
                field={withNameSpace(titleNameSpace, 'titleIdentifier.titleNumber')}
              />
            </SectionFieldWrapper>
            <SectionFieldWrapper label="Land title district">
              <Input
                disabled
                field={withNameSpace(titleNameSpace, 'titleIdentifier.landTitleDistrict')}
              />
            </SectionFieldWrapper>
            <SectionFieldWrapper label="Taxation authorities">
              <TextArea
                disabled
                field={withNameSpace(titleNameSpace, 'taxAuthorities')}
                mapFunction={(taxAuthority: TaxAuthority) => taxAuthority.authorityName}
              />
            </SectionFieldWrapper>
          </StyledFormSection>
          <StyledFormSection>
            <LtsaLandSubForm nameSpace={titleNameSpace} />
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
            <SectionFieldWrapper label="Miscellaneous notes">
              <p>{getIn(ltsaData, 'parcelInfo.orderedProduct.fieldedData.miscellaneousNotes')}</p>
            </SectionFieldWrapper>
            <SectionFieldWrapper label="Parcel status">
              <Input disabled field="parcelInfo.orderedProduct.fieldedData.status" />
            </SectionFieldWrapper>
          </StyledFormSection>
        </StyledForm>
      </Formik>
    </StyledScrollable>
  );
};

export const StyledForm = styled(Form)`
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
