PRINT N'Adding [PIMS_AGENCY]'

-- Parent Agencies.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
    , [EMAIL]
    , [ADDRESS_TO]
) VALUES (
    1
    , 'AEST'
    , 'Ministry of Advanced Education, Skills & Training'
    , 0
    , 1
    , 'kevin.brewster@gov.bc.ca'
    , 'ADM and EFO Finance, Technology and Management Services'
), (
    2
    , 'CITZ'
    , 'Ministry of Citizens Services'
    , 0
    , 1
    , 'dean.skinner@gov.bc.ca'
    , 'ADM and EFO CSD'
), (
    4
    , 'EDUC'
    , 'Ministry of Education'
    , 0
    , 1
    , 'reg.bawa@gov.bc.ca'
    , 'ADM Resource Management Division'
), (
    5
    , 'FIN'
    , 'Ministry of Finance'
    , 0
    , 1
    , 'teri.spaven@gov.bc.ca'
    , 'ADM & EFO Corporate Services Division'
), (
    6
    , 'FLNR'
    , 'Ministry of Forests, Lands, Natural Resources'
    , 0
    , 1
    , 'trish.dohan@gov.bc.ca'
    , 'ADM CSNR and EFO for FLNR'
), (
    7
    , 'HLTH'
    , 'Ministry of Health'
    , 0
    , 1
    , 'philip.twyford@gov.bc.ca'
    , 'ADM & EFO Finance and Corporate Services'
), (
    8
    , 'MAH'
    , 'Ministry of Municipal Affairs & Housing'
    , 0
    , 1
    , 'david.curtis@gov.bc.ca'
    , 'ADM & EFO Management Services Division'
), (
    9
    , 'TRAN'
    , 'Ministry of Transportation and Infrastructure'
    , 0
    , 1
    , 'nancy.bain@gov.bc.ca'
    , 'ADM & EFO Finance & Management Services Department'
), (
    10
    , 'EMPR'
    , 'Energy, Mines & Petroleum Resources'
    , 0
    , 1
    , 'wes.boyd@gov.bc.ca'
    , 'ADM  CSNR and EFO to MIRR, AGRI EMPR ENV'
), (
    11
    , 'MAG'
    , 'Ministry of Attorney General'
    , 0
    , 1
    , 'tracy.campbell@gov.bc.ca'
    , 'ADM & EFO CMSB AG and PSSG'
), (
    12
    , 'JEDC'
    , 'Jobs, Economic Development & Competitiveness'
    , 0
    , 1
    , 'joanna.white@gov.bc.ca'
    , 'A/ADM & EFO Management Services Division'
), (
    13
    , 'MTAC'
    , 'Ministry of Tourism, Arts and Culture'
    , 0
    , 1
    , 'salman.azam@gov.bc.ca'
    , 'ADM & EFO Management Services Division'
), (
    14
    , 'SDPR'
    , 'Ministry of of Social Development and Poverty Reduction'
    , 0
    , 1
    , 'jonathan.dube@gov.bc.ca'
    , 'ADM & EFO CSD'
), (
    15
    , 'MMHA'
    , 'Ministry of Mental Health and Addictions'
    , 0
    , 1
    , 'dara.landry@gov.bc.ca'
    , 'Executive Lead Corporate Services'
), (
    16
    , 'MCFD'
    , 'Ministry of Children and Family Development'
    , 0
    , 1
    , 'rob.byers@gov.bc.ca'
    , 'AMD & EFO'
), (
    17
    , 'BCPSA'
    , 'Public Service Agency'
    , 0
    , 1
    , 'bruce.richmond@gov.bc.ca'
    , 'ADM Corporate Services'
)

-- Child Agencies for HLTH.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    20
    , 7
    , 'FHA'
    , 'Fraser Health Authority'
    , 0
    , 0
), (
    21
    , 7
    , 'IHA'
    , 'Interior Health Authority'
    , 0
    , 0
), (
    22
    , 7
    , 'NHA'
    , 'Northern Health Authority'
    , 0
    , 0
), (
    23
    , 7
    , 'PHSA'
    , 'Provincial Health Services Authority'
    , 0
    , 0
), (
    24
    , 7
    , 'VCHA'
    , 'Vancouver Coastal Health Authority'
    , 0
    , 0
), (
    25
    , 7
    , 'VIHA'
    , 'Vancouver Island Health Authority'
    , 0
    , 0
)

-- Child Agencies for EMPR.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    30
    , 10
    , 'BCH'
    , 'BC Hydro'
    , 0
    , 0
)

-- Child Agencies for MAH.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    40
    , 8
    , 'BCH'
    , 'BC Housing'
    , 0
    , 0
), (
    41
    , 8
    , 'BCA'
    , 'BC Assessment'
    , 0
    , 0
), (
    42 -- Added after release.
    , 8
    , 'PRHC'
    , 'Provincial Rental Housing Corporation'
    , 0
    , 0
)

