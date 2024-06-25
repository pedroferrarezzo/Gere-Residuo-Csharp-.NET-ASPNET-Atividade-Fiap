using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Bairro;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Morador;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Morador;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GereResiduoApiControllerTests
{
    public class MoradorControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly MoradorController _moradorController;
        private readonly DbSet<MoradorModel> _mockDbSetMorador;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;

        private readonly IBairroRepository _bairroRepository;
        private readonly IBairroService _bairroService;
        private readonly IMoradorRepository _moradorRepository;
        private readonly IMoradorService _moradorService;


        public MoradorControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetMorador = MockDbSetMorador();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();

            _moradorRepository = new MoradorRepository(_mockDatabaseContext.Object);
            _moradorService = new MoradorService(_moradorRepository);
            _bairroRepository = new BairroRepository(_mockDatabaseContext.Object);
            _bairroService = new BairroService(_bairroRepository);

            _moradorController = new MoradorController(
                _moradorService,
                _autoMapper,
                _bairroService,
                _errorViewModel);

            _mockDatabaseContext.Setup(m => m.TabelaMorador).Returns(_mockDbSetMorador);
        }


        private DbSet<MoradorModel> MockDbSetMorador()
        {
            var data = new List<MoradorModel>
        {
            new MoradorModel {
                MoradorId = 1,
                BairroId = 1,
                MoradorNome = "Carlos Silva",
                MoradorEmail = "carlos.silva@example.com"
            },
            new MoradorModel {
                MoradorId = 2,
                BairroId = 2,
                MoradorNome = "Maria Oliveira",
                MoradorEmail = "maria.oliveira@example.com"
            },
            new MoradorModel {
                MoradorId = 3,
                BairroId = 3,
                MoradorNome = "João Santos",
                MoradorEmail = "joao.santos@example.com"
            }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<MoradorModel>>();
            mockDbSet.As<IQueryable<MoradorModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<MoradorModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<MoradorModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<MoradorModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }


        private AutoMapper.MapperConfiguration MapperConfiguration()
        {

            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region MORADOR
                c.CreateMap<MoradorModel, MoradorViewModel>();
                c.CreateMap<MoradorViewModel, MoradorModel>();
                c.CreateMap<MoradorCreateViewModel, MoradorModel>();
                c.CreateMap<MoradorUpdateViewModel, MoradorModel>();
                c.CreateMap<MoradorViewModelComPaginacaoReference, MoradorModel>();
                c.CreateMap<MoradorViewModelForResponse, MoradorModel>();
                c.CreateMap<MoradorModel, MoradorViewModelForResponse>();
                #endregion

            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _moradorController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MoradorViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _moradorController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<MoradorViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<MoradorViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _moradorController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MoradorViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _moradorController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<MoradorViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

    }
}