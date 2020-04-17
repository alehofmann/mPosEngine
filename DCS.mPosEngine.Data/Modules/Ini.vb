Namespace Infrastructure
	Module Ini
		Private Declare Function GetPrivateProfileString Lib "Kernel32.Dll" Alias "GetPrivateProfileStringA" _
			(ByVal sSection As String,
			 ByVal sKey As String,
			 ByVal sDefa As String,
			 ByVal sRetStr As String,
			 ByVal iSize As Integer,
			 ByVal sIniFile As String
			) As Integer

		Public Function GetValue(ByVal sSection As String, ByVal sKey As String, ByVal sDefault As String, ByVal sIni As String) As String
			Dim sb As String = Space(500)
			Dim iI As Integer

			iI = GetPrivateProfileString(sSection, sKey, sDefault, sb, sb.Length, sIni)

			Return Left(sb, iI)
		End Function
	End Module
End Namespace