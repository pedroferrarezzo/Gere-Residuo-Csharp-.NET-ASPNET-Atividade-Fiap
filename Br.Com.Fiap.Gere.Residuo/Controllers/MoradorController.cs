using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Morador;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Morador;
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
    public class MoradorController : ControllerBase
    {
        private readonly IMoradorService _service;
        private readonly IBairroService _bairroService;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public MoradorController(IMoradorService service, IMapper mapper, IBairroService bairroService, ErrorViewModel errorViewModel)
        {
            _service = service;
            _mapper = mapper;
            _bairroService = bairroService;
            _errorViewModel = errorViewModel;
        }

        /// <summary>
        /// Lista todos os moradores.
        /// </summary>
        /// <returns>Moradores cadastrados até o momento.</returns>
        /// <response code="200">Retorna os moradores cadastrados até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<MoradorViewModel>> Get()
        {
            var moradores = _service.ListarMoradores();
            var viewModelList = _mapper.Map<IEnumerable<MoradorViewModel>>(moradores);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista um morador por ID.
        /// </summary>
        /// <returns>Morador escolhido por ID.</returns>
        /// <response code="200">Retorna o morador pelo ID.</response>
        /// <response code="404">O morador de ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<MoradorViewModel> Get(int id)
        {
            var morador = _service.ObterMoradorPorId(id);

            if (morador == null)
            {
                _errorViewModel.Message = $"O morador de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<MoradorViewModel>(morador);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todos os moradores com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Moradores cadastrados até o momento.</returns>
        /// <response code="200">Retorna os moradores cadastrados até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<MoradorViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var moradoresProcessadosComPaginacaoReferenceModel = _service.ListarMoradoresPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<MoradorViewModel>>(moradoresProcessadosComPaginacaoReferenceModel);

            if (!viewModelList.IsNullOrEmpty())
            {
                var viewModelReferenceList = new MoradorViewModelComPaginacaoReference
                {
                    Moradores = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().MoradorId
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
        /// Cria um morador.
        /// </summary>
        /// <returns>O morador criado.</returns>
        /// <response code="201">Retorna o morador criado.</response>
        /// <response code="404">O Bairro de ID especificado não foi encontrado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível email repetido.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Post([FromBody] MoradorCreateViewModel viewModel)
        {
            try
            {
                var bairroExistente = _bairroService.ObterBairroPorId(viewModel.BairroId);

                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O Bairro de ID: {viewModel.BairroId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var morador = _mapper.Map<MoradorModel>(viewModel);
                _service.CriarMorador(morador);
                var moradorForResponse = _mapper.Map<MoradorViewModelForResponse>(morador);
                return CreatedAtAction(nameof(Get), new { id = moradorForResponse.MoradorId }, moradorForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando cadastrar um morador passando um email repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Atualiza um morador existente.
        /// </summary>
        /// <param name="id">ID do morador a ser atualizado.</param>
        /// <returns>O morador atualizado.</returns>
        /// <response code="200">Retorna o morador atualizado.</response>
        /// <response code="404">O morador ou o bairro não foi encontrado.</response>
        /// <response code="400">O ID no corpo da requisição diverge do ID na URL.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível email repetido.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Put(int id, [FromBody] MoradorUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.MoradorId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.MoradorId}!";
                    return BadRequest(_errorViewModel);
                }

                var moradorExistente = _service.ObterMoradorPorId(id);
                if (moradorExistente == null)
                {
                    _errorViewModel.Message = $"O morador de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                var bairroExistente = _bairroService.ObterBairroPorId(viewModel.BairroId);
                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O Bairro de ID: {viewModel.BairroId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                _mapper.Map(viewModel, moradorExistente);
                _service.AtualizarMorador(moradorExistente);

                var moradorAtualizadoForResponse = _mapper.Map<MoradorViewModelForResponse>(moradorExistente);
                var urn = Url.Action("Get", new { id = moradorAtualizadoForResponse.MoradorId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(moradorAtualizadoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando atualizar um morador passando um email repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Deleta um morador.
        /// </summary>
        /// <param name="id">ID do morador a ser deletado.</param>
        /// <response code="204">Morador deletado com sucesso.</response>
        /// <response code="404">O morador de ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            var moradorExistente = _service.ObterMoradorPorId(id);
            if (moradorExistente == null)
            {
                _errorViewModel.Message = $"O morador de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            _service.DeletarMorador(id);
            return NoContent();
        }
    }
}
