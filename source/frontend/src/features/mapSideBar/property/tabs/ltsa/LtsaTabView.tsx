import { Form, Formik, getIn } from 'formik';
import noop from 'lodash/noop';
import moment from 'moment';
import styled from 'styled-components';

import { Input, TextArea } from '@/components/common/form';
import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  InlineMessage,
  StyledInlineMessageSection,
} from '@/components/common/Section/SectionStyles';
import { LtsaOrders, OrderParent, ParcelInfo, TaxAuthority } from '@/interfaces/ltsaModels';
import { withNameSpace } from '@/utils/formUtils';

import LtsaChargeSubForm from './LtsaChargeSubForm';
import LtsaDuplicateTitleSubForm from './LtsaDuplicateTitleSubForm';
import LtsaLandSubForm from './LtsaLandSubForm';
import LtsaOwnershipInformationForm from './LtsaOwnershipInformationForm';
import LtsaTransferSubForm from './LtsaTransferSubForm';

export interface ILtsaTabViewProps {
  ltsaData?: LtsaOrders;
  ltsaRequestedOn?: Date;
  loading: boolean;
  pid?: string;
}

export const LtsaTabView: React.FunctionComponent<React.PropsWithChildren<ILtsaTabViewProps>> = ({
  ltsaData,
  ltsaRequestedOn,
  loading,
  pid,
}) => {
  const titleNameSpace = 'titleOrders.0.orderedProduct.fieldedData';

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true} />

      {!pid ? (
        <FormSection>
          <b>
            This property does not have a valid PID.
            <br />
            <br /> Only properties that are associated to a valid PID can display corresponding data
            from LTSA.
          </b>
        </FormSection>
      ) : !loading && !ltsaData ? (
        <FormSection>
          <b>
            Failed to load data from LTSA.
            <br />
            <br /> Refresh this page to try again, or select a different property. If this error
            persists, contact an administrator.
          </b>
        </FormSection>
      ) : (
        <Formik
          initialValues={ltsaData ?? defaultLtsaData}
          onSubmit={noop}
          enableReinitialize={true}
        >
          <StyledForm>
            {ltsaRequestedOn && (
              <StyledInlineMessageSection>
                <InlineMessage>
                  This data was retrieved from LTSA on{' '}
                  {moment(ltsaRequestedOn).format('DD-MMM-YYYY h:mm A')}
                </InlineMessage>
              </StyledInlineMessageSection>
            )}
            <Section header="Title Details">
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
            </Section>
            <Section header="Land">
              <LtsaLandSubForm nameSpace={titleNameSpace} />
            </Section>
            <Section header="Ownership Information">
              <LtsaOwnershipInformationForm nameSpace={titleNameSpace} />
            </Section>
            <Section header="Charges, Liens and Interests" isCollapsable initiallyExpanded>
              <LtsaChargeSubForm nameSpace={titleNameSpace} />
            </Section>
            <Section header="Duplicate Indefeasible Title">
              <LtsaDuplicateTitleSubForm nameSpace={titleNameSpace} />
            </Section>
            <Section header="Transfers">
              <LtsaTransferSubForm nameSpace={titleNameSpace} />
            </Section>
            <Section header="Notes">
              <SectionField label="Miscellaneous notes">
                {getIn(ltsaData, 'parcelInfo.orderedProduct.fieldedData.miscellaneousNotes') ||
                  'None'}
              </SectionField>
              <SectionField label="Parcel status">
                <Input disabled field="parcelInfo.orderedProduct.fieldedData.status" />
              </SectionField>
            </Section>
          </StyledForm>
        </Formik>
      )}
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
