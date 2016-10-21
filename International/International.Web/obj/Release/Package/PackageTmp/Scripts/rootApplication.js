
var rootApplication = angular.module("rootApplication", ['comDirectiveApp', 'ui.bootstrap', 'multiselect']);

rootApplication.value('applicationValues', {});

rootApplication.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
}]);

const Status = {
    Blocked: 22,
    Bound: 23,
    Cancellation: 24,
    Declined: 26,
    Endorsement: 27,
    Indicated: 29,
    LostIndicated: 31,
    LostQuoted: 32,
    Quoted: 34,
    Reversal: 37,
    RenewalPending: 38,
    Working: 40,
    Closed: 25,
    ReEntry: 35
};

const ProductSubType = {
    BuilderRisk: 3,
    Property: 1,
    Construction : 492
}

const ProductType = {
    Property: 1,
}

const DirectAssumed = {
    Direct: "DIRECT",
    Assumed: "ASSUMED"
}

const SubmissionProcess =
    {
        CreateSubmission: 0,
        EditSubmission: 1,
        ViewSubmission: 2,
        CreateAmendment: 3,
        EditAmendment: 4,
        EditReEntry: 5
    }

rootApplication.constant('applicationConstants', {

    dateTimeFormat: "MMM-DD-YYYY HH:mm",
    dateFormat: "MMM-DD-YYYY",
    validationRules: [
        // common
        {
            controlIdentifier: 'DisableLostQuotedIndicated',
            status:
                  [Status.LostIndicated,
                   Status.LostQuoted,
                      Status.Blocked,
                      Status.Declined,
                      Status.Closed
                  ],
            disabled: true
        },
        {
            controlIdentifier: 'DisableLostCancellation',
            status:
                [Status.LostIndicated,
                Status.LostQuoted,
                    Status.Blocked,
                    Status.Declined,
                    Status.Closed,
                    Status.Cancellation
                ],
            disabled: true
        },
        {
            controlIdentifier: 'ExpiryDate',
            status:
                [Status.LostIndicated,
                Status.LostQuoted,
                    Status.Blocked,
                    Status.Declined,
                    Status.Closed                   
                ],
            disabled: true
        },
        {
            controlIdentifier: 'DisableLostCancellationEndorse',
            status:
                [Status.LostIndicated,
                Status.LostQuoted,
                    Status.Blocked,
                    Status.Declined,
                    Status.Closed,
                    Status.Cancellation,
                    Status.Endorsement,
                ],
            disabled: true
        },
         {
             controlIdentifier: 'DisableUnderwriter',
             status:
                   [
                       Status.Blocked,
                       Status.Declined,
                       Status.Closed,
                       Status.Endorsement,
                       Status.Cancellation
                   ],
             currentprocess:
               [
                   SubmissionProcess.CreateAmendment
               ],
             disabled: true
         },
            {
                controlIdentifier: 'DisableLostQuotedIndicatedEndorse',
                status:
                      [
                          Status.LostIndicated,
                          Status.LostQuoted,
                          Status.Endorsement,
                          Status.Blocked,
                          Status.Declined,
                          Status.Closed,
                           Status.Cancellation
                      ],
                disabled: true
            },
        //Basic Details Validations.....   
        {
            controlIdentifier: 'NewRenewalTypeCode',
            status:
                [
                    Status.Indicated,
                    Status.LostIndicated,
                    Status.Quoted,
                    Status.LostQuoted,
                    Status.Bound,
                    Status.Reversal,
                    Status.Endorsement,
                    Status.Blocked,
                    Status.Declined,
                    Status.Closed,
                    Status.ReEntry,
                      Status.Cancellation
                ],
            currentprocess:
               [
                   SubmissionProcess.CreateAmendment
               ],
            disabled: true
        },
        {
            controlIdentifier: 'SectionCodeId',
            status:
                  [
                      Status.Indicated,
                      Status.LostIndicated,
                      Status.Quoted,
                      Status.LostQuoted,
                      Status.Working,
                      Status.Bound,
                      Status.Endorsement,
                      Status.Cancellation,
                      Status.Reversal
                  ],
            mandatory: true, mandatoryMsg: 'Please select Section Code'
        },
          {
              controlIdentifier: 'ProfitCodeId',
              status:
                    [
                        Status.Bound,
                        Status.Endorsement,
                        Status.Cancellation,
                        Status.Reversal
                    ],
              mandatory: true, mandatoryMsg: 'Please select Profit Code'
          },
           {
               controlIdentifier: 'AttachmentTypeCode',
               status:
               [
                   Status.Blocked,
                   Status.Declined,
                   Status.Closed,
                   Status.Endorsement,
                   Status.Cancellation
               ],
               currentprocess:
                [
                    SubmissionProcess.CreateAmendment
                ],
               disabled: true
           },
          {
              controlIdentifier: 'AttachmentTypeCode',
              status:
                    [
                        Status.Bound,
                        Status.Endorsement,
                        Status.Cancellation,
                        Status.Reversal,
                        Status.ReEntry
                    ],
              mandatory: true, mandatoryMsg: 'Please select Attachment Type'
          },
          {
              controlIdentifier: 'PolicyTypeID',
              status:
              [
                        Status.Blocked,
                        Status.Declined,
                        Status.Closed,
                        Status.Endorsement,
                        Status.Cancellation
              ],
              currentprocess:
                [
                    SubmissionProcess.CreateAmendment
                ],
              disabled: true
          },
         {
             controlIdentifier: 'PolicyTypeID',
             status:
                   [
                       Status.Bound,
                       Status.Endorsement,
                       Status.Cancellation,
                       Status.Reversal,
                       Status.ReEntry
                   ],
             mandatory: true, mandatoryMsg: 'Please select Policy Type'
         },
         {
             controlIdentifier: 'CoverageID',
             status:
             [

               Status.Endorsement,
               Status.Blocked,
               Status.Declined,
               Status.Closed,
               Status.Cancellation

             ],
             currentprocess:
            [
                SubmissionProcess.CreateAmendment
            ],
             disabled: true
         },
         {
             controlIdentifier: 'CoverageID',
             status:
             [
                Status.Bound,
                Status.Endorsement,
                Status.Cancellation,
                Status.Reversal,
                  Status.ReEntry
             ],
             mandatory: true, mandatoryMsg: 'Please select Coverage Code'
         },
         {
             controlIdentifier: 'RenewalofPolicyNumber',
             status:
             [
             Status.Endorsement,
             Status.Blocked,
             Status.Declined,
             Status.Closed,
             Status.Cancellation,
             Status.LostIndicated,
             Status.LostQuoted,
             Status.Reversal
             ],
             currentprocess:
                 [
                     SubmissionProcess.CreateAmendment
                 ],
             disabled: true
         },
         {
             controlIdentifier: 'RenewalofPolicyNumber',
             status:
             [
                Status.Bound
             ],
             mandatory: true, mandatoryMsg: 'Please enter the Renewal of (Policy Number)'
         },


         // Project Details

                   {
                       controlIdentifier: 'Latitude',
                       status:
                       [

                           Status.LostIndicated,
                           Status.LostQuoted,
                           Status.Endorsement,
                           Status.Blocked,
                           Status.Declined,
                           Status.Closed,
                           Status.Cancellation
                       ],
                       disabled: true
                   },
                   {
                       controlIdentifier: 'Latitude',
                       producttype:
                       [
                           ProductSubType.BuilderRisk
                       ],
                       mandatory: true, mandatoryMsg: 'Please enter Latitude'
                   },
                   {
                       controlIdentifier: 'Longitude',
                       status:
                       [
                           Status.LostIndicated,
                           Status.LostQuoted,
                           Status.Endorsement,
                           Status.Blocked,
                           Status.Declined,
                           Status.Closed,
                           Status.Cancellation
                       ],
                       disabled: true
                   },
                   {
                       controlIdentifier: 'Longitude',

                       producttype:
                       [
                           ProductSubType.BuilderRisk
                       ],
                       mandatory: true, mandatoryMsg: 'Please enter Longitude'
                   },
                   // Insured Details 


                     {
                         controlIdentifier: 'DomicileCountryId',
                         status:
                         [
                            Status.Bound,
                            Status.Endorsement,
                            Status.Cancellation,
                            Status.Reversal,
                             Status.ReEntry
                         ],
                         mandatory: true, mandatoryMsg: 'Please select Domicile Country'
                     },
                     {
                         controlIdentifier: 'DomicileStateId',
                         status:
                         [
                            Status.Bound,
                            Status.Endorsement,
                            Status.Cancellation,
                            Status.Reversal,
                             Status.ReEntry
                         ],
                         mandatory: true, mandatoryMsg: 'Please select Domicile State'
                     },
                     {
                         controlIdentifier: 'AssumedEntityType',
                         status:
                         [
                             Status.Blocked,
                             Status.Blocked,
                             Status.Cancellation,
                             Status.Declined,
                             Status.Indicated,
                             Status.LostIndicated,
                             Status.LostQuoted,
                             Status.Quoted,
                             Status.Reversal,
                             Status.RenewalPending,
                             Status.Working
                         ],
                         disabled: true
                     },

            //Other Details....
         {
             controlIdentifier: 'By_Berk_SI_FROM_Broker',
             status:
                [Status.Blocked,
                 Status.Bound,
                 Status.Cancellation,
                 Status.Declined,
                 Status.Endorsement,
                 Status.Indicated,
                 Status.LostIndicated,
                 Status.LostQuoted,
                 Status.Quoted,
                 Status.Reversal,
                  Status.ReEntry,
                 Status.RenewalPending],
             disabled: true
         },
          {
              controlIdentifier: 'UnclearAccount',
              status:
                 [Status.Blocked,                
                  Status.Cancellation,
                  Status.Declined,
                  Status.Endorsement,
                  Status.Indicated,
                  Status.LostIndicated,
                  Status.LostQuoted,
                  Status.Quoted,
                  Status.Reversal,
                   Status.ReEntry,
                  Status.RenewalPending],
              disabled: true
          },
         {
             controlIdentifier: 'ProfitCentreOffice',
             status:
                [Status.Blocked,
                 Status.Cancellation,
                 Status.Declined,
                 Status.Endorsement,
                 Status.LostIndicated,
                 Status.LostQuoted,
                 Status.RenewalPending],
             disabled: true
         },
         {
             controlIdentifier: 'IssuingOffice',
             status:
                [Status.Blocked,
                 Status.Cancellation,
                 Status.Declined,
                 Status.Endorsement,
                 Status.LostIndicated,
                 Status.LostQuoted,
                 Status.RenewalPending],
             disabled: true
         },




         //{
         //    controlIdentifier: 'IssuingOffice',
         //    status:
         //          [Status.Working
         //          ],
         //    mandatory: true, mandatoryMsg: 'Please select Issuing Office'
         //},

        //Status Dependent Details..................
         {
             controlIdentifier: 'ReasonCode',
             status:
                [Status.Blocked,
                 Status.Bound,
                 Status.Cancellation,
                 Status.Endorsement,
                 Status.Indicated,
                 Status.Quoted,
                 Status.Reversal,
                 Status.RenewalPending,
                  Status.ReEntry,
                 Status.Working],
             disabled: true
         },
         {
             controlIdentifier: 'ProcessDate',
             status:
                   [Status.Working,
                   Status.Blocked,
                   Status.Declined,
                   Status.Indicated,
                   Status.LostIndicated,
                   Status.LostQuoted,
                   Status.Quoted,
                   Status.RenewalPending,
                   Status.Closed],
             disabled: true
         },
         //Premium Detail Fields...
         {
             controlIdentifier: 'ExchangeRateDate',
             status:
                [Status.Blocked,
                Status.Cancellation,
                Status.Declined,
                Status.LostIndicated,
                Status.LostQuoted,
                Status.RenewalPending,
                Status.Closed],

             disabled: true
         },
         {
             controlIdentifier: 'ExchangeRateDate',
             status:
                   [Status.Working,
                         Status.Indicated,
                         Status.Quoted,
                         Status.Declined,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Bound,
                         Status.Reversal,
                         Status.Cancellation,
                         Status.Endorsement,
                         Status.RenewalPending
                   ],
             mandatory: true, mandatoryMsg: 'Please Select Exchange rate as on'
         },
         {
             controlIdentifier: 'LayerPercent',
             status:
                  [Status.Working,
                   Status.LostIndicated,
                   Status.LostQuoted,
                   Status.Declined,
                   Status.Blocked,
                   Status.Closed
                  ],
             disabled: true
         },
          {
              controlIdentifier: 'LayerPercent',
              status:
                    [Status.Bound,
                     Status.Quoted,
                     Status.Indicated,
                     Status.Reversal,
                     Status.ReEntry,
                     Status.Endorsement,
                     Status.Cancellation
                    ],
              mandatory: true, mandatoryMsg: 'Please Enter (%) of Layer'
          },
         {
             controlIdentifier: 'PolicyCommissionPercent',
             status:
                   [Status.Working,
                    Status.LostIndicated,
                    Status.LostQuoted,
                    Status.Declined,
                    Status.Blocked,
                    Status.Closed
                   ],
             disabled: true
         },
          {
              controlIdentifier: 'PolicyCommissionPercent',
              status:
                    [Status.Bound,
                     Status.Reversal,
                     Status.ReEntry,
                     Status.Endorsement,
                     Status.Cancellation
                    ],
              mandatory: true, mandatoryMsg: 'Please Enter Policy Commission (%)'
          },
          {
              controlIdentifier: 'OriginalCurrencyCode',
              status:
                    [
                     Status.LostIndicated,
                     Status.LostQuoted,
                     Status.Declined,
                     Status.Blocked,
                     Status.Closed
                    ],
              disabled: true
          },
          {
              controlIdentifier: 'OriginalCurrencyCode',
              status:
                    [Status.Working,
                         Status.Indicated,
                         Status.Quoted,
                         Status.Declined,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Bound,
                         Status.Reversal,
                         Status.Cancellation,
                         Status.Endorsement,
                         Status.RenewalPending
                    ],
              mandatory: true, mandatoryMsg: 'Please Select Original Currency'
          },
          {
              controlIdentifier: 'Instalment',
              status:
                    [    Status.Bound,
                         Status.Reversal,
                         Status.Cancellation,
                         Status.Endorsement,
                         Status.ReEntry
                    ],
              mandatory: true, mandatoryMsg: 'Instalments (Yes/No) is required'
          },
          {
              controlIdentifier: 'OriginalLayerLimit',
              status:
                    [Status.Working,
                     Status.LostIndicated,
                     Status.LostQuoted,
                     Status.Declined,
                     Status.Blocked,
                     Status.Closed
                    ],
              disabled: true
          },
          {
              controlIdentifier: 'OriginalAttachmentPoint',
              status:
                    [Status.Working,
                     Status.LostIndicated,
                     Status.LostQuoted,
                     Status.Declined,
                     Status.Blocked,
                     Status.Closed
                    ],
              disabled: true
          },
          {
              controlIdentifier: 'TransactionalCurrencyCode',
              status:
                    [Status.LostIndicated,
                     Status.LostQuoted,
                     Status.Declined,
                     Status.Blocked,
                     Status.Closed
                    ],
              disabled: true
          },
           {
               controlIdentifier: 'TransactionalCurrencyCode',
               status:
                      [Status.Working,
                         Status.Indicated,
                         Status.Quoted,
                         Status.Declined,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Bound,
                         Status.Reversal,
                         Status.Cancellation,
                         Status.Endorsement,
                         Status.RenewalPending
                      ],
               mandatory: true, mandatoryMsg: 'Please Select Transaction Currency'
           },
           {
               controlIdentifier: 'ConversionRateToTransactional',
               status:
                     [Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                     Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
           {
               controlIdentifier: 'DisableLostQuotedIndicated',
               status:
                     [Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                      Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
           {
               controlIdentifier: 'ConversionRateToTransactional',
               status:
                     [
                      Status.Bound,
                      Status.Cancellation,
                      Status.Endorsement,
                      Status.Indicated,
                      Status.Quoted,
                      Status.Reversal,
                      Status.ReEntry
                     ],
               mandatory: true, mandatoryMsg: 'Please Enter Conversion Rate from Original to Transactional Currency'
           },
           {
               controlIdentifier: 'ConversionRateToJurisdictional',
               status:
                     [Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                     Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
           {
               controlIdentifier: 'ConversionRateToJurisdictional',
               status:
                     [
                      Status.Bound,
                      Status.Cancellation,
                      Status.Endorsement,
                      Status.Indicated,
                      Status.Quoted,
                      Status.Reversal,
                      Status.ReEntry
                     ],
               mandatory: true, mandatoryMsg: 'Please Enter Conversion Rate from Transactional to Jurisdictional Currency'
           },
           {
               controlIdentifier: 'ConversionRateToUSD',
               status:
                     [Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                     Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
           {
               controlIdentifier: 'ConversionRateToUSD',
               status:
                     [
                      Status.Bound,
                      Status.Cancellation,
                      Status.Endorsement,
                      Status.Indicated,
                      Status.Quoted,
                      Status.Reversal,
                      Status.ReEntry
                     ],
               mandatory: true, mandatoryMsg: 'Please Enter Conversion Rate from Transactional to USD Currency'
           },
           {
               controlIdentifier: 'TransactionalSIR',
               status:
                     [Status.Working,
                      Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                     Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
           {
               controlIdentifier: 'TransactionalDeductible',
               status:
                     [Status.Working,
                      Status.LostIndicated,
                      Status.LostQuoted,
                      Status.Declined,
                     Status.Blocked,
                     Status.Closed
                     ],
               disabled: true
           },
            {
                controlIdentifier: 'TransactionalGrossPremium',
                status:
                      [Status.Bound
                      ],
                min: '1', minMsg: 'Value can not be < 1'
            },
             {
                 controlIdentifier: 'TransactionalGrossPremium',
                 status:
                       [Status.Working,
                        Status.LostIndicated,
                        Status.LostQuoted,
                        Status.Reversal,
                        Status.Declined,
                        Status.Blocked,
                        Status.Closed
                       ],
                 disabled: true
             },
             {
                 controlIdentifier: 'TransactionalGrossPremium',
                 status:
                       [Status.Quoted,
                        Status.Indicated,
                        Status.Bound,
                        Status.Endorsement,
                        Status.Cancellation,
                        Status.Reversal,
                        Status.ReEntry
                       ],
                 mandatory: true, mandatoryMsg: ' Please Enter Gross Premium in Transactional Currency'
             },
             {
                 controlIdentifier: 'TransactionalCollections',
                 status:
                       [Status.Working,
                        Status.LostIndicated,
                        Status.LostQuoted,
                        Status.Declined,
                     Status.Blocked,
                     Status.Closed
                       ],
                 disabled: true
             },
              {
                  controlIdentifier: 'TransactionalDeductions',
                  status:
                        [Status.Working,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Declined,
                     Status.Blocked,
                     Status.Closed
                        ],
                  disabled: true
              },
               {
                   controlIdentifier: 'TransactionalGSTIN',
                   status:
                         [Status.Working,
                          Status.LostIndicated,
                          Status.LostQuoted,
                          Status.Declined,
                     Status.Blocked,
                     Status.Closed
                         ],
                   disabled: true
               },
               {
                   controlIdentifier: 'TransactionalGSTIN',
                   status:
                         [Status.Bound,
                          Status.Endorsement,
                          Status.Cancellation,
                          Status.Reversal,
                          Status.ReEntry
                         ],
                   mandatory: true, mandatoryMsg: 'Please Enter GST (IN) (Transactional Currency)'
               },
               {
                   controlIdentifier: 'TransactionalGSTOUT',
                   status:
                         [Status.Working,
                          Status.LostIndicated,
                          Status.LostQuoted,
                          Status.Declined,
                     Status.Blocked,
                          Status.Closed,

                         ],
                   disabled: true
               },
               {
                   controlIdentifier: 'TransactionalGSTOUT',
                   status:
                          [Status.Bound,
                          Status.Endorsement,
                          Status.Cancellation,
                          Status.Reversal,
                             Status.ReEntry
                          ],
                   mandatory: true, mandatoryMsg: 'Please Enter GST (OUT) (Transactional Currency)'
               },
                {
                    controlIdentifier: 'TransactionalTIV',
                    status:
                          [Status.LostIndicated,
                           Status.LostQuoted,
                           Status.Declined,
                     Status.Blocked,
                     Status.Closed
                          ],
                    disabled: true
                },
                  {
                      controlIdentifier: 'TransactionalTIV',
                      product:
                      [
                          ProductType.Property
                      ],
                      mandatory: true, mandatoryMsg: 'Please Enter Total Insured Value (TIV) in Transactional Currency'
                  },
                {
                    controlIdentifier: 'JurCurrencyCode',
                    status:
                          [Status.LostIndicated,
                           Status.LostQuoted,
                           Status.Reversal,
                           Status.Declined,
                     Status.Blocked,
                     Status.Closed
                          ],
                    disabled: true
                },
                 {
                     controlIdentifier: 'JurCurrencyCode',
                     status:
                            [Status.Working,
                               Status.Indicated,
                               Status.Quoted,
                               Status.Declined,
                               Status.LostIndicated,
                               Status.LostQuoted,
                               Status.Bound,
                               Status.Reversal,
                               Status.Cancellation,
                               Status.Endorsement,
                               Status.RenewalPending
                            ],
                     mandatory: true, mandatoryMsg: 'Please Select Jurisdictional Currency'
                 },
                {
                    controlIdentifier: 'PremimumType',
                    status:
                        [
                            Status.Cancellation,
                            Status.Endorsement
                        ],
                    mandatory: true, mandatoryMsg: 'Please select Premium Type'
                },

                //Policy Details...                
               {
                   controlIdentifier: 'BindDate',
                   status:
                        [Status.Blocked,
                         Status.Cancellation,
                         Status.Declined,
                         Status.Endorsement,
                         Status.Indicated,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Quoted,
                         Status.Reversal,
                         Status.RenewalPending,
                         Status.Working,
                        Status.Closed],
                   disabled: true
               },
               {
                   controlIdentifier: 'Renewable',
                   status:
                         [Status.Bound,
                          Status.Reversal,
                           Status.ReEntry,
                          Status.Endorsement,
                          Status.Cancellation,
                          Status.Working
                         ],
                   mandatory: true, mandatoryMsg: 'Please enter Renewal (Y/N)'
               },
               {
                   controlIdentifier: 'Renewable',
                   status:
                       [Status.Blocked,
                         Status.Cancellation,
                         Status.Declined,
                         Status.Endorsement,
                         Status.Indicated,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Quoted,
                         Status.RenewalPending,
                         Status.Working,
                        Status.Closed],
                   disabled: true
               },
               {
                   controlIdentifier: 'RenewalDate',
                   status:
                         [Status.Bound,
                          Status.Reversal,
                          Status.Endorsement,
                          Status.Cancellation,
                           Status.ReEntry
                         ],
                   mandatory: true, mandatoryMsg: 'Please enter Renewal Date'
               },
                {
                    controlIdentifier: 'RenewalDate',
                    status:
                        [Status.Blocked,
                         Status.Cancellation,
                         Status.Declined,
                         Status.Endorsement,
                         Status.Indicated,
                         Status.LostIndicated,
                         Status.LostQuoted,
                         Status.Quoted,
                         Status.RenewalPending,
                         Status.Working,
                        Status.Closed],
                    disabled: true
                },
                   {
                       controlIdentifier: 'EnableInBound',
                       status:
                            [Status.Blocked,
                             Status.Cancellation,
                             Status.Declined,
                             Status.Endorsement,
                             Status.Indicated,
                             Status.LostIndicated,
                             Status.LostQuoted,
                             Status.Quoted,
                             Status.Reversal,
                             Status.RenewalPending,
                             Status.Working,
                             Status.ReEntry,
                            Status.Closed],
                       disabled: true
                   },
                     {
                         controlIdentifier: 'Admitted',
                         status:
                              [Status.Blocked,
                               Status.Cancellation,
                               Status.Declined,
                               Status.Endorsement,
                               Status.Indicated,
                               Status.LostIndicated,
                               Status.LostQuoted,
                               Status.Quoted,
                               Status.Reversal,
                               Status.RenewalPending,
                               Status.Working,

                            Status.Closed],
                         disabled: true
                     },
                   {
                       controlIdentifier: 'LTACode',
                       status:
                            [Status.Blocked,
                             Status.Cancellation,
                             Status.Declined,
                             Status.Endorsement,
                             Status.Indicated,
                             Status.LostIndicated,
                             Status.LostQuoted,
                             Status.Quoted,
                             Status.RenewalPending,
                             Status.Working,
                            Status.Closed],
                       disabled: true
                   },
                   {
                       controlIdentifier: 'RiskCountry',
                       status:
                            [Status.Blocked,
                             Status.Cancellation,
                             Status.Declined,
                             Status.Indicated,
                             Status.LostIndicated,
                             Status.LostQuoted,
                             Status.Quoted,
                             Status.RenewalPending,
                             Status.Working,
                            Status.Closed],
                       disabled: true
                   },
                    {
                        controlIdentifier: 'OffOnShoreCode',
                        status:
                        [Status.Bound,
                         Status.Reversal,
                         Status.Endorsement,
                         Status.Cancellation,
                          Status.ReEntry
                        ],
                        mandatory: true, mandatoryMsg: 'Please select Offshore/Onshore'
                    },
                    {
                         controlIdentifier: 'OffOnShoreCode',
                         status:
                             [Status.LostIndicated,
                             Status.LostQuoted,
                                 Status.Blocked,
                                 Status.Declined,
                                 Status.Closed,
                                 Status.Cancellation
                             ],
                        disabled: true
                    }
    ],

    appconstant: { fixLength: 50 },
    enums: {
        CountryEnum: {
            CountryName: 1,
            CountryCode: 2,
            ISOCode: 3
        },
        CurrentStatusEnum: {
            Blocked: 22,
            Bound: 23,
            Cancellation: 24,
            Declined: 26,
            Endorsement: 27,
            Indicated: 29,
            LostIndicated: 31,
            LostQuoted: 32,
            Quoted: 34,
            Reversal: 37,
            RenewalPending: 38,
            Working: 40,
            ReEntry: 35,
            Closed: 25
        },
        Right:
        {
            None: 0,
            View: 1,
            Edit: 2,
            Delete: 3
        },
        Newrenewal:
            {
                New: "NEWRENEWAL_N",
                Renew: "NEWRENEWAL_R"
            },
        SubmissionProcess:
            {
                CreateSubmission: 0,
                EditSubmission: 1,
                ViewSubmission: 2,
                CreateAmendment: 3,
                EditAmendment: 4,
                EditReEntry: 5,
                ViewReEntry: 6,
                ViewReversal: 7,
                SubmissionQC: 8,
                AmendmentQC: 9,
                ViewAmendment: 10
            },
        DirectAssumed: {
            Direct: "DIRECT",
            Assumed: "ASSUMED"
        },
        Lob: {
            Casualty: 2,
            Healthcare: 4
        },
        AdmittedTypeCode: {

        },
        InsuredSearch:
        {
            Insured: 0,
            ChildInsured: 1,
            InsuredAddress: 2
        },
        PremimumType: {
            AdditionalPremium: "PT_AP",
            ReturnPremium: "PT_RP"
        }

    }
});



rootApplication.run(['$rootScope', 'applicationConstants', function ($rootScope, applicationConstants) {
    $rootScope.enums = applicationConstants.enums;
    $rootScope.appconstant = applicationConstants.appconstant;
}]);

