using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Usuario;
using Br.Com.Fiap.Gere.Residuo.Utils.Token;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace Br.Com.Fiap.Gere.Residuo.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;
        private readonly IJWTUtils _jWTUtils;

        public UsuarioController(IUsuarioService service, IMapper mapper, ErrorViewModel errorViewModel, IJWTUtils jWTUtils)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
            _jWTUtils = jWTUtils;
        }

        /// <summary>
        /// Lista todos os usuários (obsoleto).
        /// </summary>
        /// <returns>Lista de todos os usuários.</returns>
        /// <response code="200">Retorna uma lista de usuários.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)}")]
        public ActionResult<IEnumerable<UsuarioViewModel>> Get()
        {
            var usuarios = _service.ListarUsuarios();
            var viewModelList = _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Obtém um usuário por ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>O usuário com o ID especificado.</returns>
        /// <response code="200">Retorna o usuário especificado.</response>
        /// <response code="404">O usuário com o ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)}")]
        public ActionResult<UsuarioViewModel> Get(int id)
        {
            var usuario = _service.ObterUsuarioPorId(id);
            if (usuario == null)
            {
                _errorViewModel.Message = $"O usuário de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<UsuarioViewModel>(usuario);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todos os usuários com paginação (Indexador: Primary Key).
        /// </summary>
        /// <param name="referencia">Referência para a paginação.</param>
        /// <param name="tamanho">Tamanho da página.</param>
        /// <returns>Lista de usuários com paginação.</returns>
        /// <response code="200">Retorna uma lista de usuários com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)}")]
        public ActionResult<IEnumerable<UsuarioViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var usuariosProcessadosComPaginacaoReferenceModel = _service.ListarUsuariosPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<UsuarioViewModel>>(usuariosProcessadosComPaginacaoReferenceModel);

            if (viewModelList != null && viewModelList.Any())
            {
                var viewModelReferenceList = new UsuarioViewModelComPaginacaoReference
                {
                    Usuarios = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().UsuarioId
                };
                return Ok(viewModelReferenceList);
            }
            else
            {
                _errorViewModel.Message = "Sem registros para esta referência!";
                return NotFound(_errorViewModel);
            }
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <returns>O novo usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível usuário com email repetido.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public ActionResult Post([FromBody] UsuarioCreateViewModel viewModel)
        {
            try
            {
                var usuario = _mapper.Map<UsuarioModel>(viewModel);
                _service.CriarUsuario(usuario);
                var usuarioForResponse = _mapper.Map<UsuarioViewModelForResponse>(usuario);

                return CreatedAtAction(nameof(Get), new { id = usuarioForResponse.UsuarioId }, usuarioForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando cadastrar um usuário passando um email repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Autentica um usuário.
        /// </summary>
        /// <returns>O token de autenticação.</returns>
        /// <response code="200">Usuário autenticado com sucesso.</response>
        /// <response code="401">Email ou senha incorretos.</response>
        [MapToApiVersion(1)]
        [HttpPost("Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Login([FromBody] UsuarioLoginViewModel user)
        {
            var authenticatedUser = _service.Autenticar(user.UsuarioEmail, user.UsuarioSenha);

            if (authenticatedUser == null)
            {
                _errorViewModel.Message = "Email ou senha incorretos!";
                return Unauthorized(_errorViewModel);
            }
            var token = _jWTUtils.GenerateJwtToken(authenticatedUser, 5);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Atualiza um usuário.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <returns>O usuário atualizado.</returns>
        /// <response code="200">Usuário atualizado com sucesso.</response>
        /// <response code="400">O ID passado na URL diverge do ID passado no corpo da requisição.</response>
        /// <response code="404">O usuário com o ID especificado não foi encontrado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível usuário com email repetido.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)}")]
        public ActionResult Put(int id, [FromBody] UsuarioUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.UsuarioId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.UsuarioId}!";
                    return BadRequest(_errorViewModel);
                }

                var usuarioExistente = _service.ObterUsuarioPorId(id);
                if (usuarioExistente == null)
                {
                    _errorViewModel.Message = $"O usuário de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                

                _mapper.Map(viewModel, usuarioExistente);
                _service.AtualizarUsuario(usuarioExistente);

                var usuarioAtualizadoForResponse = _mapper.Map<UsuarioViewModelForResponse>(usuarioExistente);
                var urn = Url.Action("Get", new { id = usuarioAtualizadoForResponse.UsuarioId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(usuarioAtualizadoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando atualizar um usuário passando um email repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Exclui um usuário.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Usuário excluído com sucesso.</response>
        /// <response code="404">O usuário com o ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)}")]
        public ActionResult Delete(int id)
        {
            var usuarioExistente = _service.ObterUsuarioPorId(id);
            if (usuarioExistente == null)
            {
                _errorViewModel.Message = $"O usuário de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            _service.DeletarUsuario(id);
            return NoContent();
        }
    }
}

