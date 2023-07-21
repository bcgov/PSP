-- Add extended properties

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

---- Add extend properties to PIMS_ACQUISITION_OWNER_REP
--PRINT N'Add extend properties to PIMS_ACQUISITION_OWNER_REP'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Additional comment concerning this owener representative.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_ACQUISITION_OWNER_REP', 
--	@level2type = N'Column', @level2name = N'COMMENT'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Indicates if the code value is inactive.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_ACQUISITION_OWNER_REP', 
--	@level2type = N'Column', @level2name = N'IS_DISABLED'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Table describing the owners'' representative that is assigned to the acquisition file.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_ACQUISITION_OWNER_REP'
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

---- Add extend properties to PIMS_ACQUISITION_OWNER_SOLICITOR
--PRINT N'Add extend properties to PIMS_ACQUISITION_OWNER_SOLICITOR'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Indicates if the code value is inactive.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_ACQUISITION_OWNER_SOLICITOR', 
--	@level2type = N'Column', @level2name = N'IS_DISABLED'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Table describing the owners'' solicitor that is assigned to the acquisition file.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_ACQUISITION_OWNER_SOLICITOR'
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

-- Add extend properties to PIMS_INSURANCE
PRINT N'Add extend properties to PIMS_INSURANCE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the non-standard insurance coverage type' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_INSURANCE', 
	@level2type = N'Column', @level2name = N'OTHER_INSURANCE_TYPE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the insurance coverage' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_INSURANCE', 
	@level2type = N'Column', @level2name = N'COVERAGE_DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Monetary limit of the insurance coverage' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_INSURANCE', 
	@level2type = N'Column', @level2name = N'COVERAGE_LIMIT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicator that digital license exists' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_INSURANCE', 
	@level2type = N'Column', @level2name = N'IS_INSURANCE_IN_PLACE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date the insurance expires' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_INSURANCE', 
	@level2type = N'Column', @level2name = N'EXPIRY_DATE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE
PRINT N'Add extend properties to PIMS_LEASE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'MoTI region associated with the lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'REGION_CODE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Project associated with this lease.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'PROJECT_ID'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Generated identifying lease/licence number' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'L_FILE_NO'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Sourced from t_fileMain.TFA_File_Number' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'TFA_FILE_NO'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Sourced from t_fileMain.TFA_File_Number || - || t_fileSub.Subfile_Sequence_Code' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'TFA_FILE_NUMBER'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Sourced from t_fileSubOverrideData.PSFile_No' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'PS_FILE_NO'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Manually etered lease description, not the legal description' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'User-specified lease category description not included in standard set of lease purposes' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_CATEGORY_OTHER_DESC'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'User-specified lease purpose description not included in standard set of lease purposes' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_PURPOSE_OTHER_DESC'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Notes accompanying lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_NOTES'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Contact of the MoTI person associated with the lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'MOTI_CONTACT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Location of documents pertianing to the lease/license' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'DOCUMENTATION_REFERENCE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Notes accompanying lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'RETURN_NOTES'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of a non-standard lease program type' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'OTHER_LEASE_PROGRAM_TYPE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of a non-standard lease/license type' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'OTHER_LEASE_LICENSE_TYPE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of a non-standard lease purpose type' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'OTHER_LEASE_PURPOSE_TYPE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Original start date of the lease/license' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'ORIG_START_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Original expiry date of the lease/license' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'ORIG_EXPIRY_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Lease/licence amount' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_AMOUNT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date current responsibility came into effect for this lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'RESPONSIBILITY_EFFECTIVE_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Inspection date' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'INSPECTION_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Notes accompanying inspection' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'INSPECTION_NOTES'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Is subject the Residential Tenancy Act' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'IS_SUBJECT_TO_RTA'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Is a commercial building' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'IS_COMM_BLDG'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Is improvement of another description' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'IS_OTHER_IMPROVEMENT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Incidcator that lease/license has expired' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'IS_EXPIRED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicator that phyical file exists' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'HAS_PHYSICAL_FILE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicator that digital file exists' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'HAS_DIGITAL_FILE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicator that physical license exists' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'HAS_PHYSICIAL_LICENSE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicator that digital license exists' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE', 
	@level2type = N'Column', @level2name = N'HAS_DIGITAL_LICENSE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Details of a lease that is inventoried in PIMS system.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE_ACTIVITY_INSTANCE
PRINT N'Add extend properties to PIMS_LEASE_ACTIVITY_INSTANCE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Associative entity between leases/licenses and activity instances.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_ACTIVITY_INSTANCE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE_CONSULTATION
PRINT N'Add extend properties to PIMS_LEASE_CONSULTATION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Placeholder for descriptive text when "Describe Other" selected.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_CONSULTATION', 
	@level2type = N'Column', @level2name = N'OTHER_DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the relationship has been disabled.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_CONSULTATION', 
	@level2type = N'Column', @level2name = N'IS_DISABLED'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE_DOCUMENT
