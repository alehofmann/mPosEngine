Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("DCS.mPosEngine.Core")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("DCS.mPosEngine.Core")> 
<Assembly: AssemblyCopyright("Copyright © Microsoft 2014")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: log4net.Config.XmlConfigurator(Watch:=False)> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("69ffdd75-88c5-4096-be28-3d70b43ba396")>

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

<Assembly: AssemblyVersion("1.0.6.0")>
<Assembly: AssemblyFileVersion("1.0.6.9")>
'v1.0.1.0 Redondea todos los montos
'v1.0.1.1 ADminFunction tiene propiedad Enabled
'v1.0.2.0 Ahora Payitem es un valueobject, definido como tal en MPosTransaction
'v1.0.2.1 Agrega InvoiceNumber
'v1.0.2.3 Mejor logueo en la parte del calculo de taxes
'v1.0.3.0 Maneja tax para que funcionen tanto sales tax como VATs
'v1.0.4.0 Cambios en la forma de calcular subtotal (Issue 4964)
'v1.0.5.0 Usa Counterovement.GetConvertedAmount en lugar de .Amount
'v1.0.5.1 PosDevice ahora tiene como property el ComputerId de datStoreComputers
'v1.0.6.0 nHibernate 5 e Implementación de descuentos
'v1.0.6.2 Correcciones en SubtotalAmount, TaxAmount y Totaldiscount
'v1.0.6.3 Le saco el descuento a Subtotal
'v1.0.6.4 Se el envía mPosTransactionId como paymentId a CommitTransaction en ExternalSystem
'v1.0.6.5 Agrego ThumbUrl a AdminFunctions
'v1.0.6.6 Fiscal Interface
'v1.0.6.9 Refund voucher support