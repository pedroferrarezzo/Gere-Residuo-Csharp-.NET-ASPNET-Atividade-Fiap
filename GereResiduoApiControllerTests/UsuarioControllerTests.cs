using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Controllers;
using Br.Com.Fiap.Gere.Residuo.Data.Context;
using Br.Com.Fiap.Gere.Residuo.Data.Repository.Usuario;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Usuario;
using Br.Com.Fiap.Gere.Residuo.Utils.Hash;
using Br.Com.Fiap.Gere.Residuo.Utils.Token;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;

namespace GereResiduoApiControllerTests
{
    public class UsuarioControllerTests
    {
        private readonly Mock<DatabaseContext> _mockDatabaseContext;
        private readonly UsuarioController _usuarioController;
        private readonly DbSet<UsuarioModel> _mockDbSetUsuario;
        private readonly IMapper _autoMapper;
        private readonly ErrorViewModel _errorViewModel;
        private readonly IHashUtils _hashUtils;
        private readonly IJWTUtils _jwtUtils;

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;

        public UsuarioControllerTests()
        {
            _mockDatabaseContext = new Mock<DatabaseContext>();
            _mockDbSetUsuario = MockDbSetUsuario();
            _autoMapper = MapperConfiguration().CreateMapper();
            _errorViewModel = new ErrorViewModel();
            _hashUtils = new HashUtils(SHA256.Create(), new System.Text.StringBuilder());
            _jwtUtils = new JWTUtils();

            _usuarioRepository = new UsuarioRepository(_mockDatabaseContext.Object);
            _usuarioService = new UsuarioService(_usuarioRepository, _hashUtils);

            _usuarioController = new UsuarioController(
                _usuarioService,
                _autoMapper,
                _errorViewModel,
                _jwtUtils);

            _mockDatabaseContext.Setup(m => m.TabelaUsuario).Returns(_mockDbSetUsuario);
        }

        private DbSet<UsuarioModel> MockDbSetUsuario()
        {
            var data = new List<UsuarioModel>
            {
                new UsuarioModel {
                    UsuarioId = 1,
                    UsuarioNome = "John Doe",
                    UsuarioEmail = "john.doe@example.com",
                    UsuarioSenha = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f",
                    UsuarioRole = UsuarioRole.OPERADOR
                },
                new UsuarioModel {
                    UsuarioId = 2,
                    UsuarioNome = "Jane Smith",
                    UsuarioEmail = "jane.smith@example.com",
                    UsuarioSenha = "3700adf1f25fab8202c1343c4b0b4e3fec706d57cad574086467b8b3ddf273ec",
                    UsuarioRole = UsuarioRole.USER
                },
                new UsuarioModel {
                    UsuarioId = 3,
                    UsuarioNome = "Bob Johnson",
                    UsuarioEmail = "bob.johnson@example.com",
                    UsuarioSenha = "2cc483689a9a5dff0ec4726dc13a797d6728069e98da7e849dda988b17fcf3f5",
                    UsuarioRole = UsuarioRole.ADMIN
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<UsuarioModel>>();
            mockDbSet.As<IQueryable<UsuarioModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<UsuarioModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<UsuarioModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<UsuarioModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }

        private AutoMapper.MapperConfiguration MapperConfiguration()
        {
            return new AutoMapper.MapperConfiguration(c =>
            {
                c.AllowNullCollections = true;
                c.AllowNullDestinationValues = true;

                #region USUARIO
                c.CreateMap<UsuarioModel, UsuarioViewModel>();
                c.CreateMap<UsuarioViewModel, UsuarioModel>();
                c.CreateMap<UsuarioCreateViewModel, UsuarioModel>();
                c.CreateMap<UsuarioUpdateViewModel, UsuarioModel>();
                c.CreateMap<UsuarioViewModelComPaginacaoReference, UsuarioModel>();
                c.CreateMap<UsuarioViewModelForResponse, UsuarioModel>();
                c.CreateMap<UsuarioModel, UsuarioViewModelForResponse>();
                c.CreateMap<UsuarioModel, UsuarioLoginViewModel>();
                c.CreateMap<UsuarioLoginViewModel, UsuarioModel>();
                #endregion
            });
        }

        [Fact]
        public async Task Get_ReturnsHttpStatusCode200()
        {
            // Arrange

            // Act
            var result = _usuarioController.Get();

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<UsuarioViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsThreeRegisters()
        {
            // Arrange

            // Act
            var result = _usuarioController.Get();

            // Assert

            var viewModelResult = Assert.IsType<ActionResult<IEnumerable<UsuarioViewModel>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(viewModelResult.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<UsuarioViewModel>>(okResult.Value);

            Assert.NotNull(model);

            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode404_WithInvalidReference()
        {
            // Arrange

            // Act
            var result = _usuarioController.GetAllReference(4, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<UsuarioViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(404, okResult.StatusCode);
        }

        [Fact]
        public void Get_ReturnsHttpStatusCode200_WithValidReference()
        {
            // Arrange

            // Act
            var result = _usuarioController.GetAllReference(1, 10);

            // Assert

            var actionResult = Assert.IsType<ActionResult<IEnumerable<UsuarioViewModelComPaginacaoReference>>>(result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            Assert.NotNull(okResult);

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}