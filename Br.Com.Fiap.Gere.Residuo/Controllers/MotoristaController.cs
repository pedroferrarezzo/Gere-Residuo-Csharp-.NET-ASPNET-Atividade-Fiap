using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Motorista;
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
    public class MotoristaController : ControllerBase
    {
        private readonly IMotoristaService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public MotoristaController(IMotoristaService service, IMapper mapper, ErrorViewModel errorViewModel)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
        }

        /// <summary>
        /// Lista todos os motoristas.
        /// </summary>
        /// <returns>Motoristas cadastrados até o momento.</returns>
        /// <response code="200">Retorna os motoristas cadastrados até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<MotoristaViewModel>> Get()
        {
            var motoristas = _service.ListarMotoristas();
            var viewModelList = _mapper.Map<IEnumerable<MotoristaViewModel>>(motoristas);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista um motorista por ID.
        /// </summary>
        /// <returns>Motorista escolhido por ID.</returns>
        /// <response code="200">Retorna o motorista pelo ID.</response>
        /// <response code="404">O motorista de ID especificado não foi encontrado.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<MotoristaViewModel> Get(int id)
        {
            var motorista = _service.ObterMotoristaPorId(id);

            if (motorista == null)
            {
                _errorViewModel.Message = $"O motorista de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<MotoristaViewModel>(motorista);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todos os motoristas com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Motoristas cadastrados até o momento.</returns>
        /// <response code="200">Retorna os motoristas cadastrados até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<MotoristaViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var motoristasProcessadosComPaginacaoReferenceModel = _service.ListarMotoristasPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<MotoristaViewModel>>(motoristasProcessadosComPaginacaoReferenceModel);

            if (!viewModelList.IsNullOrEmpty())
            {
                var viewModelReferenceList = new MotoristaViewModelComPaginacaoReference
                {
                    Motoristas = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().MotoristaId
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
        /// Cria um motorista.
        /// </summary>
        /// <returns>O motorista criado.</returns>
        /// <response code="201">Retorna o motorista criado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível número de celular ou CPF repetido.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Post([FromBody] MotoristaCreateViewModel viewModel)
        {
            try
            {
                var motorista = _mapper.Map<MotoristaModel>(viewModel);
                _service.CriarMotorista(motorista);
                var motoristaForResponse = _mapper.Map<MotoristaViewModelForResponse>(motorista);
                return CreatedAtAction(nameof(Get), new { id = motoristaForResponse.MotoristaId }, motoristaForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando cadastrar um motorista passando um número de celular ou cpf repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Atualiza um motorista existente.
        /// </summary>
        /// <param name="id">ID do motorista a ser atualizado.</param>
        /// <returns>O motorista atualizado.</returns>
        /// <response code="200">Retorna o motorista atualizado.</response>
        /// <response code="404">O motorista não foi encontrado.</response>
        /// <response code="400">O ID no corpo da requisição diverge do ID na URL.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível número de celular ou CPF repetido.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Put(int id, [FromBody] MotoristaUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.MotoristaId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.MotoristaId}!";
                    return BadRequest(_errorViewModel);
                }

                var motoristaExistente = _service.ObterMotoristaPorId(id);
                if (motoristaExistente == null)
                {
                    _errorViewModel.Message = $"O motorista de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                _mapper.Map(viewModel, motoristaExistente);
                _service.AtualizarMotorista(motoristaExistente);

                var motoristaAtualizadoForResponse = _mapper.Map<MotoristaViewModelForResponse>(motoristaExistente);
                var urn = Url.Action("Get", new { id = motoristaAtualizadoForResponse.MotoristaId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(motoristaAtualizadoForResponse);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando atualizar um motorista passando um número de celular ou cpf repetido?";
                return Conflict(_errorViewModel);
            }
        }

        /// <summary>
        /// Deleta um motorista.
        /// </summary>
        /// <param name="id">ID do motorista a ser deletado.</param>
        /// <response code="204">Motorista deletado com sucesso.</response>
        /// <response code="404">O motorista de ID especificado não foi encontrado.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível exclusão de motorista com registros associados.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var motoristaExistente = _service.ObterMotoristaPorId(id);

                if (motoristaExistente == null)
                {
                    _errorViewModel.Message = $"O motorista de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                _service.DeletarMotorista(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando excluir um motorista que possui registros associados?";
                return Conflict(_errorViewModel);
            }
        }
    }
}
