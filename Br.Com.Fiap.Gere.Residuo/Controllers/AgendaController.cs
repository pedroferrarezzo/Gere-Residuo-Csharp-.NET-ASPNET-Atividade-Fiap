using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Agenda;
using Br.Com.Fiap.Gere.Residuo.Service.Bairro;
using Br.Com.Fiap.Gere.Residuo.Service.Caminhao;
using Br.Com.Fiap.Gere.Residuo.Service.Motorista;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Agenda;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Br.Com.Fiap.Gere.Residuo.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class AgendaController : ControllerBase
    {
        private readonly IAgendaService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;
        private readonly IBairroService _bairroService;
        private readonly ICaminhaoService _caminhaoService;
        private readonly IMotoristaService _motoristaService;

        public AgendaController(IAgendaService service, IMapper mapper, ErrorViewModel errorViewModel, IBairroService bairroService, ICaminhaoService caminhaoService, IMotoristaService motoristaService)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
            _bairroService = bairroService;
            _caminhaoService = caminhaoService;
            _motoristaService = motoristaService;
        }

        /// <summary>
        /// Lista todas as agendas.
        /// </summary>
        /// <returns>Agendas criadas até o momento.</returns>
        /// <response code="200">Retorna as agendas criadas até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<AgendaViewModel>> Get()
        {
            var agendas = _service.ListarAgendas();
            var viewModelList = _mapper.Map<IEnumerable<AgendaViewModel>>(agendas);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista uma agenda por ID.
        /// </summary>
        /// <returns>Agenda escolhida por ID.</returns>
        /// <response code="200">Retorna a agenda pelo ID.</response>
        /// <response code="404">A agenda de ID especificado não foi encontrada.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<AgendaViewModel> Get(int id)
        {
            var agenda = _service.ObterAgendaPorId(id);

            if (agenda == null)
            {
                _errorViewModel.Message = $"A agenda de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<AgendaViewModel>(agenda);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todas as agendas com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Agendas criadas até o momento.</returns>
        /// <response code="200">Retorna as agendas criadas até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<AgendaViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var agendasProcessadasComPaginacaoReferenceModel = _service.ListarAgendasPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<AgendaViewModel>>(agendasProcessadasComPaginacaoReferenceModel);

            if (viewModelList != null && viewModelList.Any())
            {
                var viewModelReferenceList = new AgendaViewModelComPaginacaoReference
                {
                    Agendas = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().AgendaId
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
        /// Cria uma agenda.
        /// </summary>
        /// <returns>A agenda criada.</returns>
        /// <response code="201">Retorna a agenda criada. O atributo EstaDisponivel das entidades Bairro, caminhão e motorista é atualizado para FALSE e uma notificação é criada.</response>
        /// <response code="404">Bairro, Caminhão ou Motorista não encontrado.</response>
        /// <response code="400">Bairro, Motorista ou Caminhão não está disponível ou possui conflito com agenda existente.</response>
        [MapToApiVersion(1)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Post([FromBody] AgendaCreateViewModel viewModel)
        {
            try
            {
                var bairroExistente = _bairroService.ObterBairroPorId(viewModel.BairroId);
                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O Bairro de ID: {viewModel.BairroId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var caminhaoExistente = _caminhaoService.ObterCaminhaoPorId(viewModel.CaminhaoId);
                if (caminhaoExistente == null)
                {
                    _errorViewModel.Message = $"O Caminhão de ID: {viewModel.CaminhaoId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var motoristaExistente = _motoristaService.ObterMotoristaPorId(viewModel.MotoristaId);
                if (motoristaExistente == null)
                {
                    _errorViewModel.Message = $"O Motorista de ID: {viewModel.MotoristaId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var agenda = _mapper.Map<AgendaModel>(viewModel);
                _service.CriarAgenda(agenda);
                var agendaForResponse = _mapper.Map<AgendaViewModelForResponse>(agenda);
                return CreatedAtAction(nameof(Get), new { id = agendaForResponse.AgendaId }, agendaForResponse);
            }
            catch (Exception.BairroNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O bairro escolhido já possui uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.MotoristaNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O motorista escolhido já foi alocado para uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.CaminhaoNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O caminhão escolhido já foi alocado para uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
        }

        /// <summary>
        /// Atualiza uma agenda existente.
        /// </summary>
        /// <param name="id">ID da agenda a ser atualizada.</param>
        /// <returns>A agenda atualizada.</returns>
        /// <response code="200">Retorna a agenda atualizada. Se statusColetaDeLixoAgendada = FINALIZADA, o atributo EstaDisponivel das entidades Bairro, Caminhão e Motorista é atualizado para TRUE, além de o percentual de coleta de lixo do bairro atrelado a agenda ser atualizado - com base no peso de lixo coletado ((PesoColetadoDeLixoKg / (PesoMedioLixeirasKg * QuantidadeLixeiras))* 100).</response>
        /// <response code="404">A agenda, Bairro, Caminhão ou Motorista não foi encontrado.</response>
        /// <response code="400">Bairro, Motorista ou Caminhão não está disponível ou possui conflito com agenda existente. Também pode ocorrer quando o ID no corpo da requisição diverge do ID na URL, ou a agenda já foi finalizada.</response>
        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Put(int id, [FromBody] AgendaUpdateViewModel viewModel)
        {
            try
            {
                if (viewModel.AgendaId != id)
                {
                    _errorViewModel.Message = $"O ID passado na URL: {id} diverge do ID passado no body da requisição: {viewModel.AgendaId}!";
                    return BadRequest(_errorViewModel);
                }

                var agendaExistente = _service.ObterAgendaPorId(id);
                if (agendaExistente == null)
                {
                    _errorViewModel.Message = $"A agenda de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                var statusColetaDeLixoExistenteDaAgenda = agendaExistente.StatusColetaDeLixoAgendada;

                var bairroExistente = _bairroService.ObterBairroPorId(agendaExistente.BairroId);
                if (bairroExistente == null)
                {
                    _errorViewModel.Message = $"O Bairro de ID: {viewModel.BairroId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var caminhaoExistente = _caminhaoService.ObterCaminhaoPorId(agendaExistente.CaminhaoId);
                if (caminhaoExistente == null)
                {
                    _errorViewModel.Message = $"O Caminhão de ID: {viewModel.CaminhaoId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                var motoristaExistente = _motoristaService.ObterMotoristaPorId(agendaExistente.MotoristaId);
                if (motoristaExistente == null)
                {
                    _errorViewModel.Message = $"O Motorista de ID: {viewModel.MotoristaId} não foi encontrado!";
                    return NotFound(_errorViewModel);
                }

                _mapper.Map(viewModel, agendaExistente);
                _service.AtualizarAgenda(agendaExistente, bairroExistente, motoristaExistente, caminhaoExistente, statusColetaDeLixoExistenteDaAgenda);

                var agendaAtualizadaForResponse = _mapper.Map<AgendaViewModelForResponse>(agendaExistente);
                var urn = Url.Action("Get", new { id = agendaAtualizadaForResponse.AgendaId });
                var uri = $"{Request.Scheme}://{Request.Host}{urn}";

                Response.Headers.Add("Location", uri);
                return Ok(agendaAtualizadaForResponse);

            }
            catch (Exception.BairroNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O bairro escolhido já possui uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.MotoristaNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O motorista escolhido já foi alocado para uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.CaminhaoNaoEstaDisponivelException ex)
            {
                _errorViewModel.Message = "O caminhão escolhido já foi alocado para uma agenda em andamento!";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.AgendaPossuiNotificacoesNaoDisparadasException ex)
            {
                _errorViewModel.Message = $"Não é possível finalizar uma agenda que possui notificações a serem disparadas! - ID da notificação aberta: {ex.Message}";
                return BadRequest(_errorViewModel);
            }
            catch (Exception.AgendaJaEstaFinalizadaException ex)
            {
                _errorViewModel.Message = $"Não é possível atualizar uma agenda que já foi finalizada!";
                return BadRequest(_errorViewModel);
            }
        }

        /// <summary>
        /// Deleta uma agenda.
        /// </summary>
        /// <param name="id">ID da agenda a ser deletada.</param>
        /// <response code="204">Agenda deletada com sucesso.</response>
        /// <response code="404">Agenda não encontrada.</response>
        /// <response code="409">Erro de integridade no banco de dados. Possível exclusão de agenda com registros associados.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var agendaExistente = _service.ObterAgendaPorId(id);

                if (agendaExistente == null)
                {
                    _errorViewModel.Message = $"A agenda de ID: {id} não existe!";
                    return NotFound(_errorViewModel);
                }

                _service.DeletarAgenda(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                _errorViewModel.Message = "Erro de integridade no banco de dados. Você está tentando excluir uma agenda que possui registros associados?";
                return Conflict(_errorViewModel);
            }
        }
    }
}
