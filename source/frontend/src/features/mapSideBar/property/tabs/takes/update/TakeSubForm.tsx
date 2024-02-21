import { getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastDatePicker, Select, TextArea } from '@/components/common/form';
import { RadioGroup, yesNoRadioGroupValues } from '@/components/common/form/RadioGroup';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import AreaContainer from '@/components/measurements/AreaContainer';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { withNameSpace } from '@/utils/formUtils';

import { StyledBorderSection, StyledNoTabSection } from '../styles';

interface ITakeSubFormProps {
  takeIndex: number;
  nameSpace: string;
  onRemove: (index: number) => void;
}

const TakeSubForm: React.FunctionComponent<ITakeSubFormProps> = ({
  takeIndex,
  nameSpace,
  onRemove,
}) => {
  const formikProps = useFormikContext();
  const { values, setFieldValue, handleChange } = formikProps;
  const currentTake = getIn(values, withNameSpace(nameSpace));
  const { getOptionsByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();

  const takeTypeOptions = getOptionsByType(API.TAKE_TYPES);
  const takeStatusTypeOptions = getOptionsByType(API.TAKE_STATUS_TYPES);
  const takeSiteContamTypeOptions = getOptionsByType(API.TAKE_SITE_CONTAM_TYPES);
  const takeLandActTypeOptions = getOptionsByType(API.TAKE_LAND_ACT_TYPES).map(landAct => ({
    ...landAct,
    label: landAct.value + ' ' + landAct.label,
  }));

  const isThereSurplus = getIn(values, withNameSpace(nameSpace, 'isThereSurplus'));
  const isNewHighwayDedication = getIn(values, withNameSpace(nameSpace, 'isNewHighwayDedication'));
  const isNewInterestInSrw = getIn(values, withNameSpace(nameSpace, 'isNewInterestInSrw'));
  const isNewLandAct = getIn(values, withNameSpace(nameSpace, 'isNewLandAct'));
  const isNewLicenseToConstruct = getIn(
    values,
    withNameSpace(nameSpace, 'isNewLicenseToConstruct'),
  );

  const getModalWarning = (onOk: () => void) => {
    return (e: React.ChangeEvent<any>) => {
      if (e.target.value === 'false') {
        setModalContent({
          variant: 'info',
          title: 'Confirm change',
          message: 'The area, if provided, will be cleared. Do you wish to proceed?',
          okButtonText: 'Confirm',
          cancelButtonText: 'Cancel',
          handleOk: () => {
            onOk();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        handleChange(e);
      }
    };
  };

  return (
    <Section
      className="position-relative"
      header={currentTake?.id ? `Take ${takeIndex + 1}` : 'New Take'}
      isCollapsable={true}
      initiallyExpanded={true}
    >
      <StyledRemoveLinkButton
        style={{ position: 'absolute', right: '1rem' }}
        title="delete take"
        data-testid="take-delete-button"
        icon={<FaTrash size={24} id={`take-delete-${takeIndex}`} title="delete take" />}
        onClick={() => {
          setModalContent({
            ...getDeleteModalProps(),
            handleOk: () => {
              onRemove(takeIndex);
              setDisplayModal(false);
            },
          });
          setDisplayModal(true);
        }}
      ></StyledRemoveLinkButton>
      <SectionField label="Take type" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeTypeCode')}
          options={takeTypeOptions}
          placeholder="Select take type"
        />
      </SectionField>
      <SectionField label="Take status" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeStatusTypeCode')}
          options={takeStatusTypeOptions}
        />
      </SectionField>
      <SectionField label="Site contamination" labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeSiteContamTypeCode')}
          options={takeSiteContamTypeOptions}
        />
      </SectionField>
      <SectionField label="Description of this Take" labelWidth="12">
        <TextArea field={withNameSpace(nameSpace, 'description')} />
      </SectionField>
      <StyledNoTabSection header="Area">
        <StyledBorderSection>
          <SectionField
            label="Is there a new highway dedication? *"
            labelWidth="8"
            tooltip="The term new highway dedication includes municipal road or provincial public highway."
          >
            <RadioGroup
              field={withNameSpace(nameSpace, 'isNewHighwayDedication')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isNewHighwayDedication'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'newHighwayDedicationArea'), 0);
              })}
            />
          </SectionField>
          {isNewHighwayDedication === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'newHighwayDedicationArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'newHighwayDedicationAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'newHighwayDedicationAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.newHighwayDedicationArea}
                  field={withNameSpace(nameSpace, 'newHighwayDedicationArea')}
                />
              </SectionField>
            </>
          )}
          <SectionField
            label="Is this being acquired for MoTI inventory? *"
            labelWidth="8"
            tooltip="Selecting Yes for this option will result in the property being added to inventory."
            className="pt-4"
          >
            <RadioGroup
              field={withNameSpace(nameSpace, 'isAcquiredForInventory')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
            />
          </SectionField>
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField
            label="Is there a new registered interest in land (SRW, Easement or Covenant)? *"
            labelWidth="8"
          >
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isNewInterestInSrw')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isNewInterestInSrw'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'statutoryRightOfWayArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'srwEndDt'), '');
              })}
            />
          </SectionField>
          {isNewInterestInSrw === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'statutoryRightOfWayArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'statutoryRightOfWayAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'statutoryRightOfWayAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.statutoryRightOfWayArea}
                  field={withNameSpace(nameSpace, 'statutoryRightOfWayArea')}
                />
              </SectionField>
              <SectionField label="SRW end date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'srwEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is a there a new Land Act tenure? *" labelWidth="8">
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isNewLandAct')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isNewLandAct'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'landActArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'landActEndDt'), '');
                setFieldValue(withNameSpace(nameSpace, 'landActTypeCode'), '');
              })}
            />
          </SectionField>
          {isNewLandAct === 'true' && (
            <>
              <SectionField label="Land Act" required contentWidth="7">
                <Select
                  field={withNameSpace(nameSpace, 'landActTypeCode')}
                  placeholder="Select Land Act"
                  options={takeLandActTypeOptions}
                />
              </SectionField>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(withNameSpace(nameSpace, 'landActArea'), landArea);
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'landActAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'landActAreaUnitTypeCode'))}
                  landArea={currentTake.landActArea}
                  field={withNameSpace(nameSpace, 'landActArea')}
                />
              </SectionField>
              <SectionField label="End date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'landActEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField
            label="Is there a new License for Construction Access (TLCA/LTC)? *"
            labelWidth="8"
          >
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isNewLicenseToConstruct')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isNewLicenseToConstruct'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'licenseToConstructArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'ltcEndDt'), '');
              })}
            />
          </SectionField>
          {isNewLicenseToConstruct === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'licenseToConstructArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'licenseToConstructAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'licenseToConstructAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.licenseToConstructArea}
                  field={withNameSpace(nameSpace, 'licenseToConstructArea')}
                />
              </SectionField>

              <SectionField label="LTC end date" contentWidth="4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'ltcEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
      <StyledNoTabSection header="Surplus">
        <StyledBorderSection>
          <SectionField label="Is there a Surplus? *" labelWidth="8">
            <RadioGroup
              field={withNameSpace(nameSpace, 'isThereSurplus')}
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isThereSurplus'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'surplusArea'), 0);
              })}
            />
          </SectionField>
          {isThereSurplus === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(withNameSpace(nameSpace, 'surplusArea'), landArea);
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'surplusAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'surplusAreaUnitTypeCode'))}
                  landArea={currentTake.surplusArea}
                  field={withNameSpace(nameSpace, 'surplusArea')}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
    </Section>
  );
};

export default TakeSubForm;
