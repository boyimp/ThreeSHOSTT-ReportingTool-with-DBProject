CREATE FUNCTION [SQL#].[Math_CompoundAmortizationSchedule2]
(@LoanAmount MONEY NULL, @AnnualInterestRate DECIMAL (8, 5) NULL, @YearsOfLoan INT NULL, @PaymentsPerYear INT NULL, @LoanStartDate DATETIME NULL, @OptionalExtraPayment MONEY NULL, @UseStandardRounding BIT NULL)
RETURNS 
     TABLE (
        [PaymentNum]           INT      NULL,
        [PaymentDate]          DATETIME NULL,
        [BeginningBalance]     MONEY    NULL,
        [ScheduledPayment]     MONEY    NULL,
        [ExtraPayment]         MONEY    NULL,
        [TotalPayment]         MONEY    NULL,
        [Principal]            MONEY    NULL,
        [Interest]             MONEY    NULL,
        [EndingBalance]        MONEY    NULL,
        [CumulativeInterest]   MONEY    NULL,
        [TotalInterest]        MONEY    NULL,
        [TotalPayments]        INT      NULL,
        [PaymentsLeft]         INT      NULL,
        [CumulativePrincipal]  MONEY    NULL,
        [CumulativeAmountPaid] MONEY    NULL,
        [TotalAmountPaid]      MONEY    NULL)
AS
 EXTERNAL NAME [SQL#].[MATH].[CompoundAmortizationSchedule2]


GO

