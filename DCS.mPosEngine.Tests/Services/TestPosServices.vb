Imports DCS.mPosEngine.Services.Dto
Imports NUnit.Framework
Imports DCS.mPosEngine.Services

Namespace Services
    <TestFixture()> _
    Public Class TestPosServices
        Private _posServices As New PosServices
        'Private _posServices As New PosServices
        Dim _productServices As New ProductServices
        <Test()> _
        Public Sub TestLoginSuccess()
            Dim response As LoginDataDto

            response = _posServices.LoginCashierOnlyForTests("11", 3)

            Assert.IsTrue(response.ResultCode = LoginDataDto.ResultCodesEnum.LoginSuccess)

        End Sub

        <Test()>
        Public Sub TestLoginFailedNoOpenSession()
            Dim response As LoginDataDto

            response = _posServices.LoginCashierOnlyForTests("11", 3)

            Assert.IsTrue(response.ResultCode = LoginDataDto.ResultCodesEnum.LoginDeniedNoOpenTreasurySession)

        End Sub

        <Test()>
        Public Sub TestLoginFailedInvalidCashierId()
            Dim response As LoginDataDto

            response = _posServices.LoginCashierOnlyForTests("11", 3)

            Assert.IsTrue(response.ResultCode = LoginDataDto.ResultCodesEnum.LoginInvalidTreasuryCashierId)

        End Sub

        <Test()> _
        Public Sub TestGetProducts()
            log4net.Config.XmlConfigurator.Configure()

            Dim cardNumber As Integer = 6621952
            Dim products As IList(Of ProductDto)

            products = _productServices.GetProducts(cardNumber)
        End Sub

        <Test()> _
        Public Sub TestGetProductPages()
            Dim cardNumber As Integer = 6621952
            Dim posId As String = "VERUGO"
            Dim response As GetProductPagesResponseDto

            response = _productServices.GetProductPages(posId, cardNumber)

            Assert.IsNotNull(response.CardProduct)
            Assert.IsTrue(response.CardProduct.Id = 1)

        End Sub

        <Test()> _
        Public Sub TestSuccessfulRegisterDevice()
            Dim response As RegisterDeviceResponseDto

            response = _posServices.RegisterDevice("dk3k3k3k434239993934j393939393443j3", "123456")

            Assert.IsTrue(response.ResultCode = CheckDeviceResponseDto.ResultCodesEnum.DeviceValidAndEnabled)

            response = _posServices.RegisterDevice(9876, "123")
            Assert.IsTrue(response.ResultCode = RegisterDeviceResponseDto.ResultCodesEnum.SerialAlreadyRegistered)

        End Sub

        <Test()> _
        Public Sub TestGetPosConfig()
            Dim response1 As IList(Of PosConfigDto)
            Dim response2 As IList(Of PosConfigDto)

            response1 = _posServices.GetPosConfig("VERUGO")
            response2 = _posServices.GetPosConfig("sSDSS")

            Assert.IsNotNull(response1)
            Assert.IsNotNull(response2)

            Assert.IsTrue(response1.Count > 0)
            Assert.IsTrue(response2.Count > 0)

            Assert.IsTrue(response1.Count > response2.Count)
        End Sub
    End Class
End Namespace
