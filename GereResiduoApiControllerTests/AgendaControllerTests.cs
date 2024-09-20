using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Agenda;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Agenda;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.Service.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Utils.Email;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class AgendaControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly AgendaController _agendaController;
        private readonly DbSet<AgendaModel> _mockDbSetAgenda;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;
        private readonly ISmtpUtils _smtpUtils;

        private readonly IAgendaRepository _agendaRepository;
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly IBairroRepository _bairroRepository;
        private readonly ICaminhaoRepository _caminhaoRepository;
        private readonly IMotoristaRepository _motoristaRepository;

        private readonly IAgendaService _agendaService;
        private readonly INotificacaoService _notificacaoService;
        private readonly IBairroService _bairroService;
        private readonly ICaminhaoService _caminhaoService;
        private readonly IMotoristaService _motoristaService;


        public AgendaControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetAgenda = MockDbSetAgenda();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();
            _smtpUtils = new SmtpUtils("", "", "", 0, "");

            _notificacaoRepository = new NotificacaoRepository(_mockDatabaseContext.Object);
            _agendaRepository = new AgendaRepository(_mockDatabaseContext.Object);
            _bairroRepository = new BairroRepository(_mockDatabaseContext.Object);
            _caminhaoRepository = new CaminhaoRepository(_mockDatabaseContext.Object);
            _motoristaRepository = new MotoristaRepository(_mockDatabaseContext.Object);

            _notificacaoService = new NotificacaoService(_notificacaoRepository, _smtpUtils);
            _bairroService = new BairroService(_bairroRepository);
            _motoristaService = new MotoristaService(_motoristaRepository);
            _caminhaoService = new CaminhaoService(_caminhaoRepository);

            _agendaService = new AgendaService(
                _agendaRepository,
                _notificacaoService,
                _bairroService,
                _caminhaoService,
                _motoristaService
                );

            _agendaController = new AgendaController(
                _agendaService,
                _autoMapper,
                _errorViewModel,
                _bairroService,
                _caminhaoService,
                _motoristaService);

            _mockDatabaseContext.Setup(m => m.TabelaAgenda).Returns(_mockDbSetAgenda);

        }


        private DbSet<AgendaModel> MockDbSetAgenda()
        {

            var data = new List<AgendaModel>
            {
                new AgendaModel {
                    AgendaId = 1,
                    CaminhaoId = 1,
                    MotoristaId = 1,
                    BairroId = 1,
                    DiaColetaDeLixo = new DateOnly(2024, 06, 17),
                    TipoResiduo = TipoResiduo.ORGANICO,
                    StatusColetaDeLixoAgendada = StatusColetaDeLixo.EM_ANDAMENTO,
                    PesoColetadoDeLixoKg = 500
                },

                new AgendaModel {
                    AgendaId = 2,
                    CaminhaoId = 2,
                    MotoristaId = 2,
                    BairroId = 2,
                    DiaColetaDeLixo = new DateOnly(2024, 04, 12),
                    TipoResiduo = TipoResiduo.PLASTICO,
                    StatusColetaDeLixoAgendada = StatusColetaDeLixo.FINALIZADA,
                    PesoColetadoDeLixoKg = 350
                },
                new AgendaModel {
                    AgendaId = 3,
                    CaminhaoId = 6,
                    MotoristaId = 5,
                    BairroId = 9,
                    DiaColetaDeLixo = new DateOnly(2024, 12, 18),
                    TipoResiduo = TipoResiduo.GERAL,
                    StatusColetaDeLixoAgendada = StatusColetaDeLixo.EM_ANDAMENTO,
                    PesoColetadoDeLixoKg = 123
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<AgendaModel>>();
            mockDbSet.As<IQueryable<AgendaModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<AgendaModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<AgendaModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<AgendaModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {

            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region AGENDA
                c.CreateMap<AgendaModel, AgendaViewModel>();
                c.CreateMap<AgendaViewModel, AgendaModel>();
                c.CreateMap<AgendaCreateViewModel, AgendaModel>();
                c.CreateMap<AgendaUpdateViewModel, AgendaModel>();
                c.CreateMap<AgendaViewModelComPaginacaoReference, AgendaModel>();
                c.CreateMap<AgendaViewModelForResponse, AgendaModel>();
                c.CreateMap<AgendaModel, AgendaViewModelForResponse>();
                #endregion

            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _agendaController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<AgendaViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _agendaController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<AgendaViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<AgendaViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _agendaController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<AgendaViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _agendaController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<AgendaViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(300, okResult.StatusCode);
        }

    }
}