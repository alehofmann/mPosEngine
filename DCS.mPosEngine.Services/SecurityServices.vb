Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Data
Imports DCS.mPosEngine.Core.DataInterfaces
Imports DCSSecurity
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCS.mPosEngine.Core.Domain

Namespace Dto
    Public Class SecurityServices
        'Private ReadOnly _securityEngine As DCSSecurity.AuthorizerEngine

        'Public Sub New()
        '    Try
        '        _securityEngine = New DCSSecurity.AuthorizerEngine
        '    Catch ex As Exception
        '        Throw New ApplicationException("Error Instancing Security engine", ex)
        '    End Try

        '    Try
        '        If Not _securityEngine.Create() Then
        '            Throw New ApplicationException("Security engine returned an error: " & _securityEngine.LastErrorString & " in line " & _securityEngine.LastErrorNumber)
        '        End If

        '    Catch ex As Exception
        '        Throw New ApplicationException("Error Creating Security engine", ex)
        '    End Try

        'End Sub
        'Public Function LoginCashier(ByVal loginMode As String, ByVal cardNumber As Integer) As LoginDataDto
        '    Dim retVal As New LoginDataDto
        '    Dim user As User
        '    Dim result As Boolean

        '        If cardNumber > 0 Then

        '            result = IsUserAllowed(1, cardNumber, user)

        '            If result Then
        '                retVal.CashierCardNumber = cardNumber
        '                retVal.CashierFirstName = user.FirstName
        '                retVal.CashierId = user.Id
        '                retVal.CashierLastName = user.LastName
        '                retVal.CashierLoginName = user.Handle
        '                retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginSuccess
        '            Else
        '                retVal.ResultCode = LoginDataDto.ResultCodesEnum.LoginDenied
        '            End If
        '        Else
        '            Throw New ApplicationException("cardNumber must be > 0")                    
        '        End If

        '    Return retVal
        'End Function

        'Public Function CheckAccess(ByVal actionId As Integer, ByVal userCardNumber As Integer) As CheckAccessResponseDto
        '    Dim retVal As New CheckAccessResponseDto
        '    Dim user As User

        '    retVal = New CheckAccessResponseDto(IsUserAllowed(actionId, userCardNumber, user))

        '    Return retVal

        'End Function

        'Public Function IsUserAllowed(ByVal actionId As Integer, ByVal userCardNumber As Integer, Optional ByRef user As User = Nothing) As Boolean            
        '    Dim retVal As Boolean            
        '    Dim userDao As IUserDao = (New NHibernateDaoFactory).GetUserDao
        '    Dim returnedUser As clsUser

        '    If userCardNumber <= 0 Then
        '        Throw New ArgumentException("Invalid CardNumber: " & userCardNumber, "userCardNumber")
        '    End If

        '    Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
        '        user = userDao.GetByCardNumber(userCardNumber)
        '        unitOfWork.Commit()
        '    End Using

        '    If user Is Nothing Then
        '        retVal = False
        '    Else                
        '        retVal = _securityEngine.IsUserPermitted(7, actionId, user.Handle, returnedUser, "", userCardNumber)
        '    End If


        '    Return retVal
        'End Function
    End Class
End Namespace