PRINT N'Add extend properties to PIMS_LEASE_DOCUMENT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the relationship has been disabled.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_DOCUMENT', 
	@level2type = N'Column', @level2name = N'IS_DISABLED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Defines the relationship betwwen a lease and a document.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_DOCUMENT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE_NOTE
PRINT N'Add extend properties to PIMS_LEASE_NOTE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the relationship has been disabled.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_NOTE', 
	@level2type = N'Column', @level2name = N'IS_DISABLED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Defines the relationship betwwen a lease and a note.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_NOTE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_LEASE_TENANT
PRINT N'Add extend properties to PIMS_LEASE_TENANT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Notes associated with the lease/tenant relationship.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_TENANT', 
	@level2type = N'Column', @level2name = N'NOTE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Associates a tenant with a lease' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_LEASE_TENANT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

---- Add extend properties to PIMS_PROPERTY_ADJACENT_LAND_TYPE
--PRINT N'Add extend properties to PIMS_PROPERTY_ADJACENT_LAND_TYPE'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Property adjacent land code.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ADJACENT_LAND_TYPE', 
--	@level2type = N'Column', @level2name = N'PROPERTY_ADJACENT_LAND_TYPE_CODE'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Property adjacent land code description.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ADJACENT_LAND_TYPE', 
--	@level2type = N'Column', @level2name = N'DESCRIPTION'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Indicates if the code is disabled.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ADJACENT_LAND_TYPE', 
--	@level2type = N'Column', @level2name = N'IS_DISABLED'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Force the display order of the codes.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ADJACENT_LAND_TYPE', 
--	@level2type = N'Column', @level2name = N'DISPLAY_ORDER'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Code table to describe property adjacent land type.' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ADJACENT_LAND_TYPE'
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

---- Add extend properties to PIMS_PROP_PROP_ADJACENT_LAND_TYPE
--PRINT N'Add extend properties to PIMS_PROP_PROP_ADJACENT_LAND_TYPE'
--GO
--EXEC sp_addextendedproperty 
--	@name = N'MS_Description', @value = N'Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ADJACENT_LAND_TYPE' , 
--	@level0type = N'Schema', @level0name = N'dbo', 
--	@level1type = N'Table', @level1name = N'PIMS_PROP_PROP_ADJACENT_LAND_TYPE'
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

-- Add extend properties to PIMS_PROPERTY_IMPROVEMENT
PRINT N'Add extend properties to PIMS_PROPERTY_IMPROVEMENT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the improvements' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_IMPROVEMENT', 
	@level2type = N'Column', @level2name = N'IMPROVEMENT_DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Size of the structure (house, building, bridge, etc,)' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_IMPROVEMENT', 
	@level2type = N'Column', @level2name = N'STRUCTURE_SIZE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Addresses affected' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_IMPROVEMENT', 
	@level2type = N'Column', @level2name = N'ADDRESS'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of property improvements associated with the lease.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_IMPROVEMENT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_PROPERTY_LEASE
PRINT N'Add extend properties to PIMS_PROPERTY_LEASE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Property/lease name' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_LEASE', 
	@level2type = N'Column', @level2name = N'NAME'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Leased area measurement' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_LEASE', 
	@level2type = N'Column', @level2name = N'LEASE_AREA'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_SECURITY_DEPOSIT
PRINT N'Add extend properties to PIMS_SECURITY_DEPOSIT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the deposit type If the SECURITY_DEPOSIT_TYPE_CODE has been chosen for this scurity deposit.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT', 
	@level2type = N'Column', @level2name = N'OTHER_DEPOSIT_TYPE_DESC'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Descirption of this security deposit' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT', 
	@level2type = N'Column', @level2name = N'DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Amount paid of this security deposit' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT', 
	@level2type = N'Column', @level2name = N'AMOUNT_PAID'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date of this security deposit' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT', 
	@level2type = N'Column', @level2name = N'DEPOSIT_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of a security deposit associated with a lease.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add extend properties to PIMS_SECURITY_DEPOSIT_RETURN
PRINT N'Add extend properties to PIMS_SECURITY_DEPOSIT_RETURN'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date the lease/license was terminated or surrendered' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN', 
	@level2type = N'Column', @level2name = N'TERMINATION_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Amount of claims against the deposit' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN', 
	@level2type = N'Column', @level2name = N'CLAIMS_AGAINST'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Amount returned minus claims' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN', 
	@level2type = N'Column', @level2name = N'RETURN_AMOUNT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date of deposit return' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN', 
	@level2type = N'Column', @level2name = N'RETURN_DATE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Interest paid on the deposit to the deposit holder' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN', 
	@level2type = N'Column', @level2name = N'INTEREST_PAID'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Describes the details of the return of a security deposit.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_SECURITY_DEPOSIT_RETURN'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
