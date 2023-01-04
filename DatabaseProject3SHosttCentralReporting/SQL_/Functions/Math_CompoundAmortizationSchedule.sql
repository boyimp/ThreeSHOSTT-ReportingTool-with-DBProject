CREATE FUNCTION [SQL#].[Math_CompoundAmortizationSchedule]
(@LoanAmount FLOAT (53) NULL, @AnnualInterestRate FLOAT (53) NULL, @YearsOfLoan INT NULL, @PaymentsPerYear INT NULL, @LoanStartDate DATETIME NULL, @OptionalExtraPayment FLOAT (53) NULL)
RETURNS 
     TABLE (
        [PaymentNum]           INT        NULL,
        [PaymentDate]          DATETIME   NULL,
        [BeginningBalance]     FLOAT (53) NULL,
        [ScheduledPayment]     FLOAT (53) NULL,
        [ExtraPayment]         FLOAT (53) NULL,
        [TotalPayment]         FLOAT (53) NULL,
        [Principal]            FLOAT (53) NULL,
        [Interest]             FLOAT (53) NULL,
        [EndingBalance]        FLOAT (53) NULL,
        [CumulativeInterest]   FLOAT (53) NULL,
        [TotalInterest]        FLOAT (53) NULL,
        [TotalPayments]        INT        NULL,
        [PaymentsLeft]         INT        NULL,
        [CumulativePrincipal]  FLOAT (53) NULL,
        [CumulativeAmountPaid] FLOAT (53) NULL,
        [TotalAmountPaid]      FLOAT (53) NULL)
AS
 EXTERNAL NAME [SQL#].[MATH].[CompoundAmortizationSchedule]


GO

