using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Caminhao;
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
    public class CaminhaoController : ControllerBase
    {
        private readonly ICaminhaoService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public CaminhaoController(ICaminhaoService service, IMapper mapper, ErrorViewModel errorViewModel)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
        }

        /// <summary>
        /// Lista todos os caminhões.
        /// </summary>
        /// <returns>Caminhões cadastrados até o momento.</returns>
        /// <response code="200">Retorna os caminhões cadastrados até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<CaminhaoViewModel>> Get()
        {
            var caminhoes = _service.ListarCaminhoes();
            var viewModelList = _mapper.Map<IEnumerable<CaminhaoViewModel>>(caminhoes);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista um caminhão por ID.
        /// </summary>
        /// <returns>Caminhão escolhido por ID.</returns>
        /// <response code="200">Retorna o caminhão pelo ID.</response>
        /// <response code="404">O caminhão de ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<CaminhaoViewModel> Get(int id)
        {
            var caminhao = _service.ObterCaminhaoPorId(id);

            if (caminhao == null)
            {
                _errorViewModel.Message = $"O caminhão de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<CaminhaoViewModel>(caminhao);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todos os caminhões com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Caminhões cadastrados até o momento.</returns>
        /// <response code="200">Retorna os caminhões cadastrados até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<CaminhaoViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var caminhoesProcessadosComPaginacaoReferenceModel = _service.ListarCaminhoesPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<CaminhaoViewModel>>(caminhoesProcessadosComPaginacaoReferenceModel);

            if (!viewModelList.IsNullOrEmpty())
            {
                var viewModelReferenceList = new CaminhaoViewModelComPaginacaoReference
                {
                    Caminhoes = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().CaminhaoId
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
        /// Cria um caminhão.
        /// </summary>
        /// <returns>O caminhão criado.</returns>
        /// <response code="201">Retorna o caminhão criado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível placa de caminhão repetida.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Post([FromBody] CaminhaoCreateViewModel viewModel)
        {
            try
            {
                var caminhao = _mapper.Map<CaminhaoModel>(viewModel);
                _service.CriarCaminhao(caminhao);
                var caminhaoForResponse = _mapper.Map<CaminhaoViewModelForResponse>(caminhao);
                return CreatedAtAction(nameof(Get), new { id = caminhaoForResponse.CaminhaoId }, caminhaoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando cadastrar um caminhão passando uma placa repetida?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Atualiza um caminhão existente.
        /// </summary>
        /// <param name="id">ID do caminhão a ser atualizado.</param>
        /// <returns>O caminhão atualizado.</returns>
        /// <response code="200">Retorna o caminhão atualizado.</response>
        /// <response code="404">O caminhão não foi encontrado.</response>
        /// <response code="400">O ID no corpo da requisição diverge do ID na URL.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível placa de caminhão repetida.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Put(int id, [FromBody] CaminhaoUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.CaminhaoId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.CaminhaoId}!";
                    return BadRequest(_errorViewModel);
                }
                var caminhaoExistente = _service.ObterCaminhaoPorId(id);
                if (caminhaoExistente == null)
                {
                    _errorViewModel.Message = $"O caminhão de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }
                

                _mapper.Map(viewModel, caminhaoExistente);
                _service.AtualizarCaminhao(caminhaoExistente);

                var caminhaoAtualizadoForResponse = _mapper.Map<CaminhaoViewModelForResponse>(caminhaoExistente);
                var urn = Url.Action("Get", new { id = caminhaoAtualizadoForResponse.CaminhaoId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(caminhaoAtualizadoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando atualizar um caminhão passando uma placa repetida?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Deleta um caminhão.
        /// </summary>
        /// <param name="id">ID do caminhão a ser deletado.</param>
        /// <response code="204">Caminhão deletado com sucesso.</response>
        /// <response code="404">Caminhão não encontrado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível exclusão de caminhão com registros associados.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var caminhaoExistente = _service.ObterCaminhaoPorId(id);

                if (caminhaoExistente == null)
                {
                    _errorViewModel.Message = $"O caminhão de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                _service.DeletarCaminhao(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando excluir um caminhão que possui registros associados?";
                return Conflict(_errorViewModel);
            }
        }
    }
}
