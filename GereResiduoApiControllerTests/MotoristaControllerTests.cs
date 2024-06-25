using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Motorista;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class MotoristaControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly MotoristaController _motoristaController;
        private readonly DbSet<MotoristaModel> _mockDbSetMotorista;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;

        private readonly IMotoristaRepository _motoristaRepository;
        private readonly IMotoristaService _motoristaService;

        public MotoristaControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetMotorista = MockDbSetMotorista();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();

            _motoristaRepository = new MotoristaRepository(_mockDatabaseContext.Object);
            _motoristaService = new MotoristaService(_motoristaRepository);

            _motoristaController = new MotoristaController(
                _motoristaService,
                _autoMapper,
                _errorViewModel);

            _mockDatabaseContext.Setup(m => m.TabelaMotorista).Returns(_mockDbSetMotorista);
        }

        private DbSet<MotoristaModel> MockDbSetMotorista()
        {
            var data = new List<MotoristaModel>
            {
                new MotoristaModel {
                    MotoristaId = 1,
                    MotoristaNome = "João Silva",
                    MotoristaCpf = "12345678901",
                    MotoristaNrCelular = "998877665",
                    MotoristaNrCelularDdd = "11",
                    MotoristaNrCelularDdi = "55",
                    MotoristaEstaDisponivel = true,
                    AgendasCriadasComEsteMotorista = new List<AgendaModel>()
                },
                new MotoristaModel {
                    MotoristaId = 2,
                    MotoristaNome = "Maria Oliveira",
                    MotoristaCpf = "98765432100",
                    MotoristaNrCelular = "998877664",
                    MotoristaNrCelularDdd = "21",
                    MotoristaNrCelularDdi = "55",
                    MotoristaEstaDisponivel = false,
                    AgendasCriadasComEsteMotorista = new List<AgendaModel>()
                },
                new MotoristaModel {
                    MotoristaId = 3,
                    MotoristaNome = "Carlos Pereira",
                    MotoristaCpf = "11122233344",
                    MotoristaNrCelular = "998877663",
                    MotoristaNrCelularDdd = "31",
                    MotoristaNrCelularDdi = "55",
                    MotoristaEstaDisponivel = true,
                    AgendasCriadasComEsteMotorista = new List<AgendaModel>()
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<MotoristaModel>>();
            mockDbSet.As<IQueryable<MotoristaModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<MotoristaModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<MotoristaModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<MotoristaModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {
            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region MOTORISTA
                c.CreateMap<MotoristaModel, MotoristaViewModel>();
                c.CreateMap<MotoristaViewModel, MotoristaModel>();
                c.CreateMap<MotoristaCreateViewModel, MotoristaModel>();
                c.CreateMap<MotoristaUpdateViewModel, MotoristaModel>();
                c.CreateMap<MotoristaViewModelComPaginacaoReference, MotoristaModel>();
                c.CreateMap<MotoristaViewModelForResponse, MotoristaModel>();
                c.CreateMap<MotoristaModel, MotoristaViewModelForResponse>();
                #endregion
            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _motoristaController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MotoristaViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _motoristaController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<MotoristaViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<MotoristaViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _motoristaController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MotoristaViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _motoristaController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MotoristaViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
