Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("DCS.mPosEngine.Data")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Microsoft")> 
<Assembly: AssemblyProduct("DCS.mPosEngine.Data")> 
<Assembly: AssemblyCopyright("Copyright © Microsoft 2014")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("1652f9c8-4d71-4bb9-a625-47ebd386c024")>

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

<Assembly: AssemblyVersion("1.0.1.0")>
<Assembly: AssemblyFileVersion("1.0.1.9")>

'v1.0.0.1 AdminFunctionDao toma en cuenta propiedad Enabled
'v1.0.0.3 Usa playcardbase 1.1.4.0
'v1.0.0.4 No estaba implementado el "ChangeCardStatus", por lo cual productos que ponene en VIP la tarjeta no lo estaban haciendo
'v1.0.1.0 nHibernate 5 e Implementación de descuentos
'v1.0.1.2 Agrego ThumbUrl a AdminFunctions
'v1.0.1.3 Fiscal Interface
'v1.0.1.5 Agrego validación contra valid.dat
'v1.0.1.6 Tomo el prefijo de [International] en vez de [Taskman]
'v1.0.1.7 Corrijo bug al leer el Valid.Dat, si había espacio o salto de línea al final no funcaba
'v1.0.1.9 Refund voucher support