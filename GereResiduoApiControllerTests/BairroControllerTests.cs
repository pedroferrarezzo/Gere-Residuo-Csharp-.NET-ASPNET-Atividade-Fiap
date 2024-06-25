using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class BairroControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly BairroController _bairroController;
        private readonly DbSet<BairroModel> _mockDbSetBairro;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;

        private readonly IBairroRepository _bairroRepository;
        private readonly IBairroService _bairroService;


        public BairroControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetBairro = MockDbSetBairro();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();

            _bairroRepository = new BairroRepository(_mockDatabaseContext.Object);
            _bairroService = new BairroService(_bairroRepository);

            _bairroController = new BairroController(
                _bairroService,
                _autoMapper,
                _errorViewModel);

            _mockDatabaseContext.Setup(m => m.TabelaBairro).Returns(_mockDbSetBairro);
        }


        private DbSet<BairroModel> MockDbSetBairro()
        {
            var data = new List<BairroModel>
        {
            new BairroModel {
                BairroId = 1,
                BairroNome = "João Alves",
                QuantidadeLixeiras = 3,
                PesoMedioLixeirasKg = 20,
                PercentualColetaLixoBairro = 50,
                BairroEstaDisponivel = true
            },
            new BairroModel {
                BairroId = 2,
                BairroNome = "Centro",
                QuantidadeLixeiras = 5,
                PesoMedioLixeirasKg = 30,
                PercentualColetaLixoBairro = 75,
                BairroEstaDisponivel = true
            },
            new BairroModel {
                BairroId = 3,
                BairroNome = "Bairro Teste",
                QuantidadeLixeiras = 2,
                PesoMedioLixeirasKg = 15,
                PercentualColetaLixoBairro = 65,
                BairroEstaDisponivel = true
            }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<BairroModel>>();
            mockDbSet.As<IQueryable<BairroModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<BairroModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<BairroModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<BairroModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {

            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region BAIRRO
                c.CreateMap<BairroModel, BairroViewModel>();
                c.CreateMap<BairroViewModel, BairroModel>();
                c.CreateMap<BairroCreateViewModel, BairroModel>();
                c.CreateMap<BairroUpdateViewModel, BairroModel>();
                c.CreateMap<BairroViewModelComPaginacaoReference, BairroModel>();
                c.CreateMap<BairroViewModelForResponse, BairroModel>();
                c.CreateMap<BairroModel, BairroViewModelForResponse>();
                #endregion

            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _bairroController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<BairroViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _bairroController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<BairroViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<BairroViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _bairroController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<BairroViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _bairroController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<BairroViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

    }
}