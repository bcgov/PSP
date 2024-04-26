import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { FastDatePicker, Select, TextArea } from '@/components/common/form';
import { RadioGroup, yesNoRadioGroupValues } from '@/components/common/form/RadioGroup';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import AreaContainer from '@/components/measurements/AreaContainer';
import { Roles } from '@/constants';
import * as API from '@/constants/API';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { ApiGen_CodeTypes_LandActTypes } from '@/models/api/generated/ApiGen_CodeTypes_LandActTypes';
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
  const { hasRole } = useKeycloakWrapper();

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
  const isLeasePayable = getIn(values, withNameSpace(nameSpace, 'isLeasePayable'));
  const takeStatusTypeCode = getIn(values, withNameSpace(nameSpace, 'takeStatusTypeCode'));

  useEffect(() => {
    if (
      currentTake.completionDt &&
      currentTake.takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE
    ) {
      setFieldValue(withNameSpace(nameSpace, 'completionDt'), '');
    }
  }, [currentTake.completionDt, currentTake.takeStatusTypeCode, nameSpace, setFieldValue]);

  const getModalWarning = (onOk: () => void, isLeasePayable = false) => {
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
      } else if (isLeasePayable) {
        setModalContent({
          variant: 'info',
          title: 'Follow-up required',
          message:
            'You have created a Lease (Payable) Take. You also need to create a Lease/License File.',
          okButtonText: 'Close',
          cancelButtonText: null,
          handleOk: () => {
            handleChange(e);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        handleChange(e);
      }
    };
  };

  const canEditTake =
    currentTake?.id === 0 ||
    takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE ||
    hasRole(Roles.SYSTEM_ADMINISTRATOR);

  return (
    <Section
      className="position-relative"
      header={currentTake?.id ? `Take ${takeIndex + 1}` : 'New Take'}
      isCollapsable={true}
      initiallyExpanded={true}
    >
      {canEditTake && (
        <StyledRemoveLinkButton
          style={{ position: 'absolute', right: '1rem' }}
          title="delete take"
          data-testid="take-delete-button"
          icon={<FaTrash size={24} id={`take-delete-${takeIndex}`} title="delete take icon" />}
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
      )}

      <SectionField label="Take type" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeTypeCode')}
          options={takeTypeOptions}
          placeholder="Select take type"
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField label="Take status" required labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeStatusTypeCode')}
          options={takeStatusTypeOptions}
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField
        label="Completion date"
        required={
          currentTake.takeStatusTypeCode === ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE
        }
        tooltip={`This will be enabled when the file status is set to "Completed"`}
        labelWidth="4"
        contentWidth="5"
      >
        <FastDatePicker
          formikProps={formikProps}
          field={withNameSpace(nameSpace, 'completionDt')}
          maxDate={new Date()}
          disabled={
            currentTake.takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE
          }
        />
      </SectionField>
      <SectionField label="Site contamination" labelWidth="4" contentWidth="5">
        <Select
          field={withNameSpace(nameSpace, 'takeSiteContamTypeCode')}
          options={takeSiteContamTypeOptions}
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField label="Description of this Take" labelWidth="12">
        <TextArea field={withNameSpace(nameSpace, 'description')} disabled={!canEditTake} />
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
              disabled={!canEditTake}
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
              disabled={!canEditTake}
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
              disabled={!canEditTake}
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
                  isEditable={canEditTake}
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'statutoryRightOfWayAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.statutoryRightOfWayArea}
                  field={withNameSpace(nameSpace, 'statutoryRightOfWayArea')}
                />
              </SectionField>
              <SectionField label="SRW end date" labelWidth="3" className="mt-4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'srwEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a new Land Act tenure? *" labelWidth="8">
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
              disabled={!canEditTake}
            />
          </SectionField>
          {isNewLandAct === 'true' && (
            <>
              <SectionField label="Land Act" required contentWidth="7">
                <Select
                  field={withNameSpace(nameSpace, 'landActTypeCode')}
                  placeholder="Select Land Act"
                  options={takeLandActTypeOptions}
                  disabled={!canEditTake}
                  onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                    if (
                      [
                        ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
                        ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
                      ].includes(e.target.value)
                    ) {
                      setFieldValue(withNameSpace(nameSpace, 'landActEndDt'), '');
                    }
                  }}
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
                  isEditable={canEditTake}
                  unitCode={getIn(values, withNameSpace(nameSpace, 'landActAreaUnitTypeCode'))}
                  landArea={currentTake.landActArea}
                  field={withNameSpace(nameSpace, 'landActArea')}
                />
              </SectionField>
              {/** hide the end date for land act types that result in ownership*/}
              {![
                ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
                ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
              ].includes(currentTake.landActTypeCode) && (
                <SectionField label="End date" labelWidth="3" className="mt-4">
                  <FastDatePicker
                    field={withNameSpace(nameSpace, 'landActEndDt')}
                    formikProps={formikProps}
                    disabled={!canEditTake}
                    data-testId={withNameSpace(nameSpace, 'landActEndDt')}
                  />
                </SectionField>
              )}
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
              disabled={!canEditTake}
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
                  isEditable={canEditTake}
                  unitCode={getIn(
                    values,
                    withNameSpace(nameSpace, 'licenseToConstructAreaUnitTypeCode'),
                  )}
                  landArea={currentTake.licenseToConstructArea}
                  field={withNameSpace(nameSpace, 'licenseToConstructArea')}
                />
              </SectionField>

              <SectionField label="LTC end date" labelWidth="3" className="mt-4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'ltcEndDt')}
                  formikProps={formikProps}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a Lease (Payable)? *" labelWidth="8">
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field={withNameSpace(nameSpace, 'isLeasePayable')}
              handleChange={getModalWarning(() => {
                setFieldValue(withNameSpace(nameSpace, 'isLeasePayable'), 'false');
                setFieldValue(withNameSpace(nameSpace, 'leasePayableArea'), 0);
                setFieldValue(withNameSpace(nameSpace, 'leasePayableEndDt'), '');
              }, true)}
            />
          </SectionField>
          {isLeasePayable === 'true' && (
            <>
              <SectionField label="Area" labelWidth="12">
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'leasePayableArea'),
                      landArea,
                    );
                    formikProps.setFieldValue(
                      withNameSpace(nameSpace, 'leasePayableAreaUnitTypeCode'),
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, withNameSpace(nameSpace, 'leasePayableAreaUnitTypeCode'))}
                  landArea={currentTake.leasePayableArea}
                  field={withNameSpace(nameSpace, 'leasePayableArea')}
                />
              </SectionField>

              <SectionField label="End date" labelWidth="3" className="mt-4">
                <FastDatePicker
                  field={withNameSpace(nameSpace, 'leasePayableEndDt')}
                  formikProps={formikProps}
                  disabled={!canEditTake}
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
              disabled={!canEditTake}
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
                  isEditable={canEditTake}
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
