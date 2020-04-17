Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("mPosEngine")> 
<Assembly: AssemblyDescription("Mobile Point of Sale Backend Service")> 
<Assembly: AssemblyCompany("Alex Hofmann")> 
<Assembly: AssemblyProduct("mPosEngine")> 
<Assembly: AssemblyCopyright("Copyright © Alex Hofmann 2014")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: log4net.Config.XmlConfigurator(Watch:=False)> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("8f07a1de-ed3c-481b-b312-f627089d418f")>

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

<Assembly: AssemblyVersion("1.1.4.1")>
<Assembly: AssemblyFileVersion("1.1.4.1")>
'v1042 isDefaultCardSaleProduct
'v1044 Redondea los montos que van a la DB.
'v1046 Payitem es un valueobject, l
'v1047 Implementa InvoiceNumber
'v1050 Agrega AuthorizePayment
'v1060 Agrega GetProductPages
'v1062 Arregla lo del Tax
'v1063 Arregla lo del Tax otra vez (ver services 1.0.3.3)
'v1064 services v1.0.3.4
'v1065 PlaycardBase v1.1.0.0
'v1070 Usa el PlaycardBase que permite tener prefixletter/number alfanumericos ambos
'v1071 'Services v1.0.3.7 Usa Playcardbase 1.1.4.0
'v1072
'Core 'v1.0.2.3 Mejor logueo en la parte del calculo de taxes
'v1080 Core 'v1.0.3.0 Maneja tax para que funcionen tanto sales tax como VATs
'v1081 Core 'v1.0.4.0 Cambios en la forma de calcular subtotal/taxes (Issue 4964)
'v1082 Core 'v1.0.4.1 Corrije error de la anterior que devolvia subtotal1 en 0 (Issue 4832)
'v1083 Data 'v1.0.0.4 No estaba implementado el "ChangeCardStatus", por lo cual productos que ponene en VIP la tarjeta no lo estaban haciendo
'v1090 Nuevo Playcardbase/PlaycardStatusEngine
'v1091 Core v1.0.5.0 -> Usa Counterovement.GetConvertedAmount en lugar de .Amount
'v1092 CheckDevice devuelve el computerId (de datStoreComputers)
'	1.1.0.0	nHibernate 5, implementación de descuentos y nuevo método GetDiscountsFromProducts
'	1.1.0.1 Try-catch en New Service
'	1.1.0.2 Se cambia el método GetDiscountsFromProducts para calcular en base a cantidades
'	1.1.0.3 Se envía mPosTransactionId como paymentId en CommitTransaction de ExternalSystem
'	1.1.0.4 Agrego ThumbUrl a AdminFunctions
'	1.1.0.5 Actualizo referencias
'	1.1.1.0 Agrego funcionalidad para facturación fiscal
'	1.1.2.0 Se agregan las funciones de treasury Cash Pull, Pay Out y Get Balance
'	1.1.2.1 Cash pull y pay out ahora devuelven SuccessResponse
'	1.1.2.2 Agrego validación contra valid.dat
'	1.1.2.3 Cambio en el response de GetBalance
'	1.1.2.4 Tomo el prefijo desde [International] en vez de [Taskman]
'	1.1.2.5 Bug al leer valid.dat
'   1.1.3.0 CreditCard support
'   1.1.3.1 Correcciones a CreditCard. Se agregan los métodos ReturnCreditCardTransaction, StartCreditCardAuthorization
'   1.1.3.2 Se cambia la respuesta de StartCreditCardAuthorizer
'   1.1.3.3 Agrego CardType y CreditCardNumber a GetTransactionStatus
'   1.1.3.4 Agrego InvoiceId en ReturnCreditCardTransaction
'   1.1.3.5 CardEngine 1.0.8.0 con PassportEngine 4.2.1.1
'   1.1.3.6 Se cambió referencias de todo
'   1.1.3.8 Refund voucher support
'   1.1.3.9 Nuevo endpoint: GetInvoiceImage
'   1.1.4.0 Agrego PlayAmountToPromote en AnalyzeCard
'   1.1.4.1 Fix en CommitTransaction