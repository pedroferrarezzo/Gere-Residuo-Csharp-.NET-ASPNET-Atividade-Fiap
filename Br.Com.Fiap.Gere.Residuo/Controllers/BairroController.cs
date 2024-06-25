using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Bairro;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Br.Com.Fiap.Gere.Residuo.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BairroController : ControllerBase
    {
        private readonly IBairroService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public BairroController(IBairroService service, IMapper mapper, ErrorViewModel errorViewModel)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
        }

        /// <summary>
        /// Lista todos os bairros.
        /// </summary>
        /// <returns>Bairros cadastrados até o momento.</returns>
        /// <response code="200">Retorna os bairros cadastrados até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<BairroViewModel>> Get()
        {
            var bairros = _service.ListarBairros();
            var viewModelList = _mapper.Map<IEnumerable<BairroViewModel>>(bairros);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista um bairro por ID.
        /// </summary>
        /// <returns>Bairro escolhido por ID.</returns>
        /// <response code="200">Retorna o bairro pelo ID.</response>
        /// <response code="404">O bairro de ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<BairroViewModel> Get(int id)
        {
            var bairro = _service.ObterBairroPorId(id);

            if (bairro == null)
            {
                _errorViewModel.Message = $"O bairro de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<BairroViewModel>(bairro);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todos os bairros com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Bairros cadastrados até o momento.</returns>
        /// <response code="200">Retorna os bairros cadastrados até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<BairroViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var bairrosProcessadosComPaginacaoReferenceModel = _service.ListarBairrosPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<BairroViewModel>>(bairrosProcessadosComPaginacaoReferenceModel);

            if (!viewModelList.IsNullOrEmpty())
            {
                var viewModelReferenceList = new BairroViewModelComPaginacaoReference
                {
                    Bairros = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().BairroId
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
        /// Cria um bairro.
        /// </summary>
        /// <returns>O bairro criado.</returns>
        /// <response code="201">Retorna o bairro criado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível nome de bairro repetido.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Post([FromBody] BairroCreateViewModel viewModel)
        {
            try
            {
                var bairro = _mapper.Map<BairroModel>(viewModel);
                _service.CriarBairro(bairro);
                var bairroForResponse = _mapper.Map<BairroViewModelForResponse>(bairro);
                return CreatedAtAction(nameof(Get), new { id = bairroForResponse.BairroId }, bairroForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando cadastrar um bairro passando um nome repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Atualiza um bairro existente.
        /// </summary>
        /// <param name="id">ID do bairro a ser atualizado.</param>
        /// <returns>O bairro atualizado.</returns>
        /// <response code="200">Retorna o bairro atualizado.</response>
        /// <response code="404">O bairro não foi encontrado.</response>
        /// <response code="400">O ID no corpo da requisição diverge do ID na URL.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível nome de bairro repetido.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Put(int id, [FromBody] BairroUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.BairroId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.BairroId}!";
                    return BadRequest(_errorViewModel);
                }

                var bairroExistente = _service.ObterBairroPorId(id);
                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O bairro de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                

                _mapper.Map(viewModel, bairroExistente);
                _service.AtualizarBairro(bairroExistente);

                var bairroAtualizadoForResponse = _mapper.Map<BairroViewModelForResponse>(bairroExistente);
                var urn = Url.Action("Get", new { id = bairroAtualizadoForResponse.BairroId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(bairroAtualizadoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando atualizar um bairro passando um nome repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Deleta um bairro.
        /// </summary>
        /// <param name="id">ID do bairro a ser deletado.</param>
        /// <response code="204">Bairro deletado com sucesso.</response>
        /// <response code="404">Bairro não encontrado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível exclusão de bairro com registros associados.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var bairroExistente = _service.ObterBairroPorId(id);

                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O bairro de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                _service.DeletarBairro(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando excluir um bairro que possui registros associados?";
                return Conflict(_errorViewModel);
            }
        }
    }
}
