Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("DCS.mPosEngine.Services")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("DCS.mPosEngine.Services")> 
<Assembly: AssemblyCopyright("Copyright © Microsoft 2014")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("972bc4dd-91b1-471b-bf67-7b13d47e4edd")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("1.1.0.0")>
<Assembly: AssemblyFileVersion("1.1.1.0")>

'v1.0.0.3 Payitem es un valueobject
'v1.0.1.0 Ahora el GetTransactionInfo devuelve un InvoiceNumber y el CommitTransaction lo recibe para almacenarlo
'v1.0.2.0 Agrega AuthorizePayment
'v1.0.3.0 Agrega GetProductPages
'v1.0.3.1 No devolvia TaxInfo
'v1.0.3.3 Usa playcardbase.core que lee correctamente los Taxes por product. A su vez dejo anotado que los cambios en playcardbase.core solo fueron en el mapping file de product.hbm.xml donde se mejora el custom query que ejecuta
'v1.0.3.4 Devolvia courtesy en el campo tickets
'v1.0.3.7 Usa Playcardbase 1.1.4.0
'v1.0.4.0 'Core 'v1.0.3.0 Maneja tax para que funcionen tanto sales tax como VATs
'v1.0.5.0 Nuevo Playcardbase/PlaycardStatusEngine
'v1.0.6.0 nHibernate 5 e Implementación de descuentos
'v1.0.6.2 Corrijo bugs
'v1.0.6.3 Cambio la forma de calcular los descuentos en GetDiscountFromProducts
'v1.0.6.4 Bug en GetDiscountsFromProducts
'v1.0.6.6 No trato de mostrar los registros null en CardHistory
'v1.0.7.0 GetDiscountFromProducts -> Ahora acepta productos de externalSystem
'v1.0.7.7 Se el envía mPosTransactionId como paymentId a CommitTransaction en ExternalSystem
'v1.0.7.8 Agrego ThumbUrl a AdminFunctions
'v1.0.7.9 Actualizo referencias -> PosEngine.CoreAndData, DCS.TabEngine.Core y DCS.TabEngine.Data
'v1.0.8.0 Fiscal interface
'v1.0.9.0 Se agrega TreasuryServices.vb con funciones para Cash Pull, Pay Out y Get Balance
'v1.0.9.1 Cambios en los DTO de CashItem y TreasuryBalance
'v1.0.9.2 Cambios en PaymentService y PurchasingService
'v1.0.9.3 Cambio respuesta a StarCreditCardAuthorizer
'v1.0.9.4 Agrando unitofwork en GetProductPages
'v1.0.9.5 Cambios en GetProducts
'v1.0.9.6 Agrego CardType y CreditCardNumber a la respuesta
'v1.0.9.7 Agrego InvoiceId en ReturnCreditCardTransaction
'v1.0.9.8 ReturnCreditCardTransaction solo permite montos negativos
'v1.0.9.9 Refund voucher support
'v1.1.0.0 Agrego InvoiceImageResponseDto y PosServices.GetInvoiceImage
'v1.1.0.1 Agrego PlayAmountToPromote
'v1.1.0.2 Cambios en RedeemVoucher y CommitTransaction, continuaba la operación si no podía redimir algún voucher
'v1.1.1.0 (AleH) Muchos cambiecitos!