-- Child Agencies for MAG.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    50
    , 11
    , 'ICBC'
    , 'Insurance Coporation of BC'
    , 0
    , 0
),(
    51 -- Added after release.
    , 11
    , 'LDB'
    , 'BC Liquor Distribution Branch'
    , 0
    , 0
)

-- Child Agencies for JEDC.
--INSERT INTO dbo.[PIMS_AGENCY] (
--    [AGENCY_ID]
--    , [PARENT_AGENCY_ID]
--    , [CODE]
--    , [NAME]
--    , [IS_DISABLED]
--    , [SEND_EMAIL]
--) VALUES (
--    50
--    , 12
--    , 'BCPC'
--    , 'BC Pavillion Corporation'
--    , 0
--    , 0
--)

-- Child Agencies for EDUC.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    70
    , 4
    , 'CMB'
    , 'Capital Management Branch'
    , 0
    , 0
)

-- Child Agencies for AEST.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    80
    , 1
    , 'BCIT'
    , 'British Colubmia Institute of Technology'
    , 0
    , 0
), (
    81
    , 1
    , 'CAMC'
    , 'Camosun College'
    , 0
    , 0
), (
    82
    , 1
    , 'CAPU'
    , 'Capilano University'
    , 0
    , 0
), (
    83
    , 1
    , 'CNC'
    , 'College of New Caledonia'
    , 0
    , 0
), (
    84
    , 1
    , 'CROCK'
    , 'College of the Rockies'
    , 0
    , 0
), (
    85
    , 1
    , 'DC'
    , 'Douglas College'
    , 0
    , 0
), (
    86
    , 1
    , 'ECUAD'
    , 'Emily Carr University of Art and Design'
    , 0
    , 0
), (
    87
    , 1
    , 'JIBC'
    , 'Justice Institute of BC'
    , 0
    , 0
), (
    88
    , 1
    , 'KP'
    , 'Kwantlen Polytechnic'
    , 0
    , 0
), (
    89
    , 1
    , 'LC'
    , 'Langara College'
    , 0
    , 0
), (
    90
    , 1
    , 'NVIT'
    , 'Nichola Valley Institute of Technology'
    , 0
    , 0
), (
    91
    , 1
    , 'NLC'
    , 'Northern Lights College'
    , 0
    , 0
), (
    92
    , 1
    , 'CMC'
    , 'Coast Mountain College'
    , 0
    , 0
), (
    93
    , 1
    , 'OC'
    , 'Okanagan College'
    , 0
    , 0
), (
    94
    , 1
    , 'SC'
    , 'Selkirk College'
    , 0
    , 0
), (
    95
    , 1
    , 'SFU'
    , 'Simon Fraser University'
    , 0
    , 0
), (
    96
    , 1
    , 'TRU'
    , 'Thompson Rivers University'
    , 0
    , 0
), (
    97
    , 1
    , 'UBC'
    , 'University of BC'
    , 0
    , 0
), (
    98
    , 1
    , 'UFV'
    , 'University of the Fraser Valley'
    , 0
    , 0
), (
    99
    , 1
    , 'UNBC'
    , 'University of Northern BC'
    , 0
    , 0
), (
    100
    , 1
    , 'UVIC'
    , 'University of Victoria'
    , 0
    , 0
), (
    101
    , 1
    , 'VCC'
    , 'Vancouver Community College'
    , 0
    , 0
), (
    102
    , 1
    , 'VIU'
    , 'Vancouver Island University'
    , 0
    , 0
)

-- Child Agencies for CITZ.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    110
    , 2
    , 'RPD'
    , 'Real Property Division'
    , 0
    , 0
)

-- Child Agencies for FLNR.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    120
    , 6
    , 'CLO'
    , 'Crown Land Opportunities'
    , 0
    , 0
)

-- Child Agencies for TRAN.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    130
    , 9
    , 'PLMB'
    , 'Properties and Land Management Branch'
    , 0
    , 0
),(
    131
    , 9
    , 'BCT'
    , 'BC Transit'
    , 0
    , 0
),(
    132 -- Added after release.
    , 9
    , 'BCTFA'
    , 'BC Transportation Financing Authority'
    , 0
    , 0
)

-- Child Agencies for MTAC.
INSERT INTO dbo.[PIMS_AGENCY] (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
) VALUES (
    140
    , 13
    , 'PAVCO'
    , 'BC Pavillion Corporation'
    , 0
    , 0
)

