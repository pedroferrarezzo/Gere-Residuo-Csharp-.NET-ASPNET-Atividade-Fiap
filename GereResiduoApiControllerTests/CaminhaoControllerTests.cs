using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class CaminhaoControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly CaminhaoController _caminhaoController;
        private readonly DbSet<CaminhaoModel> _mockDbSetCaminhao;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;

        private readonly ICaminhaoRepository _caminhaoRepository;
        private readonly ICaminhaoService _caminhaoService;

        public CaminhaoControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetCaminhao = MockDbSetCaminhao();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();

            _caminhaoRepository = new CaminhaoRepository(_mockDatabaseContext.Object);
            _caminhaoService = new CaminhaoService(_caminhaoRepository);

            _caminhaoController = new CaminhaoController(
                _caminhaoService,
                _autoMapper,
                _errorViewModel);

            _mockDatabaseContext.Setup(m => m.TabelaCaminhao).Returns(_mockDbSetCaminhao);
        }

        private DbSet<CaminhaoModel> MockDbSetCaminhao()
        {
            var data = new List<CaminhaoModel>
            {
                new CaminhaoModel {
                    CaminhaoId = 1,
                    CaminhaoPlaca = "ABC1234",
                    DataFabricacao = new DateOnly(2020, 5, 1),
                    CaminhaoMarca = "Marca A",
                    CaminhaoModelo = "Modelo A",
                    CaminhaoEstaDisponivel = true
                },
                new CaminhaoModel {
                    CaminhaoId = 2,
                    CaminhaoPlaca = "DEF5678",
                    DataFabricacao = new DateOnly(2021, 3, 15),
                    CaminhaoMarca = "Marca B",
                    CaminhaoModelo = "Modelo B",
                    CaminhaoEstaDisponivel = false
                },
                new CaminhaoModel {
                    CaminhaoId = 3,
                    CaminhaoPlaca = "GHI9012",
                    DataFabricacao = new DateOnly(2019, 8, 22),
                    CaminhaoMarca = "Marca C",
                    CaminhaoModelo = "Modelo C",
                    CaminhaoEstaDisponivel = true
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<CaminhaoModel>>();
            mockDbSet.As<IQueryable<CaminhaoModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CaminhaoModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CaminhaoModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CaminhaoModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {
            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region CAMINHAO
                c.CreateMap<CaminhaoModel, CaminhaoViewModel>();
                c.CreateMap<CaminhaoViewModel, CaminhaoModel>();
                c.CreateMap<CaminhaoCreateViewModel, CaminhaoModel>();
                c.CreateMap<CaminhaoUpdateViewModel, CaminhaoModel>();
                c.CreateMap<CaminhaoViewModelComPaginacaoReference, CaminhaoModel>();
                c.CreateMap<CaminhaoViewModelForResponse, CaminhaoModel>();
                c.CreateMap<CaminhaoModel, CaminhaoViewModelForResponse>();
                #endregion
            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _caminhaoController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<CaminhaoViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _caminhaoController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<CaminhaoViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<CaminhaoViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _caminhaoController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<CaminhaoViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _caminhaoController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<CaminhaoViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
