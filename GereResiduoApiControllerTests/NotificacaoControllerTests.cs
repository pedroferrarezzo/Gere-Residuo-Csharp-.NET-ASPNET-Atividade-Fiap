using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Utils.Email;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class NotificacaoControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly NotificacaoController _notificacaoController;
        private readonly DbSet<NotificacaoModel> _mockDbSetNotificacao;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;
        private readonly ISmtpUtils _smtpUtils;

        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetNotificacao = MockDbSetNotificacao();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();
            _smtpUtils = new SmtpUtils("", "", "", 0, "");

            _notificacaoRepository = new NotificacaoRepository(_mockDatabaseContext.Object);
            _notificacaoService = new NotificacaoService(_notificacaoRepository, _smtpUtils);

            _notificacaoController = new NotificacaoController(
                _notificacaoService,
                _autoMapper,
                _errorViewModel);

            _mockDatabaseContext.Setup(m => m.TabelaNotificacao).Returns(_mockDbSetNotificacao);
        }

        private DbSet<NotificacaoModel> MockDbSetNotificacao()
        {

            var data = new List<NotificacaoModel>
            {
                new NotificacaoModel {
                    NotificacaoId = 1,
                    AgendaId = 1
                },
                new NotificacaoModel {
                    NotificacaoId = 2,
                    AgendaId = 2
                },
                new NotificacaoModel {
                    NotificacaoId = 3,
                    AgendaId = 1
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<NotificacaoModel>>();
            mockDbSet.As<IQueryable<NotificacaoModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NotificacaoModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NotificacaoModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NotificacaoModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {
            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region NOTIFICACAO
                c.CreateMap<NotificacaoModel, NotificacaoViewModel>();
                c.CreateMap<NotificacaoViewModel, NotificacaoModel>();
                c.CreateMap<NotificacaoViewModelComPaginacaoReference, NotificacaoModel>();
                c.CreateMap<NotificacaoViewModelForResponse, NotificacaoModel>();
                c.CreateMap<NotificacaoModel, NotificacaoViewModelForResponse>();
                #endregion
            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _notificacaoController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<NotificacaoViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _notificacaoController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<NotificacaoViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<NotificacaoViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _notificacaoController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<NotificacaoViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _notificacaoController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<NotificacaoViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
