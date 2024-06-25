using Asp.Versioning;
using AutoMapper;
using Br.Com.Fiap.Gere.Residuo.Model;
using Br.Com.Fiap.Gere.Residuo.Service.Notificacao;
using Br.Com.Fiap.Gere.Residuo.Validation.ActionFilters;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Notificacao;
using Br.Com.Fiap.Gere.Residuo.ViewModel.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using Br.Com.Fiap.Gere.Residuo.Utils.Email;

namespace Br.Com.Fiap.Gere.Residuo.Controllers
{
    [ApiVersion(1)]
    [ApiVersion(2)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoService _service;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        public NotificacaoController(INotificacaoService service, IMapper mapper, ErrorViewModel errorViewModel)
        {
            _service = service;
            _mapper = mapper;
            _errorViewModel = errorViewModel;
        }

        /// <summary>
        /// Lista todas as notificações.
        /// </summary>
        /// <returns>Notificações cadastradas até o momento.</returns>
        /// <response code="200">Retorna as notificações cadastradas até o momento.</response>
        [MapToApiVersion(1)]
        [Obsolete("Este método está obsoleto! Observe o header para entender as versões de API suportadas.")]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<NotificacaoViewModel>> Get()
        {
            var notificacoes = _service.ListarNotificacoes();
            var viewModelList = _mapper.Map<IEnumerable<NotificacaoViewModel>>(notificacoes);
            return Ok(viewModelList);
        }

        /// <summary>
        /// Lista uma notificação por ID.
        /// </summary>
        /// <returns>Notificação escolhida por ID.</returns>
        /// <response code="200">Retorna a notificação pelo ID.</response>
        /// <response code="404">A notificação de ID especificado não foi encontrada.</response>
        [MapToApiVersion(1)]
        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<NotificacaoViewModel> Get(int id)
        {
            var notificacao = _service.ObterNotificacaoPorId(id);

            if (notificacao == null)
            {
                _errorViewModel.Message = $"A notificação de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            var viewModel = _mapper.Map<NotificacaoViewModel>(notificacao);
            return Ok(viewModel);
        }

        /// <summary>
        /// Lista todas as notificações com paginação - Indexador: Primary Key.
        /// </summary>
        /// <returns>Notificações cadastradas até o momento.</returns>
        /// <response code="200">Retorna as notificações cadastradas até o momento com paginação.</response>
        /// <response code="404">Sem registros para esta referência.</response>
        [MapToApiVersion(2)]
        [HttpGet]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}, {nameof(UsuarioRole.USER)}")]
        public ActionResult<IEnumerable<NotificacaoViewModelComPaginacaoReference>> GetAllReference([FromQuery] int referencia = 0, [FromQuery] int tamanho = 10)
        {
            var notificacoesProcessadasComPaginacaoReferenceModel = _service.ListarNotificacoesPaginacaoReference(referencia, tamanho);
            var viewModelList = _mapper.Map<IEnumerable<NotificacaoViewModel>>(notificacoesProcessadasComPaginacaoReferenceModel);

            if (viewModelList != null && viewModelList.Any())
            {
                var viewModelReferenceList = new NotificacaoViewModelComPaginacaoReference
                {
                    Notificacoes = viewModelList,
                    PageSize = tamanho,
                    Ref = referencia,
                    NextRef = viewModelList.Last().NotificacaoId
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
        /// Deleta uma notificação.
        /// </summary>
        /// <param name="id">ID da notificação a ser deletada.</param>
        /// <response code="204">Notificação deletada com sucesso. Além disso, dispara um email para cada morador utilizando o protocolo SMTP (servidor de teste utilizado: MailTrap)</response>
        /// <response code="404">A notificação de ID especificado não foi encontrada.</response>
        [MapToApiVersion(1)]
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UsuarioRole.ADMIN)},{nameof(UsuarioRole.OPERADOR)}")]
        public ActionResult Delete(int id)
        {
            var notificacaoExistente = _service.ObterNotificacaoPorId(id);

            if (notificacaoExistente == null)
            {
                _errorViewModel.Message = $"A notificação de ID: {id} não existe!";
                return NotFound(_errorViewModel);
            }

            _service.DeletarNotificacao(id);
            return NoContent();
        }
    }
}