MERGE INTO dbo.[PIMS_AGENCY] dest
USING (
    -- Child Agencies for MAH.
    VALUES (
        42
        , 8
        , 'PRHC'
        , 'Provincial Rental Housing Corporation'
        , 0
        , 0
    )

    -- Child Agencies for MAG.
    , (
        51
        , 11
        , 'LDB'
        , 'BC Liquor Distribution Branch'
        , 0
        , 0
    )

    -- Child Agencies for EDUC.
    , (
        150
        , 4
        , 'SD 06'
        , 'School District 06'
        , 0
        , 0
    ), (
        151
        , 4
        , 'SD 19'
        , 'School District 19'
        , 0
        , 0
    ), (
        152
        , 4
        , 'SD 20'
        , 'School District 20'
        , 0
        , 0
    ), (
        153
        , 4
        , 'SD 22'
        , 'School District 22'
        , 0
        , 0
    ), (
        154
        , 4
        , 'SD 23'
        , 'School District 23'
        , 0
        , 0
    ), (
        155
        , 4
        , 'SD 28'
        , 'School District 28'
        , 0
        , 0
    ), (
        156
        , 4
        , 'SD 33'
        , 'School District 33'
        , 0
        , 0
    ), (
        157
        , 4
        , 'SD 35'
        , 'School District 35'
        , 0
        , 0
    ), (
        158
        , 4
        , 'SD 36'
        , 'School District 36'
        , 0
        , 0
    ), (
        159
        , 4
        , 'SD 37'
        , 'School District 37'
        , 0
        , 0
    ), (
        160
        , 4
        , 'SD 38'
        , 'School District 38'
        , 0
        , 0
    ), (
        161
        , 4
        , 'SD 40'
        , 'School District 40'
        , 0
        , 0
    ), (
        162
        , 4
        , 'SD 41'
        , 'School District 41'
        , 0
        , 0
    ), (
        163
        , 4
        , 'SD 42'
        , 'School District 42'
        , 0
        , 0
    ), (
        164
        , 4
        , 'SD 43'
        , 'School District 43'
        , 0
        , 0
    ), (
        165
        , 4
        , 'SD 44'
        , 'School District 44'
        , 0
        , 0
    ), (
        166
        , 4
        , 'SD 47'
        , 'School District 47'
        , 0
        , 0
    ), (
        167
        , 4
        , 'SD 62'
        , 'School District 62'
        , 0
        , 0
    ), (
        168
        , 4
        , 'SD 63'
        , 'School District 63'
        , 0
        , 0
    ), (
        169
        , 4
        , 'SD 68'
        , 'School District 68'
        , 0
        , 0
    ), (
        170
        , 4
        , 'SD 70'
        , 'School District 70'
        , 0
        , 0
    ), (
        171
        , 4
        , 'SD 71'
        , 'School District 71'
        , 0
        , 0
    ), (
        172
        , 4
        , 'SD 72'
        , 'School District 72'
        , 0
        , 0
    ), (
        173
        , 4
        , 'SD 73'
        , 'School District 73'
        , 0
        , 0
    ), (
        174
        , 4
        , 'SD 75'
        , 'School District 75'
        , 0
        , 0
    ), (
        175
        , 4
        , 'SD 78'
        , 'School District 78'
        , 0
        , 0
    ), (
        176
        , 4
        , 'SD 79'
        , 'School District 79'
        , 0
        , 0
    ), (
        177
        , 4
        , 'SD 83'
        , 'School District 83'
        , 0
        , 0
    ), (
        178
        , 4
        , 'SD 91'
        , 'School District 91'
        , 0
        , 0
    )

    -- Child Agencies for TRAN.
    , (
        132
        , 9
        , 'BCTFA'
        , 'BC Transportation Financing Authority'
        , 0
        , 0
    )
) AS src (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [CODE]
    , [NAME]
    , [IS_DISABLED]
    , [SEND_EMAIL]
)
ON dest.[AGENCY_ID] = src.[AGENCY_ID]
WHEN MATCHED THEN
    UPDATE
        SET
            dest.[PARENT_AGENCY_ID] = src.[PARENT_AGENCY_ID]
            , dest.[CODE] = src.[CODE]
            , dest.[NAME] = src.[NAME]
            , dest.[IS_DISABLED] = src.[IS_DISABLED]
            , dest.[SEND_EMAIL] = src.[SEND_EMAIL]
WHEN NOT MATCHED THEN
    INSERT (
        [AGENCY_ID]
        , [PARENT_AGENCY_ID]
        , [CODE]
        , [NAME]
        , [IS_DISABLED]
        , [SEND_EMAIL]
    ) VALUES (
        src.[AGENCY_ID]
        , src.[PARENT_AGENCY_ID]
        , src.[CODE]
        , src.[NAME]
        , src.[IS_DISABLED]
        , src.[SEND_EMAIL]
    );

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_AGENCY_ID_SEQ]
RESTART WITH 200
