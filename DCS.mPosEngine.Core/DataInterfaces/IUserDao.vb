Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface IUserDao
        Inherits IGenericDao(Of User, Integer)

        Function GetByCardNumber(ByVal cardNumber As Long) As User

    End Interface
End Namespace
